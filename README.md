#### Github: https://github.com/Kylemc1413/GameplayModifiersPlus/
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
- ### To Use Chat Integration, ensure you have set up Async Twitch to connect to your channel, then turn on the modifier, type !gm help into your chat for details on how to use it, the config for chat settings is located in `UserData/GameplayModifiersPlusChatSettings.ini`
- ### Refer to below for more information on using chat integration, if you decide not to read it and end up confused because you did not read it, go read it. But you should probably just read it in the first place and save yourself the future trouble

## IMPORTANT COMMANDS FOR USERS - GAMEPLAY COMMANDS ARE DETAILED BELOW CONFIG
- #####  '!gm help' Links to readme for the mod, lists various commands that give useful information
- ##### '!gm status' Displays whether chat integration is currently on, as well as who can currently use chat integration commands
- ##### '!gm charges' Displays the current charge count, the commands that currently have costs, and the amount of commands that can be in one message. If a command is not listed here, that means it has no cost set
#### Basic Chat Config Options (More Below)
| Option | About |
| - | - |
| `commandsPerMessage` | How many commands can be stacked in one chat message |
| `globalCommandCooldown` | How long in seconds must pass before another chat message with commands will be activated |
| `allowEveryone` | If on, everyone in chat is allowed to use commands for chat integration |
| `allowSubs` | If `allowEveryone` is off and this is on, only subs and moderators will be able to use commands, if this is also off, commands will be limited to moderators  |
| `chargesForSuperCharge` | The amount of additional charges required to activate a supercharge for a command, charge system is explained below |
| `chargesPerLevel` | The amount of charges generated every time a song starts |
| `maxCharges` | The max amount of charges that can be accumulated from `chargesPerLevel` |
| `bitsPerCharge` | If greater than 0, any message with bits will add charges based on its multiple of this amount |

### Charge System
- Every Command has a `chargeCost` that you can specify, and the command will only be activated if the amount of charges exceeds the cost, and will subtract the cost accordingly
- You can choose to either have charges only be generated automatically per level, to provide a "budget" to limit command usage, or restrict the generation of charges to bits, or both using the options explained above
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

### Changing the Config
- The config can be changed from the file, and will update itself once the game scene changes, without having to restart
- The config can also be changed from chat by the broadcaster or moderators using the `!configchange` command, the usage of which is below
#### `!configchange command property=value`
- Example Usage: `!configchange da cooldown=5` would change the cooldown for DA to 5 seconds
#### `!configchange property=value`
- To be used for Basic properties not attached to commands
- Example usage: `!configchange commandsPerMessage=2` Would change the commands per message to be 2
