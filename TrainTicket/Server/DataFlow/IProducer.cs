using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTicket.Server.DataFlow
{
    public interface IProducer<T>
    {
        void Producer(T item);
        void SetProduceEnd();
    }
}
