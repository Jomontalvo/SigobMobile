namespace SigobMobile.Models
{
    using Xamarin.Forms;

    /// <summary>
    /// Calerndar class inherit from Calendar Common Models
    /// </summary>
    public class Calendar : Common.Models.Calendar
    {
        /// <summary>
        /// Gets the color of the calendar.
        /// </summary>
        /// <value>The color of the calendar.</value>
        public Color CalendarColor
        {
            get { return Color.FromRgb(RedColor, GreenColor, BlueColor); }
        }
        /// <summary>
        /// The is bold font attribute.
        /// </summary>
        public FontAttributes IsBoldFontAttribute
        {
            get { return (ManagementCenterId > 0) ? FontAttributes.Bold : FontAttributes.None; }
        }
    }
}
