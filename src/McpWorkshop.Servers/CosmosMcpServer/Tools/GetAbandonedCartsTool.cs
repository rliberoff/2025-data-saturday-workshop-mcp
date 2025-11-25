using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

using CosmosMcpServer.Models;

namespace CosmosMcpServer.Tools;

/// <summary>
/// MCP tool for retrieving abandoned shopping carts within a specified time period.
/// </summary>
public static class GetAbandonedCartsTool
{
    /// <summary>
    /// Gets the tool definition for MCP protocol.
    /// </summary>
    /// <returns>An object containing the tool definition with name, description, and input schema.</returns>
    public static object GetDefinition()
    {
        return new
        {
            name = "get_abandoned_carts",
            description = "Obtiene carritos abandonados en las últimas N horas. IMPORTANTE: Esta herramienta requiere el parámetro 'hours' (number, opcional, por defecto 24). Usa esta herramienta cuando te pregunten sobre carritos abandonados o usuarios que no completaron sus compras.",
            inputSchema = new Dictionary<string, object>
            {
                ["type"] = "object",
                ["properties"] = new Dictionary<string, object>
                {
                    ["hours"] = new Dictionary<string, object>
                    {
                        ["type"] = "number",
                        ["description"] = "Número de horas hacia atrás para buscar carritos abandonados (ejemplo: 24, 48, 72). Por defecto: 24",
                        ["default"] = 24,
                    }
                },
            },
        };
    }

    /// <summary>
    /// Executes the abandoned carts query filtering by time period.
    /// </summary>
    /// <param name="arguments">Dictionary containing optional hours parameter.</param>
    /// <param name="abandonedCarts">Array of abandoned carts to filter.</param>
    /// <returns>An object containing the filtered abandoned carts list and summary statistics.</returns>
    public static object Execute(Dictionary<string, JsonElement> arguments, AbandonedCart[] abandonedCarts)
    {
        var hours = 24;
        if (arguments.ContainsKey("hours") && arguments["hours"].ValueKind == JsonValueKind.Number)
        {
            hours = arguments["hours"].GetInt32();
        }

        // Filtrar carritos abandonados por las horas especificadas
        var filteredCarts = abandonedCarts
            .Where(cart => cart.HoursAgo <= hours)
            .OrderByDescending(cart => cart.TotalValue)
            .ToList();

        var totalValue = filteredCarts.Sum(cart => cart.TotalValue);
        var totalItems = filteredCarts.Sum(cart => cart.Items.Length);

        var summary = $"🛒 CARRITOS ABANDONADOS (últimas {hours} horas)\n\n" +
                      $"Total carritos: {filteredCarts.Count}\n" +
                      $"Total productos: {totalItems}\n" +
                      $"Valor total: €{totalValue:N2}\n" +
                      $"Valor promedio: €{(filteredCarts.Count > 0 ? totalValue / filteredCarts.Count : 0):N2}\n\n" +
                      $"Detalles:\n" +
                      string.Join("\n", filteredCarts.Select(cart =>
                          $"- {cart.UserId}: {cart.Items.Length} producto(s), €{cart.TotalValue:N2}, abandonado hace {cart.HoursAgo}h"));

        var result = new
        {
            summary,
            totalCarts = filteredCarts.Count,
            totalValue,
            totalItems,
            carts = filteredCarts.Select(cart => new
            {
                cart.Id,
                cart.UserId,
                cart.TotalValue,
                cart.HoursAgo,
                cart.AbandonedAt,
                ItemCount = cart.Items.Length,
                Items = cart.Items.Select(item => new
                {
                    item.ProductId,
                    item.Price,
                    item.Quantity,
                }),
            }),
        };

        // Trace: log result
        Console.WriteLine($"🔍 [get_abandoned_carts] Input: hours={hours}");
        Console.WriteLine($"📤 [get_abandoned_carts] Output: {filteredCarts.Count} abandoned carts found, total value: €{totalValue:N2}");

        return result;
    }
}
