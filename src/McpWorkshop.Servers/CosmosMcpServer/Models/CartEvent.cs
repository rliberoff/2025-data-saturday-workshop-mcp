using System;

namespace CosmosMcpServer.Models;

/// <summary>
/// Represents an event related to shopping cart activities.
/// </summary>
public class CartEvent
{
    /// <summary>
    /// Gets or sets the user identifier associated with this cart event.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product identifier involved in this cart event.
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the action performed (e.g., addToCart, removeFromCart, checkout).
    /// </summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp when this event occurred.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the quantity of products involved in this event.
    /// </summary>
    public int Quantity { get; set; }
}
