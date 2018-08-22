using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PushoverClient
{
    /// <summary>
    /// Represents a response message from the Pushover API
    /// </summary>
    [DataContract]
    public class PushResponse : PushoverResponse
    {
        [IgnoreDataMember]
        public Limitations Limits { get; set; }

        public override string ToString()
        {
            return string.Format("{0} | Limits: {1}", base.ToString(), Limits);
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