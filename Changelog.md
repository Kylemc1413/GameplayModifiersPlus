#### Github: https://github.com/Kylemc1413/GameplayModifiersPlus/

#### Changelog 1.3.0
- #### Various Fixes
- #### Chat Integration
- Due to hitsound sync issues the speed changing commands are disabled for now until I can fix the issues
- Adjustments to default config values
- #### General
- No Longer skips the results screen when score submission is disabled! Will now display Red text below the difficulty on the results screen stating that the mod disabled score submission
- Improvements to Rainbow Modifier! Should now be much more pleasing visually with less "in between" colors that look bad
- New Additional Modifiers Modifier: One Color changes all the blocks to one color and allows you to use either saber to hit them
- Can now Toggle Controller Rumble from the modifiers Menu!

- #### Multiplayer Compatible Mode
- Can now turn on The plugin's BeatSaberMultiplayer Compatible mode using the toggle located below the Additional Modifiers button on the GameplayModifiersPlus menu, refer to the bottom of the readme for more information on that mode
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
