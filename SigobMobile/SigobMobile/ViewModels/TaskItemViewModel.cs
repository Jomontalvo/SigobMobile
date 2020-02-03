namespace SigobMobile.ViewModels
{
    using System.Threading.Tasks;
    using AsyncAwaitBestPractices.MVVM;
    using Models;
    using Views.Tasks;

    public class TaskItemViewModel : TaskSigob
    {
        #region Extended Properties

        #endregion

        #region Commands
        public IAsyncCommand SelectTaskCommand => new AsyncCommand(SelectTaskAsync);
        #endregion

        #region Methods
        private async Task SelectTaskAsync()
        {
            var maiViewModel = MainViewModel.GetInstance();
            maiViewModel.Task = new TaskViewModel(this);
            await App.Navigator.PushAsync(new TaskPage());
        }
        #endregion

    }
}
