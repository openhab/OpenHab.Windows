namespace openHAB.Core.Messages;

/// <summary>
/// Represents a message containing information about a trigger event.
/// </summary>
public class TriggerInfoMessage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TriggerInfoMessage"/> class.
    /// </summary>
    /// <param name="messageType">The severity level of the message.</param>
    /// <param name="message">The content of the message.</param>
    public TriggerInfoMessage(MessageSeverity messageType, string message)
    {
        Severity = messageType;
        Message = message;
    }

    /// <summary>
    /// Gets the severity level of the message.
    /// </summary>
    public MessageSeverity Severity
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets the content of the message.
    /// </summary>
    public string Message
    {
        get;
        private set;
    }
}
