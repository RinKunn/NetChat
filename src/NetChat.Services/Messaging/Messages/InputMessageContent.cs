namespace NetChat.Services.Messaging.Messages
{
    public abstract class InputMessageContent
    {

    }

    public class InputMessageTextContent : InputMessageContent
    {
        public string Text { get; }

        public InputMessageTextContent(string text)
        {
            Text = text;
        }
    }
}
