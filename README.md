# Hangman
[![Build status](https://ci.appveyor.com/api/projects/status/xxcqqkb32alb2glc?svg=true)](https://ci.appveyor.com/project/ProIcons/hangman)

A library for integrating Hangman game in different projects.

## Classes
Class | Methods | Properties | Events
----- | ------- | ---------- | ------
[HangmanGame](#hangmangame-class) | 5 | 11 | 4
[HangmanDifficulty](#hangmandifficulty-class) | 2 | 6 | 0 
[HangmanGameReport](#hangmangamereport-class) | 0 | 2 | 0
[HangmanGameState](#hangmangamestate-class) | 0 | 10 | 0

## Enums
Enums | Values
----- | ------
[HangmanState](#hangmanstate-enum) | 5
[HangmanResult](#hangmanresult-enum) | 6

## Exceptions

Exception | 
--------- |
HangmanException |
HangmanGameAlreadyStartedException |
HangmanGameNotStartedException |
HangmanGameUnableToStartException |

### HangmanGame Class

####Methods
Method | Modifier | Return Type | Parameters
------ | -------- | ----------- | ----------
[HangmanGame](#hangmangame-hangmandifficulty) | | Constructor | [HanngmanDifficulty](#hangmandifficulty-class)
[StartGame](#hangmangamestartgame-string--null) | | void | String/void
[StopGame](#hangmangamestopgame-void) | | void | void
[TryLetter](#hangmangametryletter-char--string) | | void | char/string
[TrySolve](#hangmangametrysolve-string) | | void | String

####Properties
Property | Modifier | Return Type
-------- | -------- | -----------
[Rules](#hangmangamerules) | | String
[WonGames](#hangmangamewongames) | | int
[LostGames](#hangmangamelostgames) | | int
[TimeElapsed](#hangmangametimeelapsed) | | TimeSpan
[Difficulty](#hangmangamedifficulty) | | [HanngmanDifficulty](#hangmandifficulty-class)
[IsGameStarted](#hangmangameisgamestarted) | | bool
[GivenWord](#hangmangamegivenword) | | String
[DisplayWord](#hangmangamedisplayword) | | String
[CorrectLetters](#hangmangamecorrectletters) | | List\<String\>
[IncorrectLetters](#hangmangameincorrectletters) | | List\<String\>
[History](#hangmangamehistory) | | List\<[HangmanGameReport](#hangmangamereport-class)\>

####Events
Event | Parameters
----- | ----------
[OnFinish](#hangmangameonfinish) | [HangmanGameReport](#hangmangamereport-class)
[OnStart](#hangmangameonstart) | [HangmanGameState](#hangmangamestate-class)
[OnAttempt](#hangmangameonattempt) | [HangmanGameState](#hangmangamestate-class)
[OnSecondElapsed](#hangmangameonsecondelapsed) | [HangmanGameState](#hangmangamestate-class)


##Methods

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

##Properties

###HangmanGame.Rules
Returns a String with the rules of the game and available Difficulties.

###HangmanGame.WonGames
Returns an Integer with the won games;

###HangmanGame.LostGames
Returns an Integer with the lost games;

###HangmanGame.TimeElapsed
Returns a TimeSpan with the time elapsed since the game started;

###HangmanGame.Difficulty
Returns a [HangmanDifficulty](#hangmandifficulty-class) Object with the Difficulty of the currect active game.
Sets the Difficulty of the next game. If game is active it will get changed after game finishes.

###HangmanGame.IsGameStarted
Returns a Boolean with game's current active state.

###HangmanGame.GivenWord
Returns a String with the hidden word.

###HangmanGame.DisplayWord
Returns a String with the hidden word beeing dashed and spaced only with the found letters.

###HangmanGame.CorrectLetters
Returns a String List with all the correct letters found in this game session.

###HangmanGame.IncorrectLetters
Returns a String List with all the incorrect letters found in this game session.

###HangmanGame.History
Returns a List type of [HangmanGameReport](#hangmangamereport-class) containing all the previous game records.

##Events

###HangmanGame.OnFinish
Event triggered when the game stops for any reason. Event emmits a HangmanGameReport object.
```cs
public delegate void HangmanGameFinishedEventHandler(HangmanGameReport report);
```

###HangmanGame.OnStart
Event triggered when the game starts. Event emmits a HangmanGameState object.
```cs
public delegate void HangmanGameStartedEventHandler(HangmanGameState state);
```

###HangmanGame.OnAttempt
Event triggered when TrySolve or TryLetter is invoked. Event emmits a HangmanGameState object.
```cs
public delegate void HangmanAttemptEventHandler(HangmanGameState state);
```

###HangmanGame.OnSecondElapsed
Event triggered every second after the game is started. Used for Timeout Checking, and Time calculating. Event emmits a HangmanGameState object.
```cs
public delegate void HangmanSecondElapsedEventHandler(HangmanGameState state);
```

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
