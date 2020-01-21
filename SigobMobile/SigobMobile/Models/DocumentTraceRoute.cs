namespace SigobMobile.Models
{
    using System;
    using System.Linq;
    using SigobMobile.Helpers;
    using Xamarin.Forms;

    public class DocumentTraceRoute : Common.Models.DocumentTraceRoute
    {
        #region Properties
        public int Year { get => (StartDate.Year != DateTime.Today.Year) ? StartDate.Year : 0; }

        public bool IsFirst { get => this.StepPosition == 0; }

        public bool IsLast { get => this.StepPosition == 1; }

        public bool IsMiddle { get => this.StepPosition == 2; }

        public string DurationValueTitle => (DurationMeasure == Common.Models.TimeScaleType.Hours)
                    ? $"{DurationInOffice}"
                    : this.GetDurationValueTitle();

        public string DurationMeasureTitle => (DurationMeasure == Common.Models.TimeScaleType.Hours)
                    ? Languages.Hours
                    : this.GetMeasureTitle();

        public Color StepColor { get => this.GetStepColor(); }
        #endregion

        #region Methods
        private Color GetStepColor()
        {
            return DurationInOffice switch
            {
                int n when n <= 30 => Palette.TrafficLightGreen,
                int n when n > 30 && n <= 90 => Palette.TrafficLightYellow,
                int n when n > 90 => Palette.TrafficLightRed,
                _ => Palette.TrafficLightGray,
            };
        }

        private string GetMeasureTitle()
        {
            var label = DurationInOffice switch
            {
                int n when n > 180 && n < 365 => $"{Languages.Months.First().ToString().ToUpper()}{Languages.Months.Substring(1)}",
                int n when n >= 365 => $"{Languages.Years.First().ToString().ToUpper()}{Languages.Years.Substring(1)}",
                _ => $"{Languages.Days.First().ToString().ToUpper()}{Languages.Days.Substring(1)}",
            };
            return label;
        }

        private string GetDurationValueTitle()
        {
            var value = DurationInOffice switch
            {
                int n when n > 180 && n < 365 => $"{(DurationInOffice/30):n0}",
                int n when n >= 365 => $"{(DurationInOffice/365):n0} +",
                _ => $"{DurationInOffice:n0}",
            };
            return value;
            throw new NotImplementedException();
        }
        #endregion
    }
}
