using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MqttExample.Mqtt;

namespace MqttExample
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            MqttClient mqttClient = new MqttClient();
            await mqttClient.ConnectAsync();
            await mqttClient.PublishPayload("api/v1/test", "Hello World！");
            await mqttClient.PublishPayload("api/v1/test", "Hello World2.");
            await mqttClient.DisconnectAsync();

            Console.ReadLine();
        }
    }
}
