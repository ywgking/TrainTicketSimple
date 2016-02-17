using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using TrainTicket.Models;

namespace TrainTicket.Server.DataFlow
{
    public class PersistTicketOrderProducer : IProducer<TicketOrder>
    {
        private BufferBlock<TicketOrder> m_income = new BufferBlock<TicketOrder>();
        
        private TicketSales ticketSale;
        private bool _inProduce = true;

        public PersistTicketOrderProducer()
        {
            ticketSale = TicketSales.GetIntance();
            Run();
        }

        public void Producer(TicketOrder item)
        {
            m_income.Post(item);
        }

        public void SetProduceEnd()
        {
            _inProduce = false;
        }
        private void Run()
        {
            Task task = new Task(() =>
            {
                while (_inProduce || m_income.Count > 0)
                {
                    TicketOrder item = m_income.Receive();

                    if (null != item)
                    {
                        ticketSale.SaleTrainTicket(item);
                    
                    }
                }
            });

            task.Start();
        }
    }
}
