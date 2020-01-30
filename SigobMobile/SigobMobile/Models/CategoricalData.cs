namespace SigobMobile.Models
{
    /// <summary>
    /// Categorical Data Model for Segmented Control
    /// </summary>
    public class CategoricalData
    {
        public byte Id { get; set; }
        public object Category { get; set; }
        public double? Value { get; set; }
        public string CategoryName => this.Category.ToString();
    }
}