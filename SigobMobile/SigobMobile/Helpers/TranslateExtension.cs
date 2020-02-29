namespace SigobMobile.Helpers
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Resources;
    using Interfaces;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        readonly CultureInfo ci;
        const string ResourceId = "SigobMobile.Properties.Resources";

        public string Text { get; set; }
        public IValueConverter Converter { get; set; }

        static readonly Lazy<ResourceManager> ResMgr =
            new Lazy<ResourceManager>(() => new ResourceManager(
                ResourceId,
                typeof(TranslateExtension).GetTypeInfo().Assembly));

        public TranslateExtension()
        {
            ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
            {
                return "";
            }

            var translation = ResMgr.Value.GetString(Text, ci);

            if (this.Converter != null)
            {
                translation = Converter.Convert(translation, typeof(string), null, CultureInfo.CurrentCulture).ToString() ?? translation;
            }
            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException(
                    $"Key '{Text}' was not found in resources '{ResourceId}' for culture '{ci.Name}'.", nameof(Text));
#else
                translation = Text; // returns the key, which GETS DISPLAYED TO THE USER
#endif
            }

            return translation;
        }
    }
}
