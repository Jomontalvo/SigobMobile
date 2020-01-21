﻿namespace SigobMobile.Common.Models
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

    public enum TStatusTasK

    public enum TTrafficLightStatus : long
    {
        InProgress = 0,
        Overdue = 1,
        Completed = 2,
        CloseToDeadline = 11

    }

    public enum TOriginTask

    public enum TPeriodicity

    public enum TPriority

    public enum TIconTaskOrigin
        /// <summary>
        /// <summary>
        /// <summary>
        /// <summary>
        /// <summary>

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


    public class TaskSigob
    {
        [JsonProperty("codigo")]
        public long Id { get; set; }

        [JsonProperty("asunto")]
        public string Title { get; set; }

        [JsonProperty("cambios")]
        public long Changes { get; set; }

        [JsonProperty("codigoInstrumento")]
        public int ModuleId { get; set; }

        [JsonProperty("despachoProgramador")]
        public string ProgrammerOfficeId { get; set; }

        [JsonProperty("nombreProgramador")]
        public string ProgrammerName { get; set; }

        [JsonProperty("detalle")]
        public string Description { get; set; }

        [JsonProperty("eliminado")]
        public bool Deleted { get; set; }

        [JsonProperty("entidadPadre")]
        public ParentEntity ParentEntity { get; set; }

        [JsonProperty("errorEliminacion")]
        public TTaskDeletionError DeletionError { get; set; }

        [JsonProperty("estado")]
        public TStatusTasK Status { get; set; }

        [JsonProperty("estadoReporte")]
        public int ReportStatus { get; set; }

        [JsonProperty("estadoDescripcion")]
        public string StatusDescrition { get; set; }

        [JsonProperty("fechaFinProgramada")]
        public DateTimeOffset EndProgrammedDate { get; set; }

        [JsonProperty("fechaFinalizada")]
        public DateTimeOffset EndDate { get; set; }

        [JsonProperty("fechaInicio")]
        public DateTimeOffset StartDate { get; set; }

        [JsonProperty("fechaModificacionDetalle")]
        public DateTimeOffset ModificationDatailDate { get; set; }

        [JsonProperty("fechaModificacionReporte")]
        public DateTimeOffset ModificationReportDate { get; set; }

        [JsonProperty("fechaProximoReporte")]
        public DateTimeOffset NextReportDate { get; set; }

        [JsonProperty("historicoDetalle")]
        public string HistoricalDetail { get; set; }

        [JsonProperty("historicoReporteEjecucion")]
        public string HistoriacalReport { get; set; }

        [JsonProperty("insertado")]
        public bool Inserted { get; set; }

        [JsonProperty("instrumento")]
        public SigobInstrument Instrument { get; set; }

        [JsonProperty("mensajeUltimoError")]
        public string LastErrorMessage { get; set; }

        [JsonProperty("modificaciones")]
        public long? Modifications { get; set; }

        [JsonProperty("despachoMonitor")]
        public string MonitorOfficeId { get; set; }

        [JsonProperty("nombreMonitor")]
        public string MonitorName { get; set; }

        [JsonProperty("monitorPuedeFinalizar")]
        public bool MonitorCanFinishTask { get; set; }

        [JsonProperty("nuevo")]
        public bool IsNew { get; set; }

        [JsonProperty("origen")]
        public TOriginTask Origin { get; set; }

        [JsonProperty("padre")]
        public object Parent { get; set; }

        [JsonProperty("periodicidad")]
        public TPeriodicity ReportFrequency { get; set; }

        [JsonProperty("periodicidadDescripcion")]
        public string FrequencyDescription { get; set; }

        [JsonProperty("plantilla")]
        public int Template { get; set; }

        [JsonProperty("prioridad")]
        public TPriority Priority { get; set; }

        [JsonProperty("puedeCerrarReporte")]
        public bool CanCloseReport { get; set; }

        [JsonProperty("reporteEjecucion")]
        public string Report { get; set; }

        [JsonProperty("reporteRevisado")]
        public int RevisedReport { get; set; }

        [JsonProperty("despachoResponsable")]
        public string ResponsibleOfficeId { get; set; }

        [JsonProperty("nombreResponsable")]
        public string ResponsibleName { get; set; }

        [JsonProperty("emailResponsable")]
        public string ResponsibleEmail { get; set; }

        [JsonProperty("responsableExterno")]
        public bool IsExternalResponsible { get; set; }

        [JsonProperty("soloLectura")]
        public bool ReadOnly { get; set; }

        [JsonProperty("tipo")]
        public TSigColorTuple Type { get; set; }

        [JsonProperty("tipo_Color_Red")]
        public int TypeRed { get; set; }

        [JsonProperty("tipo_Color_Green")]
        public long TypeGreen { get; set; }

        [JsonProperty("tipo_Color_Blue")]
        public long TypeBlue { get; set; }

        [JsonProperty("tipoInstrumentoOrigen")]
        public TIconTaskOrigin IconTaskOrigin { get; set; }

        [JsonProperty("veTareaPadre")]
        public bool IsParentVisible { get; set; }

        [JsonProperty("retraso")]
        public int DaysLate { get; set; }

        [JsonProperty("tieneMensaje")]
        public bool HasMessage { get; set; }

        public string InitialsOfResponsible => RegexUtilities.ExtractInitialsFromName(ResponsibleName);
    }

    public class ParentEntity
    {
        [JsonProperty("codigo")]
        public long Id { get; set; }

        [JsonProperty("codigoInstrumento")]
        public long CodigoInstrumento { get; set; }

        [JsonProperty("descripcion")]
        public string Description { get; set; }

        [JsonProperty("fecha")]
        public DateTimeOffset Date { get; set; }

        [JsonProperty("instrumento")]
        public long Instrumento { get; set; }
    }
}