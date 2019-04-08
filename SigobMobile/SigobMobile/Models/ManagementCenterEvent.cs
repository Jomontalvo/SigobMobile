namespace SigobMobile.Models
{
    using System;
    using Newtonsoft.Json;
    using Helpers;
    using Xamarin.Forms;

    #region Enum Event Attributes
    public enum EventAttribute : byte
    {
        Create,
        CreateOwner,
        ReadOnly
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

    #region Enum SIGOB Instruments Type
    public enum SgobInstruments : byte
    {
        PersonalAppointment = 5,
        Instruction = 7,
        Assignment = 11,
        ManagementCenterEvent = 12,
        Task = 40
    }
    #endregion

    /// <summary>
    /// Management Center Event.
    /// </summary>
    public class ManagementCenterEvent
    {

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
        public byte PrivacyLevel { get; set; }

        [JsonProperty("descripcion_grado_reserva")]
        public string PrivacyDescription { get; set; }

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

        public string StatusDescription
        {
            get
            {
                string status_ = string.Empty;
                switch (Status)
                {
                    case StatusAppointment.InManagement:
                        status_ = Languages.InManagementStatus;
                        break;
                    case StatusAppointment.Suspended:
                        status_ = Languages.SuspendedStatus;
                        break;
                    case StatusAppointment.Finished:
                        status_ = Languages.CompletedStatus;
                        break;
                    default:
                        status_ = Languages.InManagementStatus;
                        break;
                }
                return status_;
            }
        }
    }

    public class AgendaEvent
    {
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
        private int ColorTypeRed { get; set; }

        [JsonProperty("color_tipo_green")]
        private int ColorTypeGreen { get; set; }

        [JsonProperty("color_tipo_blue")]
        private int ColorTypeBlue { get; set; }

        [JsonProperty("color_item_red")]
        private int ColorItemRed { get; set; }

        [JsonProperty("color_item_green")]
        private int ColorItemGreen { get; set; }

        [JsonProperty("color_item_blue")]
        private int ColorItemBlue { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:SigobMobile.Models.AgendaEvent"/> 
        /// is programmed from Management Center.
        /// </summary>
        /// <value><c>true</c> if is programmed from MC enter; otherwise, <c>false</c>.</value>
        public bool IsProgrammedFromMCenter => (ProgrammedMC == 1) ? true : false;
        /// <summary>
        /// Gets the color of personal event
        /// </summary>
        /// <value>The color.</value>
        public Color CalendarColor => Color.FromRgb(ColorItemRed, ColorItemGreen, ColorItemBlue);
        /// <summary>
        /// Gets the color of the type event.
        /// </summary>
        /// <value>The color of the type.</value>
        public Color TypeColor => Color.FromRgb(ColorTypeRed, ColorTypeGreen, ColorTypeBlue);
    }

    public class ManagementCenterNewItem
    {
        [JsonProperty("tipoInstrumento")]
        public long IntrumentType { get; set; }

        [JsonProperty("nombreItemProgramar")]
        public string NewItemName { get; set; }
    }
}