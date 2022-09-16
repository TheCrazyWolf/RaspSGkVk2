using System;
using System.Threading.Tasks;
using VkNet;
using VkNet.Model;

namespace RaspSGkVk2
{
    internal class Program
    {
        static public Settings settings = new Settings();
        static public VkApi api = new VkApi();
        static public LongPollVk lpoll = new LongPollVk();
        
        static void Main(string[] args)
        {
            settings = settings.LoadSettings();

            settings.TokenVk = "vk1.a.zk8koagIasC5z9zyjZRd8LQeXV2Sd3QrrnVA1X3YOOXWr0sQtcFzeuzhPRQFX7N66x_AQuIkRW009fL8BzcAcGIbELUBtXFwo71Z36LT_DEShn48zthPCNe7-yBRl8YgTtHDnW4nBG-iJ-1cQTX0lVsyBZ5JrRg-Gd5492a6lF2qi4vTdFu8MDHq1cd-s5Yk";

            Auth();

            //lpoll.StartLongPoll();

            Task.Run(()=> lpoll.StartLongPoll());
            Console.ReadLine();

        }


        static void Auth()
        {

            try
            {
                api.Authorize(new ApiAuthParams()
                {
                    AccessToken = settings.TokenVk
                });
                WriteWaring("VK: Успешная авторизация");

            }
            catch (Exception ex)
            {
                WriteError(ex.ToString());
            }
        }

        static public void WriteError(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[{DateTime.Now}] {text}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        static public void WriteWaring(string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[{DateTime.Now}] {text}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        static public void Write(string text)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"[{DateTime.Now}] {text}");
            Console.ForegroundColor = ConsoleColor.White;
        }

    }
}
