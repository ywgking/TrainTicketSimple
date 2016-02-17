using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTicket.Models
{
   public class Train
    {
       /// <summary>
       /// 列车所属线路
       /// </summary>
        public Line TrainLine { get; set; }
      /// <summary>
      /// 列车总座位数
      /// </summary>
        public int SeatCount { get; set; }
       /// <summary>
       /// 列车编号
       /// </summary>
        public string TrainNumber { get; set; }
       /// <summary>
       /// 列车系统ID
       /// </summary>
        public int Id { get; set; }
       /// <summary>
       /// 列车起点站发车时间
       /// </summary>
        public DateTime TrainStartTime { get; set; }
       /// <summary>
       /// 列车所属日期
       /// </summary>
        public int TrainDate { get; set; }
       /// <summary>
       /// 该列车已经售出车票
       /// </summary>
        public List<Ticket> AlreadySalesTickets { get; set; }


    }
}
