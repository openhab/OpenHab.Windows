namespace openHAB.Core.Messages;


/// <summary>
/// Specifies the action to be taken.
/// </summary>
public enum Action
{
    /// <summary>
    /// Reload the data.
    /// </summary>
    Reload,

    /// <summary>
    /// No action.
    /// </summary>
    None
}

/// <summary>
/// Represents a trigger action.
/// </summary>
public class TriggerAction
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TriggerAction"/> class with the specified action and state.
    /// </summary>
    /// <param name="action">The action to be taken.</param>
    /// <param name="state">The state of the trigger action.</param>
    public TriggerAction(Action action = Action.None, object state = null)
    {
        Action = action;
        State = state;
    }

    /// <summary>
    /// Gets or sets the action to be taken.
    /// </summary>
    public Action Action
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the state of the trigger action.
    /// </summary>
    public object? State
    {
        get; set;
    }
}
