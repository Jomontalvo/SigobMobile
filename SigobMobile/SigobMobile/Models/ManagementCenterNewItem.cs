namespace SigobMobile.Models
{
    using Common.Models;
    using Helpers;

    /// <summary>
    /// Display itmes in correct Language
    /// </summary>
    public class ManagementCenterNewItem : Common.Models.ManagementCenterNewItem
    {
        /// <summary>
        /// Object name available to add in Management Center
        /// </summary>
        public string NewItemName
        {
            get
            {
                var itemName = InstrumentType switch
                {
                    SigobInstrument.PersonalAppointment => Languages.Appointment,
                    SigobInstrument.Instruction => Languages.Instruction,
                    SigobInstrument.Assignment => Languages.Assignment,
                    SigobInstrument.ManagementCenterEvent => Languages.Event,
                    SigobInstrument.Task => Languages.Task,
                    _ => string.Empty,
                };
                return itemName;
            }
        }
    }
}
