#### Github: https://github.com/Kylemc1413/GameplayModifiersPlus/


# Chat Integration for Chat Users
## IMPORTANT COMMANDS 
-  '!gm help' Links to readmes for people who don't know how the mod works
- '!gm status' Displays whether chat integration is currently on, as well as who can currently use chat integration commands
- '!gm charges' Displays the current charge count, the commands that currently have costs, and the amount of commands that can be in one message. If a command is not listed here, that means it has no cost set
- '!currentsong' Will display information about the currently playing song if the streamer is currently in a song

### Basics
- You can use any of the commands specified below by including '!gm' followed by a command in your message, i.e. '!gm da'

- The command will activate if there isn't a cooldown active for that command and you have enough charges

- You can type '!gm charges' to see the current charges and the cost of each command, or you can look at the UI described below in the streamer's game to see the current charges and active commands/cooldowns

- You can use the '!gm chargehelp' command to see more information about how the streamer has charges set up to generate

#### UI
![UI Image](https://i.imgur.com/gaK45ww.png)
- When chat integration is active, there is now a UI display in game, On the left in red are the currently active cooldowns, indicating which commands can not be used at the moment. In the center is a counter of the current charges compared to the max charges, and on the right in yellow is the commands that are currently active.

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
| Tunnel | For the duration, the player enters a tunnel |
| Left   | Rotates the level 30 degrees to the left (If 360 Chat Integration Capabilities are enabled) |
| Right   | Rotates the level 30 degrees to the right (If 360 Chat Integration Capabilities are enabled) |
| RandomRotation   | Randomly sporadically rotates the level 30 degrees to the left/right for the duration (If 360 Chat Integration Capabilities are enabled) |
| RCTTS | Force Reality Check Through the Skull to play for the duration (CoolDown cannot be lower than the duration) |
