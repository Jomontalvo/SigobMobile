namespace SigobMobile.Interfaces
{
    using System.IO;
    using System.Threading.Tasks;

    public interface IFileViewerService
    {
        Task<bool> View(Stream stream, string filename);
    }
}