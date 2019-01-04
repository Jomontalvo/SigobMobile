namespace SigobMobile.Models
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// Institutions to connect model
    /// </summary>
    public class InstitutionsConnect
    {
        /// <summary>
        /// Gets the ISO country code.
        /// </summary>
        /// <value>The ISO Country code.</value>
        [JsonProperty (PropertyName = "CodigoPaisISO")]
        public string ISOCountryCode { get; }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>The status.</value>
        [JsonProperty(PropertyName = "Estado")]
        public string Status { get; }

        /// <summary>
        /// Gets the end date of validity.
        /// </summary>
        /// <value>The end date validity.</value>
        [JsonProperty(PropertyName = "FechaFinVigencia")]
        public DateTime EndDateValidity { get; }

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
        public string Institution { get; }

        /// <summary>
        /// Gets the country.
        /// </summary>
        /// <value>The country.</value>
        [JsonProperty(PropertyName = "Pais")]
        public string Country { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:SigobMobile.Models.InstitutionsConnect"/> security login.
        /// </summary>
        /// <value><c>true</c> if security login; otherwise, <c>false</c>.</value>
        [JsonProperty(PropertyName = "SeguridadInicio")]
        public bool SecurityLogin { get; }
        /// <summary>
        /// Gets the API service URL.
        /// </summary>
        /// <value>The API service URL.</value>
        [JsonProperty(PropertyName = "URLServicio")]
        public string ApiServiceUrl { get; }
    }
}
