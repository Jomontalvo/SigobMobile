namespace SigobMobile.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using SigobMobile.Models;


    public class InternalDocumentViewModel : BaseViewModel
    {
        private List<CategoricalData> dataList;
        private ObservableCollection<CategoricalData> data;

        public ObservableCollection<CategoricalData> Data
        {
            get => this.data;
            set => SetValue(ref this.data, value);
        }

        public InternalDocumentViewModel()
        {
            this.LoadSegments();
            Data = new ObservableCollection<CategoricalData>(this.dataList);
        }

        private void LoadSegments()
        {
            this.dataList = new List<CategoricalData> {
            new CategoricalData { Id = 0, Category = "All Trays", Value = 20},
            new CategoricalData { Id = 1, Category = "Current Tray", Value = 80}
            };
        }
    }
}