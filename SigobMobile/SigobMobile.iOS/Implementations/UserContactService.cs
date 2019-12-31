[assembly: Xamarin.Forms.Dependency(typeof(SigobMobile.iOS.Implementations.UserContactService))]

namespace SigobMobile.iOS.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Contacts;
    using Foundation;
    using Interfaces;
    using Plugin.Permissions;
    using Plugin.Permissions.Abstractions;
    using SigobMobile.Common.Models;

    public class UserContactService : IUserContactsService
    {
        public async Task<IEnumerable<PhoneContact>> GetAllContacts()
        {
            //Permission
            if (!(await PermissionGranting()))
                return new List<PhoneContact>();

            //Contacts
            var keysToFetch = new[] { CNContactKey.GivenName, CNContactKey.FamilyName, CNContactKey.EmailAddresses, CNContactKey.ThumbnailImageData };
            //var containerId = new CNContactStore().DefaultContainerIdentifier;
            // using the container id of null to get all containers.
            // If you want to get contacts for only a single container type, you can specify that here
            var contactList = new List<CNContact>();

            using (var store = new CNContactStore())
            {
                var allContainers = store.GetContainers(null, out NSError error);
                foreach (var container in allContainers)
                {
                    try
                    {
                        using (var predicate = CNContact.GetPredicateForContactsInContainer(container.Identifier))
                        {
                            var containerResults = store.GetUnifiedContacts(predicate, keysToFetch, out error);
                            contactList.AddRange(containerResults);
                        }
                    }
                    catch (Exception e) { Console.WriteLine(e.Message); } // ignore missed contacts from errors
                }
            }
            var contacts = new List<PhoneContact>();
            foreach (var item in contactList)
            {
                var emails = item.EmailAddresses;
                if (emails != null)
                {
                    try
                    {
                        var stream = item.ThumbnailImageData?.AsStream();
                        contacts.Add(new PhoneContact
                        {
                            FirstName = item.GivenName,
                            LastName = item.FamilyName,
                            FullName = $"{item.GivenName} {item.FamilyName}",
                            Email = (emails.GetLength(0) > 0) ? emails[0].Value : String.Empty,
                            PhotoThumbnail = "ic_user"//Xamarin.Forms.ImageSource.FromStream(() => stream)
                    });
                    }
                    catch (Exception ex) { Console.WriteLine (ex.Message); }
                }
            }
            return contacts;
        }

        /// <summary>
        /// Check Permissions over contacts.
        /// </summary>
        /// <returns>The granting.</returns>
        public async Task<bool> PermissionGranting()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Contacts);
            if (status != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Contacts);
                status = results[Permission.Contacts];
            }
            if (status != PermissionStatus.Granted)
            {
                return false;
            }

            return true;
        }
    }
}
