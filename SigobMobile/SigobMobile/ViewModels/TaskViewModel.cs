namespace SigobMobile.ViewModels
{
    using Models;
    using Services;

    public class TaskViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;
        #endregion

        #region Attributes
        private bool isRunning;

        #endregion

        #region Properties
        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        public Event LocalTask { get; set; }

        #endregion

        #region Constructors
        public TaskViewModel(Event task)
        {
            this.apiService = new ApiService();
            this.LocalTask = task;
            this.LoadTaskDetails();
        }
        #endregion

        #region Methods
        private void LoadTaskDetails()
        {
            IsRunning = true;
            IsRunning = false;
        }
        #endregion
    }
}
