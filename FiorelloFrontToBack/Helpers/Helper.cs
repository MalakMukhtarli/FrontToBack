﻿using FiorelloFrontToBack.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FiorelloFrontToBack.Helpers
{
    public static class Helper
    {
        /// <summary>
        /// deletes images from folders
        /// </summary>
        /// <param name="root"> root </param>
        /// <param name="folder">the folder where the image you want to delete is located</param>
        /// <param name="sliderSelected"> the object of the image you want to delete </param>
        /// <returns></returns>
        public static bool DeletedPhoto(string root, string folder, Slider sliderSelected)
        {
            string pathOldImage = Path.Combine(root, folder, sliderSelected.Image);
            if (File.Exists(pathOldImage))
            {
                File.Delete(pathOldImage);
                return true;
            }
            return false;
        }
    }
}
