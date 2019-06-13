﻿using ImageWizard.Services.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ImageWizard.ImageLoaders
{
    /// <summary>in
    /// ImageDownloader
    /// </summary>
    public class HttpLoader : IImageLoader
    {
        public HttpLoader(HttpClient httpCLient)
        {
            HttpClient = httpCLient;
        }

        /// <summary>
        /// HttpClient
        /// </summary>
        private HttpClient HttpClient { get; }

        /// <summary>
        /// Download
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public async Task<OriginalImage> GetAsync(string requestUri)
        {
            HttpResponseMessage response = await HttpClient.GetAsync(requestUri);
            byte[] data = await response.Content.ReadAsByteArrayAsync();

            return new OriginalImage(requestUri, response.Content.Headers.ContentType.MediaType, data);
        }
    }
}