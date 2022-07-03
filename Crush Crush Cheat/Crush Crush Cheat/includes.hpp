#pragma once
#define WIN32_LEAN_AND_MEAN

#include <Windows.h>
#include <iostream>
#include <thread>
#include <vector>
#include <D3D11.h>
#include <DXGI.h>
#include <mono/metadata/threads.h>
#include <mono/metadata/object.h>

inline bool bExit = false;

// Main
#include "Main/Main.hpp"

// ImGui
#include "ImGui/imgui.h"
#include "ImGui/imgui_impl_win32.h"
#include "ImGui/imgui_impl_dx11.h"

// Hooking
#include "Hooking/Keiro/kiero.h"
#include "Hooking/Keiro/minhook/include/MinHook.h"
#include "Hooking/Hook.h"

// Utils
#include "Utils/Console/Console.hpp"

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

typedef HRESULT(__stdcall* Present) (IDXGISwapChain* pSwapChain, UINT SyncInterval, UINT Flags);
typedef LRESULT(CALLBACK* WNDPROC)(HWND, UINT, WPARAM, LPARAM);
typedef uintptr_t PTR;

// GUI
#include "GUI/GUI.hpp"
