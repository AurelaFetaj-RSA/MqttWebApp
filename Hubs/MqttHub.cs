using Microsoft.AspNetCore.SignalR;

namespace MqttWebApp.Hubs
{
    public class MqttHub : Hub
    {
        private readonly MqttService _mqttService;

        public MqttHub(MqttService mqttService)
        {
            _mqttService = mqttService;
        }

        public async Task SendAvailableTopics()
        {
            // Get the topics (or any data) and send it to clients
            var topics = new List<string> { "rsa/mainpage/line_status", "rsa/mainpage/m1_counter" };  // Example topics
            await Clients.All.SendAsync("ReceiveAvailableTopics", topics);
        }

        public async Task SendMessage(string topic, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", topic, message);
        }
    }
}
