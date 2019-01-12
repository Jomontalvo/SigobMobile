namespace SigobMobile.Helpers
{
    using Xamarin.Forms;
    using Interfaces;
    public static class Languages
    {
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resources.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }
    }
}
