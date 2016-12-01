using System;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace devian.gr.Hangman.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _previousIndex;
        private readonly HangmanGame _hangmanHandler = new HangmanGame(HangmanDifficulty.Easy);
        public MainWindow()
        {
            InitializeComponent();
            InitializeDifficultyComboBox();
            InitializeLetterButtons();
            InitializeVisualControlEventHandlers();
            InitializeHangmanHandlers();
            UpdateDisplayLabel("H a n g - M a n");
        }
        #region Initializers

        private void InitializeHangmanHandlers()
        {
            _hangmanHandler.OnFinish += report => Dispatcher.Invoke(() =>
            {
                foreach (Button b in LettersContainer.Children)
                    b.IsEnabled = false;

                SolveTextBox.IsEnabled = false;

                StartButton.IsEnabled = true;
                StopButton.IsEnabled = false;
                WonGamesLabel.Content = "(" + _hangmanHandler.WonGames + ")";
                LostGamesLabel.Content = "(" + _hangmanHandler.LostGames + ")";

                UpdateDisplayLabel(_hangmanHandler.DisplayWord);

                switch (report.Result)
                {
                    case HangmanResult.WonByGuessing:
                        GameState.Content = "Won - By Guessing the Word";
                        GameState.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF116419"));
                        break;
                    case HangmanResult.WonByTrying:
                        GameState.Content = "Won - By Trying letters";
                        GameState.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF116419"));
                        break;
                    case HangmanResult.LostErrors:
                        GameState.Content = "Lost - Too many mistakes";
                        GameState.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF781515"));
                        break;
                    case HangmanResult.LostTimeout:
                        GameState.Content = "Lost - Timeout";
                        GameState.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF781515"));
                        break;
                    case HangmanResult.LostByGuessing:
                        GameState.Content = "Lost - By Guessing the Word";
                        GameState.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF781515"));
                        break;
                    case HangmanResult.Stopped:
                        GameState.Content = "Stopped";
                        GameState.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF000000"));
                        break;
                }

            });

            _hangmanHandler.OnSecondElapsed += (state) => Dispatcher.Invoke(() => TimeLabel.Content = state.TimeElapsed.ToString(@"hh\:mm\:ss"));

            _hangmanHandler.OnStart += (state) => Dispatcher.Invoke(() =>
            {
                foreach (Button b in LettersContainer.Children)
                    b.IsEnabled = true;

                SolveTextBox.IsEnabled = true;
                SolveTextBox.Text = String.Empty;

                UpdateDisplayLabel(state.DisplayWord);

                CorrectLettersLabel.Content = String.Empty;
                IncorrectLettersLabel.Content = "(" + state.FailedAttempts + "/" + state.Difficulty.ToleretableErrors + ") " + String.Join(",", state.IncorrectLetters);

                DisplayWordStatsLabel.Content = "Found Letters: " + state.FoundLetters + "/" + state.TotalLetters;

                HangmanDisplayImage.Source = GetImage($"/Resources/Hangman/Hangman-0.png");

                GameState.Content = "Game Started";
                GameState.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF000000"));


            });

            _hangmanHandler.OnAttempt += (state) => Dispatcher.Invoke(() =>
            {
                CorrectLettersLabel.Content = String.Join(",", state.CorrectLetters);
                IncorrectLettersLabel.Content = "(" + state.FailedAttempts + "/" + state.Difficulty.ToleretableErrors + ") " + String.Join(",", state.IncorrectLetters);
                UpdateDisplayLabel(state.DisplayWord);
                DisplayImage(state.FailedAttempts);
                DisplayWordStatsLabel.Content = "Found Letters: " + state.FoundLetters + "/" + state.TotalLetters;

            });
        }

        private void InitializeVisualControlEventHandlers()
        {
            StartButton.Click += (sender, args) =>
            {
                if (_hangmanHandler.IsGameStarted) return;
                _hangmanHandler.StartGame();
                StartButton.IsEnabled = false;
                StopButton.IsEnabled = true;
            };
            StopButton.Click += (sender, args) =>
            {
                if (!_hangmanHandler.IsGameStarted) return;
                _hangmanHandler.StopGame();
                StartButton.IsEnabled = true;
                StopButton.IsEnabled = false;
            };
            HistoryButton.Click += (sender, args) =>
            {
                //history
            };
            RulesButton.Click += (sender, args) =>
            {
                //rules
            };
            SolveTextBox.TextChanged += (sender, args) =>
            {
                if (!_hangmanHandler.IsGameStarted) return;
                if (String.IsNullOrEmpty(SolveTextBox.Text) || _hangmanHandler.GivenWord.Length != SolveTextBox.Text.Length)
                    SolveButton.IsEnabled = false;
                else
                    SolveButton.IsEnabled = true;
                SolveTextBox.Text = SolveTextBox.Text.ToUpper();
                SolveTextBox.CaretIndex = SolveTextBox.Text.Length;
            };
            SolveButton.Click += (sender, args) =>
            {
                if (!_hangmanHandler.IsGameStarted) return;
                _hangmanHandler.TrySolve(SolveTextBox.Text.Trim());
                SolveButton.IsEnabled = false;
            };
        }

        private void InitializeDifficultyComboBox()
        {
            foreach (var difficulty in HangmanDifficulty.List)
                DifficultyComboBox.Items.Add(difficulty.Name);

            DifficultyComboBox.Items.Add("Custom...");
            DifficultyComboBox.SelectedIndex = 0;
            _previousIndex = 0;

            DifficultyComboBox.SelectionChanged += (sender, args) =>
            {
                if (DifficultyComboBox.SelectedIndex == DifficultyComboBox.Items.Count - 1)
                {
                    var difficulty = HangmanDifficultyWindow.CreateDifficulty();
                    if (difficulty != null)
                    {
                        _previousIndex = DifficultyComboBox.SelectedIndex;

                        _hangmanHandler.Difficulty = difficulty;

                        HangmanDifficulty.List.Add(difficulty);

                        DifficultyComboBox.Items.RemoveAt(DifficultyComboBox.SelectedIndex);

                        DifficultyComboBox.Items.Add(difficulty.Name);
                        DifficultyComboBox.Items.Add("Custom...");

                        DifficultyComboBox.SelectedItem = difficulty.Name;
                    }
                    else
                        DifficultyComboBox.SelectedIndex = _previousIndex;
                }

                else
                {
                    _previousIndex = DifficultyComboBox.SelectedIndex;
                    foreach (var difficulty in HangmanDifficulty.List)
                    {
                        if (difficulty.Name == (string)DifficultyComboBox.SelectedValue)
                        {
                            _hangmanHandler.Difficulty = difficulty;
                        }
                    }
                }
                Console.WriteLine(_hangmanHandler.Difficulty.ToString());

            };
        }

        private void InitializeLetterButtons()
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            var index = 0;
            foreach (var letter in letters)
            {
                var button = new Button
                {
                    Name = "Letter" + index + "Button",
                    Content = letter + "",
                    Width = 30,
                    Height = 30,
                    IsEnabled = false
                };

                button.Click += (sender, args) =>
                {
                    if (!_hangmanHandler.IsGameStarted) return;
                    _hangmanHandler.TryLetter(letter);
                    button.IsEnabled = false;
                };

                LettersContainer.Children.Add(button);
                index++;
            }
        }

        #endregion

        private void UpdateDisplayLabel(String text)
        {
            DisplayWordLabel.Text = "";
            foreach (var str in text.Split(' '))
            {
                DisplayWordLabel.Inlines.Add(new Underline(new Run(str)));
                DisplayWordLabel.Inlines.Add(new Run(" "));
            }
            DisplayWordLabel.Inlines.Remove(DisplayWordLabel.Inlines.LastInline);
        }

        private void DisplayImage(int errorCount)
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
                    HangmanDisplayImage.Source = GetImage($"/Resources/Hangman/Hangman-{z}.png");
                }
                index++;
            }
        }

        private static BitmapImage GetImage(string imageUri)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(imageUri, UriKind.RelativeOrAbsolute);
            bitmapImage.EndInit();
            return bitmapImage;
        }
    }
}
