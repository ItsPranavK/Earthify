using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

namespace Earthify.Backend
{
    /// <summary>
    /// DataModel for images. Provides access to properties and allows manipulation of image.
    /// </summary>
    public class ImageModel
    {
        public string image_url { get; set; }

        public string country { get; set; }

        public string map { get; set; }

        public string region { get; set; }

        /// <summary>
        /// Default constructor for ImageModel.
        /// </summary>
        public ImageModel()
        {

        }

        /// <summary>
        /// In-built function to download image.
        /// </summary>
        public async Task DownloadImage()
        { 
            var savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            savePicker.SuggestedFileName = GenerateImageName();

            savePicker.FileTypeChoices.Add("Image", new List<string>() { ".jpg", ".jpeg", ".png" });

            var file = await savePicker.PickSaveFileAsync();

            if (file != null)
            {
                CachedFileManager.DeferUpdates(file);

                HttpClient client = new HttpClient();
                byte[] buffer = await client.GetByteArrayAsync(new Uri(image_url));

                using (Stream stream = await file.OpenStreamForWriteAsync())
                {
                    stream.Write(buffer, 0, buffer.Length);
                }

                await CachedFileManager.CompleteUpdatesAsync(file);
            }
        }

        /// <summary>
        /// Generates file name for saved image.
        /// </summary>
        private string GenerateImageName()
        {
            string name = "";

            if (!(region.Equals("") || region == null))
            {
                name = region + " " + country;
                name = name.Replace(" ", "-");
            }
            else
            {
                name = country;
                name = name.Replace(" ", "-");
            }

            return name;
        }
    }
}
