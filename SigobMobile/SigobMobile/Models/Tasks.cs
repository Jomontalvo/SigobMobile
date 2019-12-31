namespace SigobMobile.Models
{
    using Newtonsoft.Json;
    using Common.Helpers;
    using Common.Models;
    using Helpers;

    /// <summary>
    /// Task List with Statistics
    /// </summary>
    public class Tasks
    {
        [JsonProperty("listaTareas")]
        public TaskSigob[] TaskList { get; set; }

        [JsonProperty("estadistica")]
        public TaskCategoricalData[] TaskStatistics { get; set; }

        [JsonProperty("tituloGrafica")]
        public string GraphTitleNativeLanguage { get; set; }

        [JsonProperty("queryId")]
        public TQueryOption QueryId { get; set; }

        [JsonProperty("despacho")]
        public string OfficeCode { get; set; }

        [JsonProperty("nombreFuncionario")]
        public string OfficialName { get; set; }

        public string GraphTitle
        {
            get
            {
                string title = QueryId switch
                {
                    TQueryOption.CompletedTasks => Languages.CompletedTasks,
                    TQueryOption.CompletedTasksSetByMe => Languages.CompletedTasksSetByMe,
                    TQueryOption.CompletedTasksThatMonitoring => Languages.CompletedTasksThatMonitoring,
                    TQueryOption.NewAssignedTasks => Languages.NewAssignedTasks,
                    TQueryOption.OngoingTaskSetByMe => Languages.OngoingTaskSetByMe,
                    TQueryOption.OngoingTasksThatMonitoring => Languages.OngoingTasksThatMonitoring,
                    TQueryOption.OverdueTasks => Languages.OverdueTasks,
                    TQueryOption.PendingTasks => Languages.PendingTasks,
                    TQueryOption.TaskCloseToDeadline => Languages.TaskCloseToDeadline,
                    TQueryOption.TaskCopies => Languages.TaskCopies,
                    TQueryOption.TaskMessages => Languages.TaskMessages,
                    TQueryOption.TasksOf => (OfficeCode == Settings.OfficeCode) ? Languages.MyTasks : Languages.TasksOf,
                    _ => Languages.MyTasks,
                };
                return title;
            }
        }
    }
}
