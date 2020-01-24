namespace SigobMobile.Common.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class VisibleDocumentTraceRoute
    {
        [JsonProperty("copy")]
        public CopyTraceRoute Copy { get; set; }

        [JsonProperty("traceroute")]
        public List<DocumentTraceRoute> TraceRoutes { get; set; }
    }
}
