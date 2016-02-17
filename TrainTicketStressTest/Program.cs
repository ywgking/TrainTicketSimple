using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace TrainTicketStressTest
{
    class Program
    {
        static void Main(string[] args)
        {
          
            string testurl="http://localhost:50837/Home/JsonPostTicket?Dates="+DateTime.Now.AddDays(2).ToString("yyyyMMdd")+"&Trans=CRH-001&StartStep={0}&EndStep={1}";
            Random myrandom = new Random(DateTime.Now.Millisecond);
            for(int i=0;i<100000;i++)
            {
              int start=  myrandom.Next(9)+1;
              int steps = myrandom.Next(9-start)+1;

              string UrlStr = string.Format(testurl, start, start + steps);
             WebClient TestClient = new WebClient();
             TestClient.Encoding = System.Text.Encoding.UTF8;
             TestClient.DownloadStringCompleted += TestClient_DownloadStringCompleted;
             TestClient.DownloadStringAsync(new Uri(UrlStr));
             System.Threading.Thread.Sleep(10);
            
            }
        
        }

        static void TestClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {

            if (!string.IsNullOrEmpty(e.Result))
            {
                string VerificationCode = e.Result.Replace("\"", "");
                if (string.IsNullOrEmpty(VerificationCode))
                    Console.WriteLine("订票失败");
                else
                {
                    if (VerificationCode.Contains("Error Data"))
                    {
                        Console.WriteLine(VerificationCode);
                        Console.ReadKey();
                        return;
                    }
                    string ResultUrl = "http://localhost:50837/Home/GetOrder?VerficationCode={0}";
                    WebClient ResultClient = new WebClient();
                    ResultClient.Encoding = System.Text.Encoding.UTF8;
                    ResultClient.DownloadStringCompleted += ResultClient_DownloadStringCompleted;
                    ResultUrl = string.Format(ResultUrl, VerificationCode);
                    ResultClient.DownloadStringAsync(new Uri(ResultUrl));
                }
            }
        }

        static void ResultClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            Console.WriteLine(e.Result);
           // throw new NotImplementedException();
        }
    }
}
