
namespace SigobMobile.ViewModels
{
    using System.Linq;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Models;
    using Views.ManagementCenter;
    using Xamarin.Forms;

    /// <summary>
    /// Instruction item view model.
    /// </summary>
    public class InstructionItemViewModel : Instruction
    {
        #region Commands
        public ICommand SelectInstructionCommand
        {
            get
            {
                return new RelayCommand(SelectInstruction);
            }
        }

        /// <summary>
        /// Selects the instruction.
        /// </summary>
        private async void SelectInstruction()
        {
            var instructionViewModel = MainViewModel.GetInstance();
            instructionViewModel.Instruction = new InstructionViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new InstructionPage() { Title = "Instruction" });
        }
        #endregion
    }
}
