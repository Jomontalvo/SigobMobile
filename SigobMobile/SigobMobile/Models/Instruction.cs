namespace SigobMobile.Models
{
    using Xamarin.Forms;

    public class Instruction : Common.Models.Instruction
    {
        /// <summary>
        /// Gets the color of the status.
        /// </summary>
        /// <value>The color of the status.</value>
        public Color StatusColor => (IsFinished > 0) ? (Color)Application.Current.Resources["darkGray"] :
            (ExpiryPeriod < 0) ?
                (Color)Application.Current.Resources["red"] :
                (Color)Application.Current.Resources["green"];
    }
}
