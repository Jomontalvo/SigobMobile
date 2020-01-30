[assembly: Xamarin.Forms.Dependency(typeof(SigobMobile.iOS.Implementations.FileSystemService))]
namespace SigobMobile.iOS.Implementations
{
    using System;
    using SigobMobile.Interfaces;

    public class FileSystemService : IFileSystemService
    {
        public string GetLocalFolder()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }
    }
}
