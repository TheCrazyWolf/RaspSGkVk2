using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using VkNet.Model;
using VkNet.Model.GroupUpdate;
using VkNet.Model.RequestParams;
using static RaspSGkVk2.Program;

namespace RaspSGkVk2.Models
{
    public class Controller
    {
        private Settings settings = Program.settings;

        // Основной шедулер
        public void Sheduler()
        {
            while (true)
            {
                Thread.Sleep(settings.Timer);

                if (DateTime.Now.Hour >= 22 && DateTime.Now.Hour <= 7)
                    continue;

                try
                {
                    foreach (var item in settings.SettingsVkList)
                    {
                        Thread.Sleep(500);
                        Write($"Выполяется задача #{item.IdTask}");


                        //Дата на завтра! Поправить (ТОЛЬКО 1 день)
                        var s = GetLessons(DateTime.Now.AddDays(1), item.TypeTask, Convert.ToInt32(item.Value));
                        string rasp = GetLessonsString(s);

                        if (item.ResultText != rasp)
                        {
                            item.ResultText = rasp;

                            Send(rasp, Convert.ToInt64(item.PeerId));
                        }
                        else
                        {
                            Write($"Task {item.IdTask} нет изменений в расписаний");
                        }


                    }
                }
                catch (Exception ex)
                {
                    WriteError(ex.ToString());
                }
            }
        }


        // ОСНОВНЫЕ КОМАНДЫ
        // Добавление задач
        public string FindAddNewTask(GroupUpdate groupupdate, string[] user_msg)
        {

            var findpeer = settings.SettingsVkList.FirstOrDefault(x => x.PeerId == groupupdate.Message.PeerId.ToString());
            if (findpeer != null)
                return $"Существующая беседа {groupupdate.Message.PeerId} уже привязана к (id #{findpeer.IdTask} / {findpeer.Value}) ";

            var teachers = GetTeachers();
            var groups = GetGroup();


            string text_teach = "";

            foreach (var item in user_msg)
            {
                if (item != "!привязать")
                    text_teach += $" {item}";
            }

            text_teach = text_teach.Remove(0, 1);

            var found_teach = teachers.FirstOrDefault(x => x.name.ToLower() == text_teach.ToLower());
            //var found_teach = teachers.FirstOrDefault(x => x.name.ToLower() == user_msg[1].ToLower());

            var found_group = groups.FirstOrDefault(x => x.name.ToUpper() == user_msg[1].ToUpper());

            if (found_teach != null)
            {
                SettingsVk temp = new SettingsVk()
                {
                    IdTask = settings.SettingsVkList.Count + 1,
                    TypeTask = 'T',
                    Value = found_teach.id,
                    PeerId = groupupdate.Message.PeerId.ToString()
                };

                WriteWaring($"Задача #{temp.IdTask} была добавлена. Тип - преподаватель. Значение - #{temp.Value}. Беседа #{temp.PeerId}");
                settings.SettingsVkList.Add(temp);
                settings.SaveSettings();

                return $"Расписание для {found_teach.name} привязано к беседе";
            }

            if (found_group != null)
            {
                SettingsVk temp = new SettingsVk()
                {
                    IdTask = settings.SettingsVkList.Count + 1,
                    TypeTask = 'G',
                    Value = found_group.id.ToString(),
                    PeerId = groupupdate.Message.PeerId.ToString()
                };

                WriteWaring($"Задача #{temp.IdTask} была добавлена. Тип - преподаватель. Значение - #{temp.Value}. Беседа #{temp.PeerId}");
                settings.SettingsVkList.Add(temp);
                settings.SaveSettings();
                return $"Расписание для {found_group.name} привязано к беседе";
            }

            return "Ошибка при поиске групп и преподавателей";
        }

        // Удаление задачи
        public string DeleteTask(GroupUpdate groupupdate, string[] user_msg)
        {
            var find = settings.SettingsVkList.FirstOrDefault(x => x.PeerId == groupupdate.Message.PeerId.ToString());

            if (find != null)
            {
                settings.SettingsVkList.Remove(find);
                WriteWaring("Внесены изменения в список задач");
                settings.SaveSettings();
                return $"Задача #{find.IdTask} была отменена для беседы #{groupupdate.Message.PeerId}";
            }

            return $"Ошибка при удаление задачи. Смотри консоль";
        }
        //Получить расписание вчер, сегодня и на завтра
        public string GetLessonsNow(GroupUpdate groupupdate, string[] user_msg)
        {
            var find = settings.SettingsVkList.FirstOrDefault(x => x.PeerId == groupupdate.Message.PeerId.ToString());
            if (find != null)
            {
                var lessons_tommor = GetLessons(DateTime.Now.AddDays(-1), find.TypeTask, Convert.ToInt32(find.Value));
                var lessons_today = GetLessons(DateTime.Now, find.TypeTask, Convert.ToInt32(find.Value));
                var lessons_nextday = GetLessons(DateTime.Now.AddDays(1), find.TypeTask, Convert.ToInt32(find.Value));

                var text_tommor = GetLessonsString(lessons_tommor);
                var text_today = GetLessonsString(lessons_today);
                var text_nextday = GetLessonsString(lessons_nextday);

                return $"{text_tommor}\n{text_today}\n{text_nextday}";

            }
            return $"К этой беседе не привязано ни одно расписание :(";
        }


