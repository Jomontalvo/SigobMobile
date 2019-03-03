using SigobMobile.Droid.Renderers;
using Xamarin.Forms;

[assembly: ExportRendererAttribute(typeof(SigobMobile.Helpers.IconView), typeof(IconViewRenderer))]
namespace SigobMobile.Droid.Renderers
{
    using System.ComponentModel;
    using Android.Content;
    using Android.Graphics;
    using Android.Widget;
    using Helpers;
    using Xamarin.Forms.Platform.Android;

    public class IconViewRenderer : ViewRenderer<IconView, ImageView>
    {
        private bool _isDisposed;


        public IconViewRenderer(Context context) : base(context)
        {
            AutoPackage = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }
            _isDisposed = true;
            base.Dispose(disposing);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<IconView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                SetNativeControl(new ImageView(Context));
            }
            UpdateBitmap(e.OldElement);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == IconView.SourceProperty.PropertyName)
            {
                UpdateBitmap(null);
            }
            else if (e.PropertyName == IconView.ForegroundProperty.PropertyName)
            {
                UpdateBitmap(null);
            }
        }

        private void UpdateBitmap(IconView previous = null)
        {
            if (!_isDisposed && !string.IsNullOrWhiteSpace(Element.Source))
            {
                var d = Context.GetDrawable(Element.Source).Mutate();
                d.SetColorFilter(new LightingColorFilter(Element.Foreground.ToAndroid(), Element.Foreground.ToAndroid()));
                d.Alpha = Element.Foreground.ToAndroid().A;
                Control.SetImageDrawable(d);
                ((IVisualElementController)Element).NativeSizeChanged();
            }
        }
    }


}
