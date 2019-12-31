namespace SigobMobile.Common.Helpers
{
    using System.IO;

    /// <summary>
    /// Files Helper
    /// </summary>
    public static class FilesHelper
    {        public static byte[] ReadFully(Stream input)        {            using MemoryStream ms = new MemoryStream();            input.CopyTo(ms);            return ms.ToArray();        }    }
}