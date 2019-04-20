using SigobMobile.Controls;
using SigobMobile.iOS.Renderers;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(BorderlessDatePicker), typeof(BorderlessDatePickerRenderer))]
namespace SigobMobile.iOS.Renderers
{
    using Xamarin.Forms.Platform.iOS;
    using System.ComponentModel;
    using UIKit;

    /// <summary>
    /// Borderless date picker renderer.
    /// </summary>
    public class BorderlessDatePickerRenderer : DatePickerRenderer
    {
        public new static void Init() { }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            Control.Layer.BorderWidth = 0;
            Control.BorderStyle = UITextBorderStyle.None;
        }
    }
}
