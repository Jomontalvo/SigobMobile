namespace SigobMobile.Models
{
    using Helpers;
    using Common.Models;
    using Xamarin.Forms;
    using System;

    public class TaskSigob : Common.Models.TaskSigob
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
                    : $"{((int)ModificationReportDate?.Subtract(DateTime.Today).TotalDays).ToString()} {Languages.Days.ToLower()} ";
                return label;
            }
        }

        public string PeriodicityLabel => this.ReportFrequency switch
        {
            TPeriodicity.Weekly => $"({Languages.PeriodicityWeekly})",
            TPeriodicity.Biweekly => $"({Languages.PeriodicityBiWeekly})",
            TPeriodicity.Monthly => $"({Languages.PeriodicityMonthly})",
            TPeriodicity.Bimonthly => $"({Languages.PeriodicityBiMonthly})",
            _ => $"{Languages.PeriodicityUndefined}"
        };
    }
}