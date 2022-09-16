using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static RaspSGkVk2.Program;

namespace RaspSGkVk2.Models
{
    public class Controller
    {


        /// <summary>
        /// Получение списка групп
        /// </summary>
        /// <returns></returns>
        public List<Group> GetGroup()
        {
            var json = $"https://mfc.samgk.ru/api/groups";
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
            var json = $"https://asu.samgk.ru/api/teachers";
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
    }
}
