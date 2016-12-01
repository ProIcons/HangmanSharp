using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using devian.gr.Hangman;
using devian.gr.Hangman.Exceptions;
using NUnit.Framework.Constraints;

namespace gr.devian.Hangman.Tests
{
    [TestFixture]
    public class HangmanTests
    {
        private HangmanGame _hangmanGameHandler;



        [SetUp]
        public void Setup()
        {
            _hangmanGameHandler = new HangmanGame(HangmanDifficulty.Easy);
        }


        [Test]
        public void TestStartGameWhenNoGameIsRunning()
        {
            _hangmanGameHandler.OnStart += state =>
            {
                Assert.NotNull(state);
                Assert.AreEqual(state.CorrectAttempts, 0);
                Assert.AreEqual(state.FoundLetters, 1);
                Assert.AreEqual(state.FailedAttempts, 0);
                Assert.AreEqual(state.TotalLetters, 4);
                Assert.AreEqual(state.State, HangmanState.Started);
            };
            _hangmanGameHandler.StartGame("test");

            Assert.AreEqual(_hangmanGameHandler.GivenWord, "TEST");
            Assert.AreEqual(_hangmanGameHandler.DisplayWord, "T _ _ _");
            Assert.IsTrue(_hangmanGameHandler.IsGameStarted);
            Assert.AreEqual(_hangmanGameHandler.Difficulty.Name, HangmanDifficulty.Easy.Name);

            Assert.That(_hangmanGameHandler.CorrectLetters, Is.Empty);
            Assert.That(_hangmanGameHandler.IncorrectLetters, Is.Empty);

            Assert.AreEqual(_hangmanGameHandler.WonGames, 0);
            Assert.AreEqual(_hangmanGameHandler.LostGames, 0);

            Assert.AreEqual(_hangmanGameHandler.History.Count, 0);
        }

        [Test]
        public void TestStartGameWhenGameIsRunning()
        {
            _hangmanGameHandler.StartGame("test");
            Assert.Throws<HangmanGameAlreadyStartedException>(() => _hangmanGameHandler.StartGame());
        }

        [Test]
        public void TestStopGameWhenGameIsRunning()
        {
            _hangmanGameHandler.OnFinish += report =>
            {
                Assert.NotNull(report);
                Assert.AreEqual(report.State.CorrectAttempts, 0);
                Assert.AreEqual(report.State.FoundLetters, 1);
                Assert.AreEqual(report.State.FailedAttempts, 0);
                Assert.AreEqual(report.State.TotalLetters, 4);
                Assert.AreEqual(report.State.State, HangmanState.Finished);
                Assert.AreEqual(report.Result, HangmanResult.Stopped);
            };
            _hangmanGameHandler.StartGame("test");
            _hangmanGameHandler.StopGame();

            Assert.AreEqual(_hangmanGameHandler.LostGames, 1);
            Assert.AreEqual(_hangmanGameHandler.GivenWord, "TEST");
            Assert.AreEqual(_hangmanGameHandler.DisplayWord, "T E S T");
            Assert.IsFalse(_hangmanGameHandler.IsGameStarted);
            Assert.AreEqual(_hangmanGameHandler.Difficulty.Name, HangmanDifficulty.Easy.Name);

            Assert.That(_hangmanGameHandler.CorrectLetters, Is.Empty);
            Assert.That(_hangmanGameHandler.IncorrectLetters, Is.Empty);

            Assert.AreEqual(_hangmanGameHandler.WonGames, 0);

            Assert.AreEqual(_hangmanGameHandler.History.Count, 1);

        }


        [Test]
        public void TestStopGameWhenGameIsNotRunning()
        {
            Assert.Throws<HangmanGameNotStartedException>(() => _hangmanGameHandler.StopGame());
        }

        [Test]
        public void TestChangeDifficultyWhenGameIsRunning()
        {
            _hangmanGameHandler.StartGame("test");
            _hangmanGameHandler.Difficulty = HangmanDifficulty.Extreme;
            Assert.AreNotEqual(_hangmanGameHandler.Difficulty.Name, HangmanDifficulty.Extreme.Name);
            _hangmanGameHandler.StopGame();
            _hangmanGameHandler.StartGame("test");
            Assert.AreEqual(_hangmanGameHandler.Difficulty.Name, HangmanDifficulty.Extreme.Name);
        }
        [Test]
        public void TestChangeDifficultyWhenGameIsNotRunning()
        {
            _hangmanGameHandler.Difficulty = HangmanDifficulty.Extreme;
            Assert.AreEqual(_hangmanGameHandler.Difficulty.Name, HangmanDifficulty.Extreme.Name);
        }
        [Test]
        public void TestChangeDifficultyToNull()
        {
            Assert.Throws<HangmanException>(() => _hangmanGameHandler.Difficulty = null);
        }

