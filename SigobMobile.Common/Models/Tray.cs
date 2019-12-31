
namespace SigobMobile.Common.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class Tray
    {
        [JsonProperty("totalFilas")]
        public int Rows { get; set; }

        [JsonProperty("itemsCorrespondencia")]
        public List<Document> Documents { get; set; }

    }
}
