﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
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

        public string AddNewBook(GroupUpdate groupupdate, string[] user_msg)
        {

            string text = "";
            foreach (var item in user_msg)
            {
                if(item != "!словарь")
                    text += $"{item} ";
            }

            var new_text = text.ToLower().Split("!");

            Book book = new Book()
            {
                Id = settings.Books.Count +1,
                Word = new_text[0],
                Value = new_text[1]
            };

            settings.Books.Add(book);
            settings.SaveSettings();

            WriteWaring($"Внесены изменения в словарь. #{book.Id} -> {book.Word} => {book.Value}");

            return $"Словарь изменен";
        }

        public string AddNewAdmin(GroupUpdate groupupdate, string[] user_msg)
        {
            var id_sender = groupupdate.Message.FromId;

            var find = settings.AdminsList.FirstOrDefault(x => x.Value == id_sender.Value.ToString());

            if (find == null)
                return $"У вас нет прав на добавление администраторов бота";

            ListAdmins admin = new ListAdmins()
            {
                Id = settings.AdminsList.Count + 1,
                Value =  user_msg[1]           
            };

            settings.AdminsList.Add(admin);
            settings.SaveSettings();
            WriteWaring($"Внесены изменения в список администраторов #{admin.Id} -> #{admin.Value}");

            return $"{user_msg[1]} добавлен в администраторы";

        }


        public string FindAddNewTask(GroupUpdate groupupdate, string[] user_msg)
        {
            var teachers = GetTeachers();
            var groups = GetGroup();


            string text_teach = "";

            foreach (var item in user_msg)
            {
                if(item != "!доб")
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

            if(found_group != null)
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
                text += $"{item.num}. {item.nameGroup} {item.title} {item.cab}";
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
