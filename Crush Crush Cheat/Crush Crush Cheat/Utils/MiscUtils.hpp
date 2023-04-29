#pragma once
#include "../Includes.hpp"

namespace Utils
{
	inline bool keyInit = false;
	
	struct Key
	{
		int keyCode = 0x0;
		std::string keyName = "None";
	};

	inline std::vector<Key> keys =
	{
		{VK_LBUTTON, "Left Mouse"},
		{VK_RBUTTON, "Right Mouse"},
		{VK_CANCEL, "CANCEL"},
		{VK_MBUTTON, "Middle Mouse"},
		{VK_XBUTTON1, "Mouse 4"},
		{VK_XBUTTON2, "Mouse 5"},
		{0x7, "Undefined"},
		{VK_BACK, "Backspace"},
		{VK_TAB, "Tab"},
		{0xA, "Reserved"},
		{0xB, "Reserved"},
		{VK_CLEAR, "Clear"},
		{VK_RETURN, "Enter"},
		{0xE, "Undefined"},
		{0xF, "Undefined"},
		{VK_SHIFT, "Shift"},
		{VK_CONTROL, "Ctrl"},
		{VK_MENU, "Alt"},
		{VK_PAUSE, "Pause"},
		{VK_CAPITAL, "Caps Lock"},
		{VK_KANA, "Kana"},
		{0x16, "Undefined"},
		{VK_JUNJA, "Junja"},
		{VK_FINAL, "Final"},
		{VK_KANJI, "Kanji"},
		{0x1A, "Undefined"},
		{VK_ESCAPE, "Escape"},
		{VK_CONVERT, "Convert"},
		{VK_NONCONVERT, "Non Convert"},
		{VK_ACCEPT, "Accept"},
		{VK_MODECHANGE, "Mode Change"},
		{VK_SPACE, "Space"},
		{VK_PRIOR, "Page Up"},
		{VK_NEXT, "Page Down"},
		{VK_END, "End"},
		{VK_HOME, "Home"},
		{VK_LEFT, "Left Arrow"},
		{VK_UP, "Up Arrow"},
		{VK_RIGHT, "Right Arrow"},
		{VK_DOWN, "Down Arrow"},
		{VK_SELECT, "Select"},
		{VK_PRINT, "Print"},
		{VK_EXECUTE, "Execute"},
		{VK_SNAPSHOT, "Print Screen"},
		{VK_INSERT, "Insert"},
		{VK_DELETE, "Delete"},
		{VK_HELP, "Help"},
		{VK_LWIN, "Left Win"},
		{VK_RWIN, "Right Win"},
		{VK_APPS, "Apps"},
		{0x5E, "Reserved"},
		{VK_SLEEP, "Sleep"},
		{VK_NUMPAD0, "Numpad 0"},
		{VK_NUMPAD1, "Numpad 1"},
		{VK_NUMPAD2, "Numpad 2"},
		{VK_NUMPAD3, "Numpad 3"},
		{VK_NUMPAD4, "Numpad 4"},
		{VK_NUMPAD5, "Numpad 5"},
		{VK_NUMPAD6, "Numpad 6"},
		{VK_NUMPAD7, "Numpad 7"},
		{VK_NUMPAD8, "Numpad 8"},
		{VK_NUMPAD9, "Numpad 9"},
		{VK_MULTIPLY, "Numpad *"},
		{VK_ADD, "Numpad +"},
		{VK_SEPARATOR, "Separator"},
		{VK_SUBTRACT, "Numpad -"},
		{VK_DECIMAL, "Numpad ."},
		{VK_DIVIDE, "Numpad /"},
		{VK_F1, "F1"},
		{VK_F2, "F2"},
		{VK_F3, "F3"},
		{VK_F4, "F4"},
		{VK_F5, "F5"},
		{VK_F6, "F6"},
		{VK_F7, "F7"},
		{VK_F8, "F8"},
		{VK_F9, "F9"},
		{VK_F10, "F10"},
		{VK_F11, "F11"},
		{VK_F12, "F12"},
		{VK_F13, "F13"},
		{VK_F14, "F14"},
		{VK_F15, "F15"},
		{VK_F16, "F16"},
		{VK_F17, "F17"},
		{VK_F18, "F18"},
		{VK_F19, "F19"},
		{VK_F20, "F20"},
		{VK_F21, "F21"},
		{VK_F22, "F22"},
		{VK_F23, "F23"},
		{VK_F24, "F24"},
		{VK_NUMLOCK, "Num Lock"},
		{VK_SCROLL, "Scroll Lock"},
		{VK_LSHIFT, "Left Shift"},
		{VK_RSHIFT, "Right Shift"},
		{VK_LCONTROL, "Left Ctrl"},
		{VK_RCONTROL, "Right Ctrl"},
		{VK_LMENU, "Left Alt"},
		{VK_RMENU, "Right Alt"},
		{VK_BROWSER_BACK, "Browser Back"},
		{VK_BROWSER_FORWARD, "Browser Forward"},
		{VK_BROWSER_REFRESH, "Browser Refresh"},
		{VK_BROWSER_STOP, "Browser Stop"},
		{VK_BROWSER_SEARCH, "Browser Search"},
		{VK_BROWSER_FAVORITES, "Browser Favorites"},
		{VK_BROWSER_HOME, "Browser Home"},
		{VK_VOLUME_MUTE, "Volume Mute"},
		{VK_VOLUME_DOWN, "Volume Down"},
		{VK_VOLUME_UP, "Volume Up"},
		{VK_MEDIA_NEXT_TRACK, "Media Next Track"},
		{VK_MEDIA_PREV_TRACK, "Media Prev Track"},
		{VK_MEDIA_STOP, "Media Stop"},
		{VK_MEDIA_PLAY_PAUSE, "Media Play Pause"},
		{VK_LAUNCH_MAIL, "Launch Mail"},
		{VK_LAUNCH_MEDIA_SELECT, "Launch Media Select"},
		{VK_LAUNCH_APP1, "Launch App1"},
		{VK_LAUNCH_APP2, "Launch App2"},
		{VK_OEM_1, ";"},
		{VK_OEM_PLUS, "="},
		{VK_OEM_COMMA, ","},
		{VK_OEM_MINUS, "-"},
		{VK_OEM_PERIOD, "."},
		{VK_OEM_2, "/"},
		{VK_OEM_3, "`"},
		{VK_OEM_4, "["},
		{VK_OEM_5, "\\"},
		{VK_OEM_6, "]"},
		{VK_OEM_7, "'"},
		{VK_OEM_8, "OEM_8"},
		{0xE0, "Reserved"},
		{0xE1, "OEM"},
		{VK_OEM_102, "OEM_102"},
		{0xE3, "OEM"},
		{0xE4, "OEM"},
		{VK_PROCESSKEY, "Process Key"},
		{0xE6, "OEM"},
		{VK_PACKET, "Packet"},
		{0xE8, "Unassigned"},
		{VK_ATTN, "Attn"},
		{VK_CRSEL, "CrSel"},
		{VK_EXSEL, "ExSel"},
		{VK_EREOF, "Erase EOF"},
		{VK_PLAY, "Play"},
		{VK_ZOOM, "Zoom"},
		{VK_NONAME, "Reserved"},
		{VK_PA1, "PA1"},
		{VK_CLEAR, "Clear"}
	};

