using RaspSGkVk2.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using VkNet;
using VkNet.Model;
using VkNet.Model.Keyboard;

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
            

            if(!File.Exists("prop.db"))
            {
                Prop.AppContext ef1 = new Prop.AppContext();
                ef1.Database.EnsureCreated();
            }


            Prop.Tasks tasks = new Prop.Tasks();
            Prop.Value val = new Prop.Value();
            val.ID = 2345345;

            tasks.PropsValue = new System.Collections.Generic.List<Prop.Value>();
            tasks.PropsValue.Add(val);
            tasks.PropsValue.Add(val);

            Prop.Tasks tasks2 = new Prop.Tasks();
            tasks2.PropsValue = new System.Collections.Generic.List<Prop.Value>();
            tasks2.PropsValue.Add(val);
            tasks2.PropsValue.Add(val);

            Prop.Tasks tasks3 = new Prop.Tasks();
            tasks3.PropsValue = new System.Collections.Generic.List<Prop.Value>();
            tasks3.PropsValue.Add(val);
            tasks3.PropsValue.Add(val);

            Prop.AppContext ef = new Prop.AppContext();
            ef.Add(tasks);
            ef.Add(tasks2);
            ef.Add(tasks3);
            ef.SaveChanges();


            //settings = settings.LoadSettings();

            //Auth();

            //Task.Run(()=> lpoll.StartLongPoll());

            //Controller controller = new Controller();

            //Task.Run(() => controller.Sheduler());

            //while (true)
            //{
            //    Console.ReadLine();
            //}

        }

        // Авторизация по VK API
        static void Auth()
        {

            try
            {
                api.Authorize(new ApiAuthParams()
                {
                    AccessToken = settings.TokenVk
                });
                WriteWaring("[API] Успешная авторизация");

            }
            catch (Exception ex)
            {
                WriteWaring("[API] Ошибка авторизации ->");
                WriteError(ex.ToString());
                Environment.Exit(-1);
                
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
