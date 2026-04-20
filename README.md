# Built for BepInEx 6.0.0-be
netstandard.dll: Present <br/>
Unity: 2019.4.41f2 <br/>
BepInEx Ver: 6.0.0-be.755 <br/>

## Building
Install [.NET 10.0.202 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)<br/>
Install [BepInEx 6.0.0-be.755](https://builds.bepinex.dev/projects/bepinex_be/755/BepInEx-Unity.Mono-win-x86-6.0.0-be.755%2B3fab71a.zip) into your Crush Crush game directory

OmegaWare-CrushCrush.csproj:
- Change line 31 to point to `..\YOUR\GAME\DIR\CrushCrush\CrushCrush_Data\Managed\Assembly-CSharp.dll`
- Change line 36 to point to `..\YOUR\GAME\DIR\CrushCrush\BepInEx\plugins`
- If making a PR, make sure to update the version on line 7

Then run `dotnet restore` and make any changes you want<br/>
To build run `dotnet build`

## Features
- Unlock All Items & Girls (Now combined into one feature)
- Show all album pinups
- Unlock all album date pics
- Game Speed Slider & Toggle Buttons
- Add Diamonds
- Set Current Girl To Lover
- Set All Girls Lover
- All Phone Conversations Unlocked
- Enable NSFW
- Gift quantity override
- Skip Phone Timer & Hotkeys

### TODO
- Fufill Heart Requirements/Skip To Next Love Level

## Images
Menu <br/>
![Menu](Images/Menu.png)
