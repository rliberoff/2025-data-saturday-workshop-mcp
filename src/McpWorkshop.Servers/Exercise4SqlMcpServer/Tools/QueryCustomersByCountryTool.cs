using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Exercise4SqlMcpServer.Models;

namespace Exercise4SqlMcpServer.Tools;

public class QueryCustomersByCountryTool
{
    public static object GetDefinition()
    {
        return new
        {
            name = "query_customers_by_country",
            description = "Busca y lista clientes registrados en un país específico, con opción de filtrar por ciudad. Usa esta herramienta cuando te pregunten cuántos clientes hay en un país o ciudad (ejemplo: '¿Cuántos clientes hay en España?', '¿Clientes en Madrid?').",
            inputSchema = new Dictionary<string, object>
            {
                ["type"] = "object",
                ["properties"] = new Dictionary<string, object>
                {
                    ["country"] = new Dictionary<string, object>
                    {
                        ["type"] = "string",
                        ["description"] = "Nombre del país para filtrar clientes (ejemplos: 'España', 'México', 'Argentina')"
                    },
                    ["city"] = new Dictionary<string, object>
                    {
                        ["type"] = "string",
                        ["description"] = "Nombre de la ciudad para filtrar clientes dentro del país (opcional, ejemplos: 'Madrid', 'Barcelona')"
                    }
                },
                ["required"] = new[] { "country" }
            }
        };
    }

    public static object Execute(Dictionary<string, JsonElement> arguments, Customer[] customers)
    {
        var country = arguments.ContainsKey("country") ? arguments["country"].GetString() : null;
        var city = arguments.ContainsKey("city") ? arguments["city"].GetString() : null;

        if (string.IsNullOrEmpty(country))
            throw new ArgumentException("El parámetro 'country' es requerido");

        var filtered = customers
            .Where(c => c.Country.Equals(country, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (!string.IsNullOrEmpty(city))
        {
            filtered = filtered
                .Where(c => c.City.Equals(city, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        var summary = city != null
            ? $"Encontrados {filtered.Count} cliente(s) en {city}, {country}"
            : $"Encontrados {filtered.Count} cliente(s) en {country}";

        object textContent = new Dictionary<string, object>
        {
            ["type"] = "text",
            ["text"] = $"{summary}\n\nClientes:\n{string.Join("\n", filtered.Select(c => $"- {c.Name} ({c.City}), registrado el {c.RegisteredAt:yyyy-MM-dd}"))}"
        };

        object resourceContent = new Dictionary<string, object>
        {
            ["type"] = "resource",
            ["resource"] = new Dictionary<string, object>
            {
                ["uri"] = $"sql://workshop/customers?country={country}" + (city != null ? $"&city={city}" : ""),
                ["mimeType"] = "application/json",
                ["text"] = JsonSerializer.Serialize(filtered)
            }
        };

        var result = new Dictionary<string, object>
        {
            ["content"] = new[] { textContent, resourceContent }
        };

        // Trace: log result
        Console.WriteLine($"🔍 [query_customers_by_country] Input: country={country}, city={city ?? "null"}");
        Console.WriteLine($"📤 [query_customers_by_country] Output: {filtered.Count} customers found");

        return result;
    }
}
