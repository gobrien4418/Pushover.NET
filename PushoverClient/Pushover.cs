﻿using System;
using ServiceStack;
using System.Net;
using System.Threading.Tasks;

namespace PushoverClient
{
    /// <summary>
    /// Client library for using Pushover for push notifications.  
    /// See https://pushover.net/api for more information
    /// </summary>
    public class Pushover
    {
        /// <summary>
        /// Base url for the API
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private const string BASE_MESSAGE_API_URL = "https://api.pushover.net/1/messages.json";
        private const string BASE_LICENSE_API_URL = "https://api.pushover.net/1/licenses/assign.json";
        /// <summary>
        /// The application key
        /// </summary>
        private string _appKey { get; set; }

        /// <summary>
        /// The default user or group key to send messages to
        /// </summary>
        private string _defaultUserGroupSendKey { get; set; }

        /// <summary>
        /// Create a pushover client using a source application key.
        /// </summary>
        /// <param name="appKey"></param>
        public Pushover(string appKey, string defaultUserGroupSendKey = null)
        {
            _appKey = appKey;
            _defaultUserGroupSendKey = defaultUserGroupSendKey;
        }

        public PushResponse SendPush(PushoverMessage message)
        {
            try
            {
                var limit = 0;
                var remaining = 0;
                var reset = "";
                message.AppKey = _appKey;
                if (message.Recipients.Count < 1 && _defaultUserGroupSendKey != null)
                    message.Recipients.Add(_defaultUserGroupSendKey);

                var response = BASE_MESSAGE_API_URL.PostToUrl(message.ToArgs(), responseFilter: httpRes =>
                    {
                        int.TryParse(httpRes.Headers["X-Limit-App-Limit"], out limit);
                        int.TryParse(httpRes.Headers["X-Limit-App-Remaining"], out remaining);
                        reset = httpRes.Headers["X-Limit-App-Reset"] ?? "";
                    })
                    .FromJson<PushResponse>();
                response.Limits = new Limitations
                {
                    Limit = limit,
                    Remaining = remaining,
                    Reset = reset,
                };
                return response;
            }
            catch (WebException webEx)
            {
                return webEx.GetResponseBody().FromJson<PushResponse>();
            }
        }

        public async Task<PushResponse> SendPushAsync(PushoverMessage message)
        {
            try
            {
                var limit = 0;
                var remaining = 0;
                var reset = "";
                message.AppKey = _appKey;

                var asyncResponse = await BASE_MESSAGE_API_URL.PostToUrlAsync(message.ToArgs(), responseFilter: httpRes =>
                {
                    int.TryParse(httpRes.Headers["X-Limit-App-Limit"], out limit);
                    int.TryParse(httpRes.Headers["X-Limit-App-Remaining"], out remaining);
                    reset = httpRes.Headers["X-Limit-App-Reset"] ?? "";
                });

                var response = asyncResponse.FromJson<PushResponse>();
                response.Limits = new Limitations
                {
                    Limit = limit,
                    Remaining = remaining,
                    Reset = reset,
                };
                return response;
            }
            catch (WebException webEx)
            {
                return webEx.GetResponseBody().FromJson<PushResponse>();
            }
        }

        public LicenseResponse AssignLicense(string user)
        {
            var args = new { token = _appKey, user };
            try
            {
                return BASE_LICENSE_API_URL.PostToUrl(args).FromJson<LicenseResponse>();
            }
            catch (WebException webEx)
            {
                return webEx.GetResponseBody().FromJson<LicenseResponse>();
            }
        }
    }
}