using System;
namespace SigobMobile.Common.Models
{
    public class PhoneContact
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; } //FullName
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string PhotoThumbnail { get; set; }
    }
}
