namespace SigobMobile.Common.Models
{
    using Newtonsoft.Json;

    public enum Attribute : byte
    {
        Program = 1,
        Read = 2,
        Modify = 3,
        ModifyDocuments = 4
    }

    public class Calendar
    {
        [JsonProperty("codigo_agenda")]
        public string OfficeId { get; set; }

        [JsonProperty("nombre_agenda")]
        public string AgendaName { get; set; }

        [JsonProperty("agenda_cg")]
        public int ManagementCenterId { get; set; }

        [JsonProperty("permiso")]
        public Attribute Permission { get; set; }

        [JsonProperty("propietario")]
        public bool IsOwner { get; set; }

        [JsonProperty("color_numero")]
        public int NumberColor { get; set; }

        [JsonProperty("color_red")]
        public byte RedColor { get; set; }

        [JsonProperty("color_green")]
        public byte GreenColor { get; set; }

        [JsonProperty("color_blue")]
        public byte BlueColor { get; set; }

        [JsonProperty("visible")]
        public bool IsVisible { get; set; }
        
        public string IconType => (this.ManagementCenterId != 0) ? "cal_management_center" : "cal_personal_agenda";

    }
}
