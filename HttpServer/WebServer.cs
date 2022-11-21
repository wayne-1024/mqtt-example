using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MqttExample.HttpServer
{
    internal class WebServer
    {
        private readonly Semaphore _semaphore;    // 控制并发信号量
        private readonly HttpListener _listener;  // http服务监听器

        #region 初始化web服务器相关方法
        /// <summary>
        /// 构造函数初始化web服务器，默认支持并发数为20，默认url为http://localhost:9000/。
        /// </summary>
        /// <param name="concurrentCount">支持并发数</param>
        /// <param name="url">url地址</param>
        public WebServer(int concurrentCount = 20, string url = "http://localhost:9000/")
        {
            this._semaphore = new Semaphore(concurrentCount, concurrentCount);
            this._listener = new HttpListener();
            this.Bind(url);
        }

        /// <summary>
        /// 绑定url。
        /// </summary>
        /// <param name="url">url地址</param>
        private void Bind(string url)
        {
            this._listener.Prefixes.Add(url);
        }
        #endregion

        /// <summary>
        /// web服务器开始监听处理http请求。
        /// </summary>
        public void Start()
        {
            this._listener.Start();

            Task.Run(async () =>
            {
                while (true)
                {
                    _semaphore.WaitOne();
                    var context = await _listener.GetContextAsync();
                    _semaphore.Release();

                    this.HandleRequest(context);
                }
            });
        }

        /// <summary>
        /// 处理请求回调函数。
        /// </summary>
        /// <param name="context">当前请求上下文</param>
        private void HandleRequest(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;
            var urlPath = request.Url.LocalPath.TrimStart('/');

            // 心跳成功返回数据
            Dictionary<String, Object> dict = new Dictionary<String, Object>();
            dict.Add("code", 200);
            dict.Add("data", "normal");
            dict.Add("msg","Ok");
            var result = JsonSerializer.Serialize(dict);
            var resultBin = System.Text.Encoding.Default.GetBytes(result);

            // 设置响应格式
            response.StatusCode = (int)HttpStatusCode.OK;
            response.ContentType = "application/json";
            response.ContentLength64 = resultBin.Length;
            response.OutputStream.Write(resultBin, 0, resultBin.Length);
            response.OutputStream.Close();
        }

    }
}
