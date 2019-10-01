namespace SigobMobile.Helpers
{
    using Xamarin.Forms;
    using Models;
    public class AppointmentTemplateSelector : DataTemplateSelector
    {
        public DataTemplate AllDay { get; set; }
        public DataTemplate NotAllDay { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var appointmentsTemplate = item as Event;
            if (appointmentsTemplate.IsAllDay)
            {
                return this.AllDay;
            }
            return this.NotAllDay;
        }
    }
}
