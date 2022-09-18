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
                            Send("Здарова. -> !справка", item.Message.PeerId);
                            break;
                        case "!справка":
                        case "справка":
                        case "помощь":
                        case "!помощь":
                            Send("Информация по командам:\n\n" +
                                "!расписание <вчера/сегодня/завтра>- расписание на вчера, сегодня и на завтра\n" +
                                "!привязать <ИС-22-01/Фио препода> - привязать беседу для расписания\n" +
                                "!отвязать - отвязать беседу\n" +
                                "<=Словарь=>\n" +
                                "!словарь <слово!ответ;ответ> - добавление слов в словарь> \n(Например !словарь как дела!отлично;класс;успешно)\n" +
                                "!редсловарь <слово!ответ;ответ> - редактирование словаря\n" +
                                "!слово <слово> - словарь ответов\n" +
                                "\n<=Администрирование бота=>\n" +
                                "!админ <значение ID vk> - назначить нового админа\n" +
                                "!рассылка <значение> - рассылка текста по группам\n" +
                                "!задачи - текущие задачи\n" +
                                "!удалзадачи <значение>", item.Message.PeerId);
                            break;

                        // Раписание
                        case "!расписание":
                            Send(controller.GetLessonsNow(item, user_msg), item.Message.PeerId);
                            break;
                        case "!привязать":
                            Send(controller.FindAddNewTask(item, user_msg), item.Message.PeerId);
                            break;
                        case "!отвязать":
                            Send(controller.DeleteTask(item, user_msg), item.Message.PeerId);
                            break;

                        //Админстрирование
                        case "!админ":
                            Send(controller.AddNewAdmin(item, user_msg), item.Message.PeerId);
                            break;
                        case "!рассылка":
                            Send(controller.SendAllResponse(item, user_msg), item.Message.PeerId);
                            break;
                        case "!задачи":
                            Send(controller.GetTasks(item, user_msg), item.Message.PeerId);
                            break;
                        case "!удалзадачи":
                            Send(controller.DeleteTaskAdmin(item, user_msg), item.Message.PeerId);
                            break;

                        //Развлекаловка
                        case "!словарь":
                            Send(controller.AddNewBook(item, user_msg), item.Message.PeerId);
                            break;
                        case "!редсловарь":
                            Send(controller.EditBook(item, user_msg), item.Message.PeerId);
                            break;
                        case "!слово":
                            Send(controller.CheckBook(item, user_msg), item.Message.PeerId);
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
            //Write($"[MessageSend] -> Беседа #{peerid}. Содержимое: {text}");
            Write($"[MessageSend] -> Беседа #{peerid}.");
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
