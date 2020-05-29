using Dasync.Collections;
using Flurl;
using Flurl.Http;
using Hangfire;
using Hangfire.MemoryStorage;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace YueMiaoNotifier
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (var server = new BackgroundJobServer(new ConsoleMemoryStorage()))
            {
                RecurringJob.AddOrUpdate(() => TaskInvoke(), "0 0 8 * * ?");
                Console.ReadLine();
            }
        }

        public static async Task TaskInvoke()
        {
            var seckillableList = await PullFromYueMiao().ToListAsync();
            if (seckillableList != null && seckillableList.Count > 0)
            {
                var messageBuilder = new StringBuilder();
                foreach (var (name, startTime, intro) in seckillableList)
                {
                    messageBuilder.Append(name + "</br>");
                    messageBuilder.Append(startTime + "</br>");
                    messageBuilder.Append(intro + "</br>");
                    messageBuilder.Append("</br>");
                    messageBuilder.Append("</br>");
                }
                await Notify(messageBuilder.ToString());
            }
        }

        public static IAsyncEnumerable<(string name, string startTime, string intro)> PullFromYueMiao()
        {
            return new AsyncEnumerable<(string name, string startTime, string intro)>(async yield =>
            {
                var departments = await HttpCallAndCaptureError("https://wx.healthych.com"
                    .AppendPathSegment("base/department/pageList.do")
                    .SetQueryParams(new
                    {
                        cityName = "成都市",
                        offset = 0,
                        limit = 299,
                        isOpen = 1,
                        longitude = "102.69378662109374",
                        latitude = "25.05844497680663",
                        vaccineCode = 8803//9Price 8803,4Price 8802
                    })
                    .WithHeaders(getHeaders())
                    .GetJsonAsync<Protocol<DepartmentListRespnose>>());

                if (departments?.Rows != null && departments.Rows.Length > 0)
                {
                    var seckillableList = departments.Rows.Where(x => x.IsSeckill == "1");
                    if (seckillableList.Any())
                    {
                        foreach (var department in seckillableList)
                        {
                            var vaccines = await HttpCallAndCaptureError("https://wx.healthych.com"
                                .AppendPathSegment("base/department/vaccines.do ")
                                .SetQueryParams(new
                                {
                                    depaCode = department.Code,
                                    vaccineCode = 8803//9Price 8803, 4Price 8802
                                })
                                .WithHeaders(getHeaders())
                                .GetJsonAsync<Protocol<List<VaccineListingItemResponse>>>());

                            if (vaccines != null && vaccines.Any())
                            {
                                var vaccineDetail = await HttpCallAndCaptureError("https://wx.healthych.com"
                                  .AppendPathSegment("base/department/vaccines.do ")
                                  .SetQueryParams(new
                                  {
                                      id = vaccines.First().Id,
                                  })
                                .WithHeaders(getHeaders())
                                .GetJsonAsync<Protocol<VaccineDetailResponse>>());

                                if (vaccineDetail != null)
                                {
                                    await yield.ReturnAsync((department.Name, vaccineDetail.StartTime.ToString(), vaccineDetail.Intro));
                                }
                            }
                        }
                    }
                    yield.Break();
                }
            });

            object getHeaders()
            {
                return new
                {
                    tk = "faf3f2fe412e82004ca41c178adfe8f9_9c2d872dc4d0da8eb20c3c3e206d1c24",
                    st = MD5(DateTime.Now.ToString("yyyyMMddHHmm") + "jkzx705"),
                    Referer = "https://wx.healthych.com/index.html",
                    UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_2 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.4(0x17000428) NetType/WIFI Language/zh_CN"
                };
            }

            string MD5(string source)
            {
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                byte[] bytes = Encoding.UTF8.GetBytes(source);
                string result = BitConverter.ToString(md5.ComputeHash(bytes));
                return result.Replace("-", "").ToLower();
            }

            async Task<T> HttpCallAndCaptureError<T>(Task<Protocol<T>> httpCallTask)
            {
                try
                {
                    var response = await httpCallTask;
                    return response.Data;
                }
                catch (FlurlHttpException httpExcetpion)
                {
                    Console.WriteLine(httpExcetpion);
                    return default(T);
                }
            }
        }

        public static async Task Notify(string messageBody)
        {
            await SendEmailAsync("978040259@qq.com", "九价预约提醒", messageBody);
            //await SendEmailAsync("1946352503@qq.com", "九价预约提醒", messageBody);
        }

        public static async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("FengBuJue", "978040259@qq.com"));
            emailMessage.To.Add(new MailboxAddress("Fu", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.qq.com", 25);
                await client.AuthenticateAsync("978040259@qq.com", "");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
