#### Github: https://github.com/Kylemc1413/GameplayModifiersPlus/


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
| Faster (Currently Disabled) | For the duration, the song speed is altered by the multiplier |
| Slower (Currently Disabled) | For the duration, the song speed is altered by the multiplier |

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
