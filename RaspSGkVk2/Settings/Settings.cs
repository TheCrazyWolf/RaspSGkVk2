using System;
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

        public List<SettingsVk> SettingsVkList { get; set; }
        //Лист админов
        public List<ListAdmins> AdminsList { get; set; }
        //База слов
        public List<Book> Books { get; set; }


        public Settings()
        {
            SettingsVkList = new List<SettingsVk>();
            AdminsList = new List<ListAdmins>();
            Books = new List<Book>();
        }

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

                Console.Write("Номер Администратора бота (VK ID): ");
                string idadmin = Console.ReadLine();

                ListAdmins admin = new ListAdmins()
                {
                    Id = AdminsList.Count + 1,
                    Value = idadmin
                };

                AdminsList.Add(admin);

                Book book = new Book()
                {
                    Id = Books.Count + 1,
                    Word = "привет",
                    Value = "здарова карова;здарова;хаю-хай;шалом;эм"
                };

                Book book2 = new Book()
                {
                    Id = Books.Count + 1,
                    Word = "как дела",
                    Value = "норм;нормас;в целом все как обычно через жопу;херово;ну такое;ага;как же хочется пиццы;нет слов;ты кто"
                };

                Books.Add(book);
                Books.Add(book2);

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
                IdTask = SettingsVkList.Count + 1,
                TypeTask = typeTask,
                Value = value,
                PeerId = peerId
            };

            SettingsVkList.Add(temp);
        }

        /// <summary>
        ///Удаление существуюших настроек
        /// </summary>
        /// <param name="id"></param>
        public void DelTask(int id)
        {
            var temp = SettingsVkList.FirstOrDefault(x => x.IdTask == id);
            SettingsVkList.Remove(temp);
        }

    }

}
