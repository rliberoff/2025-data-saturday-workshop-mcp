using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Exercise4RestApiMcpServer.Tools;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

Console.WriteLine("✅ RestApiMcpServer initialized");

// Health check endpoint
app.MapGet("/", () => Results.Ok(new
{
    status = "healthy",
    server = "Exercise4RestApiMcpServer",
    version = "1.0.0",
    timestamp = DateTime.UtcNow
}));

app.MapPost("/mcp", async (HttpContext context) =>
{
    using var reader = new StreamReader(context.Request.Body);
    var requestBody = await reader.ReadToEndAsync();

    // Log para debug
    Console.WriteLine($"📨 Request recibido: {requestBody}");

    var request = JsonSerializer.Deserialize<JsonElement>(requestBody);

    // Verificar que tenga las propiedades requeridas
    if (!request.TryGetProperty("method", out var methodElement))
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsJsonAsync(new
        {
            jsonrpc = "2.0",
            error = new
            {
                code = -32600,
                message = "Invalid Request: missing 'method' field"
            },
            id = request.TryGetProperty("id", out var idProp) ? idProp : (object?)null
        });
        return;
    }

    var method = methodElement.GetString();
    var id = request.TryGetProperty("id", out var idElement) ? idElement : JsonDocument.Parse("null").RootElement;

    object? result = null;

    try
    {
        switch (method)
        {
            case "initialize":
                result = new
                {
                    protocolVersion = "2024-11-05",
                    capabilities = new Dictionary<string, object>
                    {
                        ["tools"] = new { }
                    },
                    serverInfo = new
                    {
                        name = "RestApiMcpServer",
                        version = "1.0.0",
                        description = "Servidor MCP para APIs externas (REST)"
                    }
                };
                break;

            case "tools/list":
                result = new
                {
                    tools = new[]
                    {
                        CheckInventoryTool.GetDefinition(),
                        GetShippingStatusTool.GetDefinition(),
                        GetTopProductsTool.GetDefinition()
                    }
                };
                break;

            case "tools/call":
                result = HandleToolCall(request);
                break;

            default:
                throw new InvalidOperationException($"Unknown method: {method}");
        }

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            jsonrpc = "2.0",
            result,
            id
        }));
    }
    catch (Exception ex)
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            jsonrpc = "2.0",
            error = new { code = -32603, message = $"Internal error: {ex.Message}" },
            id
        }));
    }
});

Console.WriteLine("✅ RestApiMcpServer running on http://localhost:5012/mcp");
Console.WriteLine("🔧 Tools: check_inventory, get_shipping_status, get_top_products \n");

await app.RunAsync("http://localhost:5012");

static object HandleToolCall(JsonElement request)
{
    var paramsObj = request.GetProperty("params");
    var toolName = paramsObj.GetProperty("name").GetString();

    var arguments = new Dictionary<string, JsonElement>();

    // Manejar caso cuando arguments puede no existir o estar vacío
    if (paramsObj.TryGetProperty("arguments", out var argsElement))
    {
        foreach (var prop in argsElement.EnumerateObject())
        {
            arguments[prop.Name] = prop.Value;
        }
    }

    var toolResult = toolName switch
    {
        "check_inventory" => CheckInventoryTool.Execute(arguments),
        "get_shipping_status" => GetShippingStatusTool.Execute(arguments),
        "get_top_products" => GetTopProductsTool.Execute(arguments),
        _ => throw new InvalidOperationException($"Unknown tool: {toolName}")
    };

    // Envolver el resultado en el formato MCP correcto
    return new
    {
        content = new[]
        {
            new
            {
                type = "text",
                text = JsonSerializer.Serialize(toolResult)
            }
        }
    };
}
