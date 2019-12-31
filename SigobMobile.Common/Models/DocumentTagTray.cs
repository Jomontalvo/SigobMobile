namespace SigobMobile.Common.Models
{
    using Newtonsoft.Json;

    public class DocumentTagTray
    {
        [JsonProperty("clave")]
        public string Key { get; set; }

        [JsonProperty("clavePadre")]
        public string ParentKey { get; set; }

        [JsonProperty("nombre")]
        public string Name { get; set; }

        [JsonProperty("nivel")]
        public long Level { get; set; }

        [JsonProperty("cantidadItems")]
        public int ItemCount { get; set; }
    }
}
