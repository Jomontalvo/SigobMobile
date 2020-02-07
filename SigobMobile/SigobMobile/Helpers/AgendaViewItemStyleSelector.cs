namespace SigobMobile.Helpers
{
    using System;
    using Telerik.XamarinForms.Input;

    public class AgendaViewItemStyleSelector : AgendaItemStyleSelector
    {
        #region Attributes
        private readonly DateTime now;
        #endregion

        #region Properties
        public AgendaTextItemStyle CurrentMonthStyle { get; set; }
        public AgendaTextItemStyle CurrentMonthWeeksStyle { get; set; }
        public AgendaTextItemStyle TodayStyle { get; set; }
        public AgendaTextItemStyle NotTodayStyle { get; set; }
        public AgendaAppointmentItemStyle AllDayAppointmentStyle { get; set; }
        public AgendaAppointmentItemStyle NotAllDayAppointmentStyle { get; set; }
        #endregion

        #region Constructor
        public AgendaViewItemStyleSelector()
        {
            this.now = DateTime.Now;
        }
        #endregion


        #region Methods
        public override AgendaTextItemStyle SelectMonthItemStyle(AgendaMonthItem item)
        {
            if (this.now.Month == item.Date.Month && this.now.Year == item.Date.Year)
            {
                return this.CurrentMonthStyle;
            }
            return null;
        }
        public override AgendaTextItemStyle SelectWeekItemStyle(AgendaWeekItem item)
        {
            if (this.now.Month == item.StartDate.Month && this.now.Year == item.StartDate.Year)
            {
                return this.CurrentMonthWeeksStyle;
            }
            return null;
        }

        public override AgendaTextItemStyle SelectDayItemStyle(AgendaDayItem item)
        {
            if (this.now.Date == item.Date.Date)
            {
                return this.TodayStyle;
            }
            return this.NotTodayStyle;
        }

        public override AgendaAppointmentItemStyle SelectAppointmentItemStyle(AgendaAppointmentItem item)
        {
            if (item.Appointment.IsAllDay)
            {
                return this.AllDayAppointmentStyle;
            }
            return this.NotAllDayAppointmentStyle; ;
        }
        #endregion
    }
}
