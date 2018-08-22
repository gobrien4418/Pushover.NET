using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PushoverClient
{
    /// <summary>
    /// Represents a response message from the Pushover License API
    /// 
    /// {"status":1,"credits":5,"request":"4f2a071d-fc2c-4d94-9cf2-68c069a599b4"}
    /// </summary>
    [DataContract]
    public class LicenseResponse : PushoverResponse
    {
        [DataMember(Name = "credits")]
        public int Credits { get; set; }

        public override string ToString()
        {
            return string.Format("{0} Credits: {1} ", base.ToString(), Credits);
        }
    }
}