        [Test]
        public void TestTryLetterCharOnCorrectValue()
        {
            _hangmanGameHandler.OnAttempt += state =>
            {
                Assert.NotNull(state);
                Assert.AreEqual(state.CorrectAttempts, 1);
                Assert.AreEqual(state.FoundLetters, 2);
                Assert.AreEqual(state.FailedAttempts, 0);
                Assert.AreEqual(state.TotalLetters, 4);
                Assert.AreEqual(state.State, HangmanState.LetterTried);
            };
            _hangmanGameHandler.StartGame("test");
            _hangmanGameHandler.TryLetter('t');

        }
        [Test]
        public void TestTryLetterCharOnIncorrectValue()
        {
            _hangmanGameHandler.OnAttempt += state =>
            {
                Assert.NotNull(state);
                Assert.AreEqual(state.CorrectAttempts, 0);
                Assert.AreEqual(state.FoundLetters, 1);
                Assert.AreEqual(state.FailedAttempts, 1);
                Assert.AreEqual(state.TotalLetters, 4);
                Assert.AreEqual(state.State, HangmanState.LetterTried);
            };
            _hangmanGameHandler.StartGame("test");
            _hangmanGameHandler.TryLetter('a');

        }
        [Test]
        public void TestTryLetterCharOnIllegalValue()
        {

            _hangmanGameHandler.StartGame("test");
            Assert.Throws<HangmanException>(() => _hangmanGameHandler.TryLetter('.'));

        }
        [Test]
        public void TestTryLetterCharOnNull()
        {
            _hangmanGameHandler.StartGame("test");
            Assert.Throws<HangmanException>(() => _hangmanGameHandler.TryLetter(null));
        }
        [Test]
        public void TestTryLetterCharWhenGameIsNotStarted()
        {
            Assert.Throws<HangmanGameNotStartedException>(() => _hangmanGameHandler.TryLetter('a'));
        }
        [Test]
        public void TestTryLetterStringWhenGameIsNotStarted()
        {
            Assert.Throws<HangmanGameNotStartedException>(() => _hangmanGameHandler.TrySolve("a"));
        }
        [Test]
        public void TestTryLetterStringOnIllegalValue()
        {
            _hangmanGameHandler.StartGame("test");
            Assert.Throws<HangmanException>(() => _hangmanGameHandler.TryLetter("."));

        }
        [Test]
        public void TestTryLetterStringOnIncorrectValue()
        {
            _hangmanGameHandler.OnAttempt += state =>
            {
                Assert.NotNull(state);
                Assert.AreEqual(state.CorrectAttempts, 0);
                Assert.AreEqual(state.FoundLetters, 1);
                Assert.AreEqual(state.FailedAttempts, 1);
                Assert.AreEqual(state.TotalLetters, 4);
                Assert.AreEqual(state.State, HangmanState.LetterTried);
            };
            _hangmanGameHandler.StartGame("test");
            _hangmanGameHandler.TryLetter("a");
        }
        [Test]
        public void TestTryLetterStringOnCorrectValue()
        {
            _hangmanGameHandler.OnAttempt += state =>
            {
                Assert.NotNull(state);
                Assert.AreEqual(state.CorrectAttempts, 1);
                Assert.AreEqual(state.FoundLetters, 3);
                Assert.AreEqual(state.FailedAttempts, 0);
                Assert.AreEqual(state.TotalLetters, 5);
                Assert.AreEqual(state.State, HangmanState.LetterTried);
                Assert.True(state.CorrectLetters.All(_hangmanGameHandler.CorrectLetters.Contains));
                Assert.True(state.IncorrectLetters.All(_hangmanGameHandler.IncorrectLetters.Contains));
                Assert.AreEqual(state.DisplayWord, "T E E _ _");
                Assert.AreEqual(state.TimeElapsed.Seconds,_hangmanGameHandler.TimeElapsed.Seconds);
            };
            _hangmanGameHandler.StartGame("teest");
            _hangmanGameHandler.TryLetter("e");

        }
        [Test]
        public void TestTryLetterStringOnMultiCharacterValue()
        {
            _hangmanGameHandler.StartGame("TEST");
            Assert.Throws<HangmanException>(() => _hangmanGameHandler.TryLetter("vsagadgdagda"));
        }
        [Test]
        public void TestTrySolverStringOnCorrectValue()
        {
            _hangmanGameHandler.OnAttempt += state =>
            {
                Assert.NotNull(state);
                Assert.AreEqual(state.CorrectAttempts, 0);
                Assert.AreEqual(state.FoundLetters, 1);
                Assert.AreEqual(state.FailedAttempts, 0);
                Assert.AreEqual(state.TotalLetters, 5);
                Assert.AreEqual(state.State, HangmanState.SolveTried);
                Assert.AreEqual(state.Difficulty,_hangmanGameHandler.Difficulty);
                
            };
            _hangmanGameHandler.OnFinish += report =>
            {
                Assert.NotNull(report);
                Assert.AreEqual(_hangmanGameHandler.WonGames, 1);
                Assert.AreEqual(report.Result, HangmanResult.WonByGuessing);
                Assert.AreEqual(report.Word, _hangmanGameHandler.GivenWord);
            };
            _hangmanGameHandler.StartGame("teest");
            _hangmanGameHandler.TrySolve("TEEST");
        }
        [Test]
        public void TestTrySolverStringOnIncorrectValue()
        {
            _hangmanGameHandler.OnAttempt += state =>
            {
                Assert.NotNull(state);
                Assert.AreEqual(state.CorrectAttempts, 0);
                Assert.AreEqual(state.FoundLetters, 1);
                Assert.AreEqual(state.FailedAttempts, 0);
                Assert.AreEqual(state.TotalLetters, 5);
                Assert.AreEqual(state.State, HangmanState.SolveTried);
            };
            _hangmanGameHandler.OnFinish += report =>
            {
                Assert.NotNull(report);
                Assert.AreEqual(_hangmanGameHandler.LostGames, 1);
                Assert.AreEqual(report.Result, HangmanResult.LostByGuessing);
                Assert.AreEqual(report.Word, _hangmanGameHandler.GivenWord);
            };
            _hangmanGameHandler.StartGame("TEEST");
            _hangmanGameHandler.TrySolve("asd");
        }
        [Test]
        public void TestTrySolverStringOnIllegalValue()
        {
            _hangmanGameHandler.StartGame("TEEST");
            Assert.Throws<HangmanException>(() => _hangmanGameHandler.TrySolve(".g3.af"));
        }

