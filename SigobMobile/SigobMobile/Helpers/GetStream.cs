namespace SigobMobile.Helpers
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class GetStream
    {
        /// <summary>
        /// Get Stream from valid URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<MemoryStream> GetStreamFromUrl(string url)
        {
            byte[] imageData = null;
            MemoryStream ms;
            ms = null;
            try
            {
                using (var wc = new System.Net.WebClient())
                {
                    imageData = wc.DownloadData(url);
                }
                ms = new MemoryStream(imageData);
            }
            catch (Exception ex)
            {
                await App.Navigator.DisplayAlert(
                    Languages.Error,
                    $"{Languages.GeneralError}: {ex.Message}",
                    null, Languages.Accept);
            }
            return ms;
        }
    }
}
