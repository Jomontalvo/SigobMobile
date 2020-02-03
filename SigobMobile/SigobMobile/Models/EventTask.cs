namespace SigobMobile.Models
{
    using System;
    using Common.Models;
    using Helpers;
    using Xamarin.Forms;

    public class EventTask : Common.Models.EventTask
    {
        public Color TTrafficLightColor => TrafficLight switch
        {
            TTrafficLightStatus.InProgress => Palette.SelectedTrafficLightGreen,
            TTrafficLightStatus.Overdue => Palette.SelectedTrafficLightRed,
            TTrafficLightStatus.CloseToDeadline => Palette.SelectedTrafficLightYellow,
            _ => Palette.SelectedTrafficLightGray
        };

        public string ReportLabel
        {
            get
            {
                string label = string.IsNullOrEmpty(Report)
                    ? Languages.No
                    : $"{((int)LastReportUpdate?.Subtract(DateTime.Today).TotalDays).ToString()} {Languages.Days.ToLower()} ";
                return label;
            }
        }

        public string PeriodicityLabel => Periodicity switch
        {
            TPeriodicity.Weekly => $"({Languages.PeriodicityWeekly})",
            TPeriodicity.Biweekly => $"({Languages.PeriodicityBiWeekly})",
            TPeriodicity.Monthly => $"({Languages.PeriodicityMonthly})",
            TPeriodicity.Bimonthly => $"({Languages.PeriodicityBiMonthly})",
            _ => string.Empty
        };
    }
}