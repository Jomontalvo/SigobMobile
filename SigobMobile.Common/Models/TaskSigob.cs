namespace SigobMobile.Common.Models
{
    using System;
    using Newtonsoft.Json;
    using Helpers;

    public enum TTaskDeletionError
    {
        UnauthorizedUser = -1,
        NoError = 0,
        HasDerivedTasks = 1,
        HasLog = 2,
        HasReport = 3,
        IsFinished = 4,
        HasDocuments = 5,
        UnknownError = 100
    }

    public enum TStatusTasK    {        Programmed = 0,        InManagement = 1,        Finished = 6,        PendingSend = 7,        FinishedIncomplete = 8,        Suspended = 9,        ToSendByEmail = 10,        ErrorSendEmail = 11,        Delayed = 12    }

    public enum TReportStatus
    {
        NotApply = 0,
        OnTime = 1,
        Overdue = 2,
        Available = 3
    }

    public enum TTrafficLightStatus : int
    {
        InProgress = 0,
        Overdue = 1,
        Completed = 2,
        All = 10,
        CloseToDeadline = 11

    }

    public enum TOriginTask    {        None = -1,        GovernmentActionProgram = 0,        ManagementCenterPrevious = 1,        ManagementCenterDuring = 2,        ManagementCenterPost = 3,        ManagementCenterRequest = 4,        Instruction = 5,        Assignment = 6,        PersonalAgenda = 7,        Task = 8,        CommunicativeAction = 9    }

    public enum TPeriodicity : byte    {        Undefined = 0,        Weekly = 1,        Biweekly = 2,        Monthly = 3,        Bimonthly = 4    }

    public enum TPriority    {        Minimum = 0,        Low = 1,        Middle = 2,        High = 3,        Maxim = 4    }

    public enum TModuleTaskOrigin    {
        /// <summary>        /// Agenda Personal => tipo_instru = 4        /// </summary>        PersonalAgenda = 0,
        /// <summary>        /// Programas Calendario => tipo_instru = 6         /// </summary>        Goals = 1,
        /// <summary>        /// Centro de Gestión => tipo_instru = 7        /// </summary>        ManagementCenter = 2,
        /// <summary>        /// Trámites Regulares => tipo_instru = 8        /// </summary>        Workflow = 3,
        /// <summary>        /// Acción comunicacional => Tipo_instru = Z        /// </summary>        CommunicativeAction = 7    }

    public enum TQueryOption : byte
    {
        PendingTasks = 0,
        OverdueTasks = 1,
        CompletedTasks = 2,
        NewAssignedTasks = 3,
        OngoingTaskSetByMe = 4,
        OngoingTasksThatMonitoring = 5,
        CompletedTasksSetByMe = 6,
        CompletedTasksThatMonitoring = 7,
        TaskCopies = 8,
        TaskMessages = 9,
        TasksOf = 10,
        TaskCloseToDeadline = 11
    }

    public enum TChange    {
        NotSending,
        Subject,
        Date,
        Details,
        Summary,
        Status,
        DocumentsOwner,
        RestrictionsOwner,
        AlertsOwner,
        DocumentsResponsible,
        RestrictionsResponsible,
        AlertsResponsible,
        Responsible,
        SendChangesByEmail,
        ErrorSendChangesByEmail,
        Type,
        Monitor,
        StartDate,
        Priority,
        SendResponsible,
        MonitorCanFinish,
        ReportFrequency,
        ReportReview,
        PlannedCost,
        RealCost,
        PercentageOfCompletion    }

    /// <summary>
    /// Audit Status model
    /// </summary>
    public enum TAuditStatus    {        NoRequested = 0,        ToElaborate = 1,        InElaboration = 2,        Finished = 3,        Reviewed = 4    }

    /// <summary>
    /// Audit opinion (result)
    /// </summary>
    public enum TAuditOpinion
    {
        NoOpinion = 0,
        Rejected = 1,
        WithObservations = 2,
        Approved = 3
    }

    /// <summary>
    /// Office model
    /// </summary>
    public class Office
    {
        [JsonProperty("codigo")]
        public string OfficeId { get; set; }

        [JsonProperty("nombre")]
        public string Name { get; set; }

        [JsonProperty("cargo")]
        public object Position { get; set; }

        [JsonProperty("area")]
        public object Area { get; set; }

        [JsonProperty("correoElectronico")]
        public object Email { get; set; }

        [JsonProperty("institucion")]
        public object Institution { get; set; }

        [JsonProperty("externo")]
        public bool IsExternal { get; set; }
    }

    /// <summary>
    /// Task Changes object model
    /// </summary>
    public class TaskChanges
    {
        [JsonProperty("valor")]
        public long Value { get; set; }

        [JsonProperty("vacio")]
        public bool IsEmpty { get; set; }
    }

    /// <summary>
    /// General Task Model 
    /// </summary>
    public class TaskSigob
    {
        [JsonProperty("actualizando")]
        public bool IsUpdating { get; set; }

        [JsonProperty("asunto")]
        public string Title { get; set; }

        [JsonProperty("cambios")]
        public long ChangesValue { get; set; }

        [JsonProperty("codigo")]
        public int Id { get; set; }

        [JsonProperty("codigoDespachoAuditor")]
        public string AuditorOfficeId { get; set; }

        [JsonProperty("codigoInstrumento")]
        public TModuleTaskOrigin ModuleId { get; set; }

        [JsonProperty("costoEjecutado")]
        public double CostExecuted { get; set; }

        [JsonProperty("costoProgramado")]
        public double CostPlanned { get; set; }

        [JsonProperty("programador")]
        public Office Programmer { get; set; }

        [JsonProperty("detalle")]
        public string Description { get; set; }

        [JsonProperty("eliminado")]
        public bool IsDeleted { get; set; }

        [JsonProperty("entidadPadre")]
        public ParentEntity ParentEntity { get; set; }

        [JsonProperty("errorEliminacion")]
        public int DeletionError { get; set; }

        [JsonProperty("estado")]
        public TStatusTasK Status { get; set; }

        [JsonProperty("estadoAuditoria")]
        public TAuditStatus AuditStatus { get; set; }
        
        [JsonProperty("estadoReporte")]
        public TReportStatus ReportStatus { get; set; }

        [JsonProperty("estadoSemaforo")]
        public TTrafficLightStatus TrafficLight { get; set; }

        [JsonProperty("fechaFinalizada")]
        public DateTime? EndDate { get; set; }

        [JsonProperty("fechaFinProgramada")]
        public DateTime ProgrammedEndDate { get; set; }

        [JsonProperty("fechaInicio")]
        public DateTime? StartDate { get; set; }

        [JsonProperty("fechaModificacionDetalle")]
        public DateTime? ModificationDetailDate { get; set; }

        [JsonProperty("fechaModificacionReporte")]
        public DateTime? ModificationReportDate { get; set; }

        [JsonProperty("fechaProximoReporte")]
        public DateTime? NextReportDate { get; set; }

        [JsonProperty("historicoDetalle")]
        public string HistoricalDetail { get; set; }

        [JsonProperty("historicoReporteEjecucion")]
        public string HistoricalReport { get; set; }

        [JsonProperty("insertado")]
        public bool IsInserted { get; set; }

        [JsonProperty("instrumento")]
        public SigobInstrument Instrument { get; set; }

        [JsonProperty("mensajeUltimoError")]
        public string LastErrorMessage { get; set; }

        [JsonProperty("modificaciones")]
        public TaskChanges Changes { get; set; }

        [JsonProperty("monitor")]
        public Office Monitor { get; set; }

        [JsonProperty("monitorPuedeFinalizar")]
        public bool MonitorCanFinishTask { get; set; }

        [JsonProperty("nuevo")]
        public bool IsNew { get; set; }

        [JsonProperty("origen")]
        public TOriginTask Source { get; set; }

        //[JsonProperty("padre")]
        //public TaskModel Parent { get; set; }

        [JsonProperty("periodicidad")]
        public TPeriodicity ReportFrequency { get; set; }

        [JsonProperty("plantilla")]
        public int Template { get; set; }

        [JsonProperty("porcentajeAvance")]
        public int Completion { get; set; }

        [JsonProperty("prioridad")]
        public TPriority Priority { get; set; }

        [JsonProperty("puedeCerrarReporte")]
        public bool CanCloseReport { get; set; }

        [JsonProperty("reiteraciones")]
        public int ReiterationsCount { get; set; }

        [JsonProperty("reporteEjecucion")]
        public string Report { get; set; }

        [JsonProperty("reporteRevisado")]
        public int RevisedReport { get; set; }

        [JsonProperty("responsable")]
        public Office Responsible { get; set; }

        [JsonProperty("revisado")]
        public bool IsTaskReviewed { get; set; }

        [JsonProperty("soloLectura")]
        public bool ReadOnly { get; set; }

        [JsonProperty("tieneAuditoria")]
        public bool HasAudit { get; set; }

        [JsonProperty("tieneMensaje")]
        public bool HasMessage { get; set; }

        [JsonProperty("tipo")]
        public TSigColorTuple Type { get; set; }

        [JsonProperty("veTareaPadre")]
        public bool CanViewTaskParent { get; set; }

        [JsonProperty("tipoColorRed")]
        public byte TipoColorRed { get; set; }

        [JsonProperty("tipoColorGreen")]
        public byte TipoColorGreen { get; set; }

        [JsonProperty("tipoColorBlue")]
        public byte TipoColorBlue { get; set; }

        [JsonProperty("retraso")]
        public int Delay { get; set; }
    }

    /// <summary>
    /// Parent Entity Model
    /// </summary>
    public class ParentEntity
    {
        [JsonProperty("codigo")]
        public int Id { get; set; }

        [JsonProperty("codigoInstrumento")]
        public int InstrumentId { get; set; }

        [JsonProperty("descripcion")]
        public string Description { get; set; }

        [JsonProperty("fecha")]
        public DateTime Date { get; set; }

        [JsonProperty("instrumento")]
        public SigobInstrument Instrument { get; set; }
    }
}
