using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Web;

namespace CodeEditor
{
    public class RealTimeExecutionService
    {
        private ClientWebSocket _webSocket;
        private Uri _serverUri;

        public RealTimeExecutionService()
        {
            _serverUri = new Uri("ws://localhost:8000/ws/execute/");
            _webSocket = new ClientWebSocket();
        }

        public async Task ConnectAsync()
        {
            await _webSocket.ConnectAsync(_serverUri, CancellationToken.None);
        }

        public async Task SendCodeAsync(string language, string code, string input)
        {
            var message = new
            {
                language = language,
                code = code,
                input = input
            };
            var json = JsonConvert.SerializeObject(message);
            var bytes = Encoding.UTF8.GetBytes(json);
            await _webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task<string> ReceiveOutputAsync()
        {
            var buffer = new byte[4096];
            var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            var response = Encoding.UTF8.GetString(buffer, 0, result.Count);
            return response;
        }

        public async Task DisconnectAsync()
        {
            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Done", CancellationToken.None);
        }
    }
}