        [Test]
        public void TestTrySolverStringOnNull()
        {
            _hangmanGameHandler.StartGame("TEEST");
            Assert.Throws<HangmanException>(() => _hangmanGameHandler.TrySolve(null));
        }
        [Test]
        public void TestTrySolverWhenGameIsNotStarted()
        {
            Assert.Throws<HangmanGameNotStartedException>(() => _hangmanGameHandler.TrySolve("test"));
        }

        [Test]
        public void TestGameTimeOut()
        {

            Assert.False(_hangmanGameHandler.IsGameStarted);

            _hangmanGameHandler.Difficulty = new HangmanDifficulty("Test", 2, 8, true, 1);

            _hangmanGameHandler.OnFinish += report =>
           {
               Assert.AreEqual(report.Result, HangmanResult.LostTimeout);
           };

            _hangmanGameHandler.StartGame("TEEST");
            _hangmanGameHandler.ForceTimeout();

        }
        [Test]
        public void TestGameLostByTries()
        {

            _hangmanGameHandler.Difficulty = new HangmanDifficulty("Test", 0, 8);

            _hangmanGameHandler.OnFinish += report =>
            {
                Assert.AreEqual(report.Result, HangmanResult.LostErrors);
            };

            _hangmanGameHandler.StartGame("TEEST");
            _hangmanGameHandler.TryLetter('a');

        }
        [Test]
        public void TestGameWonByTries()
        {

            _hangmanGameHandler.OnFinish += report =>
            {
                Assert.AreEqual(report.Result, HangmanResult.WonByTrying);
            };

            _hangmanGameHandler.StartGame("T");
            _hangmanGameHandler.TryLetter('T');

        }

        [Test]
        public void TestInitializeWithNullDifficulty()
        {
            Assert.Throws<HangmanException>(() => _hangmanGameHandler = new HangmanGame(null));
        }

        [Test]
        public void TestStartGameViaApiSuccess()
        {
            _hangmanGameHandler.StartGame();
            Assert.True(!String.IsNullOrEmpty(_hangmanGameHandler.GivenWord));
            Assert.True(_hangmanGameHandler.GivenWord.Length >= _hangmanGameHandler.Difficulty.MinimumLetters);
        }

        [Test]
        public void TestStartGameViaApiFail()
        {
            _hangmanGameHandler.WordProvider = "http://www.devian2.gr/{0}";
            Assert.Throws<HangmanGameUnableToStartException>(() => _hangmanGameHandler.StartGame()); 
        }
        [Test]
        public void TestRulesNotEmpty()
        {
            Assert.IsNotEmpty(_hangmanGameHandler.Rules);
        }


    }
}
