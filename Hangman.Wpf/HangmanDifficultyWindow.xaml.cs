using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace devian.gr.Hangman.Wpf
{
    /// <summary>
    /// Interaction logic for HangmanDifficultyWindow.xaml
    /// </summary>
    public partial class HangmanDifficultyWindow
    {
        HangmanDifficulty _difficulty;

        public static HangmanDifficulty CreateDifficulty()
        {
            var hdwHandler = new HangmanDifficultyWindow();
            hdwHandler.ShowDialog();

            return hdwHandler._difficulty;
        }
        public HangmanDifficultyWindow()
        {

            InitializeComponent();
            DifficultySave.Click += (sender, args) =>
            {
                foreach (var d in HangmanDifficulty.List)
                {
                    if (!d.Name.Equals(DifficultyName.Text)) continue;
                    MessageBox.Show("There is already a Difficulty called " + d.Name, "Hangman", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (Validation.GetHasError(DifficultyTErrors) || Validation.GetHasError(DifficultyTLimit) ||
                    Validation.GetHasError(DifficultyMLetters)) return;
                if (DifficultyTErrors.Value == null || DifficultyMLetters.Value == null ||
                    DifficultyTLimit.Value == null || DifficultyTLimitBox.IsChecked == null) return;
                _difficulty = new HangmanDifficulty(
                    DifficultyName.Text,
                    (int)DifficultyTErrors.Value,
                    (int)DifficultyMLetters.Value,
                    (bool)DifficultyTLimitBox.IsChecked,
                    (int)DifficultyTLimit.Value);
                Close();
            };

        }
    }
}
