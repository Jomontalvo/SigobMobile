namespace SigobMobile.Models
{
    using System;
    using Common.Models;
    using Telerik.XamarinForms.Input;
    using Xamarin.Forms;

    /// <summary>
    /// Event object (type IAppointment)
    /// </summary>
    public class Event : IAppointment
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Title { get; set; }
        public Color Color { get; set; }
        public bool IsAllDay { get; set; }
        public string Detail { get; set; }
        public char ModuleType { get; set; }
        public string Owner { get; set; }
        public string OwnerInitials { get; set; }
        public string Programmer { get; set; }
        public int TypeId { get; set; }
        public Color TypeColor { get; set; }
        public byte SecurityLevel { get; set; }
        public bool IsLocked { get; set; }
        public bool IsTentative { get; set; }
        public bool IsHighlighted { get; set; }
        public bool IsTask { get; set; }
        public bool IsVisible { get; set; }
        public StatusAppointment Status { get; set; }
        public TitleFontAttribute TitleFont
        {
            get
            {
                TitleFontAttribute result = TitleFontAttribute.None;
                int fontFormat = Convert.ToByte(IsTentative) + Convert.ToByte(IsHighlighted);
                switch (fontFormat)
                {
                    case 1:
                        if (IsTentative) result = TitleFontAttribute.Italic;
                        if (IsHighlighted) result = TitleFontAttribute.Bold;
                        break;
                    case 2:
                        result = TitleFontAttribute.BoldAndItalic;
                        break;
                }
                return result;
            }
        }
        public byte IconSize { get; set; }
    }
}
