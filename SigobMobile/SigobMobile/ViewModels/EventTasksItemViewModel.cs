using System.Threading.Tasks;
using AsyncAwaitBestPractices.MVVM;
using SigobMobile.Helpers;
using SigobMobile.Views.Tasks;

namespace SigobMobile.ViewModels
{
    public class EventTasksItemViewModel : Common.Models.EventTask
    {
        #region Commands
        public IAsyncCommand SelectTaskCommand => new AsyncCommand(SelectTask);

        /// <summary>
        /// Selects the instruction.
        /// </summary>
        private async Task SelectTask()
        {
            var instructionViewModel = MainViewModel.GetInstance();
            instructionViewModel.Task = new TaskViewModel(this.Id);
            await App.Navigator.PushAsync(new TaskPage() { Title = Languages.Task });
        }
        #endregion
    }
}
