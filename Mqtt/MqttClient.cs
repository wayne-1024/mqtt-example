using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;

namespace MqttExample.Mqtt
{
    /// <summary>
    /// 封装MQTTnet包中发布消息方法。
    /// 使用示例e.g：
    ///     MqttClient mqttClient = new MqttClient();                      // 默认主机: localhost, 默认端口: 1883
    ///     await mqttClient.ConnectAsync();                               // 异步等待client连接broken
    ///     await mqttClient.PublishPayload("api/v1/test", "Hello World")  // 发布消息Hello World到主题api/v1/text
    ///     await mqttClient.DisconnectAsync();                            // 异步关闭client与broken连接
    /// </summary>
    internal class MqttClient
    {
        private readonly IMqttClient _client;
        private readonly MqttClientOptions _options;

        #region 构造函数，初始化mqtt client。
        /// <summary>
        /// 根据默认地址localhost和默认端口1883连接Mqtt客户端。
        /// </summary>
        public MqttClient()
        {
            _client = new MqttFactory().CreateMqttClient();
            _options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .Build();
        }

        /// <summary>
        /// 根据主机地址与端口号构建Mqtt客户端。
        /// </summary>
        /// <param name="hostname">broken主机地址，默认为localhost。</param>
        /// <param name="port">broken主机端口号，默认为1883</param>
        public MqttClient(string hostname = "localhost", int port = 1883) 
        {
            _client = new MqttFactory().CreateMqttClient();
            _options = new MqttClientOptionsBuilder()
                .WithTcpServer(hostname, port)
                .Build();
        }
        #endregion


        #region mqtt client连接/断开MQTT broken。
        /// <summary>
        /// 连接MQTT Client。
        /// </summary>
        public async Task ConnectAsync()
        {
            var response = await _client.ConnectAsync(_options, CancellationToken.None);

            Console.WriteLine("The MQTT client is connected.");
        }

        /// <summary>
        /// 断开与MQTT Client的连接。
        /// </summary>
        public async Task DisconnectAsync()
        {
            await _client.DisconnectAsync();

            Console.WriteLine("The MQTT client is disconnected.");
        }
        #endregion


        /// <summary>
        /// 发布消息到broken。
        /// </summary>
        /// <param name="topic">主题</param>
        /// <param name="payload">消息载荷</param>
        /// <param name="contentType">内容（消息载荷）格式</param>
        public async Task PublishPayloadAsync(string topic, string payload, string contentType="application/json")
        {
            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithContentType(contentType)
                .WithPayload(payload)
                .Build();
            await _client.PublishAsync(applicationMessage);
        }

    }
}
