using System;

namespace CosmosMcpServer.Models;

/// <summary>
/// Represents an abandoned shopping cart.
/// </summary>
public class AbandonedCart
{
    /// <summary>
    /// Gets or sets the unique identifier for the abandoned cart.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user identifier associated with this abandoned cart.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the items in the abandoned cart.
    /// </summary>
    public CartItem[] Items { get; set; } = Array.Empty<CartItem>();

    /// <summary>
    /// Gets or sets the total value of all items in the cart.
    /// </summary>
    public decimal TotalValue { get; set; }

    /// <summary>
    /// Gets or sets the number of hours since the cart was abandoned.
    /// </summary>
    public int HoursAgo { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the cart was abandoned.
    /// </summary>
    public DateTime AbandonedAt { get; set; }
}
