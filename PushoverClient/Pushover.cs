using System;
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
        private const string BASE_API_URL = "https://api.pushover.net/1/messages.json";

        /// <summary>
        /// The application key
        /// </summary>
        private string AppKey { get; set; }

        /// <summary>
        /// The default user or group key to send messages to
        /// </summary>
        private string DefaultUserGroupSendKey { get; set; }

        /// <summary>
        /// Create a pushover client using a source application key.
        /// </summary>
        /// <param name="appKey"></param>
        public Pushover(string appKey)
        {
            AppKey = appKey;
        }

        public PushResponse SendPush(PushoverMessage message)
        {
            //TODO: Create args from message
            try
            {
                var limit = 0;
                var remaining = 0;
                var reset = "";
                message.AppKey = AppKey;
                
                var response = BASE_API_URL.PostToUrl(message.ToArgs(), responseFilter: httpRes =>
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
            //TODO: Create args from message
            try
            {
                var limit = 0;
                var remaining = 0;
                var reset = "";
                message.AppKey = AppKey;

                var asyncResponse = await BASE_API_URL.PostToUrlAsync(message.ToArgs(), responseFilter: httpRes =>
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
    }
}