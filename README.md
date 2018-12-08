# GameplayModifiersPlus
#### Github: https://github.com/Kylemc1413/GameplayModifiersPlus/

## Adds additional modifiers to the game, as well as Twitch Chat Integration

### Requires AsyncTwitch and BeatSaberCustomUI 

#### For Usage of the modifiers, refer to their tooltips that explain what they do

### To Use Chat Integration, ensure you have set up Async Twitch to connect to your channel, then turn on the modifier, type !gm help into your chat for details on how to use it, the config for chat settings is located in UserData/GameplayModifiersPlusChatSettings.ini


#### Basic Chat Config Options (More Below)
```
commandsPerMessage: How many commands can be stacked in one chat message
globalCommandCooldown: How long in seconds must pass before another chat message with commands will be activated
allowEveryone: If on, everyone in chat is allowed to use commands for chat integration
allowSubs: If allowEveryone is off and this is on, only subs and moderators will be able to use commands, if this is also off, commands will be limited to moderators
chargesForSuperCharge: The amount of additional charges required to activate a supercharge for a command, charge system is explained below
chargesPerLevel: The amount of charges generated every time a song starts
maxCharges: The max amount of charges that can be accumulated from chargesPerLevel
bitsPerCharge: If greater than 0, any message with bits will add charges based on its multiple of this amount
```
### Charge System
- Every Command has a chargeCost that you can specify, and the command will only be activated if the amount of charges exceeds the cost, and will subtract the cost accordingly
- You can choose to either have charges only be generated automatically per level, to provide a "budget" to limit command usage, or restrict the generation of charges to bits, or both using the options explained above
- SuperCharges require additional charges on top of the chargeCost to use a command, but extend the duration of the effects to be much, much greater...
### Command Chat Config Options
- Since there are enough commands that listing every single option here would be cumbersome, I will explain the "properties" a command can have and what they do
```
chargeCost: The base amount of charges required to use a command
Duration: The base amount of time in seconds the effects of a command last
Cooldown: How long in seconds before that command can be used again
Min: For random commands, the min value of the random range
Max: For random commands, the max value of the random range
```
### Current Commands
- DA: Temporarily activates disappearing arrows for the duration
- Smaller: Makes the notes much smaller for the duration
- Larger: Makes the notes much larger for the duration
- Random: Makes the note size random within the set range for the duration
- InstaFail: For the duration, any mistake will cause instant failure, denoted by red healthbar
- Invincible: For the duration, cannot lose any health, denoted by Gold health bar
- NjsRandom: Randomizes the note jump speed of the notes for the duration
- NoArrows: Turns the map into no arrows mode for the duration
- Funky: For the duration, notes are funky
- Rainbow: Randomizes note colors for the duration, currently left colors will be random warm colors, right will be random cool colors

### Changing the Config
- The config can be changed from the file, and will update itself once the game scene changes, without having to restart
- The config can also be changed from chat by the broadcaster or moderators using the !configchange command, the usage of which is below
#### !configchange command property=value
- Example Usage: '!configchange da cooldown=5' would change the cooldown for da to 5 seconds
#### !configchange property=value
- To be used for Basic properties not attached to commands
- Example usage: '!configchange commandsPerMessage=2' Would change the commands per message to be 2
