namespace SigobMobile.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// User authorized model
    /// </summary>
    public class UserAuthorized
    {
        [JsonProperty(PropertyName = "usuario")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "despacho")]
        public string Despacho { get; set; }

        [JsonProperty(PropertyName = "codigoFuncionario")]
        public string FunctionaryCode { get; set; }

        [JsonProperty(PropertyName = "codigoInstitucionFuncionario")]
        public string InstitutionCode { get; set; }

        [JsonProperty(PropertyName = "nombre")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "nombreInstitucionFuncionario")]
        public string FunctionaryInstitution { get; set; }

        [JsonProperty(PropertyName = "cargo")]
        public string Position { get; set; }

        [JsonProperty(PropertyName = "nombreArea")]
        public string Area { get; set; }

        [JsonProperty(PropertyName = "permisoDirectorio")]
        public string ModifiyDirectory { get; set; }

        [JsonProperty(PropertyName = "codigoArea")]
        public string AreaCode { get; set; }

        [JsonProperty(PropertyName = "codigoAmbiente")]
        public string GraphicEnviromentCode { get; set; }

        [JsonProperty(PropertyName = "destinatario")]
        public string Destinatary { get; set; }

        [JsonProperty(PropertyName = "veContadores")]
        public string ViewCounters { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "estado")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "nombreInstitucion")]
        public string Institution { get; set; }

        [JsonProperty(PropertyName = "cambioPwd")]
        public string ChangePwdEnabled { get; set; }

        [JsonProperty(PropertyName = "tieneCertificado")]
        public string HasCertificate { get; set; }

        [JsonProperty(PropertyName = "sigla")]
        public string Acronym { get; set; }

        [JsonProperty(PropertyName = "pais1")]
        public string Country1 { get; set; }

        [JsonProperty(PropertyName = "pais2")]
        public string Country2 { get; set; }

        [JsonProperty(PropertyName = "pais3")]
        public string Country3 { get; set; }

        [JsonProperty(PropertyName = "paisDefault")]
        public string DefaultCountry { get; set; }

        [JsonProperty(PropertyName = "nombrePaisDefault")]
        public string DefaultCountryName { get; set; }

        [JsonProperty(PropertyName = "provincia1")]
        public string Province1 { get; set; }

        [JsonProperty(PropertyName = "ciudad1")]
        public string City1 { get; set; }

        [JsonProperty(PropertyName = "ciudad2")]
        public string City2 { get; set; }

        [JsonProperty(PropertyName = "nombreCiudadDefault")]
        public string DefaultCityName { get; set; }

        [JsonProperty(PropertyName = "ciudad3")]
        public string City3 { get; set; }

        [JsonProperty(PropertyName = "nombreProvinciaDefault")]
        public string DefaultPovinceName { get; set; }

        [JsonProperty(PropertyName = "provincia3")]
        public string Province3 { get; set; }

        [JsonProperty(PropertyName = "provincia2")]
        public string Province2 { get; set; }

        [JsonProperty(PropertyName = "provinciaDefault")]
        public string DefaultProvince { get; set; }

        [JsonProperty(PropertyName = "ciudadDefault")]
        public string DefaultCity { get; set; }

        [JsonProperty(PropertyName = "diasLargos")]
        public List<string> LongNameDays { get; set; }

        [JsonProperty(PropertyName = "diasCortos")]
        public List<string> ShortNameDays { get; set; }

        [JsonProperty(PropertyName = "usuarioBD")]
        public string UserDataBase { get; set; }

        [JsonProperty(PropertyName = "intentos")]
        public int Attemps { get; set; }

        [JsonProperty(PropertyName = "passwordDB")]
        public string PasswordDataBase { get; set; }
    }


    /// <summary>
    /// User basic profile model
    /// </summary>
    public class UserBasicProfile
    {
        [JsonProperty("despacho")]
        public string OfficeId { get; set; }

        [JsonProperty("nombres")]
        public string FirstName { get; set; }

        [JsonProperty("apellidos")]
        public string LastName { get; set; }

        [JsonProperty("institucion")]
        public string Institution { get; set; }

        [JsonProperty("cargo")]
        public string Position { get; set; }

        [JsonProperty("area")]
        public string Area { get; set; }

        [JsonProperty("telefono")]
        public string Phone { get; set; }

        [JsonProperty("celular")]
        public object CellPhone { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("imagenUsuario")]
        public string UserImage { get; set; }
    }
}

