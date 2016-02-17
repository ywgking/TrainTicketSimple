using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTicket.Models
{
   public class Ticket
    {
       /// <summary>
       /// 车票凭证号
       /// </summary>
        public string VarcharNumber { get; set; }
       /// <summary>
       /// 车票所属车次
       /// </summary>
        public Train MyTrain { get; set; }
       /// <summary>
       /// 车票上车站
       /// </summary>
        public Step StartStep { get; set; }
       /// <summary>
       /// 车票下车站
       /// </summary>
        public Step EndStep { get; set; }

        public string VerificationCode { get; set; }
  
    }
}
