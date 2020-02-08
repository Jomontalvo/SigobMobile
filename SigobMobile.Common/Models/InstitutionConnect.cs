namespace SigobMobile.Common.Models
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// Institutions to connect model
    /// </summary>
    public class InstitutionConnect
    {
        /// <summary>
        /// Gets the ISO country code.
        /// </summary>
        /// <value>The ISO Country code.</value>
        [JsonProperty(PropertyName = "CodigoPaisISO")]
        public string ISOCountryCode { get; set; }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>The status.</value>
        [JsonProperty(PropertyName = "Estado")]
        public string Status { get; set; }

        /// <summary>
        /// Gets the end date of validity.
        /// </summary>
        /// <value>The end date validity.</value>
        [JsonProperty(PropertyName = "FechaFinVigencia")]
        public DateTime? EndDateValidity { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets the name of institution.
        /// </summary>
        /// <value>The institution.</value>
        [JsonProperty(PropertyName = "Institucion")]
        public string Institution { get; set; }

        /// <summary>
        /// Gets the country.
        /// </summary>
        /// <value>The country.</value>
        [JsonProperty(PropertyName = "Pais")]
        public string Country { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:SigobMobile.Models.InstitutionsConnect"/> security login.
        /// </summary>
        /// <value><c>true</c> if security login; otherwise, <c>false</c>.</value>
        [JsonProperty(PropertyName = "SeguridadInicio")]
        public bool? SecurityLogin { get; set; }
        /// <summary>
        /// Gets the API service URL.
        /// </summary>
        /// <value>The API service URL.</value>
        [JsonProperty(PropertyName = "URLServicio")]
        public string UrlApiService { get; set; }
        /// <summary>
        /// API REST version number
        /// </summary>
        [JsonProperty(PropertyName = "version")]
        public double Version { get; set; }
        /// <summary>
        /// Gets the name sort.
        /// </summary>
        /// <value>The name sort.</value>
        public string NameSort
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ISOCountryCode) || ISOCountryCode.Length == 0)
                    return "?";
                return ISOCountryCode[0].ToString().ToUpper();
            }
        }

    }
}