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

namespace RaspSGkVk2
{
    internal class LongPollVk
    {
        public void StartLongPoll()
        {
            Write("Подключение к LongPoll серверу");
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
                    WriteError(ex.ToString());
                }
            }
        }


        public void CheckPoll(BotsLongPollHistoryResponse response)
        {
            foreach (var item in response.Updates)
            {
                
                if (item.Type == GroupUpdateType.MessageNew)
                {
                    string user_msg = item.Message.Body.ToLower();

                    if (user_msg == "[club186654758|@sgkmeme] хай")
                    {
                        api.Messages.Send(new MessagesSendParams()
                        {
                            Message = "хай",
                            PeerId = item.Message.ChatId,
                            RandomId = new Random().Next()
                        });
                    }

                }
            }
        }



    }
}
