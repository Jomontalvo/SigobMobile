[assembly: Xamarin.Forms.Dependency(typeof(SigobMobile.Droid.Implementations.UserContactsService))]

namespace SigobMobile.Droid.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Android.App;
    using Android.Database;
    using Android.Provider;
    using Interfaces;
    using Common.Models;

    public class UserContactsService : IUserContactsService
    {
       
        public async Task<IEnumerable<PhoneContact>> GetAllContacts()
        {
            var contactList = new List<PhoneContact>();
            //var uri = ContactsContract.CommonDataKinds.Phone.ContentUri;
            var uri = ContactsContract.Contacts.ContentUri;

            //string[] projection = { ContactsContract.Contacts.InterfaceConsts.Id,
                //ContactsContract.Contacts.InterfaceConsts.DisplayName,
                //ContactsContract.CommonDataKinds.Phone.Number,
                //ContactsContract.CommonDataKinds.Email.Address
                //};

            //var uri = ContactsContract.Contacts.ContentUri;
            var projection = new string[]{ 
            ContactsContract.Contacts.InterfaceConsts.LookupKey, 
            ContactsContract.Contacts.InterfaceConsts.DisplayName, 
            ContactsContract.CommonDataKinds.Phone.Number,
            ContactsContract.Contacts.InterfaceConsts.PhotoThumbnailUri};
            await Task.Run(() => {
                //var cursor = Android.App.Application.Context.ContentResolver.Query(uri, projection, null, null, null);
                ICursor cursorLookUpKey = Application.Context.ContentResolver.Query(uri, projection, null, null, null);
                if (cursorLookUpKey.MoveToFirst())
                {
                    do
                    {
                        try
                        {
                            var lookupKey = cursorLookUpKey.GetString(cursorLookUpKey.GetColumnIndex(projection[0]));
                            string fullName = cursorLookUpKey.GetString(cursorLookUpKey.GetColumnIndex(projection[1]));
                            string phone = cursorLookUpKey.GetString(cursorLookUpKey.GetColumnIndex(projection[2]));
                            var photoUri = Android.Net.Uri.Parse(new System.Uri(cursorLookUpKey.GetString(cursorLookUpKey.GetColumnIndex(projection[3]))).ToString());
                            var stream = Application.Context.ContentResolver.OpenInputStream(photoUri);
                            if (!string.IsNullOrEmpty(lookupKey) &&
                                !string.IsNullOrEmpty(fullName) &&
                                !fullName.ToLower().Contains('@'))
                            {
                                //Add contacto to list
                                var muser = new PhoneContact
                                {
                                    Id = lookupKey,
                                    FullName = fullName.Trim(),
                                    PhoneNumber = phone,
                                    Email = phone,
                                    //PhotoThumbnail = Xamarin.Forms.ImageSource.FromStream(() => stream)
                                };
                                contactList.Add(muser);
                            }
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                    } while (cursorLookUpKey.MoveToNext());
                }
                cursorLookUpKey.Close();
            });
            //var OrderContactList = contactList.OrderBy(item => item.FullName);
            return contactList;
        }
    }
}


/* Get Email
 * try
    {
        string contactWhere = ContactsContract.ContactsColumns.LookupKey + " = ?";
        string[] contactWhereParams = { lookupKey };
        var cursor = Application.Context.ContentResolver.Query(
            ContactsContract.Data.ContentUri,
            null,
            contactWhere,
            contactWhereParams,
            null
        );
        if (cursor.MoveToFirst())
        {
            var emailWhere = ContactsContract.ContactsColumns.LookupKey + " = ? AND " + ContactsContract.DataColumns.Mimetype + " = ?";
            var emailWhereParams = new string[] { lookupKey, ContactsContract.CommonDataKinds.Email.ContentItemType };
            var emailCursor = Application.Context.ContentResolver.Query(
                ContactsContract.Data.ContentUri,
                null,
                emailWhere,
                emailWhereParams,
                null
            );
            if (emailCursor.MoveToFirst())
            {
                email = emailCursor.GetString(emailCursor.GetColumnIndex(ContactsContract.CommonDataKinds.Email.Address));
            }
            emailCursor.Close();
        }
        cursor.Close();
        if (!string.IsNullOrEmpty(lookupKey) && !string.IsNullOrEmpty(email))
        {
            //Add contacto to list
            var muser = new PhoneContact
            {
                Id = lookupKey,
                FullName = fullName.Trim(),
                Email = email
            };
            contactList.Add(muser);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    } */