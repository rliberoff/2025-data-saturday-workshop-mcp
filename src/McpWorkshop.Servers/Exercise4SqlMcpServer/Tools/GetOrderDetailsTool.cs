using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Exercise4SqlMcpServer.Models;

namespace Exercise4SqlMcpServer.Tools;

public static class GetOrderDetailsTool
{
    public static object GetDefinition()
    {
        return new
        {
            name = "get_order_details",
            description = "Obtiene información detallada de un pedido específico, incluyendo cliente, producto, cantidad y monto total. IMPORTANTE: Esta herramienta requiere el parámetro 'orderId' (integer). Usa esta herramienta cuando te pregunten sobre un pedido específico por su número o ID (ejemplo: 'pedido 1001', 'pedido número 1001', 'order 1001').",
            inputSchema = new Dictionary<string, object>
            {
                ["type"] = "object",
                ["properties"] = new Dictionary<string, object>
                {
                    ["orderId"] = new Dictionary<string, object>
                    {
                        ["type"] = "integer",
                        ["description"] = "El número o ID del pedido a consultar (ejemplo: 1001, 1002, etc.)"
                    }
                },
                ["required"] = new[] { "orderId" }
            }
        };
    }

    public static object Execute(Dictionary<string, JsonElement> arguments, Order[] orders, Customer[] customers, Product[] products)
    {
        if (!arguments.ContainsKey("orderId"))
        {
            throw new ArgumentException("Se requiere el parámetro 'orderId'");
        }

        var orderId = arguments["orderId"].GetInt32();

        var order = orders.FirstOrDefault(o => o.Id == orderId);

        if (order == null)
        {
            var notFoundResult = new
            {
                found = false,
                message = $"No se encontró el pedido con ID {orderId}"
            };

            // Trace: log result
            Console.WriteLine($"🔍 [get_order_details] Input: orderId={orderId}");
            Console.WriteLine($"📤 [get_order_details] Output: Order not found");

            return notFoundResult;
        }

        var customer = customers.FirstOrDefault(c => c.Id == order.CustomerId);
        var product = products.FirstOrDefault(p => p.Id == order.ProductId);

        var result = new
        {
            found = true,
            order = new
            {
                id = order.Id,
                customerId = order.CustomerId,
                customerName = customer?.Name ?? "Unknown",
                customerEmail = customer?.Email,
                productId = order.ProductId,
                productName = product?.Name ?? "Unknown",
                quantity = order.Quantity,
                totalAmount = order.TotalAmount,
                orderDate = order.OrderDate,
                status = order.Status
            }
        };

        // Trace: log result
        Console.WriteLine($"🔍 [get_order_details] Input: orderId={orderId}");
        Console.WriteLine($"📤 [get_order_details] Output: Order found - customer={customer?.Name}, product={product?.Name}, amount=€{order.TotalAmount}, status={order.Status}");

        return result;
    }
}
