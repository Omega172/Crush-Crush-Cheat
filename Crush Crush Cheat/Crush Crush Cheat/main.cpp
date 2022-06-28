#include "includes.hpp"
#include "Features/UnlockGirls/UnlockGirls.h"
#include "Features/ModifyGiftQuantity/ModifyGiftQuantity.h"
#include "Features/InfiniteDiamonds/InfiniteDiamonds.h"

bool attached = false;
Console con = Console::instance(true);
UnlockGirls unlockGirls;
ModifyGiftQuantity modGiftQuantity;
InfiniteDiamonds infiniteDiamonds;

DWORD WINAPI MainThread(LPVOID lpReserved)
{
	do
	{	
		// Initalize Hooks
		MH_Initialize();
		unlockGirls.Create();
		unlockGirls.Toggle();
		modGiftQuantity.Create();
		infiniteDiamonds.Create();
		
		attached = true;

	} while (!attached);

	while (attached)
	{
		if (GetAsyncKeyState(VK_INSERT) & 0x1)
		{
			modGiftQuantity.Toggle();
			infiniteDiamonds.Toggle();
		}
		
		if (GetAsyncKeyState(VK_END) & 0x1)
		{
			std::cout << "Unloading!\n";
			attached = false;
		}
	}

	// Restore Hooks
	unlockGirls.Destroy();
	modGiftQuantity.Destroy();

	// Free Utils
	std::this_thread::sleep_for(std::chrono::seconds(3));
	con.free();

	std::this_thread::sleep_for(std::chrono::milliseconds(100));
	FreeLibraryAndExitThread((HMODULE)lpReserved, 1);
	return TRUE;
}

BOOL WINAPI DllMain(HMODULE hMod, DWORD dwReason, LPVOID lpReserved)
{
	switch (dwReason)
	{
	case DLL_PROCESS_ATTACH:
		DisableThreadLibraryCalls(hMod);
		CreateThread(nullptr, 0, MainThread, hMod, 0, nullptr);
		break;
	case DLL_PROCESS_DETACH:
		//kiero::shutdown();
		break;
	}
	return TRUE;
}