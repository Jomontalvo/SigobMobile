namespace SigobMobile.Common.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// User basic profile model
    /// </summary>
    public class UserBasicProfile
    {
        [JsonProperty("despacho")]
        public string OfficeId { get; set; }

        [JsonProperty("nombres")]
        public string FirstName { get; set; }

        [JsonProperty("apellidos")]
        public string LastName { get; set; }

        [JsonProperty("institucion")]
        public string Institution { get; set; }

        [JsonProperty("cargo")]
        public string Position { get; set; }

        [JsonProperty("area")]
        public string Area { get; set; }

        [JsonProperty("telefono")]
        public string Phone { get; set; }

        [JsonProperty("celular")]
        public string CellPhone { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("sexo")]
        public byte? Gender { get; set; }

        [JsonProperty("imagenUsuario")]
        public string UserImage { get; set; }
    }
}
