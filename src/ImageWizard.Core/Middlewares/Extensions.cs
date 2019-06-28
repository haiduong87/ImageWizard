﻿using ImageWizard.Core.ImageCaches;
using ImageWizard.Core.ImageLoaders;
using ImageWizard.Core.Middlewares;
using ImageWizard.Core.Settings;
using ImageWizard.Filters;
using ImageWizard.Filters.ImageFormats;
using ImageWizard.ImageLoaders;
using ImageWizard.ImageStorages;
using ImageWizard.Middlewares;
using ImageWizard.Settings;
using ImageWizard.SharedContract;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard
{
    public static class Extensions
    {
        public static IApplicationBuilder UseImageWizard(this IApplicationBuilder app)
        {
            app.UseMiddleware<ImageWizardMiddleware>();

            return app;
        }

        public static IImageWizardBuilder AddImageWizard(this IServiceCollection services, ImageWizardCoreSettings settings)
        {
            services.AddSingleton(x => new CryptoService(settings.Key));
            services.AddSingleton(settings);

            ImageWizardBuilder configuration = new ImageWizardBuilder(services);

            services.AddSingleton(configuration);

            return configuration;
        }
   
    }
}