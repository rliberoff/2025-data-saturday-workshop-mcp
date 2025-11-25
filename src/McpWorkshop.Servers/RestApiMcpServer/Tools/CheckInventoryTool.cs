using System;
using System.Collections.Generic;
using System.Text.Json;

namespace RestApiMcpServer.Tools;

/// <summary>
/// MCP tool for checking product inventory availability.
/// </summary>
public static class CheckInventoryTool
{
    /// <summary>
    /// Gets the tool definition for MCP protocol.
    /// </summary>
    /// <returns>An object containing the tool definition with name, description, and input schema.</returns>
    public static object GetDefinition()
    {
        return new
        {
            name = "check_inventory",
            description = "Verifica el inventario disponible de un producto específico. IMPORTANTE: Esta herramienta requiere el parámetro 'productId' (number, requerido) que es el ID numérico del producto (ejemplo: 101, 102, 105). NO uses nombres de productos, solo IDs numéricos. Usa esta herramienta cuando te pregunten sobre disponibilidad, stock, o inventario de un producto específico.",
            inputSchema = new Dictionary<string, object>
            {
                ["type"] = "object",
                ["properties"] = new Dictionary<string, object>
                {
                    ["productId"] = new Dictionary<string, object>
                    {
                        ["type"] = "number",
                        ["description"] = "ID del producto a verificar"
                    }
                },
                ["required"] = new[] { "productId" },
            },
        };
    }

    /// <summary>
    /// Executes the inventory check for a specified product.
    /// </summary>
    /// <param name="arguments">Dictionary containing the productId parameter.</param>
    /// <returns>An object containing the inventory status and details.</returns>
    /// <exception cref="ArgumentException">Thrown when the productId parameter is missing.</exception>
    public static object Execute(Dictionary<string, JsonElement> arguments)
    {
        if (!arguments.ContainsKey("productId"))
        {
            throw new ArgumentException("El parámetro 'productId' es requerido");
        }

        var productId = arguments["productId"].GetInt32();

        // Simulate API call delay
        System.Threading.Thread.Sleep(100);

        // Simulate inventory data
        var random = new Random(productId);
        var inStock = random.Next(0, 100) > 20;
        var quantity = inStock ? random.Next(5, 50) : 0;
        var warehouses = new[] { "Madrid", "Barcelona", "Valencia" };
        var warehouse = warehouses[random.Next(0, 3)];

        var result = new
        {
            content = new[]
            {
                new
                {
                    type = "text",
                    text = $"📦 INVENTARIO - Producto #{productId}\n\n" +
                           $"Estado: {(inStock ? "✅ DISPONIBLE" : "❌ AGOTADO")}\n" +
                           $"Cantidad: {quantity} unidades\n" +
                           $"Almacén: {warehouse}\n" +
                           $"Última actualización: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC",
                },
            },
        };

        // Trace: log result
        Console.WriteLine($"🔍 [check_inventory] Input: productId={productId}");
        Console.WriteLine($"📤 [check_inventory] Output: inStock={inStock}, quantity={quantity}, warehouse={warehouse}");

        return result;
    }
}
