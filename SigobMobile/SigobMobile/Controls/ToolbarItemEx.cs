namespace SigobMobile.Controls
{
    using Xamarin.Forms;

    public class ToolbarItemEx : ToolbarItem
    {
        //NOTE: Default value is true, because toolbar items are by default visible when added to the Toolbar
        public static readonly BindableProperty IsVisibleProperty = BindableProperty.Create(nameof(IsVisible), typeof(bool), typeof(ToolbarItemEx), true, BindingMode.TwoWay, null, OnIsVisibleChanged);

        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        public ContentPage ParentPage
        {
            get { return Parent as ContentPage; }
        }

        private static void OnIsVisibleChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var item = bindable as ToolbarItemEx;

            bool newVisible = (bool)newvalue;

            if (item != null && item.Parent == null)
                return;

            if (item != null)
            {
                var items = ((Page)item.Parent)?.ToolbarItems;

                if (Equals(items, null)) return;
                if ((bool)newvalue && !items.Contains(item))
                {
                    Device.BeginInvokeOnMainThread(() => { items.Add(item); });
                }
                else if (!(bool)newvalue && items.Contains(item))
                {
                    Device.BeginInvokeOnMainThread(() => { items.Remove(item); });
                }
            }

            //var items = item.ParentPage.ToolbarItems;
            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    if (newVisible && !items.Contains(item))
            //    {
            //        items.Add(item);
            //    }
            //    else if (!newVisible && items.Contains(item))
            //    {
            //        items.Remove(item);
            //    }
            //});
        }
    }
}