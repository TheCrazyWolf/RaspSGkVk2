﻿using RaspSGkVk2.Models;
using System;
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
            
            settings = settings.LoadSettings();

            Auth();

            //KeyboardBuilder key = new KeyboardBuilder();
            //key.AddButton(new AddButtonParams()
            //{
            //    Label = "test"
            //});

            //key.AddButton(new AddButtonParams()
            //{
            //    Label = "test2"
            //});

            //key.AddButton(new AddButtonParams()
            //{
            //    Label = "test3"
            //});

            //MessageKeyboard keyboard = key.Build();

            //api.Messages.Send(
            //    new VkNet.Model.RequestParams.MessagesSendParams()
            //    {
            //        PeerId = 133156422,
            //        Message = "Выбери кнопку",
            //        RandomId = new Random().Next(),
            //        Keyboard = keyboard
            //    });

            Task.Run(()=> lpoll.StartLongPoll());

            Controller controller = new Controller();
            
            Task.Run(() => controller.Sheduler());

            while (true)
            {
                Console.ReadLine();
            }

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
