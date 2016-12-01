using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using static System.Console;

namespace devian.gr.Hangman.Console
{
    enum ActivePage
    {
        Main,
        Rules,
        Difficulties,
        History
    }
    class Console
    {
        private static readonly HangmanGame _hangmanHandler = new HangmanGame(HangmanDifficulty.Easy);
        const char Highlight = (char)0x2501;
        private static Point HangmanImageCoordinates;
        private static Point HangmanMenuCoordinates;
        private static Point HangmanHeaderCoordinates;
        private static ActivePage page;

        public static void Main(String[] args)
        {

            System.Console.OutputEncoding = Encoding.Unicode;
            //WriteHr('=');

            _hangmanHandler.OnFinish += report =>
            {

                CheckGameState(report);
                PrintGameState(report.State);
            };
            _hangmanHandler.OnSecondElapsed += state =>
            {
                if (page == ActivePage.Main)
                    PrintGameState(state);
            };
            _hangmanHandler.OnStart += state =>
            {
                var prevLeft = CursorLeft;
                var prevTop = CursorTop;
                SetCursorPosition(2, (int)HangmanImageCoordinates.Y);
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 2; j < BufferWidth - 2; j++)
                    {
                        Write(" ");
                    }

                    WriteLine();
                    SetCursorPosition(2, CursorTop);
                }
                SetCursorPosition(2, (int)HangmanImageCoordinates.Y);
                PrintHangman(0, 0, 0);
                SetCursorPosition(prevLeft, prevTop);
                PrintGameState(state);
                WriteGameState("Started");
            };
            _hangmanHandler.OnAttempt += state =>
            {
                PrintGameState(state);

            };
            Game();

        }
        private static void DisplayImage(int errorCount)
        {
            string imageSeries;
            switch (_hangmanHandler.Difficulty.ToleretableErrors)
            {
                case 0:
                    imageSeries = "6";
                    break;
                case 1:
                    imageSeries = "4,6";
                    break;
                case 2:
                    imageSeries = "2,4,6";
                    break;
                case 3:
                    imageSeries = "1,2,4,6";
                    break;
                case 4:
                    imageSeries = "2,3,4,5,6";
                    break;
                case 5:
                    imageSeries = "1,2,3,4,5,6";
                    break;
                default:
                    imageSeries = "0,1,2,3,4,5,6";
                    break;
            }
            var index = 1;
            foreach (var v in imageSeries.Split(','))
            {
                int z;
                int.TryParse(v, out z);
                if (index == errorCount)
                {

                    PrintHangman(z, 0, 0);
                }
                index++;
            }
        }
        public static void Header()
        {
            WriteHr(0x2554, 0x2550, 0x2557);
            WriteDisplayWord("H a n g M a n");
            WriteHr(0x255A, 0x2550, 0x255D);
            //WriteHr('=');
        }

        public static void PrintHangmanHeader(String str)
        {
            PrintHangmanHeader(str, CursorLeft, CursorTop);
        }
        public static void PrintHangmanHeader(String str, int x, int y)
        {
            HangmanHeaderCoordinates = new Point(x, y);
            SetCursorPosition(x, y);
            WriteHr(0x2554, 0x2550, 0x2557);
            WriteEmptyBordered(8);
            PrintHangman(3, -8, 0);
            WriteHr(0x255A, 0x2550, 0x255D);
            SetCursorPosition(CursorLeft, CursorTop - 9);
            WriteDisplayWord(str);
        }

        public static void Game()
        {
            Game(CursorLeft, CursorTop);
        }
        public static void Game(int x, int y)
        {
            bool br = false;
            page = ActivePage.Main;
            HangmanMenuCoordinates = new Point(x, y);
            SetCursorPosition(x, y);
            WriteHr(0x2554, 0x2550, 0x2557);
            WriteDisplayWord("H a n g M a n");
            WriteHr(0x2560, 0x2550, 0x2563);
            WriteEmptyBordered(8);
            PrintHangman(0, -8, 0);

            WriteHr(0x2560, 0x2550, 0x2563);
            WriteBordered("1. Start Game", _hangmanHandler.IsGameStarted);
            WriteBordered("2. Stop Game", !_hangmanHandler.IsGameStarted);
            WriteBordered("");
            WriteBordered("3. Try Letter", !_hangmanHandler.IsGameStarted);
            WriteBordered("4. Try Solve", !_hangmanHandler.IsGameStarted);
            WriteBordered("");
            WriteBordered("5. Difficulty Selector");
            WriteBordered("");
            WriteBordered("6. History");
            WriteBordered("7. Rules");
            WriteBordered("");
            WriteBordered("0. Exit");
            WriteHr(0x2560, 0x2550, 0x2563);
            WriteBordered("Choice : ");
            WriteHr(0x255A, 0x2550, 0x255D);
            SetCursorPosition(11, CursorTop - 2);
            do
            {
                var Choice = System.Console.ReadKey(true).KeyChar - '0';
                if (Choice >= 0 && Choice <= 7)
                {
                    switch (Choice)
                    {
                        case 1:
                            if (!_hangmanHandler.IsGameStarted)
                                _hangmanHandler.StartGame();
                            break;
                        case 2:
                            if (_hangmanHandler.IsGameStarted)
                                _hangmanHandler.StopGame();
                            break;
                        case 3:
                            if (_hangmanHandler.IsGameStarted)
                            {
                                bool esc = false;
                                SetCursorPosition(2, CursorTop);
                                Write("Choice (Letter): ");
                                char letter = ' ';
                                do
                                {
                                    var key = ReadKey(true);
                                    if (key.Key == ConsoleKey.Escape || key.Key == ConsoleKey.D0)
                                    {
                                        esc = true;
                                        break;
                                    }
                                    letter = key.KeyChar;

                                } while (!Char.IsLetter(letter));
                                if (!esc)
                                {
                                    Write(letter);
                                    _hangmanHandler.TryLetter(letter);
                                }
                                SetCursorPosition(0, CursorTop);
                                WriteBordered("Choice :                                       ");
                                SetCursorPosition(11, CursorTop - 1);
                            }
                            break;
                        case 4:
                            if (_hangmanHandler.IsGameStarted)
                            {
                                SetCursorPosition(2, CursorTop);
                                Write("Choice (Solve): ");
                                bool esc = false;
                                bool canSubmit = false;
                                string word = string.Empty;

                                do
                                {
                                    var key = ReadKey(true);
                                    char letter = key.KeyChar;
                                    if (key.Key == ConsoleKey.Escape || key.Key == ConsoleKey.D0)
                                    {
                                        esc = true;
                                        break;
                                    }
                                    else if (key.Key == ConsoleKey.Enter)
                                    {
                                        if (canSubmit)
                                            break;
                                    }
                                    else if (key.Key == ConsoleKey.Backspace)
                                    {
                                        if (word.Length - 1 >= 0)
                                        {
                                            word = word.Substring(0, word.Length - 1);
                                            Write("\b \b");
                                            SetCursorPosition(18, CursorTop);
                                            if (word.Length == _hangmanHandler.GivenWord.Length)
                                            {
                                                ForegroundColor = ConsoleColor.DarkGreen;
                                                canSubmit = true;
                                            }
                                            else
                                            {
                                                ForegroundColor = ConsoleColor.DarkRed;
                                                canSubmit = false;
                                            }
                                            Write(word.ToUpper());
                                            ForegroundColor = ConsoleColor.Gray;
                                        }
                                    }
                                    else if (Char.IsLetter(letter))
                                    {
                                        word += letter;
                                        SetCursorPosition(18, CursorTop);
                                        if (word.Length == _hangmanHandler.GivenWord.Length)
                                        {
                                            ForegroundColor = ConsoleColor.DarkGreen;
                                            canSubmit = true;
                                        }
                                        else
                                        {
                                            ForegroundColor = ConsoleColor.DarkRed;
                                            canSubmit = false;
                                        }
                                        Write(word.ToUpper());
                                        ForegroundColor = ConsoleColor.Gray;
                                    }

                                } while (true);
                                if (!esc)
                                {
                                    _hangmanHandler.TrySolve(word);
                                }
                                SetCursorPosition(0, CursorTop);
                                WriteBordered("Choice :                                       ");
                                SetCursorPosition(11, CursorTop - 1);
                            }
                            break;
                        case 5:
                            page = ActivePage.Difficulties;
                            Clear();
                            SetCursorPosition(0, 0);
                            WriteHr(0x2554, 0x2550, 0x2557);
                            WriteDisplayWord("H a n g M a n");
                            WriteHr(0x2560, 0x2550, 0x2563);
                            WriteCenter("Difficulty Picker", true);
                            WriteHr(0x2560, 0x2550, 0x2563);
                            int index = 1;
                            foreach (var item in HangmanDifficulty.List)
                            {
                                WriteBordered("");
                                ForegroundColor = item.Name == _hangmanHandler.Difficulty.Name ? ConsoleColor.DarkGreen : ConsoleColor.Gray;
                                SetCursorPosition(2, CursorTop - 1);
                                WriteLine(index + ". " + item.Name);
                                ForegroundColor = ConsoleColor.Gray;
                                index++;
                            }
                            WriteBordered("");
                            WriteBordered(0 + ". Back");
                            WriteHr(0x2560, 0x2550, 0x2563);
                            WriteBordered("Choice : ");
                            WriteHr(0x255A, 0x2550, 0x255D);
                            SetCursorPosition(11, CursorTop - 2);
                            int choice;
                            do
                            {
                                choice = System.Console.ReadKey(true).KeyChar - '0';
                            } while (choice < 0 || choice >= index);

                            if (choice == 0)
                            {
                                Clear();
                                br = true;
                                break;
                            }
                            _hangmanHandler.Difficulty = HangmanDifficulty.List[choice - 1];
                            Clear();
                            br = true;
                            break;

                        case 7:
                            page = ActivePage.Rules;
                            Clear();
                            SetCursorPosition(0, 0);
                            WriteHr(0x2554, 0x2550, 0x2557);
                            WriteDisplayWord("H a n g M a n");
                            WriteHr(0x2560, 0x2550, 0x2563);
                            WriteCenter("Rules", true);
                            WriteHr(0x2560, 0x2550, 0x2563);
                            foreach (String str in _hangmanHandler.Rules.Split('\n'))
                            {
                                int pL = BufferWidth - 4;
                                string[] words = str.Split(' ');
                                var parts = new Dictionary<int, string>();
                                string part = string.Empty;
                                int partCounter = 0;
                                foreach (var word in words)
                                {
                                    if (part.Length + word.Length < pL)
                                    {
                                        part += string.IsNullOrEmpty(part) ? word : " " + word;
                                    }
                                    else
                                    {
                                        parts.Add(partCounter, part);
                                        part = word;
                                        partCounter++;
                                    }
                                }
                                parts.Add(partCounter, part);
                                foreach (var item in parts)
                                {
                                    WriteBordered(item.Value);
                                }
                            }
                            WriteHr(0x2560, 0x2550, 0x2563);
                            WriteCenter("Press any key to exit.", true);
                            WriteHr(0x255A, 0x2550, 0x255D);
                            ReadKey(true);
                            Clear();
                            br = true;

                            break;
                        case 0:
                            return;


                    }
                }
            } while (true && !br);

            Game(x, y);
        }
        // ________
        //|/      |
        //|      (_)
        //|      \|/
        //|       |
        //|      / \
        //|
        //|___
        public static void CheckGameState(HangmanGameReport report)
        {
            String str = string.Empty;
            switch (report.Result)
            {
                case HangmanResult.WonByGuessing:
                    str = "Won - By Guessing the Word";
                    ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                case HangmanResult.WonByTrying:
                    str = "Won - By Trying letters";
                    ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                case HangmanResult.LostErrors:
                    str = "Lost - Too many mistakes";
                    ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case HangmanResult.LostTimeout:
                    str = "Lost - Timeout";
                    ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case HangmanResult.LostByGuessing:
                    str = "Lost - By Guessing the Word";
                    ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case HangmanResult.Stopped:
                    str = "Stopped";
                    ForegroundColor = ConsoleColor.DarkRed;
                    break;
            }
            WriteGameState(str);

        }

        public static void WriteGameState(String str)
        {
            int prevTop = CursorTop;
            int prevLeft = CursorLeft;
            SetCursorPosition((BufferWidth / 2) - str.Length / 2, (int)HangmanImageCoordinates.Y + 4);
            Write(str);
            ForegroundColor = ConsoleColor.Gray;
            SetCursorPosition(prevLeft, prevTop);
        }
        public static void PrintGameState(HangmanGameState state)
        {
            int prevTop = CursorTop;
            int prevLeft = CursorLeft;

            SetCursorPosition(0, (int)HangmanImageCoordinates.Y);
            if (!String.IsNullOrEmpty(_hangmanHandler.GivenWord))
                WriteDisplayWord(_hangmanHandler.DisplayWord.Replace("_", " "));
            var str = "(" + state.FoundLetters + "/" + state.TotalLetters + ")";
            SetCursorPosition((BufferWidth / 2) - str.Length / 2, CursorTop);
            Write("(");
            ForegroundColor = ConsoleColor.DarkGreen;
            Write(state.FoundLetters);
            ForegroundColor = ConsoleColor.Gray;
            Write("/");
            ForegroundColor = ConsoleColor.White;
            Write(state.TotalLetters);
            ForegroundColor = ConsoleColor.Gray;
            Write(")");

            str = "Won    (" + _hangmanHandler.WonGames + ")";
            SetCursorPosition((BufferWidth / 4) - str.Length / 2, CursorTop + 4);
            Write(str);
            str = "(" + _hangmanHandler.LostGames + ")    Lost";
            SetCursorPosition(BufferWidth - (BufferWidth / 4) - str.Length / 2, CursorTop);
            Write(str);

            str = state.TimeElapsed.ToString(@"hh\:mm\:ss");
            SetCursorPosition((BufferWidth / 2) - str.Length / 2, CursorTop);
            WriteCenter(str, true);

            ForegroundColor = ConsoleColor.DarkGreen;
            str = String.Join(",", state.CorrectLetters);
            SetCursorPosition((BufferWidth / 4) - str.Length / 2, CursorTop);
            Write(str);
            str = String.Join(",", state.IncorrectLetters);
            ForegroundColor = ConsoleColor.DarkRed;
            SetCursorPosition(BufferWidth - (BufferWidth / 4) - str.Length / 2, CursorTop);
            Write(str);
            ForegroundColor = ConsoleColor.Gray;
            SetCursorPosition(0, (int)HangmanImageCoordinates.Y);
            DisplayImage(state.FailedAttempts);

            SetCursorPosition(0, (int)HangmanMenuCoordinates.Y + 13);
            WriteBordered("1. Start Game", _hangmanHandler.IsGameStarted);
            WriteBordered("2. Stop Game", !_hangmanHandler.IsGameStarted);
            WriteBordered("");
            WriteBordered("3. Try Letter", !_hangmanHandler.IsGameStarted);
            WriteBordered("4. Try Solve", !_hangmanHandler.IsGameStarted);

            SetCursorPosition(prevLeft, prevTop);

        }
        public static void PrintHangman(int state, int start = 0, int fromRight = 0)
        {

            int prevTop = CursorTop;
            int prevLeft = CursorLeft;
            int Top = CursorTop + start;
            int Left = BufferWidth - 13 - fromRight;
            HangmanImageCoordinates = new Point(Left, Top);
            SetCursorPosition(Left, Top);
            Write(" ________  \n");
            SetCursorPosition(Left, ++Top);
            Write("|/      |  \n");
            SetCursorPosition(Left, ++Top);
            Write("|          \n");
            SetCursorPosition(Left, ++Top);
            Write("|          \n");
            SetCursorPosition(Left, ++Top);
            Write("|          \n");
            SetCursorPosition(Left, ++Top);
            Write("|          \n");
            SetCursorPosition(Left, ++Top);
            Write("|          \n");
            SetCursorPosition(Left, ++Top);
            Write("|___       \n");
            Top -= 7;
            switch (state)
            {
                case 6:
                    SetCursorPosition(Left + 9, Top + 5);
                    WriteLine(@"\");
                    goto case 5;
                case 5:
                    SetCursorPosition(Left + 7, Top + 5);
                    WriteLine(@"/");
                    goto case 4;
                case 4:
                    SetCursorPosition(Left + 9, Top + 3);
                    WriteLine(@"/");
                    goto case 3;
                case 3:
                    SetCursorPosition(Left + 7, Top + 3);
                    WriteLine(@"\");
                    goto case 2;
                case 2:
                    SetCursorPosition(Left + 8, Top + 3);
                    WriteLine("|");
                    SetCursorPosition(Left + 8, Top + 4);
                    WriteLine("|");
                    goto case 1;
                case 1:
                    SetCursorPosition(Left + 7, Top + 2);
                    WriteLine("(" + (char)0x221E + ")");
                    break;
            }
            SetCursorPosition(prevLeft, prevTop);
        }

        public static void WriteEmptyBordered(int times = 1)
        {
            for (int i = 1; i <= times; i++)
            {
                WriteBordered("");
            }
        }
        public static void WriteBordered(String str, bool disabled = false, bool returnToText = false, int b = 0x2551)
        {
            Write((char)b + " ");
            if (disabled)
                System.Console.ForegroundColor = ConsoleColor.DarkGray;
            Write(str);
            if (disabled)
                System.Console.ForegroundColor = ConsoleColor.Gray;
            SetCursorPosition(BufferWidth - 2, CursorTop);
            Write((char)b);
            if (returnToText)
                SetCursorPosition(2 + str.Length + 1, CursorTop);
            else
                WriteLine();
        }
        public static void WriteCenter(String str, bool bordered = false, int b = 0x2551)
        {
            if (bordered)
                Write((char)b);
            SetCursorPosition(BufferWidth / 2 - (str.Length / 2), CursorTop);
            Write(str);
            if (bordered)
            {
                SetCursorPosition(BufferWidth - 2, CursorTop);
                WriteLine((char)b);
            }
        }
        public static void WriteDisplayWord(String str)
        {
            WriteCenter(str, true);
            var str2 = String.Concat(Enumerable.Repeat(Highlight + " ", (int)Math.Ceiling((double)str.Length / 2)));
            str2 = str2.Substring(0, str2.Length - 1);
            WriteCenter(str2, true);
        }

        public static void WriteHr(int l, int m, int r)
        {
            SetCursorPosition(0, CursorTop);
            Write((char)l);
            Write(string.Concat(Enumerable.Repeat((char)m, BufferWidth - 3)));
            WriteLine((char)r);
        }
    }
}
