namespace CosmosMcpServer.Models;

/// <summary>
/// Represents an item in a shopping cart.
/// </summary>
public class CartItem
{
    /// <summary>
    /// Gets or sets the product identifier.
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the price of the product.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the quantity of this product in the cart.
    /// </summary>
    public int Quantity { get; set; }
}
