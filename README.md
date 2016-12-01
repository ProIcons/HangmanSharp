# Hangman
[![Build status](https://ci.appveyor.com/api/projects/status/xxcqqkb32alb2glc?svg=true)](https://ci.appveyor.com/project/ProIcons/hangman)

A library for integrating Hangman game in different projects.

## Classes
Class | Methods | Properties | Events
----- | ------- | ---------- | ------
[HangmanGame](#hangmangame-class) | 5 | 11 | 4
[HanngmanDifficulty](#hangmandifficulty-class) | 2 | 6 | 0 

### HangmanGame Class
Method | Modifier | Return Type | Parameters
------ | -------- | ----------- | ----------
[HangmanGame](#hangmangame-hangmandifficulty) | | Constructor | [HanngmanDifficulty](#hangmandifficulty-class)
[StartGame](#hangmangame-startgame-string--null) | | void | String/void
[StopGame](#hamemangame-stopgame-void) | | void | void
[TryLetter](#hangmangame-tryletter-char--string) | | void | char/string
[TrySolve](#hangmangame-trysolve-string) | | void | String

Property | Modifier | Return Type
-------- | -------- | -----------
[Rules](#hanmangame-rules) | | String
[WonGames](#hangmangame-wongames) | | int
[LostGames](#hangmangame-lostgames) | | int
[TimeElapsed](#hangmangame-timeelapsed) | | TimeSpan
[Difficulty](#hangmangame-difficulty) | | [HanngmanDifficulty](#hangmandifficulty-class)
[IsGameStarted](#hangmangame-isgamestarted) | | bool
[GivenWord](#hangmangame-givenword) | | String
[DisplayWord](#hangmangame-displayword) | | String
[CorrectLetters](#hangmangame-correctletters-list-string) | | List\<String\>
[IncorrectLetters](#hangmangame-incorrectletters-list-string) | | List\<String\>
[History](#hangmangame-history-list-hangmangamereport) | | List\<[HangmanGameReport](#hangmangamereport-class)\>


### HangmanGame (HangmanDifficulty)
Initializes a HangmanGame object with a defined Difficulty Level
```cs
HangmanGame _gameHandler = new HangmanGame(HangmanDifficulty.Easy);
```

###HangmanGame.StartGame (String = null)
Starts the game with a random word fetched from a word provider service. If parameter is defined starts the game with the word defined.

Without parameter
```cs
HangmanGame _gameHandler = new HangmanGame(HangmanDifficulty.Easy);
_gameHandler.StartGame();
```

With parameter
```cs
HangmanGame _gameHandler = new HangmanGame(HangmanDifficulty.Easy);
_gameHandler.StartGame("test");
```
**Throws** HangmanGameAlreadyStartedException if game is already started.
**Throws** HangmanGameUnableToStartException if word provider is offline and no parameter is given.

###HangmanGame.StopGame (void)
Stops the game if is started. When it stops the game, a point to LostGames is beeing added.
```cs
HangmanGame _gameHandler = new HangmanGame(HangmanDifficulty.Easy);
_gameHandler.StartGame();
_gameHandler.StopGame();
```
**Throws** HangmanGameNotStartedException if game is already started.

###HangmanGame.TryLetter (char / string)
If game is started it tries to find a letter on the word. 

Raises an OnAttempt event.
```cs
HangmanGame _gameHandler = new HangmanGame(HangmanDifficulty.Easy);
_gameHandler.StartGame();
_gameHandler.TryLetter('c');
_gameHandler.TryLetter("a");
```
**Throws** HangmanException if char is not in [a-Z] range.

**Throws** HangmanException if string is more than 1 character.

**Throws** HangmanException if string is not in [a-Z] range.

**Throws** HangmanGameNotStartedException if game is not started.

###HangmanGame.TrySolve (string)
If game is started it tries to solve the game. If guess is correct player wins the game, and a point is added on WonGames, otherwise a point is added on LostGames

Raises an OnAttempt event. 

```cs
HangmanGame _gameHandler = new HangmanGame(HangmanDifficulty.Easy);
_gameHandler.StartGame();
_gameHandler.TryLetter("Test");
```
**Throws** HangmanException if string is not in [a-Z] range.

**Throws** HangmanGameNotStartedException if game is not started.


## HangmanDifficulty Class

### HangmanDifficulty Class
Method | Return Type | Parameters
------ | ----------- | ----------

Initialize the object with a Difficulty

```cs
HangmanGame gameHandler = new HangmanGame(HangmanDifficulty.Easy);
 ```

<p align="center"><img src ="https://cloud.githubusercontent.com/assets/3339081/20802776/d05d9b28-b7f5-11e6-9657-bd8826f9a4a5.gif"/></p>

<p align="center"><img src ="https://cloud.githubusercontent.com/assets/3339081/20802821/f46e9fe4-b7f5-11e6-9dcb-c333b0ebaf78.gif" /></p>
