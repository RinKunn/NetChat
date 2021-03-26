﻿using System;
using NetChat.Services.Messaging.Users;

namespace NetChat.Services.Messaging.Messages
{
    public class Message
    {
        public Message(MessageData messageData, UserData userData, Message replyToMessage)
        {
            MessageData = messageData ?? throw new ArgumentNullException(nameof(messageData));
            UserData = userData ?? throw new ArgumentNullException(nameof(userData));
            ReplyToMessage = replyToMessage;
        }
        public MessageData MessageData { get; }
        public UserData UserData { get; }
        public Message ReplyToMessage { get; }
    }
}
