using System.Text;
using Microsoft.AspNetCore.SignalR;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Server;
using MqttWebApp.Hubs;

public class MqttService
{
    private readonly IMqttClient _mqttClient;
    private readonly IHubContext<MqttHub> _hubContext;
    private readonly List<string> _subscribedTopics = new List<string>();

    public MqttService(IHubContext<MqttHub> hubContext)
    {
        _hubContext = hubContext;

        var mqttFactory = new MqttFactory();
        _mqttClient = mqttFactory.CreateMqttClient();

        var options = new MqttClientOptionsBuilder()
            .WithClientId("MqttWebAppClient")
            .WithTcpServer("test.mosquitto.org", 1883)
            .WithCleanSession()
            .Build();

        _mqttClient.ConnectedAsync += async e =>
        {
            Console.WriteLine("Connected to MQTT broker");

            await SubscribeToTopics();
        };

        _mqttClient.DisconnectedAsync += async e => 
        {
            System.Diagnostics.Debug.WriteLine("Disconnected from MQTT broker.");
        };

        _mqttClient.ApplicationMessageReceivedAsync += async e =>
        {
            Console.WriteLine($"Received message: {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");

            //System.Diagnostics.Debug.WriteLine("Message received event triggered.");
            var message = e.ApplicationMessage;
            string topic = message.Topic;
            var payload = Encoding.UTF8.GetString(message.Payload);

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", topic, payload);

            //ProcessIncomingMessage(message.Topic, payload);
            
            //return Task.CompletedTask;
        };

        _mqttClient.ConnectAsync(options).Wait();
    }

    private async Task SubscribeToTopics()
    {
        try
        {
            await _mqttClient.SubscribeAsync("rsa/mainpage/line_status");
            await _mqttClient.SubscribeAsync("rsa/mainpage/m1_counter");

            System.Diagnostics.Debug.WriteLine("Subscribed to topics.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Subscription failed: {ex.Message}");
        }
    }
}
