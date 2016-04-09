using System;
using System.IO;
using System.Web.Security;

namespace FrameworkLibrary
{
    public class ImageHelper
    {
        public static void ResizeImage(string OriginalFile, int NewWidth, int MaxHeight, bool OnlyResizeIfWider)
        {
            if (OriginalFile.StartsWith("."))
                return;

            try
            {
                FileInfo OrigFileInfo = new FileInfo(OriginalFile);

                System.Drawing.Image FullsizeImage = System.Drawing.Image.FromFile(OriginalFile);

                // Prevent using images internal thumbnail
                FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
                FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);

                if (OnlyResizeIfWider)
                {
                    if (FullsizeImage.Width <= NewWidth)
                    {
                        NewWidth = FullsizeImage.Width;
                    }
                }

                int NewHeight = FullsizeImage.Height * NewWidth / FullsizeImage.Width;
                if (NewHeight > MaxHeight)
                {
                    // Resize with height instead
                    NewWidth = FullsizeImage.Width * MaxHeight / FullsizeImage.Height;
                    NewHeight = MaxHeight;
                }

                System.Drawing.Image NewImage = FullsizeImage.GetThumbnailImage(NewWidth, NewHeight, null, IntPtr.Zero);

                // Clear handle to original file so that we can overwrite it if necessary
                FullsizeImage.Dispose();

                // Save resized picture
                NewImage.Save(OrigFileInfo.DirectoryName + "/resized/" + OrigFileInfo.Name);
            }
            catch (Exception ex)
            {
                ErrorHelper.LogException(ex);
            }
        }

        public static string GetGravatarImageURL(string emailId, int imgSize, bool identicon = false)
        {
            string hash = string.Empty;
            string imageURL = string.Empty;

            // Convert emailID to lower-case
            emailId = emailId.ToLower();
            hash = FormsAuthentication.HashPasswordForStoringInConfigFile(emailId, "MD5").ToLower();

            var parameters = "";

            if (identicon)
                parameters = "&d=identicon";

            // build Gravatar Image URL
            imageURL = "http://www.gravatar.com/avatar/" + hash + ".jpg?s=" + imgSize + parameters;

            return imageURL;
        }
    }
}