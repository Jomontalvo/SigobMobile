namespace SigobMobile.Models
{
    using Newtonsoft.Json;
    using Common.Models;
    using Helpers;
    using Xamarin.Forms;

    public class TaskCategoricalData
    {
        [JsonProperty("id")]
        public TTrafficLightStatus Id { get; set; }

        [JsonProperty("label")]
        public string Category
        {
            get
            {
                string statusName = Id switch
                {
                    TTrafficLightStatus.InProgress => Languages.InProgressStatus,
                    TTrafficLightStatus.Overdue => Languages.OverdueStatus,
                    TTrafficLightStatus.Completed => Languages.CompletedStatus,
                    TTrafficLightStatus.CloseToDeadline => Languages.CloseToDeadlinedStatus,
                    _ => Languages.InManagementStatus,
                };
                return statusName;
            }
        }

        [JsonProperty("value")]
        public int? Value { get; set; }

        [JsonProperty("colorSerie")]
        public string ColorName { get; set; }

        [JsonProperty("a")]
        public byte A { get; set; }

        [JsonProperty("r")]
        public byte R { get; set; }

        [JsonProperty("g")]
        public byte G { get; set; }

        [JsonProperty("b")]
        public byte B { get; set; }

        public Color ColorSerie { get => Color.FromRgba(R, G, B, A); }
    }
}
