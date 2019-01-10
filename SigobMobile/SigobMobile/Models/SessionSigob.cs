namespace SigobMobile.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Active Sigob Session object model.
    /// </summary>
    public class SessionSigob
    {
        [JsonProperty(PropertyName = "usuarioLogeado")]
        public UserAuthorized LoggedUser { get; set; }

        [JsonProperty(PropertyName = "intentos")]
        public int Attemps { get; set; }

        [JsonProperty(PropertyName = "errorCode")]
        public int ErrorCode { get; set; }

        [JsonProperty(PropertyName = "tieneCentroGestion")]
        public int HasCG { get; set; }

        [JsonProperty(PropertyName = "nombreEventoAgenda")]
        public string EventName { get; set; }

        [JsonProperty(PropertyName = "nombreCorrespondencia")]
        public string CorrespondenceName { get; set; }

        [JsonProperty(PropertyName = "authToken")]
        public string AuthToken { get; set; }

        [JsonProperty(PropertyName = "databaseToken")]
        public string DatabaseToken { get; set; }

        [JsonProperty(PropertyName = "logoInsitucion")]
        public string InstitutionLogo { get; set; }
    }
}
