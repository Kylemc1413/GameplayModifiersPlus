#### Github: https://github.com/Kylemc1413/GameplayModifiersPlus/
###### Obligatory SellOut Message: If you like the mod, consider contributing to my snack fund: [Here](https://ko-fi.com/kyle1413k)
# Adds additional modifiers to the game, as well as Twitch Chat Integration

- ### Requires BS Utils, StreamCore, and BeatSaberMarkupLanguage 
~~#### Requires StreamCore if you plan on using Chat Integration~~

# Standard Usage
- ### Modifiers can be found in the GameplayModifiersPlus tab of the Mods section of the Gameplay Setup Panel when selecting songs
- ### Most modifiers will disable score submission, and because of this default to off when the game is started
# Chat Integration
- ## Make sure to edit the TwitchLoginInfo.ini located in the UserData/StreamCore folder, which can be found in your game installation folder, in the same directory as your game executable, if it does not exist, make sure you have StreamCore installed through the mod installer, and run the game to the main menu, then close the game.
- ## GamePlayModifiersPlusChatSettings.ini will be genereated in the UserData folder after running the game with the plugin installed
##### StreamCore Setup Guide
- Example StreamCore Config
![StreamCore Config Image](https://imgur.com/a/r34mRMz)
- Make sure the game is closed before editing the config
- Username: The name of the user the plugin will send chat messages through
- ChannelName: The name of the channel the plugin will monitor the chat of to look for messages
- OauthKey: The oauth key for the user put in the Username section which is required for the plugin to be able to send messages through that user, you can generate one for that account at https://twitchapps.com/tmi/
- OauthKey MUST Include the 'oauth:' part of the key that is generated
- Do NOT freely give out your oauth key to other people, (And no that is not my actual oauth key in the image, just an example)

- ### To Use Chat Integration, ensure you have set up StreamCore to connect to your channel, then turn on the modifier, type !gm help into your chat for details on how to use it, the config for chat settings is located in `UserData/GameplayModifiersPlusChatSettings.ini` (As of Version 1.6.0 or Later it is located in `UserData/GameplayModifiersPlus.ini` instead)
- ### Refer to below for more information on using chat integration, if you decide not to read it and end up confused because you did not read it, go read it. But you should probably just read it in the first place and save yourself the future trouble

## IMPORTANT COMMANDS FOR USERS - GAMEPLAY COMMANDS ARE DETAILED BELOW CONFIG
- #####  '!gm help' Links to readme for the mod, lists various commands that give useful information
- ##### '!gm status' Displays whether chat integration is currently on, as well as who can currently use chat integration commands
- ##### '!gm charges' Displays the current charge count, the commands that currently have costs, and the amount of commands that can be in one message. If a command is not listed here, that means it has no cost set
#### UI
![UI Image](https://i.imgur.com/gaK45ww.png)
- When chat integration is active, there is now a UI display in game, On the left in red are the currently active cooldowns, indicating which commands can not be used at the moment. In the center is a counter of the current charges compared to the max charges, and on the right in yellow is the commands that are currently active.

#### Basic Chat Config Options (More Below)
| Option | About |
| - | - |
| `uiOnTop` | Whether the in game chat integration UI is on the top or bottom |
| `showCooldownOnMessage` | Whether cooldowns are displayed in chat with the message for command activation |
| `allowModCommands` | Whether the channel's moderators are allowed to edit the config from chat, if off only the broadcaster can |
| `commandsPerMessage` | How many commands can be stacked in one chat message |
| `globalCommandCooldown` | How long in seconds must pass before another chat message with commands will be activated |
| `allowEveryone` | If on, everyone in chat is allowed to use commands for chat integration |
| `allowSubs` | If `allowEveryone` is off and this is on, only subs and moderators will be able to use commands, if this is also off, commands will be limited to moderators  |
| `chargesForSuperCharge` | The amount of additional charges required to activate a supercharge for a command, charge system is explained below |
| `chargesPerLevel` | The amount of charges generated every time a song starts |
| `maxCharges` | The max amount of charges that can be accumulated from `chargesPerLevel` |
| `bitsPerCharge` | If greater than 0, any message with bits will add charges based on its multiple of this amount |
| `timeForCharges` | How often to add charges during a song, if not 0 |
| `chargesOverTime` | If charges over time is active, how many charges to give every occurance of the time period |
| `resetChargesperLevel` | If true, charges will be reset after every song |
| `chatintegration360` | If true, 360 Degree Chat Integration Capabilities will be enabled |
### Charge System
- Every Command has a `chargeCost` that you can specify, and the command will only be activated if the amount of charges exceeds the cost, and will subtract the cost accordingly
- You can choose to either have charges only be generated automatically per level, to provide a "budget" to limit command usage, or restrict the generation of charges to bits, or both using the options explained above
- You can also set charges to be generated over time during a song, as outlined above, in addition to or instead of the above options
- SuperCharges require additional charges on top of the `chargeCost` to use a command, but extend the duration of the effects to be much, much greater...
### Command Chat Config Options
- Since there are enough commands that listing every single option here would be cumbersome, I will explain the "properties" a command can have and what they do

| Option | About |
| - | - |
| `chargeCost` | The base amount of charges required to use a command |
| `Duration` | The base amount of time in seconds the effects of a command last |
| `Cooldown` | How long in seconds before that command can be used again |
| `Min` | For random commands, the min value of the random range |
| `Max` | For random commands, the max value of the random range |
| `Chance` | For commands based on chance, the chance (0-1) of something occuring |
| `Multiplier` | For multiplier based commands, like faster or slower commands, the multiplier used. I.E. 1.2 = 120% |
### Current Commands
| Command | Description |
| - | - |
| DA | Temporarily activates disappearing arrows for the duration |
| Smaller | Makes the notes much smaller for the duration |
| Larger | Makes the notes much larger for the duration |
| Random | Makes the note size random within the set range for the duration |
| InstaFail | For the duration, any mistake will cause instant failure, denoted by red health bar |
| Invincible | For the duration, cannot lose any health, denoted by Gold health bar |
| NjsRandom | Randomizes the note jump speed of the notes for the duration |
| NoArrows | Turns the map into no arrows mode for the duration |
| Funky | For the duration, notes are funky |
| Rainbow | Randomizes note colors for the duration, currently left colors will be random warm colors, right will be random cool colors |
| Pause | Literally just pauses the game. Has it's own seperate global command cooldown you can set, `pauseGlobalCooldown`, instead of a standard cooldown |
| Bombs | For the duration, notes have a chance to be replaced with bombs |
| Poison | For the duration, health regen is disabled |
| OffsetRandom | For the duration, randomizes the note spawn offset |
| Mirror | For the duration, mirrors the map |
| Reverse | For the duration, reverses the direction the notes come from|
| Faster | For the duration, the song speed is altered by the multiplier|
| Slower | For the duration, the song speed is altered by the multiplier|
| Left   | Rotates the level 30 degrees to the left (If 360 Chat Integration Capabilities are enabled) |
| Right   | Rotates the level 30 degrees to the right (If 360 Chat Integration Capabilities are enabled) |
| RandomRotation   | Randomly sporadically rotates the level 30 degrees to the left/right for the duration (If 360 Chat Integration Capabilities are enabled) |


## Moderator Commands
### Reset Command
- Moderators can use '!gm reset' to deactivate all non permament commands (Permanent ones being things like noarrows and pause), reset all active cooldowns, and reset charges to the charges per level, or 0 if that value is set to 0
### Changing the Config
- The config can be changed from the file, and will update itself once the game scene changes, without having to restart
- The config can also be changed from chat by the broadcaster or moderators using the `!configchange` command, the usage of which is below
#### `!configchange command property=value`
- Example Usage: `!configchange da cooldown=5` would change the cooldown for DA to 5 seconds
#### `!configchange property=value`
- To be used for Basic properties not attached to commands
- Example usage: `!configchange commandsPerMessage=2` Would change the commands per message to be 2

## Multiplayer Mode (Currently Disabled)
### Setup
- If you have the BeatSaberMultiplayer plugin installed you can enable GMP's multiplayer functionality by toggling 'Allow in Multiplayer' in the modifiers menu, located below the Additional Modifiers Button
- When the multiplayer functionality is enabled, if you're in a multiplayer room with another person using the same version of GMP with the multiplayer functionality enabled, the mode will activate when a song starts
- When the mode is active, players in the lobby that have GMP active will have (GMP) appended to their names
### Basics
- Multiplayer GMP functions similarly to Chat Integration, in that there will be a charge counter wherever you have the chat integration UI set to appear, with active commands appearing in yellow to the right of it, and commands on cooldown appearing to the left of it in red.
- In multiplayer however, the commands you recieve are sent by other players
- Every few seconds you have a chance of generating a charge, when your charge meter fills you recieve a random powerup, pressing either of the triggers on your controllers will send the power to other players in the lobby using your version of GMP
