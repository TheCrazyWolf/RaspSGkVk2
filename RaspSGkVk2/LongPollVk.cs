using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet.Model;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.RequestParams;
using static RaspSGkVk2.Program;
using VkNet.Model.GroupUpdate;
using System.Text.RegularExpressions;
using System.Threading;

namespace RaspSGkVk2
{
    internal class LongPollVk
    {
        public void StartLongPoll()
        {
            Write("Подключение к серверу");
            while (true)
            {
                try
                {
                    //Реализовать автоматическую выддачу ID группы
                    var s = api.Groups.GetLongPollServer((ulong)Program.settings.IdGroup);
                    var poll = api.Groups.GetBotsLongPollHistory(new BotsLongPollHistoryParams()
                    {
                        Server = s.Server,
                        Key = s.Key,
                        Ts = s.Ts,
                        Wait = 25
                    });

                    if (poll?.Updates == null) continue;

                    CheckPoll(poll);

                }
                catch (Exception ex)
                {
                    Write("Ошибка при подключении к серверу");
                    WriteError(ex.ToString());
                }
            }
        }


        public void CheckPoll(BotsLongPollHistoryResponse response)
        {
            //Write($"{response.Updates.Count()} событие.");
            foreach (var item in response.Updates)
            {
                
                if (item.Type == GroupUpdateType.MessageNew)
                {
                    Write($"[MessageNew] <- Беседа #{item.Message.PeerId}. Отправитель #{item.Message.FromId}. Содержимое: {item.Message.Text}");

                    var user_msg = new Regex("\\[.*\\][\\s,]*").Replace(item.Message.Text.ToLower(),"").Split(" ");

                    //string user_msg = new Regex("\\[.*\\][\\s,]*").Replace(item.Message.Text.ToLower(),"");
                    //var command = user_msg.Split(" ");

                    Models.Controller controller = new Models.Controller();

                    switch (user_msg[0])
                    {

                        case "начать":
                        case "старт":
                            Send("Добро пожаловать, сейчас бот все еще разрабатывается. Посмотри справку - !помощь", item.Message.PeerId);
                            break;

                        case "!доб":
                            Send(controller.FindAddNewTask(item, user_msg), item.Message.PeerId);
                            break;
                        case "!админ":
                            Send(controller.AddNewAdmin(item, user_msg), item.Message.PeerId);
                            break;
                        case "!словарь":
                            Send(controller.AddNewBook(item, user_msg), item.Message.PeerId);
                            break;
                        case "!преподы":
                            var test = controller.GetTeachers();
                            string text = "";

                            foreach (var item2 in test)
                            {
                                Thread.Sleep(900);
                                Send($"{item2.id}. {item2.name}", item.Message.PeerId);
                                
                            }
                            //Send(text, item.Message.PeerId);

                            break;
                        default:
                            Send(controller.GetAnswer(item, user_msg), item.Message.PeerId);
                            break;
                    }


                }

            }
        }



        /// <summary>
        /// Отправка сообщений в вк!
        /// </summary>
        /// <param name="text"></param>
        /// <param name="peerid"></param>
        public void Send(string text, long? peerid)
        {
            Write($"[MessageSend] -> Беседа #{peerid}. Содержимое: {text}");
            try
            {
                api.Messages.Send(new MessagesSendParams()
                {

                    Message = text,
                    PeerId = peerid,
                    RandomId = new Random().Next()
                });
            }
            catch (Exception ex)
            {
                WriteError(ex.ToString());
            }
        }



    }
}
