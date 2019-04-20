
namespace SigobMobile.ViewModels
{
    using System.Threading.Tasks;
    using AsyncAwaitBestPractices.MVVM;
    using Models;
    using Views.ManagementCenter;
    using Xamarin.Forms;

    /// <summary>
    /// Instruction item view model.
    /// </summary>
    public class InstructionItemViewModel : Instruction
    {
        #region Commands
        public IAsyncCommand SelectInstructionCommand => new AsyncCommand(SelectInstruction);

        /// <summary>
        /// Selects the instruction.
        /// </summary>
        private async Task SelectInstruction()
        {
            var instructionViewModel = MainViewModel.GetInstance();
            instructionViewModel.Instruction = new InstructionViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new InstructionPage() { Title = "Instruction" });
        }
        #endregion
    }
}
