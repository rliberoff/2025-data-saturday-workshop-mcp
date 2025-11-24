using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Exercise4CosmosMcpServer.Models;
using Exercise4CosmosMcpServer.Tools;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Load data
var sessions = LoadData<UserSession>("../../../data/sessions.json");
var cartEvents = LoadData<CartEvent>("../../../data/cart-events.json");
var abandonedCarts = LoadData<AbandonedCart>("../../../data/abandoned-carts.json");
Console.WriteLine($"✅ Loaded {sessions.Length} sessions, {cartEvents.Length} cart events, {abandonedCarts.Length} abandoned carts");

// Health check endpoint
app.MapGet("/", () => Results.Ok(new
{
    status = "healthy",
    server = "Exercise4CosmosMcpServer",
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
                        ["resources"] = new { },
                        ["tools"] = new { }
                    },
                    serverInfo = new
                    {
                        name = "CosmosMcpServer",
                        version = "1.0.0",
                        description = "Servidor MCP para analítica de comportamiento (Cosmos)"
                    }
                };
                break;

            case "resources/list":
                result = new
                {
                    resources = new[]
                    {
                        new
                        {
                            uri = "cosmos://analytics/user-sessions",
                            name = "User Sessions",
                            description = "Sesiones de usuario con métricas de navegación",
                            mimeType = "application/json"
                        },
                        new
                        {
                            uri = "cosmos://analytics/cart-events",
                            name = "Cart Events",
                            description = "Eventos del carrito de compras",
                            mimeType = "application/json"
                        },
                        new
                        {
                            uri = "cosmos://analytics/abandoned-carts",
                            name = "Abandoned Carts",
                            description = "Carritos abandonados con detalles de productos y valores",
                            mimeType = "application/json"
                        }
                    }
                };
                break;

            case "resources/read":
                result = HandleResourceRead(request);
                break;

            case "tools/list":
                result = new
                {
                    tools = new[]
                    {
                        GetAbandonedCartsTool.GetDefinition(),
                        AnalyzeUserBehaviorTool.GetDefinition()
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

Console.WriteLine("✅ CosmosMcpServer running on http://localhost:5011/mcp");
Console.WriteLine("📊 Resources: user-sessions, cart-events");
Console.WriteLine("🔧 Tools: get_abandoned_carts, analyze_user_behavior \n");

await app.RunAsync("http://localhost:5011");

T[] LoadData<T>(string path)
{
    var json = File.ReadAllText(path);
    var options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };
    return JsonSerializer.Deserialize<T[]>(json, options) ?? Array.Empty<T>();
}

object HandleResourceRead(JsonElement request)
{
    var paramsObj = request.GetProperty("params");
    var uri = paramsObj.GetProperty("uri").GetString();

    var data = uri switch
    {
        "cosmos://analytics/user-sessions" => JsonSerializer.Serialize(sessions),
        "cosmos://analytics/cart-events" => JsonSerializer.Serialize(cartEvents),
        "cosmos://analytics/abandoned-carts" => JsonSerializer.Serialize(abandonedCarts),
        _ => throw new ArgumentException($"Unknown resource URI: {uri}")
    };

    return new
    {
        contents = new[]
        {
            new
            {
                uri,
                mimeType = "application/json",
                text = data
            }
        }
    };
}

object HandleToolCall(JsonElement request)
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
        "get_abandoned_carts" => GetAbandonedCartsTool.Execute(arguments, abandonedCarts),
        "analyze_user_behavior" => AnalyzeUserBehaviorTool.Execute(arguments, sessions, cartEvents),
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
