[assembly: Xamarin.Forms.Dependency(typeof(SigobMobile.Droid.Implementations.FileSystemService))]
namespace SigobMobile.Droid.Implementations
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
