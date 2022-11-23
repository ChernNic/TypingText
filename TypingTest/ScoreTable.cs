using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TypingTest
{
    class ScoreTable
    {
        public void Table()
        {
            ConsoleKeyInfo Key;
            do
            {
                Console.CursorVisible = false;
                Console.SetCursorPosition(0, 0);
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("|                                                    ScoreTable                                                        |");
                Console.ResetColor();

                List<User> users;
                if (!File.Exists("ScoreTable.json"))
                {
                    FileStream fileStream = File.Create("ScoreTable.json");
                    users = new List<User>();
                    fileStream.Dispose();
                }
                else
                {
                    string usersInfo = File.ReadAllText("ScoreTable.json");
                    users = JsonConvert.DeserializeObject<List<User>>(usersInfo);
                }

                int i = 1;

                foreach (User user in users)
                {
                    Console.SetCursorPosition(0, i);
                    Console.Write(i + " | "+ user.Name + " " + user.LettersPerMinute + " letters per minute " + user.LettersPerSecond + " letters per second " + user.Mistakes + " mistakes");
                    i++;
                }

                Key = Console.ReadKey(true);
            } while (Key.Key != ConsoleKey.Escape);
            KeyboardMenu.MainMenu();

        }
    }
}
