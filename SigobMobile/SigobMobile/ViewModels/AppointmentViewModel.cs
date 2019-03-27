namespace SigobMobile.ViewModels
{
    using Models;

    public class AppointmentViewModel : BaseViewModel
    {
        #region Properties
        public Event LocalAppointment { get; set; }
        #endregion


        #region Constructors
        public AppointmentViewModel(Event appointment)
        {
            this.LocalAppointment = appointment;
        }
        #endregion
    }
}
