using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PushoverClient
{
    /// <summary>
    /// Represents a response message from the Pushover API
    /// </summary>
    [DataContract]
    public class PushResponse
    {
        [DataMember(Name = "status")]
        public int Status { get; set; }

        [DataMember(Name = "request")]
        public string Request { get; set; }

        [DataMember(Name = "errors")]
        public List<string> Errors { get; set; }

        [IgnoreDataMember]
        public Limitations Limits { get; set; }

        [IgnoreDataMember]
        public bool Succeeded => Status == 1;

        public override string ToString()
        {
            return string.Format("Status: {0} | Request: {1} | Errors: {2} | Limits: {3}", Status, Request, string.Join(",", Errors), Limits);
        }
    }

    public class Limitations
    {
        public int Limit { get; set; }
        public int Remaining { get; set; }
        public string Reset { get; set; }

        public override string ToString()
        {
            return string.Format("[Limit: {0} | Remaining: {1} | Reset: {2}]", Limit, Remaining, Reset);
        }
    }
}