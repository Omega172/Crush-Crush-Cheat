#pragma once
#define WIN32_LEAN_AND_MEAN

#include <Windows.h>
#include <iostream>
#include <sstream>
#include <thread>
#include <vector>
#include <D3D11.h>
#include <DXGI.h>
#include <mono/metadata/threads.h>
#include <mono/metadata/object.h>

inline bool bExit = false;
inline bool bExtraDebug = false;

// Main
#include "Main/Main.hpp"

// ImGui
#include "ImGui/imgui.h"
#include "ImGui/imgui_impl_win32.h"
#include "ImGui/imgui_impl_dx11.h"
#include "GUI/Custom.hpp"

// Utils
#include "Utils/Console/Console.hpp"
#include "Utils/Logging/Color.hpp"
#include "Utils/Logging/Logging.hpp"
#include "Utils/MiscUtils.hpp"

// Hooking
#include "Hooking/Keiro/kiero.h"
#include "Hooking/Keiro/minhook/include/MinHook.h"
#include "Hooking/Hook.h"

// SDK
#include "SDK/Mono.hpp"
#include "SDK/Girls.hpp"
#include "SDK/Enums.hpp"

// Features
#include "Features/Watermark.hpp"
inline bool bWatermark = true;
inline bool bWatermarkFPS = true;
#include "Features/UnlockGirls.hpp"
inline UnlockGirls unlockGirls;
#include "Features/SpeedHack.hpp"
inline SpeedHack speedHack;
#include "Features/ModifyGiftQuantity.hpp"
inline ModifyGiftQuantity modifyGiftQuantity;
#include "Features/InfiniteDiamonds.hpp"
inline InfiniteDiamonds infiniteDiamonds;
#include "Features/PhoneSkip.hpp"
inline PhoneSkip phoneSkip;
#include "Features/ModGirls.hpp"
inline ModGirls modGirls;
#include "Features/AlbumUnlock.hpp"
inline AlbumUnlock albumUnlock;
#include "Features/JobUnlock.hpp"
inline JobUnlock jobUnlock;
#include "Features/HobbiesUnlock.hpp"
inline HobbiesUnlock hobbiesUnlock;
#include "Features/NSFW.hpp"
inline NSFW nsfw;

typedef HRESULT(__stdcall* Present) (IDXGISwapChain* pSwapChain, UINT SyncInterval, UINT Flags);
typedef LRESULT(CALLBACK* WNDPROC)(HWND, UINT, WPARAM, LPARAM);
typedef uintptr_t PTR;
