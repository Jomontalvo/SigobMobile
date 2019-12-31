namespace SigobMobile.Common.Models
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class EventParticipants
    {
        [JsonProperty("internos")]
        public string InternalContacts { get; set; }

        [JsonProperty("externos")]
        public string ExternalContacts { get; set; }

        [JsonProperty("contactosMovil")]
        public List<Participant> MobileContacts { get; set; }
    }
}
