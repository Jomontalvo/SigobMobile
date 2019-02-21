namespace SigobMobile.Models
{
    using Newtonsoft.Json;

    public enum TypeApplication
    {
        Correspondence = 2,
        ManagementCenter = 3,
        Communications = 5,
        Goals = 6,
        WorkFlows = 8,
        Tasks = 9
    }

    public class ApplicationMenuItem
    {
        /// <summary>
        /// Gets or sets the type application based in tipo_instru
        /// </summary>
        /// <value>The type application.</value>
        [JsonProperty(PropertyName = "tipo_instru")]
        public TypeApplication TypeApplication { get; set; }

        [JsonProperty(PropertyName = "nombre_aplicacion")]
        public string ApplicationName { get; set; }

        [JsonProperty(PropertyName = "visible")]
        public bool IsVisible { get; set; }

        [JsonProperty(PropertyName = "mensaje_1")]
        public string Message_1 { get; set; }

        [JsonProperty(PropertyName = "mensaje_2")]
        public string Message_2 { get; set; }

        /// <summary>
        /// Gets or sets the number of new items related to Application
        /// </summary>
        /// <value>The new items.</value>
        [JsonProperty(PropertyName = "numero_items_nuevos")]
        public string NewItems { get; set; }

        /// <summary>
        /// Gets the module icon.
        /// </summary>
        /// <value>The module icon string</value>
        public string IconModule
        {
            get { return (IsVisible) ? $"ic_app_{TypeApplication}" : $"ic_app_{TypeApplication}_gray"; }
        }
    }

}