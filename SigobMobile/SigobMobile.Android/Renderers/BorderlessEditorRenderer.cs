using SigobMobile.Droid.Renderers;
using SigobMobile.Helpers;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(BorderlessEditor), typeof(BorderlessEditorRenderer))]
namespace SigobMobile.Droid.Renderers
{
    using Android.Content;
    using Android.Text;
    using Xamarin.Forms.Platform.Android;

    public class BorderlessEditorRenderer : EditorRenderer
    {
        public BorderlessEditorRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                this.Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
                this.Control.SetRawInputType(InputTypes.TextFlagNoSuggestions);
            }
        }
    }
}