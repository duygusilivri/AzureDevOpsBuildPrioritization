using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MyBumpConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //organization url
            string orgUrl = "https://dev.azure.com/OrgName/";

            //pat of the user who has rights to administer the queue
            string pat = "aaaabbbbcccc";

            //id of the pool. to find the id: GET https://dev.azure.com/OrgName/_apis/distributedtask/pools?api-version=6.1-preview.1
            string poolId = "15";  //e.g.15

            //encode personal access token                   
            string credentials = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", pat)));

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(orgUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                //get the list of job requests in the queue         
                HttpResponseMessage response = client.GetAsync("_apis/distributedtask/pools/" + poolId + "/jobrequests?includeStatus=true").Result;

                if (response.IsSuccessStatusCode)
                {
                    List<JobRequest> listToBump = new List<JobRequest>();

                    bool scheduledBuildInQueue = false;

                    string responseBody = await response.Content.ReadAsStringAsync();
                    JobRequestResponse jobRequests = JsonConvert.DeserializeObject<JobRequestResponse>(responseBody);
                    for(int i=0; i< jobRequests.count; i++ )
                    {
                        JobRequest jobRequest = jobRequests.value[i];

                        //active request if null
                        string result = jobRequest.result;

                        //manually triggered or CI, if False
                        string isScheduled = jobRequest.data.IsScheduledKey;

                        //already running if not null
                        ReservedAgent reservedAgent = jobRequest.reservedAgent;

                        //waiting request in the queue
                        if (result is null && reservedAgent is null) 
                        {
                            if(isScheduled.Equals("False"))
                            {
                                listToBump.Add(jobRequest);
                            }
                            else
                            {
                                scheduledBuildInQueue = true;    
                            }
                        }
                    }

                    //no need to bump any builds if there are no scheduled builds in the queue
                    if (scheduledBuildInQueue) 
                    {
                        //reversing the list to keep the requests in the same order (FIFO) after bumping
                        listToBump.Reverse();   
                        foreach (JobRequest jobRequest in listToBump)
                        {
                            //bump the build to the top of the queue
                            string jobId = jobRequest.requestId.ToString();
                            string url = orgUrl + "_apis/distributedtask/pools/"+ poolId + "/jobrequests/"+ jobId + "?lockToken=00000000-0000-0000-0000-000000000000&updateOptions=1&api-version=6.1-preview.1";
                            string jsonContent = "{" + "\"requestId\":" + jobId + "}";

                            HttpClient client2 = new HttpClient();
                            client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                                Convert.ToBase64String(
                                    System.Text.ASCIIEncoding.ASCII.GetBytes(
                                        string.Format("{0}:{1}", "", pat))));

                            var method = new HttpMethod("PATCH");
                            var request = new HttpRequestMessage(method, url)
                            {
                                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
                            };

                            HttpResponseMessage response2 = new HttpResponseMessage();
                            response2 = await client2.SendAsync(request);
                        }
                    }  
                }
            }
        }
    }  
}
