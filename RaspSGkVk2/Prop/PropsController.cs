using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RaspSGkVk2.Prop
{
    internal class PropsController
    {

        private static List<Settings> _settings = new List<Settings>();
        private static List<Tasks> _tasks = new List<Tasks>();
        private static List<Book> _book = new List<Book>();

        public PropsController()
        {
            UpdateList();
        }

        private void UpdateList()
        {
            using (BotDB ef = new BotDB())
            {
                _settings = ef.Settings.ToList();
                _tasks = ef.Tasks.ToList();
                _book = ef.Book.ToList();
                ef.SaveChangesAsync();
            }
        }


        // ------------------------------- SETTINGS ------------------------------------ //

        public void AddBot(long idgroup, int timer, string token)
        {
            Settings set = new Settings()
            {
                IdGroup = idgroup,
                Timer = timer,
                TokenVk = token
            };

            using (BotDB ef = new BotDB())
            {
                ef.Add(set);
                ef.SaveChanges();
            }

            UpdateList();
        }

        public List<Settings> GetSettings()
        {
            UpdateList();
            return _settings;
        }


        // ------------------------------- TASK ------------------------------------ //








    }
}
