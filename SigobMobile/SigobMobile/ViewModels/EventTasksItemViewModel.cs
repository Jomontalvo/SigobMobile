namespace SigobMobile.ViewModels
{
    using System.Threading.Tasks;
    using AsyncAwaitBestPractices.MVVM;
    using Helpers;
    using Models;
    using Views.Tasks;

    public class EventTasksItemViewModel : TaskSigob
    {
        #region Commands
        public IAsyncCommand SelectTaskCommand => new AsyncCommand(SelectTask);

        /// <summary>
        /// Selects the instruction.
        /// </summary>
        private async Task SelectTask()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Task = new TaskViewModel(this, true);
            await App.Navigator.PushAsync(new TaskPage() { Title = $"{Languages.Task} [{this.Id}]" });
        }
        #endregion
    }
}
