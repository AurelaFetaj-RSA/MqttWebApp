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

        //public Task SendMessage(string topic, string message)
        //{
        //    return Task.CompletedTask;
        //}

        //public async Task SendMessage(string topic, string message)
        //{
        //    await Clients.All.SendAsync("ReceiveMessage", topic, message);
        //}

        //public async Task SendAvailableTopics()
        //{
        //    var topics = _mqttService.GetSubscribedTopics();
        //    await Clients.Caller.SendAsync("ReceiveAvailableTopics", topics);
        //}

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
