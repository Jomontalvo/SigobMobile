namespace SigobMobile.Models
{
    using System;
    using Newtonsoft.Json;
    using Telerik.XamarinForms.Input;
    using Xamarin.Forms;

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
    /// Appointment item.
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
        public byte SecurityLevel { get; set; }

        [JsonProperty("tipo_instru")]
        public int ModuleType { get; set; }

        [JsonProperty("tentativa")]
        public bool IsTentative { get; set; }

        [JsonProperty("visible")]
        public byte IsVisible { get; set; }

    }

    /// <summary>
    /// Event object (type IAppointment)
    /// </summary>
    public class Event : IAppointment
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Title { get; set; }
        public Color Color { get; set; }
        public bool IsAllDay { get; set; }
        public string Detail { get; set; }
        public int ModuleType { get; set; }
        public string Owner { get; set; }
        public string Programmer { get; set; }
        public int TypeId { get; set; }
        public Color TypeColor { get; set; }
        public byte SecurityLevel { get; set; }
        public bool IsLocked { get; set; }
        public bool IsTentative { get; set; }
        public bool IsHighlighted { get; set; }
        public bool IsTask { get; set;  }
        public bool IsVisible { get; set; }
        public StatusAppointment Status { get; set; }
        public TitleFontAttribute TitleFont
        {
            get
            {
                TitleFontAttribute result = TitleFontAttribute.None;
                int fontFormat = Convert.ToByte(IsTentative) + Convert.ToByte(IsHighlighted);
                switch (fontFormat)
                {
                    case 1:
                        if (IsTentative) result = TitleFontAttribute.Bold;
                        if (IsHighlighted) result = TitleFontAttribute.Italic;
                        break;
                    case 2:
                        result = TitleFontAttribute.BoldAndItalic;
                        break;
                }
                return result;
            }
        }
    }
}
