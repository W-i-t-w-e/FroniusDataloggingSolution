using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FroniusDataLoggerShared.Models
{
    public class ResultModel
    {
        public int DeviceId { get; set; }
        public DeviceType DeviceModel { get; set; }
        public QueryCommand Command { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public double Result { get; set; }
    }
}
