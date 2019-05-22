namespace SigobMobile.Helpers
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    /// <summary>
    /// Grouping a ListView. K is a multuple value key: SortName, Name Grouping and Image.
    /// </summary>
    public class Grouping<K, T> : ObservableCollection<T>
    {
        public K Key { get; private set; }
        public string AlphaIndex { get; private set; }
        public string Name { get; private set; }
        public string ImageGroup { get; private set; }
        public Grouping(K key, IEnumerable<T> items)
        {
            string[] array = key.ToString().Split('|'); 
            Key = key;
            AlphaIndex = (!string.IsNullOrEmpty(array[0])) ? array[0].ToUpper() : string.Empty;
            Name = array[1];
            ImageGroup = (!string.IsNullOrEmpty(array[2])) ? $"ic_{array[2].ToLower()}" : string.Empty;
            foreach (var item in items)
                this.Items.Add(item);
        }
    }
}