        // АДМИНИСТРИРОВАНИЕ
        // Добавление нового админа
        public string AddNewAdmin(GroupUpdate groupupdate, string[] user_msg)
        {

            if (!isAdmin(groupupdate, user_msg))
                return "Нет прав";

            ListAdmins admin = new ListAdmins()
            {
                Id = settings.AdminsList.Count + 1,
                Value = user_msg[1]
            };

            settings.AdminsList.Add(admin);
            settings.SaveSettings();
            WriteWaring($"Внесены изменения в список администраторов #{admin.Id} -> #{admin.Value}");

            return $"{user_msg[1]} добавлен в администраторы";

        }
        // Массовая рассылка
        public string SendAllResponse(GroupUpdate groupupdate, string[] user_msg)
        {
            if (!isAdmin(groupupdate, user_msg))
                return "Нет прав";

            string command = "";
            foreach (var item in user_msg)
            {
                if (item != "!рассылка")
                    command += $" {item}";
            }

            command = command.Remove(0, 1);

            WriteWaring($"Пользователь {groupupdate.Message.FromId} иниициировал рассылку c текстом: {command}");
            SendAll(command);

            return $"Рассылка выполнена";

        }
        // Вывод всех задач
        public string GetTasks(GroupUpdate groupupdate, string[] user_msg)
        {
            if (!isAdmin(groupupdate, user_msg))
                return "Нет прав";

            string text = "Активные задачи\n";

            foreach (var item in settings.SettingsVkList)
            {
                text += $"#{item.IdTask}. Peer #{item.PeerId}. Value {item.Value}\n";
            }

            return text;


        }
        //Удаление задач
        public string DeleteTaskAdmin(GroupUpdate groupupdate, string[] user_msg)
        {
            if (!isAdmin(groupupdate, user_msg))
                return "Нет прав";

            var findtask = settings.SettingsVkList.FirstOrDefault(x => x.IdTask.ToString() == user_msg[1]);
            if (findtask != null)
            {
                settings.SettingsVkList.Remove(findtask);
                settings.SaveSettings();
                return $"Задача #{findtask.IdTask} для беседы #{findtask.PeerId} удалена";
            }
            else
            {
                return $"Ошибка при удалении";
            }

        }
        //Рассылка всем!
        public void SendAll(string text)
        {
            foreach (var item in settings.SettingsVkList)
            {
                Thread.Sleep(800);
                Send(text, Convert.ToInt64(item.PeerId));
            }
        }




