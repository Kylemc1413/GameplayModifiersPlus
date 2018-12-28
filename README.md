#### Github: https://github.com/Kylemc1413/GameplayModifiersPlus/


#### Changelog 1.1.5
- #### Chat Integration
- 3 New Commands! '!gm bombs' will randomly replace notes with bombs for the duration of the command, the chance of a note becoming a bomb can be adjusted in the config. '!gm faster' and '!gm slower' will change the song speed, just like how faster song, slower song, and practice mode do! The multiplier for each as well as the typical command options can be adjusted, and default to the values faster song and slower song use
- Various bugfixes
#### Changelog 1.1.0
- #### Chat Integration
- New Command! '!gm pause' Will literally just pause the streamer's game when activated if the charge requirements are met, and will activate a global cooldown for all commands
- Mods can now use !gm reset to deactivate all non permanent commands (Basically everything except noarrows), reset cooldowns, and reset charges to the charges present at the start of a level
- Actual UI for chat integration in songs, Explained in more detail in the chat integration section, has settings to have it be in the top center or bottom center of the screen
- Charges can now be generated over time as well as the other ways, for example generating 2 charges every 10 seconds during a song, or whatever other configuration you would like to use
- Charges can now reset at the end of every song! This option can of course be toggled in the config
- Can now have commands not display the cooldowns they trigger when activated, as the in game ui displays cooldowns (It is recommended you keep the cooldowns showing in chat as well if you don't have a low stream delay)
- #### Bug Fixes
- Super Charges will now only consume charges/activate for the chat message attempting to use them, rather than consuming the super charges instantly and then triggering for whatever message has a command capable of activating
- Fixed issue with rainbow changing the block colors past the length of the song if the song was exited with rainbow on for users without custom colors installed
- Various other small fixes
#### Changelog 1.0.5
- ##### General
- Added 2 new modifiers to additional modifiers: Note Size adjustment, rather than just random note size, and Gnome On Miss
- Revisions to readme to try and make it easier to understand
- ##### Chat Integration
- Minor fixes to chat integration commands
- Addition: how many commands are allowed per message is now also shown in '!gm charges' 
- All costs are now shown in '!gm charges' even if the cost is 0, as some people were confused by commands not being visible there
#### Changelog 1.0.2: Fixed issue preventing score submission

# Adds additional modifiers to the game, as well as Twitch Chat Integration

- ### Requires AsyncTwitch and BeatSaberCustomUI 

# Standard Usage
- ### There are several modifiers added to the menu that do not require setting up Async Twitch, however you must do so if you plan on using chat integration
- #### Additional modifiers can be accessed through the ' Additional modifiers' option in the GameplayModfiersPlus option in the modifers panel, which can be accessed by scrolling down the right side of modifiers, below no fail
- #### For Usage of the modifiers, refer to their tooltips that explain what they do
- #### Any modifier that disables score submission defaults to off every time you start the game, including chat integration

# Chat Integration
- ## Make sure to edit the Async Twitch Config in the USERDATA folder, not in Config
##### Async Twitch Setup Guide
- Example Async Config
![Async Config Image](https://i.imgur.com/d9abU8R.png)
- Make sure the game is closed before editing the config
- Username: The name of the user the plugin will send chat messages through
- ChannelName: The name of the channel the plugin will monitor the chat of to look for messages
- OauthKey: The oauth key for the user put in the Username section which is required for the plugin to be able to send messages through that user, you can generate one for that account at https://twitchapps.com/tmi/
- OauthKey MUST Include the 'oauth:' part of the key that is generated
- Do NOT freely give out your oauth key to other people, (And no that is not my actual oauth key in the image, just an example)

- ### To Use Chat Integration, ensure you have set up Async Twitch to connect to your channel, then turn on the modifier, type !gm help into your chat for details on how to use it, the config for chat settings is located in `UserData/GameplayModifiersPlusChatSettings.ini`
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
| Faster | For the duration, the song speed is altered by the multiplier |
| Slower | For the duration, the song speed is altered by the multiplier |

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
