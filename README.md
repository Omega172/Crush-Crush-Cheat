# Crush-Crush-Cheat
A Cheat made for the Steam version of [Crush Crush](https://store.steampowered.com/app/459820/Crush_Crush/)

# Why?
Before I started this project, I was looking into reverse engineering and hacking Unity games, and I know the best way I learn is by doing.
So I looked around on steam to find a free game made in Unity and I found Crush Crush, I was not interested in the content of the game just that it was made in Unity.

# Features
- An in-game menu created by hooking the games Direct-X 11 with keiro and using Dear-ImGui to render
- The ability to unload the DLL at anytime to resume normal game function
- A console attached to the game used for outputing debug information and hook status with a button to enable and disable it
- A simple watermark in the top right corner that displays the cheat's title and the current FPS
- Hooks for many of the games internal functions using MinHook

# Cheats
- Menu
-- Toggle using the **Insert** key

- Cheat Section
-- **Unload**: A button that will unload the cheat from the game, which can also be achieved by pressing the **End** key

- Speed Hack Section
-- This cheat modifies the games internal time scale to increase or decrease the simulation speed
-- **Factor**: An unlabled intiger input field that specifies how much to increase the time scale by
-- **Speed Hack**: A checkbox which enables or disables this feature

- Diamonds Section
-- This cheat adds diamonds (the premium currency) to the current save
-- **Quantity**: An unlabled intiger input field specifying the quanyity of diamonds to add (can be negitive)
-- **Give Diamonds**: A button that will execute the games GiveDiamond function

- Gifts Section
-- This cheat enables a hook which upon gifting one of the girls an item modifies the quantity to the set number
-- **Quantity**: An intiger input field specifying what the number of gifts should be set to
-- **Override Gift Quantity**: A checkbox which enables or disables this feature

- Misc Section
-- **Unlock All Girls**: A button, this cheat will unlock all of the characters requardless of you owning the correct bundles or if you meet their requirements (Will not persist after reload of a save if disabled, although the girls stats and progress will persist if the game is saved)
-- **Phone Skip Wait**: A button, this cheat will skip the timer between messages of the current open phone conversation (will do nothing if the phone is closed)
-- **Unlock Date Picks**: A button, this cheat will unlock all of the memory album date photos for all currently unlocked girls (Will persist if the game is saved)
-- **All Girls Lover**: A checkbox, this cheat enables a hook which is triggered on game update, which will set all girls relation ship level to **Lover** (Will persist if the game is saved)
-- **Unlock Phone Convos**: A checkbox, this cheat enables a hook that will unlock all phone conversations requardless of you owning the correct bundles or if you meet their requirements (Will not persist after save if disabled, although the progress of any conversations will persist if the game is saved)
-- **Unlock Pinups**: A checkbox, this cheat will reveal all memory album pinup photos while enabled, a quick note is that two of the pinups will remain hidden because they don't actually exist to my knowledge (Will not persist)