        // Проверка на админа
        public bool isAdmin(GroupUpdate groupupdate, string[] user_msg)
        {
            var id_sender = groupupdate.Message.FromId;

            var find = settings.AdminsList.FirstOrDefault(x => x.Value == id_sender.Value.ToString());

            if (find != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        // Развлечение
        // Пополнение словаря
        public string AddNewBook(GroupUpdate groupupdate, string[] user_msg)
        {

            string text = "";
            foreach (var item in user_msg)
            {
                if (item != "!словарь")
                    text += $"{item} ";
            }

            var new_text = text.ToLower().Split("!");

            var find = settings.Books.FirstOrDefault(x => x.Word == new_text[0]);
            if (find != null)
                return $"Ошибка при занесение данных в словарь. Слово уже существует. Используйте !редсловарь";

            Book book = new Book()
            {
                Id = settings.Books.Count + 1,
                Word = new_text[0],
                Value = new_text[1]
            };

            settings.Books.Add(book);
            settings.SaveSettings();

            WriteWaring($"Внесены изменения в словарь. #{book.Id} -> {book.Word} => {book.Value}");

            return $"Словарь изменен";
        }
        // Редактирование словаря
        public string EditBook(GroupUpdate groupupdate, string[] user_msg)
        {

            string text = "";
            foreach (var item in user_msg)
            {
                if (item != "!редсловарь")
                    text += $"{item} ";
            }

            var new_text = text.ToLower().Split("!");

            var find = settings.Books.FirstOrDefault(x => x.Word == new_text[0]);
            if (find == null)
                return $"Ошибка при изменений в словарь. Слово не найдено! - Используйте !словарь";

            find.Word = new_text[0];
            find.Value = new_text[1];

            settings.SaveSettings();

            WriteWaring($"Внесены изменения в словарь. #{find.Id} -> {find.Word} => {find.Value}");

            return $"Словарь изменен";
        }
        // Показ словаря
        public string CheckBook(GroupUpdate groupupdate, string[] user_msg)
        {

            string text = "";
            foreach (var item in user_msg)
            {
                if (item != "!слово")
                    text += $"{item} ";
            }

            var new_text = text.ToLower().Split("!");

            text = text.TrimEnd(' ');

            var find = settings.Books.FirstOrDefault(x => x.Word == text);
            if (find == null)
                return $"Слово не найдено в словаре";

            string msg = $"На слово '{find.Word}' могу отвечать: ";

            var answer = find.Value.Split(";");

            foreach (var item in answer)
            {
                msg += $"{item}/";
            }

            return msg;
        }
        //Ответы из словаря, случайные ответы
        public string GetAnswer(GroupUpdate groupupdate, string[] user_msg)
        {
            string question = "";
            foreach (var item in user_msg)
            {
                question += $" {item}";
            }

            question = question.Remove(0, 1);


            var find = settings.Books.FirstOrDefault(x => x.Word == question);

            if(find != null)
            {
                var answer = find.Value.Split(";");

                return answer[new Random().Next(0, answer.Length)];

            }
            else
            {
                var answerRandom = settings.Books[new Random().Next(0, settings.Books.Count)].Value.Split(";");

                return answerRandom[new Random().Next(0, answerRandom.Length)];
                
            }


        }


        /// <summary>
        /// Получение списка групп
        /// </summary>
        /// <returns></returns>
        public List<Group> GetGroup()
        {
            var json = Response("https://mfc.samgk.ru/api/groups");
            try
            {
                var groups = JsonSerializer.Deserialize<List<Group>>(json);
                return groups;
            }
            catch (Exception ex)
            {
                WriteError(ex.ToString());
            }
            return null;
        }

        /// <summary>
        /// Получение массива преподавателей
        /// </summary>
        /// <returns></returns>
        public List<Teacher> GetTeachers()
        {
            var json = Response("https://asu.samgk.ru/api/teachers");
            try
            {
                var teachers = JsonSerializer.Deserialize<List<Teacher>>(json);
                return teachers;
            }
            catch (Exception ex)
            {
                WriteError(ex.ToString());
            }
            return null;
        }

        /// <summary>
        /// Получение Занятий в виде массива
        /// </summary>
        /// <param name="date"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public RaspApi GetLessons(DateTime date, char type, int value)
        {
            var json = "";
            switch (type)
            {
                case 'G':
                    json = Response($"https://asu.samgk.ru/api/schedule/{value}/{date.ToString("yyyy-MM-dd")}");
                    break;
                case 'T':
                    json = Response($"https://asu.samgk.ru/api/schedule/teacher/{date.ToString("yyyy-MM-dd")}/{value}");
                    break;
            }

            RaspApi lessons = JsonSerializer.Deserialize<RaspApi>(json);
            return lessons;
        }

        /// <summary>
        /// Получение занятий в виде строки
        /// </summary>
        /// <param name="lesons"></param>
        /// <returns></returns>
        public string GetLessonsString(RaspApi lesons)
        {
            string text = $"Расписание на {lesons.date}\n";

            foreach (var item in lesons.lessons)
            {
                text += $"{item.num}. {item.nameGroup} {item.title} {item.cab}\n";
            }

            return text;
        }


        /// <summary>
        /// Получение Json ответа
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string Response(string url)
        {
            try
            {
                Write($"[asu.samgk.ru] -> {url}");
                using (var wb = new WebClient())
                {
                    wb.Headers.Set("Accept", "application/json");
                    return wb.DownloadString(url);
                }
            }
            catch (Exception ex)
            {
                WriteError(ex.ToString());
            }

            return null;
        }

        /// <summary>
        /// Отправка сообщений в вк!
        /// </summary>
        /// <param name="text"></param>
        /// <param name="peerid"></param>
        public void Send(string text, long? peerid)
        {
            Write($"[MessageSend] -> Беседа #{peerid}.");
            //Write($"[MessageSend] -> Беседа #{peerid}. Содержимое: {text}");
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
