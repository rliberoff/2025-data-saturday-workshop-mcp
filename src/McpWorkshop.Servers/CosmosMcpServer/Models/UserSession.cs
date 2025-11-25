using System;

namespace CosmosMcpServer.Models;

/// <summary>
/// Represents a user session with browsing activity metrics.
/// </summary>
public class UserSession
{
    /// <summary>
    /// Gets or sets the unique identifier for the session.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user identifier associated with this session.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the start time of the session.
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Gets or sets the end time of the session.
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Gets or sets the number of pages viewed during this session.
    /// </summary>
    public int PagesViewed { get; set; }

    /// <summary>
    /// Gets or sets the list of actions performed during this session.
    /// </summary>
    public string[] Actions { get; set; } = Array.Empty<string>();
}
