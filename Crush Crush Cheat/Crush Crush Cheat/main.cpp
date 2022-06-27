#include "includes.hpp"
#include "Features/UnlockGirls/UnlockGirls.h"

bool attached = false;
Console con = Console::instance(true);

DWORD WINAPI MainThread(LPVOID lpReserved)
{
	do
	{
		std::cout << "Init\n";
		attached = true;

	} while (!attached);

	while (attached)
	{
		if (GetAsyncKeyState(VK_INSERT) & 0x1)
		{
			std::cout << "Unlocking Girls!\n";
			UnlockGirls();
		}
		
		if (GetAsyncKeyState(VK_END) & 0x1)
		{
			std::cout << "Unloading!\n";
			attached = false;
		}
	}

	con.free();

	Sleep(5000);
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