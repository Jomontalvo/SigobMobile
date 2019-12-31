namespace SigobMobile.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Models;

    /// <summary>
    /// User contacts service.
    /// </summary>
    public interface IUserContactsService
    {
        Task<IEnumerable<PhoneContact>> GetAllContacts();
    }
}
