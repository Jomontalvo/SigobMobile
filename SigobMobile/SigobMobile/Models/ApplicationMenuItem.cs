namespace SigobMobile.Models
{
    using Newtonsoft.Json;

    public class ApplicationMenuItem
    {
        /// <summary>
        /// Gets or sets the type application based in tipo_instru
        /// </summary>
        /// <value>The type application.</value>
        [JsonProperty(PropertyName = "tipo_instru")]
        public int TypeApplication { get; set; }

        [JsonProperty(PropertyName = "nombre_aplicacion")]
        public string NameApplication { get; set; }

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
        public int NewItems { get; set; }
    }
}
