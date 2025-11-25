using System;

namespace SqlMcpServer.Models;

/// <summary>
/// Represents an order entity.
/// </summary>
public class Order
{
    /// <summary>
    /// Gets or sets the unique identifier for the order.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the customer identifier associated with this order.
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the product identifier for the ordered item.
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity of products ordered.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the total amount for this order.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the order was placed.
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// Gets or sets the current status of the order.
    /// </summary>
    public string Status { get; set; } = "Confirmed";
}
