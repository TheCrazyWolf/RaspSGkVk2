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

namespace RaspSGkVk2
{
    internal class LongPollVk
    {
        public void StartLongPoll()
        {
            Write("[LongPoll] Подключение к серверу");
            while (true)
            {
                try
                {
                    //Реализовать автоматическую выддачу ID группы
                    var s = api.Groups.GetLongPollServer(186654758);
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
                    Write("[LongPoll] Ошибка при подключении к серверу");
                    WriteError(ex.ToString());
                }
            }
        }


        public void CheckPoll(BotsLongPollHistoryResponse response)
        {
            Write($"[LongPoll] Получено {response.Updates.Count()} событий");
            foreach (var item in response.Updates)
            {
                
                if (item.Type == GroupUpdateType.MessageNew)
                {
                    Write($"[LongPoll] MessageNew -> Беседа #{item.Message.PeerId}. Отправитель #{item.Message.FromId}. Содержимое: {item.Message.Text}");

                    string user_msg = new Regex("\\[.*\\][\\s,]*").Replace(item.Message.Text.ToLower(),"");
                    var command = user_msg.Split(" ");

                    if (user_msg == "начать")
                    {
                        Send("Добро пожаловать, сейчас бот все еще разрабатывается. Посмотри справку - !помощь", item.Message.PeerId);
                    }
                    else if (command[0] == "!помощь")
                    {
                        Send("ОК!", item.Message.PeerId);
                    }
                    else if (command[0] == "!админ")
                    {
                        if (item.Message.FromId != 133156422)
                        {
                            Send("ОК!", item.Message.PeerId);
                        }
                        else
                        {
                            Send("ОК!", item.Message.PeerId);
                        }

                    }


                }

            }
        }



        public void Send(string text, long? peerid)
        {
            Write($"[VK] Message.Send -> Беседа #{peerid}. Содержимое: {text}");
            api.Messages.Send(new MessagesSendParams()
            {

                Message = text,
                PeerId = peerid,
                RandomId = new Random().Next()
            });
        }



    }
}
