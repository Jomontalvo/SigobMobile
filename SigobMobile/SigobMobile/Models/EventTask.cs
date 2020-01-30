namespace SigobMobile.Models
{

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
    }
}