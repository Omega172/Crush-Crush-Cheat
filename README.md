# Built for BepInEx 6.0.0-be
netstandard.dll: Present

Unity: 2019.4.41f2

BepInEx Ver: 6.0.0-be.755

# Building Yourself:
Install [.NET 10.0.202 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)

Install [BepInEx 6.0.0-be.755](https://builds.bepinex.dev/projects/bepinex_be/755/BepInEx-Unity.Mono-win-x86-6.0.0-be.755%2B3fab71a.zip) into your Crush Crush game directory

OmegaWare-CrushCrush.csproj:
 - Change line 31 to point to `..\YOUR\GAME\DIR\CrushCrush\CrushCrush_Data\Managed\Assembly-CSharp.dll`
 - Change line 36 to point to `..\YOUR\GAME\DIR\CrushCrush\BepInEx\plugins`

If making a PR, make sure to update the version on line 7

Then run `dotnet restore` and make your changes.

To build run `dotnet build`

```
TODO:
	Port from C++:
		- Gift quantity override
		- Skip Phone Timer & Key
		- Show All Phone Conversations
		- Enable NSFW

	DLC Unlocker?
```

```
Implemented:
	- Unlock All Items & Girls (Now combined into one feature)
	- Show all album pinups
	- Unlock all album date pics
	- Game Speed Slider & Toggle Buttons
	- Add Diamonds
	- Set Current Girl To Lover
	- Set All Girls Lover
