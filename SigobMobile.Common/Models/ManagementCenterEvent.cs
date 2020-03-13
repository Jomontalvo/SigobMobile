namespace SigobMobile.Common.Models
{
    using System;
    using System.Drawing;
    using Newtonsoft.Json;

    #region Enum using in Event

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

    public enum ConfirmAttendance : byte
    {
        Accept = 0,
        Tentative = 1,
        Decline = 2
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

    public enum EventTaskMoment : byte
    {
        Previous = 0,
        Support = 1,
        Later = 2,
        All = 3
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
        public short EventTypeId { get; set; }

        [JsonProperty("descripcion_tipo_evento")]
        public string EventTypeDescription { get; set; }

        //[JsonProperty("colorTipoEvento")]
        //public Color EventTypeColor { get; set; }

        [JsonProperty("inicio")]
        public DateTime Start { get; set; }

        [JsonProperty("fin")]
        public DateTime End { get; set; }

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

        [JsonProperty("tipoInstrumento")]
        public char ModuleType { get; set; }

        [JsonProperty("fechaActualizacion")]
        public DateTime? LastUpdate { get; set; }

        [JsonProperty("comentario")]
        public string GuestComment { get; set; }

        [JsonProperty("grado_reserva")]
        public SecurityLevelEvent PrivacyLevel { get; set; }

        [JsonProperty("tentativo")]
        public bool Tentative { get; set; }

        [JsonProperty("estado_gestion")]
        public StatusAppointment Status { get; set; }

        [JsonProperty("notificacion")]
        public bool Notification { get; set; }

        [JsonProperty("pendiente")]
        public byte Pending { get; set; }
        [JsonProperty("revisado")]        public bool Reviewed { get; set; }

        [JsonProperty("aviso")]
        public short Alert { get; set; }

        [JsonProperty("tamano_ficha_ejecutiva")]
        public int AbstractDocSize { get; set; }

        [JsonProperty("tamano_acta")]
        public int RecordDocSize { get; set; }

        [JsonProperty("programado_cg")]
        public bool IsProgrammedFromMCenter { get; set; }

        [JsonProperty("participa")]
        public bool IsParticipant { get; set; }

        [JsonProperty("nombre_programador")]
        public string ProgrammerName { get; set; }

        [JsonProperty("anotaciones")]
        public string Annotations { get; set; }

        [JsonProperty("descripcionPadre")]
        public string ParentDescription { get; set; }

        [JsonProperty("asistencia")]
        public ConfirmAttendance EventAttendance { get; set; }

        [JsonProperty("confirmaAsistenciaPropia")]
        public bool CanConfirmOwnerAttendance { get; set; }

        [JsonProperty("confirmaAsistenciaTitularCg")]
        public bool CanConfirmChiefAttendance { get; set; }

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

        [JsonProperty("tituloTareasPrevias")]
        public string PreviousTaskTitle { get; set; }

        [JsonProperty("tituloTareasLogisticas")]
        public string SupportTaskTitle { get; set; }

        [JsonProperty("tituloTareasSeguimiento")]
        public string CommitmentTaskTitle { get; set; }

    }
}