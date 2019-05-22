namespace SigobMobile.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using SigobMobile.Models;

    /// <summary>
    /// User contacts service.
    /// </summary>
    public interface IUserContactsService
    {
        Task<IEnumerable<PhoneContact>> GetAllContacts();
    }
}
