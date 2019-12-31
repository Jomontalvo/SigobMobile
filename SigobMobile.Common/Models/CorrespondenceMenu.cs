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
    }
}
