using System;
using System.Collections.Generic;
using System.Threading;
using System.Media;
using System.Diagnostics;
using Newtonsoft.Json;
using System.IO;

namespace TypingTest
{
    class TypingSpeedTest
    {
        private int CountOfLetters = 0;
        private int Mistakes = 0;
        bool TimeIsOver;
        string UserName;

        public void Test()
        {
            SoundPlayer soundPlayer = new SoundPlayer(@"Assets/error.wav");

            TimeIsOver = false;

            Console.Write("Write your name: ");
            UserName = Console.ReadLine();
            if (UserName == "") UserName = "John Doe";
            Console.Clear();

            char[] textToWrite = TextToWrite();
            foreach (char ch in textToWrite) // выписывает текст
            {
                Console.Write(ch);
            }

            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(45, 9);
            Console.WriteLine("                                    ");
            Console.SetCursorPosition(45, 10);
            Console.WriteLine("       Press Enter to begin         ");
            Console.SetCursorPosition(45, 11);
            Console.WriteLine("                                    ");

            Console.ResetColor();
            ConsoleKeyInfo Key;
            do
            {
                Key = Console.ReadKey(true);
            } while (Key.Key != ConsoleKey.Enter);

            Console.ResetColor();
            Console.SetCursorPosition(45, 9);
            Console.WriteLine("                                    ");
            Console.SetCursorPosition(45, 10);
            Console.WriteLine("                                    ");
            Console.SetCursorPosition(45, 11);
            Console.WriteLine("                                    ");

            new Thread(Timer).Start();

            int i = 0;
            int j = 0;
            foreach (char letter in textToWrite)
            {
                if (TimeIsOver) break;

                Console.CursorVisible = true;

                char userKey = Console.ReadKey(true).KeyChar;

                try
                {
                    Console.SetCursorPosition(i, j);
                }
                catch (ArgumentOutOfRangeException)
                {
                    j++;
                    i = 0;
                    Console.SetCursorPosition(i, j);
                }

                if (userKey == letter)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write(letter);
                    Console.ResetColor();
                    CountOfLetters++;
                }
                else
                {
                    soundPlayer.Play();
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.Write(letter);
                    Console.ResetColor();
                    Mistakes++;
                }
                i++;
            }
            Console.CursorVisible = false;
            Score();
        }

        private void Timer()
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            do
            {
                Console.CursorVisible = false;

                Console.SetCursorPosition(0, 15);
                Console.WriteLine($"Time Left: 0:{60 - stopwatch.ElapsedMilliseconds / 1000}");
                Thread.Sleep(1000);
            }
            while (60 - stopwatch.ElapsedMilliseconds / 1000 >= 0);

            stopwatch.Stop();
            stopwatch.Reset();

            TimeIsOver = true;
        }

        private void AddToScoreTable()
        {
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

            users.Add(new User(UserName, CountOfLetters, Math.Round(Convert.ToDouble(CountOfLetters) / 60, 3), Mistakes));

            users.Sort((x, y) => x.LettersPerMinute.CompareTo(y.LettersPerMinute));
            users.Reverse();

            string json = JsonConvert.SerializeObject(users);
            File.WriteAllText("ScoreTable.json", json);
        }

        private void Score()
        {
            Console.SetCursorPosition(0, 16);
            Console.WriteLine($"Result: {CountOfLetters} letters per minute // {Math.Round(Convert.ToDouble(CountOfLetters) / 60 , 3)} letters per second. \nMistakes: {Mistakes}\nPress ENTER to repeat.\nPress ESC to exit.");

            AddToScoreTable();

            CountOfLetters = 0;
            Mistakes = 0;

            ConsoleKeyInfo Key;
            do
            {
                Key = Console.ReadKey(true);
                if (Key.Key == ConsoleKey.Escape)
                {
                    KeyboardMenu.MainMenu();
                }
            } while (Key.Key != ConsoleKey.Enter);

            Console.Clear();
            Test();
        }

        private char[] TextToWrite()
        {
            string[] text = { "Безопаснее всего было бы на Земле, подумал он. Но как ты там будешь без меня? И как я здесь буду один? Можно было бы попросить Анку, чтобы дружила с тобой там. Но как я буду здесь без тебя? Нет, на Землю мы полетим вместе. Я сам поведу корабль, а ты будешь сидеть рядом, и я буду все тебе объяснять. Чтобы ты ничего не боялась. Чтобы ты сразу полюбила Землю. Чтобы ты никогда не жалела о своей страшной родине. Потому что эта не твоя родина. Потому что твоя родина отвергла тебя. Потому что ты родилась на тысячу лет раньше своего срока. Добрая, верная, самоотверженная, бескорыстная… Такие, как ты, рождались во все эпохи кровавой истории наших планет. Ясные, чистые души, не знающие ненависти, не приемлющие жестокость. Жертвы. Бесполезные жертвы. Гораздо более бесполезные, чем Гур Сочинитель или Галилей. Потому что такие, как ты, даже не борцы. Чтобы быть борцом, нужно уметь ненавидеть, а как раз этого вы не умеете. Так же, как и мы теперь…",
                              "Она благодарно потерлась носом о его плечо и поцеловала в щеку и снова стала рассказывать, как нынче вечером пришел от отца соседский мальчик. Отец лежит. Его выгнали из канцелярии и на прощание сильно побили палками. Последнее время он вообще ничего не ест, только пьет — стал весь синий, дрожащий. Еще мальчик сказал, что объявился брат — раненый, но веселый и пьяный, в новой форме. Дал отцу денег, выпил с ним и опять грозился, что они всех раскатают. Он теперь в каком-то особом отряде лейтенантом, присягнул на верность Ордену и собирается принять сан. Отец просил, чтобы она домой пока ни в коем случае не приходила. Брат грозился с ней разделаться за то, что спуталась с благородным, рыжая стерва.",
                              "Роберт подумал о Тане, как она терпеливо сидит внизу и ждет. Патрик все бубнил, придвигаясь и отодвигаясь, голос его то громыхал, то становился еле слышен, и Роберт, как всегда, очень скоро потерял нить его рассуждений. Он кивал, картинно морщил лоб, подымал и опускал брови, но он решительно ничего не понимал и с невыносимым стыдом думал, что Таня сидит там, внизу, уткнув подбородок в колени, и ждет, пока он закончит свой важный и непостижимый для непосвященных разговор с ведущими нуль-физиками планеты, пока он не выскажет ведущим нуль-физикам свою, совершенно оригинальную точку зрения по вопросу, из-за которого его беспокоят так поздно ночью, и пока ведущие нуль-физики, удивляясь и покачивая головами, не занесут эту точку зрения в свои блокноты."};

            Random random = new Random();
            char[] result = text[random.Next(0, text.Length)].ToCharArray();

            return result;
        }
    }
}
