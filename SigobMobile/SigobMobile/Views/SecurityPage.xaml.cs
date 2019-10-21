namespace SigobMobile.Views
{
    using Xamarin.Forms;

    public partial class SecurityPage : ContentPage
    {
        public SecurityPage()
        {
            InitializeComponent();
        }

        void Handle_NewPassword_Focused(object sender, FocusEventArgs e)
        {
            IconViewNewPassword.IsVisible = e.IsFocused;
            IconViewVerifyPassword.IsVisible = !e.IsFocused;
        }

        void Handle_VerifyPassword_Focused(object sender, FocusEventArgs e)
        {
            IconViewVerifyPassword.IsVisible = e.IsFocused;
            IconViewNewPassword.IsVisible = !e.IsFocused;
        }
    }
}
