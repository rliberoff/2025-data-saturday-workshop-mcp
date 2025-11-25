using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

using SqlMcpServer.Models;

namespace SqlMcpServer.Tools;

/// <summary>
/// MCP tool for calculating aggregated sales statistics.
/// </summary>
public static class GetSalesSummaryTool
{
    /// <summary>
    /// Gets the tool definition for MCP protocol.
    /// </summary>
    /// <returns>An object containing the tool definition with name, description, and input schema.</returns>
    public static object GetDefinition()
    {
        return new
        {
            name = "get_sales_summary",
            description = "Calcula estadísticas agregadas de ventas: total de pedidos, ventas totales, ticket promedio, productos más vendidos. Usa esta herramienta para preguntas sobre métricas generales de ventas (ejemplo: 'estadísticas de ventas', 'total de ventas', 'productos más vendidos').",
            inputSchema = new Dictionary<string, object>
            {
                ["type"] = "object",
                ["properties"] = new Dictionary<string, object>
                {
                    ["startDate"] = new Dictionary<string, object>
                    {
                        ["type"] = "string",
                        ["description"] = "Fecha inicial del período a analizar (opcional, formato: YYYY-MM-DD, ejemplo: '2024-01-01')",
                    },
                    ["endDate"] = new Dictionary<string, object>
                    {
                        ["type"] = "string",
                        ["description"] = "Fecha final del período a analizar (opcional, formato: YYYY-MM-DD, ejemplo: '2024-12-31')",
                    },
                    ["status"] = new Dictionary<string, object>
                    {
                        ["type"] = "string",
                        ["description"] = "Filtrar por estado del pedido (opcional, valores: 'completed', 'pending', 'cancelled')",
                    }
                },
            },
        };
    }

    /// <summary>
    /// Executes the sales summary calculation with optional date range and status filtering.
    /// </summary>
    /// <param name="arguments">Dictionary containing optional startDate, endDate, and status parameters.</param>
    /// <param name="orders">Array of orders to analyze.</param>
    /// <returns>An object containing the sales summary and statistics.</returns>
    public static object Execute(Dictionary<string, JsonElement> arguments, Order[] orders)
    {
        DateTime? startDate = null;
        DateTime? endDate = null;
        string? status = null;

        if (arguments.ContainsKey("startDate") && arguments["startDate"].ValueKind == JsonValueKind.String)
        {
            DateTime.TryParse(arguments["startDate"].GetString(), out var start);
            startDate = start;
        }

        if (arguments.ContainsKey("endDate") && arguments["endDate"].ValueKind == JsonValueKind.String)
        {
            DateTime.TryParse(arguments["endDate"].GetString(), out var end);
            endDate = end;
        }

        if (arguments.ContainsKey("status") && arguments["status"].ValueKind == JsonValueKind.String)
        {
            status = arguments["status"].GetString();
        }

        var filtered = orders.AsEnumerable();

        if (startDate.HasValue)
        {
            filtered = filtered.Where(o => o.OrderDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            filtered = filtered.Where(o => o.OrderDate <= endDate.Value);
        }

        if (!string.IsNullOrEmpty(status))
        {
            filtered = filtered.Where(o => o.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
        }

        var ordersList = filtered.ToList();
        var totalSales = ordersList.Sum(o => o.TotalAmount);
        var totalOrders = ordersList.Count;
        var averageOrderValue = totalOrders > 0 ? totalSales / totalOrders : 0;

        var statusBreakdown = ordersList
            .GroupBy(o => o.Status)
            .Select(g => new { status = g.Key, count = g.Count(), total = g.Sum(o => o.TotalAmount) })
            .ToList();

        var periodText = startDate.HasValue && endDate.HasValue
            ? $"del {startDate:yyyy-MM-dd} al {endDate:yyyy-MM-dd}"
            : startDate.HasValue
                ? $"desde {startDate:yyyy-MM-dd}"
                : endDate.HasValue
                    ? $"hasta {endDate:yyyy-MM-dd}"
                    : "todo el período";

        object textContent = new Dictionary<string, object>
        {
            ["type"] = "text",
            ["text"] = $"📊 RESUMEN DE VENTAS {periodText}\n\n" +
                       $"Total Ventas: €{totalSales:F2}\n" +
                       $"Total Pedidos: {totalOrders}\n" +
                       $"Valor Promedio: €{averageOrderValue:F2}\n\n" +
                       $"Desglose por Estado:\n" +
                       string.Join("\n", statusBreakdown.Select(s => $"- {s.status}: {s.count} pedidos (€{s.total:F2})")),
        };

        object resourceContent = new Dictionary<string, object>
        {
            ["type"] = "resource",
            ["resource"] = new Dictionary<string, object>
            {
                ["uri"] = "sql://workshop/sales-summary",
                ["mimeType"] = "application/json",
                ["text"] = JsonSerializer.Serialize(new
                {
                    summary = new
                    {
                        totalSales,
                        totalOrders,
                        averageOrderValue,
                        period = periodText,
                    },
                    breakdown = statusBreakdown,
                }),
            },
        };

        var result = new Dictionary<string, object>
        {
            ["content"] = new[] { textContent, resourceContent },
        };

        // Trace: log result
        Console.WriteLine($"🔍 [get_sales_summary] Input: startDate={startDate?.ToString("yyyy-MM-dd") ?? "null"}, endDate={endDate?.ToString("yyyy-MM-dd") ?? "null"}, status={status ?? "null"}");
        Console.WriteLine($"📤 [get_sales_summary] Output: {totalOrders} orders, totalSales=€{totalSales:F2}, avgOrderValue=€{averageOrderValue:F2}");

        return result;
    }
}
