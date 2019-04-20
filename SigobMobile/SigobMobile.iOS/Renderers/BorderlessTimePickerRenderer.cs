using SigobMobile.Controls;
using SigobMobile.iOS.Renderers;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(BorderlessTimePicker), typeof(BorderlessTimePickerRenderer))]
namespace SigobMobile.iOS.Renderers
{
    using System.ComponentModel;
    using UIKit;
    using Xamarin.Forms.Platform.iOS;

    /// <summary>
    /// Borderless time picker renderer.
    /// </summary>
    public class BorderlessTimePickerRenderer : TimePickerRenderer
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
