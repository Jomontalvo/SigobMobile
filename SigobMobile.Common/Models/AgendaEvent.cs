namespace SigobMobile.Common.Models
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// Agenda Event Class
    /// </summary>

    public class AgendaEvent
    {
        public int Id { get; set; }

        [JsonProperty("titulo")]
        public string Title { get; set; }

        [JsonProperty("tipo_evento")]
        public int EventTypeId { get; set; }

        [JsonProperty("descripcion_tipo_evento")]
        public string EventTypeDescription { get; set; }

        [JsonProperty("ubicacion")]
        public string Location { get; set; }

        [JsonProperty("inicio")]
        public DateTimeOffset Start { get; set; }

        [JsonProperty("fin")]
        public DateTimeOffset End { get; set; }

        [JsonProperty("todo_el_dia")]
        public bool IsAllDay { get; set; }

        [JsonProperty("grado_reserva")]
        public short PrivacyLevel { get; set; }

        [JsonProperty("descripcion_grado_reserva")]
        public string PrivacyDescription { get; set; }

        [JsonProperty("resumen")]
        public string Summary { get; set; }

        [JsonProperty("tipo_instrumento")]
        public char InstrumentType { get; set; }

        [JsonProperty("progaramado_por")]
        public string ProgrammerOfficeId { get; set; }

        [JsonProperty("nombre_programador")]
        public string ProgrammerName { get; set; }

        [JsonProperty("agenda_de")]
        public string OwnerOfficeId { get; set; }

        [JsonProperty("nombre_calendario")]
        public string CalendarName { get; set; }

        [JsonProperty("comentario")]
        public string Comment { get; set; }

        [JsonProperty("pendiente")]
        public byte Pending { get; set; }

        [JsonProperty("revisado")]
        public bool IsReviewed { get; set; }

        [JsonProperty("estado_gestion")]
        public StatusAppointment Status { get; set; }

        [JsonProperty("aviso")]
        public short Alert { get; set; }

        [JsonProperty("tentativo")]
        public byte Tentative { get; set; }

        [JsonProperty("anotaciones")]
        public string Annotations { get; set; }

        [JsonProperty("programado_cg")]
        public byte ProgrammedMC { get; set; }

        [JsonProperty("participante")]
        public bool IsParticipant { get; set; }

        [JsonProperty("fecha_actualizacion")]
        public DateTimeOffset UpdateDate { get; set; }

        [JsonProperty("color_tipo_red")]
        public int ColorTypeRed { get; set; }

        [JsonProperty("color_tipo_green")]
        public int ColorTypeGreen { get; set; }

        [JsonProperty("color_tipo_blue")]
        public int ColorTypeBlue { get; set; }

        [JsonProperty("color_item_red")]
        public int ColorItemRed { get; set; }

        [JsonProperty("color_item_green")]
        public int ColorItemGreen { get; set; }

        [JsonProperty("color_item_blue")]
        public int ColorItemBlue { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:SigobMobile.Models.AgendaEvent"/> 
        /// is programmed from Management Center.
        /// </summary>
        /// <value><c>true</c> if is programmed from MC enter; otherwise, <c>false</c>.</value>
        public bool IsProgrammedFromMCenter => (ProgrammedMC == 1) ? true : false;
    }
}
