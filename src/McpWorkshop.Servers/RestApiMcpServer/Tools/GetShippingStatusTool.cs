using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace RestApiMcpServer.Tools;

public class GetShippingStatusTool
{
    public static object GetDefinition()
    {
        return new
        {
            name = "get_shipping_status",
            description = "Obtiene el estado de envío y tracking de un pedido específico. IMPORTANTE: Esta herramienta requiere el parámetro 'orderId' (number, requerido) que es el ID numérico del pedido (ejemplo: 1001, 1002). Usa esta herramienta cuando te pregunten sobre el estado de envío, tracking, entrega o dónde está un pedido.",
            inputSchema = new Dictionary<string, object>
            {
                ["type"] = "object",
                ["properties"] = new Dictionary<string, object>
                {
                    ["orderId"] = new Dictionary<string, object>
                    {
                        ["type"] = "number",
                        ["description"] = "ID del pedido a rastrear"
                    }
                },
                ["required"] = new[] { "orderId" }
            }
        };
    }

    public static object Execute(Dictionary<string, JsonElement> arguments)
    {
        if (!arguments.ContainsKey("orderId"))
            throw new ArgumentException("El parámetro 'orderId' es requerido");

        var orderId = arguments["orderId"].GetInt32();

        // Simulate API call delay
        System.Threading.Thread.Sleep(150);

        // Simulate shipping status
        var random = new Random(orderId);
        var statuses = new[] { "pending", "shipped", "in_transit", "delivered" };
        var status = statuses[random.Next(0, statuses.Length)];
        var trackingNumber = $"ES{orderId:D6}{random.Next(1000, 9999)}";
        var carrier = new[] { "DHL", "UPS", "Correos", "SEUR" }[random.Next(0, 4)];
        var estimatedDelivery = DateTime.UtcNow.AddDays(random.Next(1, 7));

        var statusEmoji = status switch
        {
            "pending" => "⏳",
            "shipped" => "📮",
            "in_transit" => "🚚",
            "delivered" => "✅",
            _ => "📦"
        };

        var result = new
        {
            content = new[]
            {
                new
                {
                    type = "text",
                    text = $"{statusEmoji} ESTADO DE ENVÍO - Pedido #{orderId}\n\n" +
                           $"Estado: {status.ToUpper()}\n" +
                           $"Número de seguimiento: {trackingNumber}\n" +
                           $"Transportista: {carrier}\n" +
                           $"Entrega estimada: {estimatedDelivery:yyyy-MM-dd}\n" +
                           $"Última actualización: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC"
                }
            }
        };

        // Trace: log result
        Console.WriteLine($"🔍 [get_shipping_status] Input: orderId={orderId}");
        Console.WriteLine($"📤 [get_shipping_status] Output: status={status}, carrier={carrier}, tracking={trackingNumber}");

        return result;
    }
}
