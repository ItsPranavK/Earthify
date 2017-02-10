using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;

namespace Earthify.Backend
{
    /// <summary>
    /// Main class of Earthify. Backend used to generate app data and provide important functions.
    /// </summary>
    public class EathifyData
    {
        /// <summary>
        /// Stores all data items
        /// </summary>
        public static List<ImageModel> img_data;

        /// <summary>
        /// Initializes app data and generates data items.
        /// </summary>
        public static async Task InitializeData()
        {
            img_data = new List<ImageModel>();

            // Retrieves data.
            StorageFile dataFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Earthify.Backend/data.txt"));
            string dataBuffer = await FileIO.ReadTextAsync(dataFile);

            // Parses data buffer.
            XElement xml = XElement.Parse(dataBuffer);
            List<XElement> elements = xml.Descendants("item").ToList();

            foreach (XElement item in elements)
            {
                // Generates data item for images.
                ImageModel img = new ImageModel();

                img.image_url = item.Element("image").Value;
                img.country = item.Element("country").Value;
                img.map = item.Element("map").Value;
                img.region = item.Element("region").Value;

                img_data.Add(img);
            }

            // Clears temporary data.
            dataFile = null;
            dataBuffer = null;
            xml = null;
            elements = null;
        }

        /// <summary>
        /// Generates and returns a random image from data items.
        /// </summary>
        public static async Task<ImageModel> GenerateRandomImage()
        {
            // Generates a random number 
            int random_int = new Random().Next(0, (img_data.Count - 1));

            return img_data[random_int];
        }

        /// <summary>
        /// Changes the image by generating a new image based on argument.
        /// </summary>
        /// <param name="currentImage">Current selected image</param>
        /// <param name="val">If integer is greater than or equal to 0, next image is generated. If integer is less than 0, previous image is generated</param>
        public static async Task<ImageModel> ChangeImage(ImageModel currentImage, int val)
        {
            ImageModel img = new ImageModel();

            if (val >= 0)
            {
                int currentIndex = img_data.IndexOf(currentImage);

                if ((currentIndex + 1) != img_data.Count)
                {
                    img = img_data[currentIndex + 1];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                int currentIndex = img_data.IndexOf(currentImage);

                if ((currentIndex - 1) >= 0)
                {
                    img = img_data[currentIndex - 1];
                }
                else
                {
                    return null;
                }

            }

            return img;
        }
    }
}
