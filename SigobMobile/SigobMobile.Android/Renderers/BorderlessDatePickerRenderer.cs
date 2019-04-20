using SigobMobile.Droid.Renderers;
using SigobMobile.Controls;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(BorderlessDatePicker), typeof(BorderlessDatePickerRenderer))]
namespace SigobMobile.Droid.Renderers
{
    using Android.Content;
    using Xamarin.Forms.Platform.Android;

    /// <summary>
    /// Borderless date picker renderer.
    /// </summary>
    public class BorderlessDatePickerRenderer : DatePickerRenderer
    {
        public BorderlessDatePickerRenderer(Context context) : base(context){}
        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                Control.Background = null;

                var layoutParams = new MarginLayoutParams(Control.LayoutParameters);
                layoutParams.SetMargins(0, 0, 0, 0);
                LayoutParameters = layoutParams;
                Control.LayoutParameters = layoutParams;
                Control.SetPadding(0, 0, 0, 0);
                SetPadding(0, 0, 0, 0);
            }
        }
    }
}