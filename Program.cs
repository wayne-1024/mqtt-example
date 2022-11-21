using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MqttExample.Entity;
using MqttExample.Mqtt;

namespace MqttExample
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // 简单测试mqtt
            // await Test01();

            // 测试json
            // await Test02();

            // 循环读取数据发送
            await Test03();
        }

        /// <summary>
        /// 简单测试
        /// </summary>
        /// <returns></returns>
        public static async Task Test01()
        {
            MqttClient mqttClient = new MqttClient();
            await mqttClient.ConnectAsync();
            await mqttClient.PublishPayloadAsync("api/v1/test", "Hello World！");
            await mqttClient.PublishPayloadAsync("api/v1/test", "Hello World2.");
            await mqttClient.DisconnectAsync();

            Console.ReadLine();
        }

        /// <summary>
        /// 测试json与mqtt
        /// </summary>
        /// <returns></returns>
        public static async Task Test02()
        {
            // 1) 连接broken
            MqttClient mqttClient = new MqttClient();
            await mqttClient.ConnectAsync();

            // 2) 序列化对象为json数据
            var temperatureHumidity = new TemperatureHumidityEntity();
            temperatureHumidity.Temperature = 35.6;
            temperatureHumidity.Humidity = 65.2;
            var jsonData = JsonSerializer.Serialize(temperatureHumidity);

            // 3) 发送数据
            await mqttClient.PublishPayloadAsync("api/v1/temperature_humidity", jsonData);

            // 4) 关闭连接
            await mqttClient.DisconnectAsync();

            Console.ReadLine();
        }

        /// <summary>
        /// 测试json与mqtt，可循环发送测试
        /// </summary>
        /// <returns></returns>
        public static async Task Test03()
        {
            var topic = "api/v1/temperature_humidity";

            // 1) 连接broken
            MqttClient mqttClient = new MqttClient();
            await mqttClient.ConnectAsync();

            // 2) 循环读取数据
            while (true)
            {
                // 2.1) 读取数据
                TemperatureHumidityEntity temperatureHumidity = new TemperatureHumidityEntity();
                Console.Write("请输入温度:");
                temperatureHumidity.Temperature = Convert.ToDouble(Console.ReadLine());
                Console.Write("请输入湿度:");
                temperatureHumidity.Humidity = Convert.ToDouble(Console.ReadLine());

                // 2.2) 发送数据
                await mqttClient.PublishPayloadAsync(topic, JsonSerializer.Serialize(temperatureHumidity));

                Console.WriteLine("Send Success!");

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey();
                    Console.WriteLine(key);
                    switch (key.Key)
                    {
                        case ConsoleKey.Q:
                            {
                                // 3) 退出关闭连接
                                await mqttClient.DisconnectAsync();
                                return;
                            }
                    }
                }
            }
        }
    }
}
