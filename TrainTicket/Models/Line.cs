using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTicket.Models
{
    public class Line   
    {
        public int Id { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string LineName { get; set; }
        /// <summary>
        /// 站点数量
        /// </summary>
        public int StepCount { get; set; }

        public List<Step> StepList { get; set; }

        public Step GetStep (int StepNmuber){
            return StepList[StepNmuber-1];
        }
    }
}
