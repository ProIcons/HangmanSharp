# Hangman
[![Build status](https://ci.appveyor.com/api/projects/status/xxcqqkb32alb2glc?svg=true)](https://ci.appveyor.com/project/ProIcons/hangman)

A library for integrating Hangman game in different projects.

## Classes
Class | Methods | Properties | Events
----- | ------- | ---------- | ------
[HangmanGame](#hangmangame-class) | [5](#methods) | [11](#properties) | [4](#events)
[HangmanDifficulty](#hangmandifficulty-class) | [2](#methods-2) | [6](#properties-2) | 0 
[HangmanGameReport](#hangmangamereport-class) | 0 | [2](#properties-4) | 0
[HangmanGameState](#hangmangamestate-class) | 0 | [10](#properties-6) | 0

## Enums
Enums | Values
----- | ------
[HangmanState](#hangmanstate-enum) | [5](#values)
[HangmanResult](#hangmanresult-enum) | [6](#values-1)

## Exceptions

Exception | 
--------- |
HangmanException |
HangmanGameAlreadyStartedException |
HangmanGameNotStartedException |
HangmanGameUnableToStartException |


# HangmanGame Class

##[Methods](#methods-1)
Method | Modifier | Return Type | Parameters
------ | -------- | ----------- | ----------
[HangmanGame](#hangmangame-hangmandifficulty) | | Constructor | [HanngmanDifficulty](#hangmandifficulty-class)
[StartGame](#hangmangamestartgame-string--null) | | void | String/void
[StopGame](#hangmangamestopgame-void) | | void | void
[TryLetter](#hangmangametryletter-char--string) | | void | char/string
[TrySolve](#hangmangametrysolve-string) | | void | String

##[Properties](#properties-1)
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

##[Events](#events-1)
Event | Parameters
----- | ----------
[OnFinish](#hangmangameonfinish) | [HangmanGameReport](#hangmangamereport-class)
[OnStart](#hangmangameonstart) | [HangmanGameState](#hangmangamestate-class)
[OnAttempt](#hangmangameonattempt) | [HangmanGameState](#hangmangamestate-class)
[OnSecondElapsed](#hangmangameonsecondelapsed) | [HangmanGameState](#hangmangamestate-class)

---

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


---

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


---

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

---

# HangmanDifficulty Class

##[Methods](#methods-3)
Method | Modifier | Return Type | Parameters
------ | -------- | ----------- | ----------
[HangmanDifficulty](#hangmandifficultystringintintboolint) | | Constructor | String, int, int, bool, int
[ToString](#hangmandifficultytostring) | | String | void

##[Properties](#properties-3)
Property | Modifier | Return Type
-------- | -------- | -----------
[Name](#hangmandifficultyname) | | String
[ToleretableErrors](#hangmandifficultytoleretableerrors) | | int
[MinimumLetters](#hangmandifficultyminimumletters) | | int
[IsTimeLimited](#hangmandifficultyistimelimited) | | bool
[TimeLimit](#hangmandifficultytimelimit) | | int
[List](#hangmandifficultylist) | Static | List\<HangmanDifficulty\>
[Easy](#hangmandifficultyeasy) | Static | HangmanDifficulty
[Medium](#hangmandifficultymedium) | Static | HangmanDifficulty
[Hard](#hangmandifficultyhard) | Static | HangmanDifficulty
[Extreme](#hangmandifficultyextreme) | Static | HangmanDifficulty

---

##Methods

### HangmanDifficulty(string,int,int,bool,int)
Initializes a HangmanDifficulty object with user defined game constraints

Parameters:

Type | Name | Constraints | Description
---- | ---- | ----------- | -----------
String | Name | | Defines the name of the Difficulty
int | toleretableErrors | [0-6] | Defines the maximum toleretable errors a user can do.
int | minimumLetters | [4-20] | Defines the minimum letter count.
bool | isTimeLimited | true/false | Defines whether the game will be time limited. [Optional]
int | TimeLimit | [0-3600] | Defines the time limit in seconds. [Optional]

```cs
HangmanDifficulty difficulty = new HangmanDifficulty("Custom Difficulty",1,10,true,300);
HangmanGame _gameHandler = new HangmanGame(difficulty);
```

### HangmanDifficulty.ToString()
Returns a String with Object's information.

---

##Properties

### HangmanDifficulty.Name
Returns the Difficulty name.

### HangmanDifficulty.ToleretableErrors
Returns the maximum Toleretable Errors.

### HangmanDifficulty.MinimumLetters
Returns the minimum word letters.

### HangmanDifficulty.IsTimeLimited
Returns whether the game's difficulty is time limited.

### HangmanDifficulty.TimeLimit
Returns the time limit in seconds

### HangmanDifficulty.List
Returns a List of HangmanDifficulty Objects.

### HangmanDifficulty.Easy
Returns an easy HangmanDifficulty Object.

### HangmanDifficulty.Medium
Returns a medium HangmanDifficulty Object.

### HangmanDifficulty.Hard
Returns a hard HangmanDifficulty Object.

### HangmanDifficulty.Extreme
Returns an extreme HangmanDifficulty Object.

---

# HangmanGameReport Class

##[Properties](#properties-5)
Property | Modifier | Return Type
-------- | -------- | -----------
[Result](#hangmangamereportresult) | | HangmanResult
[Word](#hangmangamereportword) | | String
[State](#hangmangamereportstate) | | HangmanGameState

---

##Properties

### HangmanGameReport.Result
Returns a HangmanResult enum value.

### HangmanGameReport.Word
Returns a String containing the game's word.

### HangmanGameReport.State
Returns a HangmanGameState object.

---

# HangmanGameState Class

##[Properties](#properties-7)
Property | Modifier | Return Type
-------- | -------- | -----------
[TimeElapsed](#hangmangamestatetimeelapsed) | | TimeSpan
[Difficulty](#hangmangamestatedifficulty) | | [HanngmanDifficulty](#hangmandifficulty-class)
[DisplayWord](#hangmangamestatedisplayword) | | String
[CorrectLetters](#hangmangamestatecorrectletters) | | List\<String\>
[IncorrectLetters](#hangmangamestateincorrectletters) | | List\<String\>
[CorrectAttempts](#hangmangamestatecorrectattempts) | | int
[FailedAttemmpts](#hangmangamestatefailedattempts) | | int
[TotalLetters](#hangmangamestatetotalletters) | | int
[FoundLetters](#hangmangamestatefoundletters) | | int
[State](#hangmangamestatestate) | | [HangmanState](#hangmanstate-enum)


##Properties

### HangmanGameState.TimeElapsed
Returns a TimeSpan with the time elapsed from start since this game state.

### HangmanGameState.Difficulty
Returns a HangmanDifficulty object with the game's difficulty.

### HangmanGameState.DisplayWord
Returns a String with the hidden word beeing dashed and spaced only with the found letters.

### HangmanGameState.CorrectLetters
Returns a String List with all the correct letters found in this game session.

### HangmanGameState.IncorrectLetters
Returns a String List with all the incorrect letters found in this game session.

### HangmanGameState.TotalLetters
Returns the length of the word.

### HangmanGameState.FoundLetters
Returns the number of found letters on the hidden word.

### HangmanGameState.State
Returns a HangmanState enum value.

### HangmanGameState.CorrectAttempts
Returns the number of correct attempts.

### HangmanGameState.FailedAttempts
Returns the number of failed attempts.

---

# HangmanState Enum

##Values
Name | Value
---- | ----- 
LetterTried | 0
SolveTried | 1
Started | 2
Stopped | 3
Finished | 4

---

# HangmanResult Enum

##Values
Name | Value
---- | ----- 
WonByGuessing | 0
WonByTrying| 1
LostTimeout | 2
LostErrors | 3
LostByGuessing | 4
Stopped | 5

---

# Examples

---

##Console


<p align="center"><img src ="https://cloud.githubusercontent.com/assets/3339081/20802776/d05d9b28-b7f5-11e6-9657-bd8826f9a4a5.gif"/></p>


---


## GUI


<p align="center"><img src ="https://cloud.githubusercontent.com/assets/3339081/20802821/f46e9fe4-b7f5-11e6-9dcb-c333b0ebaf78.gif" /></p>
