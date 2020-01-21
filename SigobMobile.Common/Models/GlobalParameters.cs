﻿namespace SigobMobile.Common.Models
{
    using System;
    using System.Drawing;
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

    /// <summary>
    /// General definition of SIGOB Tuple
    /// </summary>
    public class TSigTuple
    {
        [JsonProperty("codigo")]
        public int Id { get; set; }

        [JsonProperty("descripcion")]
        public string Description { get; set; }
    }

    /// <summary>
    /// Color tuple (Inherit tuple)
    /// </summary>
    public class TSigColorTuple : TSigTuple
    {
        [JsonProperty("color")]
        public string Color { get; set; }

        /// <summary>
        /// Color with Alpha
        /// </summary>
        public Color ColorAlpha => System.Drawing.Color.FromArgb(175, System.Drawing.Color.FromName(Color));
    }

    /// <summary>
    /// Class for any SegmentedControl
    /// </summary>
    public class Segment
    {
        public int Id { get; set; }
        public byte QueryId { get; set; }
        public string SegmentName { get; set; }
        public override string ToString()
        {
            return this.SegmentName;
        }
    }
}