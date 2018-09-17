using System;
using System.Collections.Generic;

namespace PushoverClient
{
    public class EmergencyMessage : PushoverMessage
    {
        public override Priority Priority
        {
            get => Priority.Emergency;
            set { throw new Exception("The Priority property of an Emergency Message may not be modified."); }
        }

        public int RetrySeconds { get; set; } = 30;
        public int ExpireSeconds { get; set; } = 180;

        public EmergencyMessage(List<string> recipients) : base(recipients) { Init(); }

        public EmergencyMessage()
        {
            Init();
        }

        private void Init()
        {
            Notification = NotificationSound.Persistent;
        }

        public override object ToArgs()
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
                priority = Priority.Emergency.ToString(),
                sound = NotificationSound.Persistent.ToString(),
                retry = RetrySeconds,
                expire = ExpireSeconds
            };
        }
    }
}
