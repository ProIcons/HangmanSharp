using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Diagnostics;
using System.Threading;
using devian.gr.Hangman.Exceptions;
using Timer = System.Timers.Timer;

namespace devian.gr.Hangman
{
    public delegate void HangmanGameFinishedEventHandler(HangmanGameReport report);
    public delegate void HangmanGameStartedEventHandler(HangmanGameState state);

    public delegate void HangmanAttemptEventHandler(HangmanGameState state);

    public delegate void HangmanSecondElapsedEventHandler(HangmanGameState state);

    public class HangmanGame
    {

        #region Events

        public event HangmanGameFinishedEventHandler OnFinish;
        public event HangmanGameStartedEventHandler OnStart;
        public event HangmanAttemptEventHandler OnAttempt;
        public event HangmanSecondElapsedEventHandler OnSecondElapsed;

        #endregion

        #region Private Properties

        private HangmanDifficulty _difficulty;
        private HangmanDifficulty _difficultyPending;

        private readonly Random _random = new Random(DateTime.Now.Millisecond);

        private readonly Stopwatch _stopwatch = new Stopwatch();

        private readonly Timer _timer = new Timer() { AutoReset = true, Interval = 1000, };

        private HangmanGameState _lastGameState;
        private HangmanState _lastState;

        #endregion

        #region Public Accessors

        public String WordProvider { get; set; } = "http://randomword.setgetgo.com/get.php?len={0}";

        public String Rules
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(
                    "HangmanGame is a guessing game. A random word gets fetched from a dictionary and the player tries " +
                    "to guess it by suggesting letters or numbers, within a certain number of guesses.");

                sb.AppendLine(
                    "The word to guess is represented by a row of dashes, representing each letter of the word.\n" +
                    "If the player suggests a letter which occurs in the word, the programm displays it in all its correct positions " +
                    "and in a placeholder bellow the dashed word. If the suggested letter does not occur in the word, the programm draws " +
                    "one element of a hanged man stick figure as a tally mark, and displays the suggested letter on a different placeholder bellow the dashed word.");

                sb.AppendLine("");
                sb.AppendLine(
                    "There are different difficulties. Each difficulty has its own restrictions.\n\nDifficulties: ");

                foreach (var diff in HangmanDifficulty.List)
                {
                    sb.AppendLine(diff.ToString());
                    sb.AppendLine("");
                }

