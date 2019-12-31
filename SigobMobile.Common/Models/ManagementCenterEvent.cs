namespace SigobMobile.Common.Models
{
    using System;
    using Newtonsoft.Json;
    using SigobMobile.Common.Models;

    #region Enum Event Attributes
    public enum EventAttribute : byte
    {
        Create,
        CreateOwner,
        ReadOnly
    }

    public enum SecurityLevelEvent : byte
    {
        /// <summary>
        /// Public Event
        /// </summary>
        Public = 0,
        /// <summary>
        /// Evento interno
        /// </summary>
        Low = 1,
        /// <summary>
        /// Evento semi privado
        /// </summary>
        Medium = 2,
        /// <summary>
        /// Evento privado
        /// </summary>
        High = 3,
        /// <summary>
        /// Evento confidencial
        /// </summary>
        Private = 4
    }

    public enum AgendaAttribute : byte
    {
        None,
        Program,
        ReadOnly,
        Maintain
    }

    public enum EventViewAuthorization : byte
    {
        OnlyParticipate,
        OnlyWithTasks,
        BySecuritylevel
    }

    public enum EventTentativeAttribute : byte
    {
        Register,
        ReadOnly,
        NotAuthorized
    }

    public enum EventAbstractOrRecordAttribute : byte
    {
        CreateAndModify,
        ReadOnly,
        NotAuthorized
    }

    public enum EventDocumentsAttribute : byte
    {
        NotAuthorized,
        ReadOnly,
        InsertAndModify
    }

    public enum EventParticipantsAttribute : byte
    {
        NotAuthorized,
        ReadOnly,
        InsertAndModify
    }

    public enum EventAnnotationAttribute : byte
    {
        NotAuthorized,
        ReadOnly,
        FullControl
    }

    public enum EventTasksAttribute: byte
    {
        ProgramAndViewAll,
        ProgramAndViewOnlyOwns,
        ViewAll,
        ViewOnlyAssigned,
        NotAuthorized
    }
    #endregion

    #region Enum Event Properties
    public enum EventSecurytyLevel : byte
    {
        Minimum,
        Low,
        Medium,
        High,
        Maximum
    }

    #endregion

    /// <summary>
    /// Management Center Event.
    /// </summary>
    public class ManagementCenterEvent
    {
        public int Id { get; set; }

        [JsonProperty("codigo_centro_gestion")]
        public int ManagementCenterId { get; set; }

        [JsonProperty("titulo")]
        public string Title { get; set; }

        [JsonProperty("codigo_tipo_evento")]
        public int EventTypeId { get; set; }

        [JsonProperty("descripcion_tipo_evento")]
        public string EventTypeDescription { get; set; }

        [JsonProperty("inicio")]
        public DateTimeOffset Start { get; set; }

        [JsonProperty("fin")]
        public DateTimeOffset End { get; set; }

        [JsonProperty("ubicacion")]
        public string Location { get; set; }

        [JsonProperty("resumen")]
        public string Summary { get; set; }

        [JsonProperty("progaramado_por")]
        public string ProgrammerOfficeId { get; set; }

        [JsonProperty("agenda_de")]
        public string OwnerOfficeId { get; set; }

        [JsonProperty("nombre_calendario")]
        public string CalendarName { get; set; }

        [JsonProperty("grado_reserva")]
        public SecurityLevelEvent PrivacyLevel { get; set; }

        [JsonProperty("tentativo")]
        public bool Tentative { get; set; }

        [JsonProperty("estado_gestion")]
        public StatusAppointment Status { get; set; }

        [JsonProperty("notificacion")]
        public bool Notification { get; set; }

        [JsonProperty("aviso")]
        public short Alert { get; set; }

        [JsonProperty("tamano_ficha_ejecutiva")]
        public long AbstractDocSize { get; set; }

        [JsonProperty("tamano_acta")]
        public long RecordDocSize { get; set; }

        [JsonProperty("terminado")]
        public bool Finished { get; set; }

        [JsonProperty("programado_cg")]
        public bool IsProgrammedFromMCenter { get; set; }

        [JsonProperty("participa")]
        public bool IsParticipant { get; set; }

        [JsonProperty("nombre_programador")]
        public string ProgrammerName { get; set; }

        [JsonProperty("anotaciones")]
        public string Annotations { get; set; }

        [JsonProperty("puede_modificar")]
        public EventAttribute AttributeOnEvents { get; set; }

        [JsonProperty("modifica_participantes")]
        public EventParticipantsAttribute ModifyParticipants { get; set; }

        [JsonProperty("ver_ficha_ejecutiva")]
        public EventAbstractOrRecordAttribute AbstractAttribute { get; set; }

        [JsonProperty("ver_actas")]
        public EventAbstractOrRecordAttribute RecordAttribute { get; set; }

        [JsonProperty("ver_documentos")]
        public EventDocumentsAttribute DocumentsAttribute { get; set; }

        [JsonProperty("tarea_previa")]
        public EventTasksAttribute PreviousTask { get; set; }

        [JsonProperty("tarea_apoyo")]
        public EventTasksAttribute SupportTasks { get; set; }

        [JsonProperty("tarea_posterior")]
        public EventTasksAttribute LaterTasks { get; set; }

        [JsonProperty("totalTareas")]
        public int TaskAccount { get; set; }

        [JsonProperty("totalParticipantes")]
        public int ParticipantsAccount { get; set; }

        [JsonProperty("totalDocumentos")]
        public int DocumentsAccount { get; set; }

        [JsonProperty("nombre_evento_singular")]
        public string SingularEventName { get; set; }

        [JsonProperty("nombre_evento_plural")]
        public string PluralEventName { get; set; }

        [JsonProperty("tituloActa")]
        public string RecordTitle { get; set; }

        [JsonProperty("tituloCarpetasTrabajo")]
        public string DocumentsTitle { get; set; }
    }
}