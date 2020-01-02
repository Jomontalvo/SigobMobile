namespace SigobMobile.Models
{
    using Xamarin.Forms;
    using Palette = Helpers.Palette;
    using Common.Models;
    using System;
    using Helpers;

    public class Document : Common.Models.Document
    {
        public string CodeAndManagementTime => $"{RegistrationCode} " +
            $"{(this.DaysStayInOffice > 0 ? " (" + this.DaysStayInOffice + Languages.Days + ")": string.Empty)}";

        public Color ColorStatus => (this.Status) switch
        {
            ManagementTermStatus.Red => Palette.SelectedTrafficLightRed,
            ManagementTermStatus.Green => Palette.SelectedTrafficLightGreen,
            ManagementTermStatus.Yellow => Palette.SelectedTrafficLightYellow,
            _ => Color.Transparent

        };

        public Color DateColor
        {
            get
            { return (this.Status != ManagementTermStatus.White ) ? Color.White : ((Color)Application.Current.Resources["grayContent"]); }
        }

        public string RegistrationDateString
        {
            get
            {
                string date = this.RegistrationDate.ToLocalTime().ToString("MMM dd, yy");
                if  (this.RegistrationDate.Year == DateTime.Today.Year)
                {
                    date = (DateTime.Today.AddDays(1) - RegistrationDate.ToLocalTime()).TotalDays switch
                    {
                        double days when days >= 0 && days < 1 => Languages.Today,
                        double days when days >= 0 && days < 1 => Languages.Yesterday,
                        _ => this.RegistrationDate.ToLocalTime().ToString("MMM dd"),
                    };
                }
                return date;
            }
        }
    }
}