	inline int GetKey()
	{
		if (!keyInit)
		{
			for (int i = 0x30; i < 0x39; i++)
			{
				keys.push_back({ i, std::to_string(i) });
			}

			for (int i = 0x3A; i < 0x40; i++)
			{
				keys.push_back({ i, "Undefiend"});
			}

			for (int i = 0x41; i < 0x5A; i++)
			{
				keys.push_back({ i, std::string(1, (char)i) });
			}

			for (int i = 0x88; i < 0x8F; i++)
			{
				keys.push_back({ i, "Unassigned" });
			}

			for (int i = 0x92; i < 0x96; i++)
			{
				keys.push_back({ i, "OEM" });
			}

			for (int i = 0x97; i < 0x9F; i++)
			{
				keys.push_back({ i, "Unassigned" });
			}

			for (int i = 0xC1; i < 0xD7; i++)
			{
				keys.push_back({ i, "Reserved" });
			}

			for (int i = 0xD8; i < 0xDA; i++)
			{
				keys.push_back({ i, "Unassigned" });
			}
			
			for (int i = 0xE9; i < 0xF5; i++)
			{
				keys.push_back({ i, "OEM" });
			}

			keyInit = true;
		}

		std::this_thread::sleep_for(std::chrono::milliseconds(500));

		while (true)
		{
			for (int i = 0; i < 255; i++)
			{
				if (GetAsyncKeyState(i) & 0x1)
				{
					return i;
				}
			}
		}
	}

	inline std::string GetKeyName(int key)
	{
		for (auto& k : keys)
		{
			if (k.keyCode == key)
			{
				return k.keyName;
			}
		}

		return "Unknown";
	}
};