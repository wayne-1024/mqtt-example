using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttExample.Entity
{
    /// <summary>
    /// 温湿度实体
    /// </summary>
    internal class TemperatureHumidityEntity
    {
        public double Temperature { get; set; }

        public double Humidity { get; set; }
    }
}
