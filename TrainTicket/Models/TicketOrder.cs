using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTicket.Models
{
   public class TicketOrder
    {
       public TicketOrder()
       {
           myguid = Guid.NewGuid().ToString("X");
       }
       /// <summary>
       /// 保证购票订单唯一编号，实际系统中可根据购票用户以及购票时间等信息确定唯一值
       /// </summary>
       private string myguid;
        public int TrainDate { get; set; }
        public string TrainNumber { get; set; }
        public int StartStep { get; set; }
        public int EndStep { get; set; }

        public string OrderMessage { get; set; }
        public string VarcharNumber { get; set; }
        public string VerificationCode { get; private set; }

        public bool IsValidate()
        {
            if (StartStep < EndStep && StartStep > 0)
            {
                VerificationCode = TrainNumber + TrainDate + StartStep + EndStep + myguid;
                return true;
            }
            return false;
        }
        
    }
}
