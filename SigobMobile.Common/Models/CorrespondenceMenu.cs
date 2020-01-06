using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SigobMobile.Common.Models
{
    public class CorrespondenceMenu
    {
        [JsonProperty("nombreBandejas")]
        public string TrayName { get; set; }

        [JsonProperty("bandejas")]
        public List<DocumentTray> DocumentTrays { get; set; }

        [JsonProperty("nombreClasificadores")]
        public string TagsName { get; set; }

        [JsonProperty("clasificadores")]
        public List<DocumentTagTray> DocumentTagTrays { get; set; }

        [JsonProperty("filtrosFecha")]
        public List<TimeFilterTuple> TimeFilters { get; set; }
    }

    public class TimeFilterTuple
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("filtroFecha")]
        public string TimeFrameName { get; set; }

        public override string ToString()
        {
            return TimeFrameName;
        }
    }
}
