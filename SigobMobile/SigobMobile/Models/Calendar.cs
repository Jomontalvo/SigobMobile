namespace SigobMobile.Models
{
    using Newtonsoft.Json;
    using Xamarin.Forms;

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

        /// <summary>
        /// Gets the color of the calendar.
        /// </summary>
        /// <value>The color of the calendar.</value>
        public Color CalendarColor
        {
            get { return Color.FromRgb(RedColor, GreenColor, BlueColor); } 
        }
        /// <summary>
        /// The is bold font attribute.
        /// </summary>
        public FontAttributes IsBoldFontAttribute
        {
            get { return (ManagementCenterId > 0) ? FontAttributes.Bold : FontAttributes.None; } 
        }
    }
}
