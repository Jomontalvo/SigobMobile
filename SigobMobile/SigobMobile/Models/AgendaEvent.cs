namespace SigobMobile.Models
{
    using SigobMobile.Common.Helpers;
    using Xamarin.Forms;

    /// <summary>
    /// Agenda Event inherits Agenda Event Common
    /// </summary>
    public class AgendaEvent : Common.Models.AgendaEvent
    {
        /// <summary>
        /// Gets the color of personal event
        /// </summary>
        /// <value>The color.</value>
        public Color CalendarColor => Color.FromRgb(ColorItemRed, ColorItemGreen, ColorItemBlue);
        /// <summary>
        /// Gets the color of the type event.
        /// </summary>
        /// <value>The color of the type.</value>
        public Color TypeColor => Color.FromRgb(ColorTypeRed, ColorTypeGreen, ColorTypeBlue);
        /// <summary>
        /// Record is editable (remove biutton enabled)
        /// </summary>
        public bool IsEditable => OwnerOfficeId == Settings.OfficeCode || ProgrammerOfficeId == Settings.OfficeCode ;
    }
}
