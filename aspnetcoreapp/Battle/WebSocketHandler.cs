using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Collections.Concurrent;

public class WebSocketHandler
{
    private readonly HttpListener _httpListener = new();
    private readonly ConcurrentDictionary<Guid, WebSocket> _clients = new();

    public event Action<Guid, JsonDocument>? OnMessageReceived;

    public int StartServer()
    {
        string url = "http://localhost:8080/ws/";
        _httpListener.Prefixes.Add(url);
        _httpListener.Start();
        Console.WriteLine($"WebSocket server started at {url}");

        Task.Run(async () =>
        {
            while (true)
            {
                var context = await _httpListener.GetContextAsync();
                if (context.Request.IsWebSocketRequest)
                {
                    _ = HandleClientAsync(context);
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                }
            }
        });

        return 8080;
    }

    private async Task HandleClientAsync(HttpListenerContext context)
    {
        var wsContext = await context.AcceptWebSocketAsync(null);
        var webSocket = wsContext.WebSocket;
        var clientId = Guid.NewGuid();
        _clients[clientId] = webSocket;
        Console.WriteLine($"Client {clientId} connected.");

        try
        {
            var buffer = new byte[4096];
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    break;
                }

                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                try
                {
                    var json = JsonDocument.Parse(message);
                    OnMessageReceived?.Invoke(clientId, json);
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Invalid JSON: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error with client {clientId}: {ex.Message}");
        }
        finally
        {
            _clients.TryRemove(clientId, out _);
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
            webSocket.Dispose();
            Console.WriteLine($"Client {clientId} disconnected.");
        }
    }

    public async Task SendMessageAsync(Guid clientId, object message)
    {
        if (_clients.TryGetValue(clientId, out var webSocket) && webSocket.State == WebSocketState.Open)
        {
            var json = JsonSerializer.Serialize(message);
            var buffer = Encoding.UTF8.GetBytes(json);
            await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}