namespace SigobMobile.Models
{
    using Helpers;
    using Common.Models;
    using Xamarin.Forms;
    using System;

    public class TaskSigob : Common.Models.TaskSigob
    {
        /// <summary>
        /// TrafficLight Color 
        /// </summary>
        public Color TTrafficLightColor => TrafficLight switch
        {
            TTrafficLightStatus.InProgress => Palette.SelectedTrafficLightGreen,
            TTrafficLightStatus.Overdue => Palette.SelectedTrafficLightRed,
            TTrafficLightStatus.CloseToDeadline => Palette.SelectedTrafficLightYellow,
            _ => Palette.SelectedTrafficLightGray

        };

        /// <summary>
        /// Label if report exists
        /// </summary>
        public string ReportLabel
        {
            get
            {
                string label = Languages.No;
                if (!string.IsNullOrEmpty(Report))
                {
                    int days = Math.Abs((int)ModificationReportDate?.Subtract(DateTime.Today).TotalDays);
                    label = days switch
                    {
                        int n when n >= 0 && n < 31 => $"{days.ToString()} {Languages.Days.ToLower()}",
                        _ => $"{(int)days / 30} {Languages.Months.ToLower()}"
                    };
                }
                return label;
            }
        }

        /// <summary>
        /// Label for Frequency Report
        /// </summary>
        public string PeriodicityLabel => this.ReportFrequency switch
        {
            TPeriodicity.Weekly => $"({Languages.PeriodicityWeekly})",
            TPeriodicity.Biweekly => $"({Languages.PeriodicityBiWeekly})",
            TPeriodicity.Monthly => $"({Languages.PeriodicityMonthly})",
            TPeriodicity.Bimonthly => $"({Languages.PeriodicityBiMonthly})",
            _ => $"{Languages.PeriodicityUndefined}"
        };

        /// <summary>
        /// Percentage of completion (% Format)
        /// </summary>
        public string PercentageOfCompletion => (Completion >= 0) ? $"{Completion:N0}%" : $"N/A";

    }
}