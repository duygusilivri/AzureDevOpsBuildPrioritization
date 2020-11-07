using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBumpConsole
{
    public class Self
    {
        public string href { get; set; }
    }

    public class Web
    {
        public string href { get; set; }
    }

    public class Links
    {
        public Self self { get; set; }
        public Web web { get; set; }
    }

    public class ReservedAgent
    {
        public Links _links { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string version { get; set; }
        public string osDescription { get; set; }
        public bool enabled { get; set; }
        public string status { get; set; }
        public string provisioningState { get; set; }
        public string accessPoint { get; set; }
    }

    public class Web2
    {
        public string href { get; set; }
    }

    public class Self2
    {
        public string href { get; set; }
    }

    public class Links2
    {
        public Web2 web { get; set; }
        public Self2 self { get; set; }
    }

    public class Definition
    {
        public Links2 _links { get; set; }
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Web3
    {
        public string href { get; set; }
    }

    public class Self3
    {
        public string href { get; set; }
    }

    public class Links3
    {
        public Web3 web { get; set; }
        public Self3 self { get; set; }
    }

    public class Owner
    {
        public Links3 _links { get; set; }
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Data
    {
        public string ParallelismTag { get; set; }
        public string IsScheduledKey { get; set; }
    }

    public class AgentSpecification
    {
        public string VMImage { get; set; }
    }

    public class JobRequest
    {
        public int requestId { get; set; }
        public DateTime queueTime { get; set; }
        public DateTime assignTime { get; set; }
        public DateTime receiveTime { get; set; }
        public DateTime finishTime { get; set; }
        public string result { get; set; }
        public string serviceOwner { get; set; }
        public string hostId { get; set; }
        public string scopeId { get; set; }
        public string planType { get; set; }
        public string planId { get; set; }
        public string jobId { get; set; }
        public List<string> demands { get; set; }
        public ReservedAgent reservedAgent { get; set; }
        public Definition definition { get; set; }
        public Owner owner { get; set; }
        public Data data { get; set; }
        public int poolId { get; set; }
        public AgentSpecification agentSpecification { get; set; }
        public string orchestrationId { get; set; }
        public bool matchesAllAgentsInPool { get; set; }
        public int priority { get; set; }
    }

    public class JobRequestResponse
    {
        public int count { get; set; }
        public List<JobRequest> value { get; set; }
    }
}
