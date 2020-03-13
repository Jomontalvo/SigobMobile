namespace SigobMobile.Common.Models
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// Status appointment.
    /// </summary>
    public enum StatusAppointment : byte
    {
        InManagement = 0,
        Finished = 1,
        Suspended = 2
    }

    public enum TitleFontAttribute: byte
    {
        None = 0,
        Bold = 1,
        Italic = 2,
        BoldAndItalic = 3
    }

    /// <summary>
    /// Appointment item of Calendar List
    /// </summary>
    public class AppointmentItem
    {
        [JsonProperty("codigo")]
        public int Id { get; set; }

        [JsonProperty("es_evento_tarea")]
        public bool IsTask { get; set; }

        [JsonProperty("agenda_de")]
        public string AgendaOwner { get; set; }

        [JsonProperty("despacho_programador")]
        public string ProgrammerAgenda { get; set; }

        [JsonProperty("inicio")]
        public DateTimeOffset Start { get; set; }

        [JsonProperty("fin")]
        public DateTimeOffset End { get; set; }

        [JsonProperty("descripcion")]
        public string Description { get; set; }

        [JsonProperty("lugar")]
        public string Place { get; set; }

        [JsonProperty("estado_gestion")]
        public StatusAppointment Status { get; set; }

        [JsonProperty("color_item_red")]
        public short RedColorItem { get; set; }

        [JsonProperty("color_item_green")]
        public short GreenColorItem { get; set; }

        [JsonProperty("color_item_blue")]
        public short BlueColorItem { get; set; }

        [JsonProperty("color_tipo_red")]
        public short RedColorType { get; set; }

        [JsonProperty("color_tipo_green")]
        public short GreenColorType { get; set; }

        [JsonProperty("color_tipo_blue")]
        public short BlueColorType { get; set; }

        [JsonProperty("codigo_tipo")]
        public int TypeId { get; set; }

        [JsonProperty("destacado")]
        public bool IsHighlighted { get; set; }

        [JsonProperty("grado_reserva")]
        public SecurityLevelEvent SecurityLevel { get; set; }

        [JsonProperty("tipo_instru")]
        public char ModuleType { get; set; }

        [JsonProperty("tentativa")]
        public bool IsTentative { get; set; }

        [JsonProperty("visible")]
        public byte IsVisible { get; set; }

        [JsonProperty("iniciales")]
        public string OwnerInitials { get; set; }
    }
}
