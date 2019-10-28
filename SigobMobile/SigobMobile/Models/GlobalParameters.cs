namespace SigobMobile.Models
{
    using System;
    using Newtonsoft.Json;

    #region Enum SIGOB Instruments Type
    public enum SigobInstrument
    {
        None = -1,        MIA = 0,        MiaAdministration = 1,        Messages = 2,        Directory = 3,        Agenda = 4,        PersonalAppointment = 5,        Correspondence = 6,        GovernmentActionProgram = 7,        GapTask = 8,        GapEvent = 9,        ManagementCenter = 10,        Assignment = 11,        ManagementCenterEvent = 12,        ManagementCenterRequest = 13,        Instruction = 14,        EventTask = 15,        RequestTask = 16,        InstructionTask = 17,        RegularStructuredProcedure = 18,        ControlVersion = 19,        Synchronizer = 20,        GapGroup = 21,        Workflow = 22,        Activity = 23,        GapCard = 24,        GapMacroEvent = 25,        Opportunity = 26,        Alert = 27,        Problem = 28,        GapGroupCard = 29,        GapGroupMacroEvent = 30,        GapGroupOpportunity = 31,        GapGroupAlert = 32,        GapGroupProblema = 33,        WorkflowTemplate = 34,        ActorsTopicsMonitoring = 35,        FactsMonitoring = 36,        NewsMonitoring = 37,        CommunicativeAction = 38,        CommunicativeActionTask = 39,        Task = 40,        Indicator = 41,        BillMonitoring = 42,        DocumentTraceRoute = 43,        ExternalDocumentRecord = 44,        DocumentQuickRecord = 45,        SIGOBITO = 46,        GoalsGroup = 47,        Goals = 48,        GoalsMilestone = 49,        CitizenWish = 50
    }
    #endregion

    public class GlobalParameters
    {
        /// <summary>
        /// Firebase Central URL API
        /// </summary>
        [JsonProperty("masterBackendUrl")]
        public Uri UrlMasterBackend { get; set; }
        /// <summary>
        /// Contact us url parameter
        /// </summary>
        [JsonProperty("masterBackendWebContactUs")]
        public Uri UrlWebContactUs { get; set; }
        /// <summary>
        /// Web content url parameter
        /// </summary>
        [JsonProperty("masterBackendWebContent")]
        public Uri UrlWebContent { get; set; }
        /// <summary>
        /// Help url parameter
        /// </summary>
        [JsonProperty("masterBackendWebHelp")]
        public Uri UrlWebHelp { get; set; }
        /// <summary>
        /// Terms and conditions url parameter
        /// </summary>
        [JsonProperty("masterBackendWebTerms")]
        public Uri UrlWebTerms { get; set; }
    }

    public class TSigTuple
    {
        [JsonProperty("codigo")]
        public long Id { get; set; }

        [JsonProperty("descripcion")]
        public string Description { get; set; }
    }

    public class TSigColorTuple : TSigTuple
    {
        [JsonProperty("color")]
        public string Color { get; set; }
    }

}
