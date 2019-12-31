namespace SigobMobile.Models
{
    using Xamarin.Forms;
    using Palette = Helpers.Palette;
    using Common.Models;

    public class Document : Common.Models.Document
    {
        public Color ColorStatus => (this.Status) switch
        {
            ManagementTermStatus.Red => Palette.SelectedTrafficLightRed,
            ManagementTermStatus.Green => Palette.SelectedTrafficLightGreen,
            ManagementTermStatus.Yellow => Palette.SelectedTrafficLightYellow,
            _ => Color.Transparent

        };
    }
}
