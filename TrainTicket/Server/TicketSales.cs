using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTicket.Models;

namespace TrainTicket.Server
{
    public class TicketSales
    {
        /// <summary>
        /// 测试用站点数量
        /// </summary>
        const int TEST_STEP_COUNT = 10;
        /// <summary>
        /// 测试用火车座位数
        /// </summary>
        const int TEST_TRAIN_SEAT_COUNT = 50;

        private TicketSales()
        {
            Line testline = new Line();
            testline.Id = 1;
            testline.LineName = "测试线路";
            testline.StepCount = TEST_STEP_COUNT;
            testline.StepList = new List<Step>();
            for (int i = 0; i < testline.StepCount; i++)
            {
                Step mystep = new Step();
                mystep.StepNumber = i + 1;
                mystep.StepName = "测试站点" + mystep.StepNumber.ToString().PadLeft(3, '0');
                testline.StepList.Add(mystep);
            }
            ///创建测试车次，为早上8点发车。
            CreateUpdateTrain(testline, "CRH-001", TEST_TRAIN_SEAT_COUNT, new DateTime(2016, 2, 1, 8, 0, 0));
        }

        private static TicketSales _ticketSales;

        public static TicketSales GetIntance()
        {
            if (_ticketSales == null)
                _ticketSales = new TicketSales();
            return _ticketSales;
        }
        private Dictionary<string, TicketOrder> AlreadySaleTicketDic = new Dictionary<string, TicketOrder>();
        public Dictionary<int, Dictionary<string, Train>> TrainDic { get; private set; }

        public bool CreateUpdateTrain(Line line, string TrainNumber, int SeatCount, DateTime StartTime)
        {
            ///创建近三天可售的火车车次
            for (int i = 0; i < 3; i++)
            {
                DateTime CreateDate = DateTime.Now.AddDays(i + 1);
                int date = int.Parse(CreateDate.ToString("yyyyMMdd"));
                if (TrainDic == null)
                    TrainDic = new Dictionary<int, Dictionary<string, Train>>();
                if (!TrainDic.ContainsKey(date))
                {
                    Train MyTrain = new Train();
                    MyTrain.TrainLine = line;
                    MyTrain.TrainNumber = TrainNumber;
                    MyTrain.TrainDate = date;
                    MyTrain.AlreadySalesTickets = new List<Ticket>();
                    MyTrain.SeatCount = SeatCount;
                    MyTrain.TrainStartTime = StartTime;
                    Dictionary<string, Train> DateTrainDic = new Dictionary<string, Train>();
                    DateTrainDic.Add(TrainNumber, MyTrain);
                    TrainDic.Add(date, DateTrainDic);
                }
            }
            return true;
        }

        public TicketOrder GetTicket( string VerificationCode)
        {
            if (AlreadySaleTicketDic.ContainsKey(VerificationCode))
            {
                return AlreadySaleTicketDic[VerificationCode];
            }
            return null;
        }
        public bool SaleTrainTicket(TicketOrder TOrder)
        {
            try
            {
                if (TOrder.StartStep < TOrder.EndStep && TOrder.StartStep > 0)
                {
                    if (TrainDic.ContainsKey(TOrder.TrainDate))
                    {
                        if (TrainDic[TOrder.TrainDate].ContainsKey(TOrder.TrainNumber))
                        {
                            Train MyTrain = TrainDic[TOrder.TrainDate][TOrder.TrainNumber];
                            ///加锁保证每个车次只有一个线程访问
                            lock (MyTrain)
                            {
                                List<Ticket> AlreadySalesTickets = MyTrain.AlreadySalesTickets;

                                ///当前请求购票起始站点之前售出，并且未在当前购票起始站点下车的票数
                                int BeforeStepSalesCount = AlreadySalesTickets.Count(p => (p.StartStep.StepNumber < TOrder.StartStep && p.EndStep.StepNumber > TOrder.EndStep));
                                ///当前请求购票起始站点已经售出的票数
                                int ThisStepSalesCount = AlreadySalesTickets.Count(p => p.StartStep.StepNumber == TOrder.StartStep);
                                ///当前请求购票起始站点之后，请求购票结束站点之前上车的票数
                                int BetweenSalesCount = AlreadySalesTickets.Count(p => p.StartStep.StepNumber > TOrder.StartStep && p.StartStep.StepNumber < TOrder.EndStep);
                                if (MyTrain.SeatCount - BeforeStepSalesCount - ThisStepSalesCount - BetweenSalesCount > 0)
                                {
                                    Ticket mytichet = new Ticket();
                                    mytichet.MyTrain = MyTrain;
                                    mytichet.StartStep = MyTrain.TrainLine.GetStep(TOrder.StartStep);
                                    mytichet.EndStep = MyTrain.TrainLine.GetStep(TOrder.EndStep);
                                    mytichet.VarcharNumber = MyTrain.TrainLine.LineName + MyTrain.TrainDate.ToString() + MyTrain.TrainNumber + "_" + mytichet.StartStep.StepNumber + "-" + mytichet.EndStep.StepNumber + "_" + AlreadySalesTickets.Count.ToString();
                                    TOrder.VarcharNumber = mytichet.VarcharNumber;
                                    TOrder.OrderMessage = "订票成功，您的取票订单号为："+TOrder.VarcharNumber;
                                    mytichet.VerificationCode = TOrder.VerificationCode;
                                  
                                    AlreadySalesTickets.Add(mytichet);
                                    AlreadySaleTicketDic.Add(mytichet.VerificationCode, TOrder);
                                    return true;
                                }
                                TOrder.OrderMessage = "对不起，您请求购买的车票已经售完，请更换车次时间。";

                            }
                        }

                    }
                    if (string.IsNullOrEmpty(TOrder.OrderMessage))
                        TOrder.OrderMessage = "对不起，您请求的车票不在销售期内，请更换车次时间。";
                }
                else
                {
                    TOrder.OrderMessage = "请选择正确的目的站点";
                }
            }
            catch (Exception ex)
            {
                TOrder.OrderMessage = "购票失败,系统错误：" + ex.Message;
            }
            AlreadySaleTicketDic.Add(TOrder.VerificationCode, TOrder);
            return false;
        }
    }
}
