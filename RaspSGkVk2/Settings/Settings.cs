﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static RaspSGkVk2.Program;

namespace RaspSGkVk2
{
    [Serializable]
    public class Settings
    {
        //Токен от группы ВК
        public string TokenVk { get; set; }
        //Тайм аут между запросами
        public int Timer { get; set; }
        //Лист с настройками бесед

        public long IdGroup { get; set; }

        public List<SettingsVk> SettingsVk { get; set; }
        //Лист админов
        public List<ListAdmins> AdminsList { get; set; }
        //База слов
        public List<Book> Books { get; set; }


        /// <summary>
        /// Загрузка настроек из файла
        /// </summary>
        /// <returns></returns>
        public Settings LoadSettings()
        {
            switch (File.Exists("settings.json"))
            {
                case true:
                    var json = File.ReadAllText("settings.json");
                    var temp = JsonSerializer.Deserialize<Settings>(json);

                    if ((temp.TokenVk == "") || temp.Timer == 0 || temp.IdGroup == 0)
                        FirstStart();

                    return temp;
                case false:
                    FirstStart();
                    return this;
            }
        }

        public void FirstStart()
        {
            try
            {
                Console.Clear();
                Console.Write("Введить токен группы: ");
                TokenVk = Console.ReadLine();

                Console.Write("Введите # группы: ");
                IdGroup = Convert.ToInt64(Console.ReadLine());

                Console.Write("Таймер в милисекундах(для АСУ СГК): ");
                Timer = Convert.ToInt32(Console.ReadLine());
                SaveSettings();
            }
            catch (Exception ex)
            {
                FirstStart();
            }

        }

        /// <summary>
        /// Сохранение настроек
        /// </summary>
        public void SaveSettings()
        {
            var json = JsonSerializer.Serialize<Settings>(this);
            File.WriteAllText("settings.json",json);
        }

        /// <summary>
        /// Добавление нвых настроек
        /// </summary>
        /// <param name="typeTask"></param>
        /// <param name="value"></param>
        /// <param name="peerId"></param>
        public void AddNewTask(char typeTask, string value, string peerId)
        {
            SettingsVk temp = new SettingsVk()
            {
                IdTask = SettingsVk.Count + 1,
                TypeTask = typeTask,
                Value = value,
                PeerId = peerId
            };

            SettingsVk.Add(temp);
        }

        /// <summary>
        ///Удаление существуюших настроек
        /// </summary>
        /// <param name="id"></param>
        public void DelTask(int id)
        {
            var temp = SettingsVk.FirstOrDefault(x => x.IdTask == id);
            SettingsVk.Remove(temp);
        }

    }

}
