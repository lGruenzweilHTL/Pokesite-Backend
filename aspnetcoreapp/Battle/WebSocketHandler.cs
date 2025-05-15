using System.Net;
using System.Net.Sockets;
using System.Text;

public class WebSocketHandler
{
    private TcpListener? _webSocketServer;
    private NetworkStream? _clientStream;

    public int StartServer()
    {
        _webSocketServer = new TcpListener(IPAddress.Loopback, 0); // Use port 0 to auto-assign a free port
        _webSocketServer.Start();

        int assignedPort = ((IPEndPoint)_webSocketServer.LocalEndpoint).Port;
        Console.WriteLine($"WebSocket server started on port {assignedPort}");

        /*Task.Run(() =>
        {
            while (true)
            {
                TcpClient client = _webSocketServer.AcceptTcpClient();
                Console.WriteLine("A client has connected to the WebSocket.");
                _clientStream = client.GetStream();

                HandleWebSocketConnection();
            }
        });*/

        return assignedPort;
    }

    private void HandleWebSocketConnection()
    {
        try
        {
            byte[] buffer = new byte[1024];
            int bytesRead = _clientStream!.Read(buffer, 0, buffer.Length);
            string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            if (request.Contains("Upgrade: websocket"))
            {
                string response = "HTTP/1.1 101 Switching Protocols\r\n" +
                                  "Connection: Upgrade\r\n" +
                                  "Upgrade: websocket\r\n" +
                                  "Sec-WebSocket-Accept: " + GenerateWebSocketAcceptKey(request) + "\r\n\r\n";

                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                _clientStream.Write(responseBytes, 0, responseBytes.Length);
                Console.WriteLine("WebSocket handshake completed.");

                Task.Run(ListenForMessages);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling WebSocket connection: {ex.Message}");
        }
    }

    private void ListenForMessages()
    {
        try
        {
            byte[] buffer = new byte[1024];
            while (true)
            {
                int bytesRead = _clientStream!.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;

                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Received message: {message}");

                // Notify the GameLoop or other components about the message
                OnMessageReceived?.Invoke(message);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while listening for messages: {ex.Message}");
        }
    }

    public void SendMessage(string message)
    {
        if (_clientStream == null)
        {
            Console.WriteLine("No client connected to send a message.");
            return;
        }

        byte[] messageBytes = Encoding.UTF8.GetBytes(message);
        byte[] frame = new byte[2 + messageBytes.Length];
        frame[0] = 0x81; // FIN + text frame
        frame[1] = (byte)messageBytes.Length;
        Array.Copy(messageBytes, 0, frame, 2, messageBytes.Length);

        _clientStream.Write(frame, 0, frame.Length);
    }

    private static string GenerateWebSocketAcceptKey(string request)
    {
        const string magicString = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
        string key = request.Split("\r\n")
            .FirstOrDefault(line => line.StartsWith("Sec-WebSocket-Key:"))
            ?.Split(":")[1]
            .Trim();

        string acceptKey = Convert.ToBase64String(
            System.Security.Cryptography.SHA1.Create()
                .ComputeHash(Encoding.UTF8.GetBytes(key + magicString))
        );

        return acceptKey;
    }

    public event Action<string>? OnMessageReceived;
}