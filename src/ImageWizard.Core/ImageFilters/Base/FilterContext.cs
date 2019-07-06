﻿using ImageWizard.ImageFormats;
using ImageWizard.ImageFormats.Base;
using ImageWizard.Settings;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Filters
{
    /// <summary>
    /// FilterContext
    /// </summary>
    public class FilterContext
    {
        public FilterContext(ImageWizardSettings settings, Image<Rgba32> image, IImageFormat imageFormat)
        {
            Settings = settings;
            Image = image;
            ImageFormat = imageFormat;
        }

        /// <summary>
        /// Settings
        /// </summary>
        public ImageWizardSettings Settings { get; }

        /// <summary>
        /// Image
        /// </summary>
        public Image<Rgba32> Image { get; }

        /// <summary>
        /// ImageFormat
        /// </summary>
        public IImageFormat ImageFormat { get; set; }
    }
}
