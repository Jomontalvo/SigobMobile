namespace SigobMobile.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public enum Gender : byte
    {
        Undefined = 0,
        Male = 1,
        Female = 2
    }

    /// <summary>
    /// Participant model
    /// </summary>
    public class Participant
    {
        [JsonProperty("codigo_despacho")]
        public string OfficeId { get; set; }

        [JsonProperty("nombre_funcionario")]
        public string FullName { get; set; }

        [JsonProperty("cargo_funcionario")]
        public string Position { get; set; }

        [JsonProperty("area_funcionario")]
        public string Area { get; set; }

        [JsonProperty("codigo_persona")]
        public int PersonId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("telefono")]
        public string PhoneNumber { get; set; }

        [JsonProperty("sexo")]
        public Gender Gender { get; set; }
    }

    /// <summary>
    /// Event participants model
    /// </summary>
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
