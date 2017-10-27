# Unity Exercise 

Clone this Github project and expand it by adding a new game to it.

The project is made in Unity 5.3

It implements a very simple game called React, which briefly displays a stimulus (rectangle) and the player has to respond as quickly as possible.

The codebase is structured so that more complex games can be built on top of it easily and its behavior can be controlled using an xml parameter file (which we call the session file).
See the 'Overview' section below for more details on the structure of the project.


Once you're familiar with the game and how the code is organized, you will be implementing one new game in this project.
In this game you should be able to:

- Specify in the session file whether the stimulus (rectangle) position per trial is random or predefined.
  - The predefined position should be defined inside the session file on a per trial basis. 
  - While the random position should be generated based off a defined range.
- Specify in the session file whether the stimulus should sometimes appear red, in which case the player should NOT respond in order to get a correct response.
- Save all the new game parameters (position, isRed, etc...) when creating a session log file at the end of a game.
- Log important events such as the result of each Trial to a trace file by utilizing the GUILog functionality that the project has, instead of Unity's Debug.Log


# Things to keep in mind

- Treat this exercise as a real world scenario where we ask you to add a new game to our existing project.
- The original React game should remain unchanged.
- The new code should maintain the formatting conventions of the original code.


# Project Overview

- **Stimulus** - An event that a player has to respond to.
- **Session** - A session refers to an entire playthrough of a game.
- **Trial** - A trial is when a player has to respond to a stimulus, which becomes marked as a success or failure depending on the player's response.
- **TrialResult** - A result contains data for how the player responded during a Trial.
- **Session File** - A session file contains all the Trials that will be played during a session, as well as any additional variables that allow us to control and customize the game.
- **Session Log** - An xml log file generated at the end of a session, contains all the attributes defined in the source Session file as well as all the Trial results that were generated during the game session.
- **Trace Log** - A text log file generated using GUILog for debugging and analytical purposes. GUILog requires a SaveLog() function to be called at the end of a session in order for the log to be saved.
- **GameController** - Tracks all the possible game options and selects a defined game to be played at the start of the application.
- **InputController** - Checks for player input and sends an event to the Active game that may be assigned.
- **GUILog** - A trace file logging solution, similar to Unity's Debug.Log, except this one creates a unique log file in the application's starting location.
- **GameBase** - The base class for all games.
- **GameData** - A base class, used for storing game specific data.
- **GameType** - Used to distinguish to which game a Session file belongs to.


# Submission

For your submission, extend this README documenting the rules of the new game, how the code works, how scoring works in the new game, and any other interesting or useful things you can think of for us to take into consideration. Then zip the git repository and send it to us.


Rules of the game: The rules of the game are simple. The player will wait for a image(stimulus) to appear on screen before reacting. If the image is a green square then they press enter to get a correct response which is recorded into a session log. The other side of that is if the stimulus is red then the player should NOT press space for a correct response. Those are the two "CORRECT" cases. The incorrect cases range from responding to a red stimulus, not responding to the stimulus at all, and guessing before the stimulus is revealed.

How the code works: So the code took a bit for me to understand since I have never worked with XMl with C# before and was a learning curve. I broke up how React worked and learned how the code intereacts with each other. I'll start with one of the external components such as the Session File. In my case "NewGame.xml". So what this does it allows us to customize the game without opening unity by modifying tags and values. Those ranging from trial conditions such as position, is it random, is it red as well as settings. Those settings mainly being if the Trials are shuffled and what game the session file is for.

Below I will describe certain files that are needed to get it to work and describe how they work together.

TrialResult.cs: This is where you set the cases of the game such as if it fails, passes, the responseTimes, and accuracy. It gives you the data you need to store the result as well as the response time of the player. Making it able to output the data to an xml file

Trial: This file is where we set and store our attributes, these attributes are more for setting the position and tags for the stimulus as well as parsing it from a xml file.

SessionData: This is where we mostly save the information about the type of game and how the trials are processed

GameType: This is where we store what games we have

GameData: This is where we parse and output data about the game and its trials

SessionUtil: Gets Trials based on the game type, shuffles if needs be and swaps them around if need be.

GameController:Initilizes the logging system, assigns the game we wants, graps a session file and at the end write to the appropiate logfiles

DestroyAfterAnim: Destroys the animator on a gameobject when done.

GameBase: This stores the states of the game as GameEvents, sets up the sessiondata, what trial is running, and the current trial. It writes to the GUILOG what is going on during every part and saves that data such as if a player responded, when trials start and end as well.

GUILog: It writes log files to the appropiate folders. Gives it data such as timestamps, error messages, information and other useful information.

Folders: It creates the folders and seperates the data into appropiate sections such as log files, tracefiles, and files if its unfinished

NewGame: Has the responses for each case the game will need as well all components that need to be manipulated such as uiCanvas, stimulus, feedback text, and instruction text. It grabs a session file before runnin a sessionfile, grabbing the trials and act according to the properties in it. It stores when the trial starts and ends, manipulating the stimuli based on the trial such as isRed or isRandom. At the end it adds the result to a log file which is stored with what happened during each trial.

NewGameData: Has all the relevant data to NewGame such as guessTimeLimit, responseTimeLimit, GeneratedDuration

NewGameTrial: Parses data for NewGame as well able to parse out data for the duration

How scoring works: Scoring works based on if the user responded in time as well to the correct color. There are two main ways to score. One is if it is green then the player must respond before the respond duration is over. If it is red then they will not respond. If they do then they fail. The other ways to not score is to guess, do not respond to a green stimuli, or respond to a red stimuli.


Thought process/useful things: The first thing I had to figure out was how did React worked. So I went through and made a copy of React so I could tinker with the code. I never worked with XML like this and was honestly confused how it worked together with file system. I figured it out in the end and addapted it to NewGame since the requirements want to expand on it.

After that I had to figure out how the game interacted with the Trial files. Once i figured that out and set the XML files to meet the requirements I had to see how the game took it and implemented it.

Going forward to expand it you can add more colors and shapes since the XML file can be easily adjusted to get the trials you want as well as shuffling the Games themself more. If you don't want it shuffled set up a dummy scene that acts like a menu that can transition between games and able to select different sessionfiles.

Sidenote: I made a fork of the project so I can work in my own repositroies yet give you guys the oppertunity to pull the changes.