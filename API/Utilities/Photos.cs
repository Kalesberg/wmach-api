using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;
using API.Models;
using API.Utilities;
using Newtonsoft.Json.Linq;
using System.Net;

namespace API.Utilities
{
    public static class Photos
    {
        public static void Save(byte[] data, string Id, string saveLocation)
        {
            foreach (var size in Enum.GetValues(typeof(PhotoSize)))
            {
                var img = ProcessImage((PhotoSize)size, data);
                var converter = new ImageConverter();
                var imgByteArray = (byte[])converter.ConvertTo(img, typeof(byte[]));
                WritePhoto(Id, (PhotoSize)size, imgByteArray, saveLocation);
            }
        }

        public static Bitmap ProcessImage(PhotoSize size, byte[] data)
        {
            //CONVERTS BYTE ARRAY TO STREAM AND THEN TO PICTURE
            var stream = new MemoryStream(data, true);
            var image = Image.FromStream(stream);

            //CALCULATE A RATIO SO WHE KNOW WHAT THE ASPECT LOOKS LIKE. SCALE DYNAMICALLY BASED ON THIS
            decimal ratioY = Decimal.Divide(image.Height, image.Width);

            switch (size)
            {
                case PhotoSize.Small:
                    return ResizeImage(image, (int)(100 * ratioY), 100);
                case PhotoSize.Medium:
                    return ResizeImage(image, (int)(500 * ratioY), 500);
                case PhotoSize.Large:
                    return ResizeImage(image, (int)(1000 * ratioY), 1000);
                case PhotoSize.Original:
                    return ResizeImage(image, image.Height, image.Width);
                default:
                    return null;
            }

        }

        private static Bitmap ResizeImage(Image image, int height, int width)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static void WritePhoto(string fileName, PhotoSize size, byte[] byteArr, string saveLocation)
        {
            try
            {
                string fileDir = ConfigurationManager.AppSettings[saveLocation];
                var filePath = String.Format(fileDir, size.ToString(), fileName);
                var exists = File.Exists(filePath);
                if (!exists) File.WriteAllBytes(String.Format(filePath, size.ToString(), fileName), byteArr);
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }

    public enum PhotoSize
    {
        Small,
        Medium,
        Large,
        Original
    }

    //moving to cloud azure storage

    public static class StorageHelper
    {

        public static bool IsImage(string file)
        {
            string[] formats = new string[] { "jpg", "png", "gif", "jpeg" };

            return formats.Any(item => file == item);
        }
        public static bool IsFile(string file)
        {
            string[] formats = new string[] { "jpg", "png", "gif", "jpeg", "pdf", "doc", "docx", "csv", "xls", "xlsx", "txt" };

            return formats.Any(item => file == item);
        }
        public static bool IsVideo(string file)
        {
            string[] formats = new string[] { "avi", "mpeg", "mp4" };

            return formats.Any(item => file == item);
        }
        public static string FileType(string file)
        {
            switch (file)
            {
                case "image/jpeg":
                    return "jpeg";
                case "image/png":
                    return "png";
                case "image/gif":
                    return "gif";
                case "application/msword":
                    return "doc";
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
                    return "docx";
                case "application/pdf":
                    return "pdf";
                case "text/csv":
                    return "csv";
                case "application/vnd.ms-excel":
                    return "xls";
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                    return "xlsx";
                case "text/plain":
                    return "txt";
                case "video/x-msvideo":
                    return "avi";
                case "video/mpeg":
                    return "mpeg";
                case "video/mp4":
                    return "mp4";
                default:
                    return "";
            }
        }

        public static async Task<string> UploadFileToStorage(byte[] filebytes, string fileName, string containerName, string contentType, bool isImage, bool isPublic)
        {
            // Create storagecredentials object by reading the values from the configuration (appsettings.json)

            StorageCredentials storageCredentials = new StorageCredentials(ConfigurationManager.AppSettings["AccountName"], ConfigurationManager.AppSettings["AccountKey"]);

            // Create cloudstorage account by passing the storagecredentials
            CloudStorageAccount storageAccount = new CloudStorageAccount(storageCredentials, true);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Get reference to the blob container by passing the name by reading the value from the configuration (appsettings.json)
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            if (!container.Exists())
            {
                await container.CreateAsync();
                BlobContainerPermissions permissions = new BlobContainerPermissions
                {
                    PublicAccess = isPublic ? BlobContainerPublicAccessType.Blob : BlobContainerPublicAccessType.Off
                };
                await container.SetPermissionsAsync(permissions);
            }

            // Get the reference to the block blob from the container
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

            blockBlob.Properties.ContentType = contentType;
            // Upload the file
            await blockBlob.UploadFromByteArrayAsync(filebytes, 0, filebytes.Length);

            if (isImage)
            {
                CloudBlockBlob blockBlobl = container.GetBlockBlobReference(fileName.Replace("original", "large"));
                CloudBlockBlob blockBlobm = container.GetBlockBlobReference(fileName.Replace("original", "medium"));
                CloudBlockBlob blockBlobs = container.GetBlockBlobReference(fileName.Replace("original", "small"));
                blockBlobl.Properties.ContentType = contentType;
                blockBlobm.Properties.ContentType = contentType;
                blockBlobs.Properties.ContentType = contentType;
                byte[] large = savePhotosSize(filebytes, PhotoSize.Large);
                byte[] medium = savePhotosSize(filebytes, PhotoSize.Medium);
                byte[] small = savePhotosSize(filebytes, PhotoSize.Small);
                await blockBlobl.UploadFromByteArrayAsync(large, 0, large.Length);
                await blockBlobm.UploadFromByteArrayAsync(medium, 0, medium.Length);
                await blockBlobs.UploadFromByteArrayAsync(small, 0, small.Length);
            }

            return blockBlob.Uri.AbsoluteUri;
        }

        public static byte[] savePhotosSize(byte[] filebytes, PhotoSize size)
        {
            var img = Photos.ProcessImage((PhotoSize)size, filebytes);
            var converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));

        }

