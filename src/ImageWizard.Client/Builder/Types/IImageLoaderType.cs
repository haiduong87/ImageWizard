﻿using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Client.Builder.Types
{
    public interface IImageLoaderType : IImageBuildUrl, IImageUrlBuilder
    {
        IImageFilters Image(string loaderType, string loaderSource);
    }
}
