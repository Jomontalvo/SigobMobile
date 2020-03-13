namespace SigobMobile.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Common.Models;

    /// <summary>
    /// Participant item view model.
    /// </summary>
    public class ParticipantItemViewModel : Participant
    {
        #region Properties
        public bool IsRemovable { get; set; }
        #endregion

        #region Commands
        public ICommand RemoveParticipantCommand => new RelayCommand(RemoveParticipant);
        public ICommand AddParticipantCommand => new RelayCommand(AddParticipant);
        #endregion

        #region Methods
        private void RemoveParticipant()
        {
            var participantsViewModel = MainViewModel.GetInstance().Participants;
            participantsViewModel.GuestContacts.Remove(this);
            participantsViewModel.participantList.Remove(this);
            participantsViewModel.AvailableContacts.Add(this);
            participantsViewModel.availableList.Add(this);
        }
        private void AddParticipant()
        {
            var participantsViewModel = MainViewModel.GetInstance().Participants;
            participantsViewModel.AvailableContacts.Remove(this);
            participantsViewModel.availableList.Remove(this);
            participantsViewModel.GuestContacts.Add(this);
            participantsViewModel.participantList.Add(this);
            participantsViewModel.Filter = string.Empty;
            participantsViewModel.IsFinding = false;
        }
        #endregion
    }
}