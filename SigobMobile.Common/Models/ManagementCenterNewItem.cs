namespace SigobMobile.Common.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Model for Items Available to Add in Management Center
    /// </summary>
    public class ManagementCenterNewItem
    {
        [JsonProperty("tipoInstrumento")]
        public SigobInstrument InstrumentType { get; set; }
    }
}