        public static string GetContainerSASToken(string uri)
        {
            StorageCredentials storageCredentials = new StorageCredentials(ConfigurationManager.AppSettings["AccountName"], ConfigurationManager.AppSettings["AccountKey"]);
            string uriContainer = uri.Substring(0, IndexOfNth(uri, "/", 4));
            Uri urilink = new Uri(uriContainer);

            CloudBlobContainer container = new CloudBlobContainer(urilink, storageCredentials);
            // Create a new access policy for the account.
            SharedAccessBlobPolicy policy = new SharedAccessBlobPolicy()
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessExpiryTime = DateTime.UtcNow.AddDays(7)
            };

            // Return the SAS token.
            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
            string token = container.GetSharedAccessSignature(policy);
            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => false;
            return token;
        }
        public static string GetBlobSASToken(string uri)
        {
            StorageCredentials storageCredentials = new StorageCredentials(ConfigurationManager.AppSettings["AccountName"], ConfigurationManager.AppSettings["AccountKey"]);
            Uri urilink = new Uri(uri);

            CloudBlockBlob blob = new CloudBlockBlob(urilink, storageCredentials);
            // Create a new access policy for the account.
            SharedAccessBlobPolicy policy = new SharedAccessBlobPolicy()
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(24)
            };

            // Return the SAS token.
            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
            string token = blob.GetSharedAccessSignature(policy);
            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => false;
            return token;
        }


        public static int IndexOfNth(string str, string c, int n)
        {
            int s = -1;

            for (int i = 0; i < n; i++)
            {
                s = str.IndexOf(c, s + 1);

                if (s == -1) break;
            }

            return s;
        }


    }

}

namespace API.Models
{
    public class Files
    {
        public int ID { get; set; }
        public string Data { get; set; }
    }

}

namespace API.Data
{
    public partial class DAL
    {
        public async Task<List<Files>> getFileURLFromCloud(List<Files> tokens, string containerName, string folderName, bool isPublic, bool isImage, bool isVideo = false)
        {
            List<Files> files = tokens;
            int imageLimitSize = 5000000; //5mb
            int fileLimitSize = 20000000; //20mb
            int videoLimitSize = 100000000; //100mb
            int limitSize = isImage ? imageLimitSize : isVideo ? videoLimitSize : fileLimitSize;
            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;

            var tasks = files.Select(async f =>
            {
                string type = f.Data.Substring(f.Data.IndexOf(":") + 1, f.Data.IndexOf(";") - f.Data.IndexOf(":") - 1);
                string file = f.Data.Substring(f.Data.IndexOf(",") + 1);
                string filetype = StorageHelper.FileType(type);
                if (isImage ? StorageHelper.IsImage(filetype) : isVideo ? StorageHelper.IsVideo(filetype) : StorageHelper.IsFile(filetype))
                {
                    byte[] fileBytes = Convert.FromBase64String(file);
                    if (fileBytes.Length <= limitSize)
                    {
                        var result = await StorageHelper.UploadFileToStorage(fileBytes, folderName + Guid.NewGuid().ToString() + "." + filetype, containerName, type, isImage, isPublic);
                        f.Data = result;
                    }
                    else //file size too big
                        f.Data = "error";
                }
                else //format is incorrect
                    f.Data = "error";
            });
            await Task.WhenAll(tasks);
            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => false;
            return files;

        }
    }

}
