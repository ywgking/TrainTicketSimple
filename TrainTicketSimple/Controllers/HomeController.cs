using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainTicket.Models;
using TrainTicket.Server;

namespace TrainTicektSimple.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ///创建测试线路及站点
           
            TicketSales MySales = TicketSales.GetIntance();
          
            List<SelectListItem> Dates = new List<SelectListItem>();
            foreach (var item in MySales.TrainDic.Keys)
            {
                SelectListItem selectitem = new SelectListItem();
                selectitem.Text = item.ToString();
                selectitem.Value = item.ToString();
                Dates.Add(selectitem);
            }
            ViewBag.Dates = Dates;
            return View();
        }
        public ActionResult PostTicket(int Dates,string Trans,int StartStep,int EndStep)
        {
           
            TicketOrder MyOrder = new TicketOrder();
            MyOrder.EndStep = EndStep;
            MyOrder.StartStep = StartStep;
            MyOrder.TrainDate = Dates;
            MyOrder.TrainNumber = Trans;
            if (MyOrder.IsValidate())
            {
                TicketDispatch.GetIntence().StartTicketOrder(MyOrder);
                ViewBag.Message = "正在处理您的订单，请耐心等待。";
                ViewBag.VerficationCode = MyOrder.VerificationCode;
            }
            else
                ViewBag.Message = "请购买正确的始发地与目的地车票！";

            //if (mysales.SaleTrainTicket(Trans, StartStep, EndStep, Dates, ref varcharnumber, ref errormessage))
            //{
            //    ViewBag.Message = "购票成功，您的订单号为：" + varcharnumber;
            //}
            //else
            //    ViewBag.Message = errormessage;

            return View();
        }

        public JsonResult JsonPostTicket(int Dates, string Trans, int StartStep, int EndStep)
        {
            TicketOrder MyOrder = new TicketOrder();
            MyOrder.EndStep = EndStep;
            MyOrder.StartStep = StartStep;
            MyOrder.TrainDate = Dates;
            MyOrder.TrainNumber = Trans;
            if (MyOrder.IsValidate())
            {
                TicketDispatch.GetIntence().StartTicketOrder(MyOrder);
                return Json(MyOrder.VerificationCode,JsonRequestBehavior.AllowGet);
            }
            else
                return Json("Error Data："+StartStep+"-"+EndStep,JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrder(string VerficationCode)
        {
            TicketSales mysales = TicketSales.GetIntance();
            TicketOrder MyTicketOrder = mysales.GetTicket(VerficationCode);
            if (MyTicketOrder != null)
                return Json(MyTicketOrder.OrderMessage, JsonRequestBehavior.AllowGet);
            else
                return Json("订单还未处理，请稍后再试！",JsonRequestBehavior.AllowGet);
        }
        public ActionResult About()
        {
            ViewBag.Message = "简单火车售票系统.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "";

            return View();
        }
    }
}