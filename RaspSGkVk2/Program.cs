using RaspSGkVk2.Models;
using System;
using System.Threading.Tasks;
using VkNet;
using VkNet.Model;

namespace RaspSGkVk2
{
    internal class Program
    {
        // Экземпляр для доступа к настройкам
        static public Settings settings = new Settings();
        //Экземпляр для доступа к ВК АПИ
        static public VkApi api = new VkApi();
        //Экземпляр для работы с ВК АПИ
        static public LongPollVk lpoll = new LongPollVk();
        
        static void Main(string[] args)
        {
            
            settings = settings.LoadSettings();

            //ListAdmins admin = new ListAdmins()
            //{
            //    Id = settings.AdminsList.Count + 1,
            //    Value = "133156422"
            //};

            //settings.AdminsList.Add(admin);
            //settings.SaveSettings();

            Auth();

            //lpoll.StartLongPoll();

            Task.Run(()=> lpoll.StartLongPoll());

            Controller controller = new Controller();
            Task.Run(() => controller.Sheduler());

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
                WriteWaring("[VK] Успешная авторизация");

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
