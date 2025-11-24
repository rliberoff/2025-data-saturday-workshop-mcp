using System;

namespace Exercise4CosmosMcpServer.Models;

public class UserSession
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int PagesViewed { get; set; }
    public string[] Actions { get; set; } = Array.Empty<string>();
}

public class CartEvent
{
    public string UserId { get; set; } = string.Empty;
    public int ProductId { get; set; }
    public string Action { get; set; } = string.Empty; // addToCart, removeFromCart, checkout
    public DateTime Timestamp { get; set; }
    public int Quantity { get; set; }
}

public class AbandonedCart
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public CartItem[] Items { get; set; } = Array.Empty<CartItem>();
    public decimal TotalValue { get; set; }
    public int HoursAgo { get; set; }
    public DateTime AbandonedAt { get; set; }
}

public class CartItem
{
    public int ProductId { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
