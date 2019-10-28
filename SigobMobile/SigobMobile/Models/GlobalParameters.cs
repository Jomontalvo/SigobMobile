﻿namespace SigobMobile.Models
{
    using System;
    using Newtonsoft.Json;

    #region Enum SIGOB Instruments Type
    public enum SigobInstrument
    {
        None = -1,
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