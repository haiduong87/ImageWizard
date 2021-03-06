using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ImageWizard.MongoDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ImageWizard.Analytics;
using ImageWizard.Azure;
using System.Security.Cryptography;
using Microsoft.AspNetCore.WebUtilities;
using ImageWizard.Filters;
using ImageWizard.ImageSharp.Filters;
using ImageWizard.AWS;
using ImageWizard.SkiaSharp;
using ImageWizard.Core.Types;
using ImageWizard.Settings;
using Microsoft.Extensions.Options;
using ImageWizard.FFMpegCore;

namespace ImageWizard.TestApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //generate random key
            byte[] keyBuffer = new byte[64];
            RandomNumberGenerator.Create().GetBytes(keyBuffer);

            string key = WebEncoders.Base64UrlEncode(keyBuffer);
            //string key = "DEMO-KEY---PLEASE-CHANGE-THIS-KEY---PLEASE-CHANGE-THIS-KEY---PLEASE-CHANGE-THIS-KEY---==";

            services.AddImageWizard(x =>
            {
#if DEBUG
                x.AllowUnsafeUrl = true;
#else
                x.AllowUnsafeUrl = false;
#endif
                x.UseClintHints = true;
                x.UseETag = true;
                x.Key = key;
            })
                .AddImageSharp(MimeTypes.Jpeg, MimeTypes.Png, MimeTypes.Gif, MimeTypes.Bmp)
                    .WithOptions(x =>
                                {
                                    x.ImageMaxHeight = 4000;
                                    x.ImageMaxWidth = 4000;
                                })
                    .WithFilter<ResizeFilter>()
                //.AddSkiaSharp()
                //    .WithOptions(x =>
                //                {
                //                    x.ImageMaxHeight = 4000;
                //                    x.ImageMaxWidth = 4000;
                //                })
                //    .WithFilter<ImageWizard.SkiaSharp.Filters.ResizeFilter>()
                .AddSvgNet()
                //.SetFileCache()
                //.SetMongoDBCache()
                .AddHttpLoader(x =>
                {
                    //x.RefreshMode = ImageLoaderRefreshMode.EveryTime;
                    x.SetHeader("Api", "XYZ");
                })
                .AddFileLoader()
                .AddYoutubeLoader()
                .AddGravatarLoader()
                .AddFFMpegCore()
                .AddAnalytics()
                .AddAzureBlob(x =>
                {
                    x.ConnectionString = "";
                    x.ContainerName = "MyContainer";
                })
                .AddAWS(x =>
                {
                    x.AccessKeyId = "";
                    x.SecretAccessKey = "";
                    x.BucketName = "MyBucket";
                })
                ;

            services.AddImageWizardClient(x =>
            {
#if DEBUG
                x.UseUnsafeUrl = true;
#endif
                x.Key = key;
            });

            services.AddRazorPages();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<ImageWizardOptions> options)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(x =>
            {
                x.MapRazorPages();
                x.MapControllers();
                x.MapImageWizard(options.Value.BasePath);
            });
        }
    }
}
