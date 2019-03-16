namespace SigobMobile.ViewModels
{
    using Models;

    /// <summary>
    /// Event agenda view model.
    /// </summary>
    public class EventAgendaViewModel : BaseViewModel
    {
        public Event LocalAppointment { get;  set;  }

        public EventAgendaViewModel(Event appoitment)
        {
            this.LocalAppointment = appoitment;
        }
    }
}
