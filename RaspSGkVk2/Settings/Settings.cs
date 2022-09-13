using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RaspSGkVk2
{
    [Serializable]
    public class Settings
    {
        public string TokenVk { get; set; }
        public int Timer { get; set; }
        public List<SettingsVk> SettingsVk { get; set; }


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
                    return temp;
                case false:
                    SaveSettings();
                    return this;
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
        public void AddNewSetting(char typeTask, string value, string peerId)
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
        public void DelSettings(int id)
        {
            var temp = SettingsVk.FirstOrDefault(x => x.IdTask == id);
            SettingsVk.Remove(temp);
        }

    }

}
