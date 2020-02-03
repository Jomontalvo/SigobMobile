namespace SigobMobile.Common.Models
{
    using System;
    using Newtonsoft.Json;

    public enum DocumentType
    {
        None = 0,
        Word = 1,
        Excel = 2,
        PowerPoint = 3,
        TIFF = 4,
        Writer = 5,
        Calc = 6,
        Impress = 7,
        JPG	= 8,
        WMV	= 9,
        WMA = 10,
        PDF	= 11
    }

    public enum DocumentSource
    {
        None = 0,
        CalendarProgram = 1,
        CalendarProgramEvent = 2,
        GoalTask	= 3,
        MgCenterEvent = 4,
        MgCenterRequest	= 5,
        MgCenterInstruction = 6,
        MgCenterEventTask = 7,
        MgCenterRequestTask = 8,
        MgCenterInstructionTask = 9,
        Correspondence	= 10,
        InformationAnalysisModule	= 11,
        ProcessActivityTemplate = 12,
        PrecessActivity = 13,
        ProcessCase	= 14,
        CorrespondenceTraceRoute = 15,
        AgendaTask	= 16,
        MgCenterDirectTask = 17,
        MonitoringNews = 18,
        MonitorigFact = 19,
        CummunicationalTask = 20,
        LawProyectMonitoring = 21,
        Goal = 22,
        IntermediateGoal = 23,
        MacroGoal = 24
    }

    public class Attachment
    {
        [JsonProperty("codigoDocumento")]
        public long Id { get; set; }

        [JsonProperty("despachoElaborador")]
        public string OwnerCode { get; set; }

        [JsonProperty("elaborador")]
        public string OwnerFullName { get; set; }

        [JsonProperty("nombre")]
        public string AttachmentName { get; set; }

        [JsonProperty("tipo")]
        public DocumentType FileType { get; set; }

        [JsonProperty("adjunto")]
        public bool Attached { get; set; }

        [JsonProperty("fecha")]
        public DateTime Date { get; set; }

        [JsonProperty("fechaUltimaModificacion")]
        public DateTime LastModified { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("largo")]
        public int Lenght { get; set; }

        [JsonProperty("password")]
        public object Password { get; set; }

        [JsonProperty("permiso")]
        public byte Attribute { get; set; }

        [JsonProperty("codigoCarpeta")]
        public int FolderId { get; set; }

        [JsonProperty("descripcionCarpeta")]
        public string FolderName { get; set; }

        [JsonProperty("propietario")]
        public bool IsFolderOwner { get; set; }

        [JsonProperty("permiteAdjuntar")]
        public bool AllowAttach { get; set; }

        [JsonProperty("firmado")]
        public bool IsSigned { get; set; }

        [JsonProperty("firmadoPorMi")]
        public bool IsSignedByMe { get; set; }

        public string ImageType => $"ic_doc_type_{FileType}".ToLower();
    }
}
