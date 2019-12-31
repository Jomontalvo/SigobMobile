namespace SigobMobile.Models
{
    using Common.Models;
    using Helpers;
    using Newtonsoft.Json;

    public class ApplicationMenuItem : Common.Models.ApplicationMenuItem
    {
        [JsonProperty(PropertyName = "nombre_aplicacion")]
        public string ApplicationName
        {
            get
            {
                string appName = TypeApplication switch
                {
                    TypeApplication.ManagementCenter => Languages.ManagementCenterAppName,
                    TypeApplication.Tasks => Languages.TaskManagementAppName,
                    TypeApplication.Goals => Languages.InstitutionalGoalsAppName,
                    TypeApplication.Correspondence => Languages.CorrespondenceAppName,
                    TypeApplication.WorkFlows => Languages.WorkflowProcessAppName,
                    TypeApplication.Communications => Languages.CommunicationsActionsAppName,
                    _ => Languages.GeneralError,
                };
                return appName;
            }
        }
    }
}