                return sb.ToString();

            }
        }
        public int WonGames { get; private set; }

        public int LostGames { get; private set; }
        public TimeSpan TimeElapsed => _stopwatch.Elapsed;

        public HangmanDifficulty Difficulty
        {
            get
            {
                return _difficulty;
            }

            set
            {
                if (value != null)
                    if (!IsGameStarted)
                        _difficulty = value;
                    else
                        _difficultyPending = value;
                else
                    throw new HangmanException("Game Difficulty must not be null.");
            }
        }

        public Boolean IsGameStarted { get; private set; }

        public string GivenWord { get; private set; }

        public List<string> CorrectLetters { get; } = new List<string>();

        public List<string> IncorrectLetters { get; } = new List<string>();

        public List<HangmanGameReport> History { get; } = new List<HangmanGameReport>();

        #endregion

        public HangmanGame(HangmanDifficulty diff)
        {
            if (diff == null) throw new HangmanException("Game Difficulty must not be null.");
            _difficulty = diff;
            IsGameStarted = false;

            _timer.Elapsed += (sender, args) =>
            {
                if (IsGameStarted)
                {
                    CheckGameState();
                    OnSecondElapsed?.Invoke(_lastGameState);
                }

            };
        }

        #region Public Methods

        public void StartGame(String word = null)
        {
            if (IsGameStarted)
                throw new HangmanGameAlreadyStartedException();
            if (_difficultyPending != null)
            {
                _difficulty = _difficultyPending;
                _difficultyPending = null;
            }
            var client = new WebClient();
            if (word != null)
            {
                GivenWord = word.ToUpper();
            }
            else
            {
                var wordLength = _random.Next(_difficulty.MinimumLetters, 20);
                try
                {
                    GivenWord = client.DownloadString(String.Format(WordProvider,wordLength)).ToUpper();
                }
                catch (WebException)
                {
                    throw new HangmanGameUnableToStartException("Couldn't fetch a random word from Online API");
                }
            }
            _lastState = HangmanState.Started;
            CorrectLetters.Clear();
            IncorrectLetters.Clear();
            _timer.Start();
            _stopwatch.Reset();
            _stopwatch.Start();
            IsGameStarted = true;
            FetchGameState();
            OnStart?.Invoke(_lastGameState);
        }

        public String DisplayWord => !String.IsNullOrEmpty(GivenWord) ? String.Join(" ", GivenWord.Select((c, i) => CorrectLetters.Contains(c.ToString()) || i == 0 || !IsGameStarted ? c : '_').ToArray()) : String.Empty;

        public void ForceTimeout()
        {
            Thread.Sleep(1000);
            CheckGameState();
        }
        public void TryLetter(char letter)
        {
            if (Char.IsLetter(letter))
                TryLetter(letter.ToString().ToUpper());
            else
            {
                throw new HangmanException("TryLetter(char) expected a Letter of the English Alphabet. Got " + letter);
            }
        }

        public void TrySolve(String word)
        {
            if (String.IsNullOrEmpty(word))
                throw new HangmanException("TrySolve(string) expected a string of letters of the English Alphabet. Got empty or null string");
            if (word.Any(x => !char.IsLetter(x)))
                throw new HangmanException("TrySolve(string) expected a string of letters of the English Alphabet. Got " + word);
            if (!IsGameStarted) throw new HangmanGameNotStartedException();
            _lastState = HangmanState.SolveTried;
            CheckGameState();
            OnAttempt?.Invoke(_lastGameState);
            if (GivenWord.Equals(word.ToUpper()))
            {
                var report = new HangmanGameReport()
                {
                    Result = HangmanResult.WonByGuessing,
                    Word = GivenWord,
                    State = _lastGameState
                };
                WonGames++;
                EndGame(report);
            }
            else
            {
                var report = new HangmanGameReport()
                {
                    Result = HangmanResult.LostByGuessing,
                    Word = GivenWord,
                    State = _lastGameState

                };
                LostGames++;
                EndGame(report);
            }
        }
        public void TryLetter(String letter)
        {
            if (String.IsNullOrEmpty(letter))
                throw new HangmanException("TryLetter(string) expected a Letter of the English Alphabet. Got empty or null string");
            if (letter.Length > 1)
                throw new HangmanException("TryLetter(string) expected a Letter of the English Alphabet. Got " + letter);
            if (letter.Any(x => !char.IsLetter(x)))
                throw new HangmanException("TryLetter(string) expected a Letter of the English Alphabet. Got " + letter);

            if (!IsGameStarted) throw new HangmanGameNotStartedException();
            _lastState = HangmanState.LetterTried;
            var c = letter.ToUpper();
            if (GivenWord.Contains(c))
            {
                if (!CorrectLetters.Contains(c))
                {
                    CorrectLetters.Add(c);
                    FetchGameState();
                    OnAttempt?.Invoke(_lastGameState);
                }
            }
            else
            {
                if (!IncorrectLetters.Contains(c))
                {
                    IncorrectLetters.Add(c);
                    FetchGameState();
                    OnAttempt?.Invoke(_lastGameState);
                }
            }
            CheckGameState();

        }

        public void StopGame()
        {
            if (!IsGameStarted) throw new HangmanGameNotStartedException();
            _lastState = HangmanState.Stopped;
            FetchGameState();
            var report = new HangmanGameReport()
            {
                Result = HangmanResult.Stopped,
                Word = GivenWord,
                State = _lastGameState
            };
            LostGames++;
            EndGame(report);
        }

        #endregion

        #region Private Methods

        private void EndGame(HangmanGameReport report)
        {

            _lastState = HangmanState.Finished;
            report.State.State = _lastState;
            History.Add(report);
            _stopwatch.Stop();
            _timer.Stop();
            IsGameStarted = false;
            OnFinish?.Invoke(report);
        }

        private void CheckGameState()
        {
            if (!IsGameStarted) return;
            FetchGameState();

            if (_difficulty.IsTimeLimited && _stopwatch.Elapsed.Seconds >= _difficulty.TimeLimit)
            {
                var report = new HangmanGameReport()
                {
                    Result = HangmanResult.LostTimeout,
                    Word = GivenWord,
                    State = _lastGameState

                };
                LostGames++;
                EndGame(report);
            }
            else if (IncorrectLetters.Count > _difficulty.ToleretableErrors)
            {
                var report = new HangmanGameReport()
                {
                    Result = HangmanResult.LostErrors,
                    Word = GivenWord,
                    State = _lastGameState
                };
                LostGames++;
                EndGame(report);
            }
            else if (!DisplayWord.Contains("_"))
            {
                var report = new HangmanGameReport()
                {
                    Result = HangmanResult.WonByTrying,
                    Word = GivenWord,
                    State = _lastGameState
                };
                WonGames++;
                EndGame(report);
            }

        }

        private void FetchGameState()
        {
            _lastGameState = new HangmanGameState()
            {
                CorrectAttempts = CorrectLetters.Count,
                FailedAttempts = IncorrectLetters.Count,
                CorrectLetters = CorrectLetters.Select(str => (String)str.Clone()).ToList(),
                IncorrectLetters = IncorrectLetters.Select(str => (String)str.Clone()).ToList(),
                Difficulty = Difficulty,
                FoundLetters = DisplayWord.Replace(" ", String.Empty).Count(f => f != '_'),
                TotalLetters = GivenWord.Length,
                State = _lastState,
                TimeElapsed = _stopwatch.Elapsed,
                DisplayWord = DisplayWord
            };
        }
        #endregion

    }
}

