namespace SigobMobile.Common.Models
{
    using Newtonsoft.Json;

    public class Sender
    {
        [JsonProperty("docID")]
        public string IdNumber { get; set; }

        [JsonProperty("nombre")]
        public string FullName { get; set; }

        [JsonProperty("cargo")]
        public string Position { get; set; }

        [JsonProperty("institucion")]
        public string Institution { get; set; }

        [JsonProperty("pais")]
        public string Country { get; set; }

        [JsonProperty("provincia")]
        public string Province { get; set; }

        [JsonProperty("ciudad")]
        public string City { get; set; }

        [JsonProperty("calle")]
        public string StreetAddress { get; set; }

        [JsonProperty("telefono")]
        public string PhoneNumber { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("area")]
        public string Area { get; set; }

        [JsonProperty("funcionario")]
        public bool IsOfficial { get; set; }
    }
}
