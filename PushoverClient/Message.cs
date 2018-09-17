using System;
using System.Collections.Generic;

namespace PushoverClient
{
    public class PushoverMessage
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public List<string> Recipients { get; set; } = new List<string>();
        public bool Html { get; set; }
        public string Url { get; set; } = "";
        public string UrlTitle { get; set; } = "";
        public virtual Priority Priority { get; set; } = Priority.Normal;
        public NotificationSound Notification { get; set; } = NotificationSound.Pushover;
        public string AppKey { get; set; }

        public PushoverMessage() { }

        public PushoverMessage(List<string> recipients)
        {
            Recipients = recipients;
        }

        public virtual object ToArgs()
        {
            ValidateArgs();

            return new
            {
                token = AppKey,
                user = String.Join(",", Recipients),
                title = Title,
                message = Text,
                html = Html ? "1" : "0",
                url = Url,
                url_title = UrlTitle,
                priority = Priority.ToString(),
                sound = Notification.ToString()
            };

        }

        protected void ValidateArgs()
        {
            if (Recipients == null || Recipients.Count < 1)
            {
                throw new ArgumentException("Recipients must be supplied", nameof(Recipients));
            }

            //We have a list of recipients, max is 50.
            if (Recipients.Count > 50)
            {
                throw new ArgumentException("Recipients exceeded the maximum of 50", nameof(Recipients));
            }

            if (!string.IsNullOrWhiteSpace(Title))
            {
                if (Title.Length > 250)
                {
                    throw new ArgumentException("Title is limited to 250 characters", nameof(Title));
                }
            }

            if (string.IsNullOrWhiteSpace(Text))
            {
                throw new ArgumentException("Message must be supplied", nameof(Text));
            }

            if (Text.Length > 1024)
            {
                throw new ArgumentException("Message is limited to 1024 characters", nameof(Text));
            }

            if (string.IsNullOrWhiteSpace(Url) && !string.IsNullOrWhiteSpace(UrlTitle))
            {
                throw new ArgumentException("Url must be supplied when UrlTitle is set", nameof(Url));
            }


            if (!string.IsNullOrWhiteSpace(UrlTitle))
            {
                if (UrlTitle.Length > 100)
                {
                    throw new ArgumentException("UrlTitle is limited to 100 characters", nameof(UrlTitle));
                }
            }

            if (!string.IsNullOrWhiteSpace(Url))
            {
                if (Url.Length > 512)
                {
                    throw new ArgumentException("Url is limited to 512 characters", nameof(Url));
                }
            }
        }
    }
}