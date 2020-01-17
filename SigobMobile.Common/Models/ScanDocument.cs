namespace SigobMobile.Common.Models
{
    using Newtonsoft.Json;

    public enum TScanType : byte
    {
        TIFF = 0,
        PDF = 1
    }

    /// <summary>
    /// Model for scanned document
    /// </summary>
    public class ScanDocument
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("scannedType")]
        public TScanType ScannedType { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("documento")]
        public string Document { get; set; }
    }
}
