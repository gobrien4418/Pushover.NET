using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PushoverClient
{
    /// <summary>
    /// Represents a response message from the Pushover API
    /// </summary>
    [DataContract]
    public class PushoverResponse
    {
        [DataMember(Name = "status")]
        public int Status { get; set; }

        [DataMember(Name = "request")]
        public string Request { get; set; }

        [DataMember(Name = "errors")]
        public List<string> Errors { get; set; }

        [IgnoreDataMember]
        public bool Succeeded => Status == 1;

        public override string ToString()
        {
            return string.Format("Status: {0} | Request: {1} | Errors: {2} | ", Status, Request, string.Join(",", Errors));
        }
    }
}