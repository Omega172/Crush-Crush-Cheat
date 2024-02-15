#include "pch.h"

// Include the respective rendering API hooks
#if FRAMEWORK_RENDER_D3D11
#include "Hooks/D3D11/D3D11Hooks.h"
#endif

#if FRAMEWORK_RENDER_D3D12
#include "Hooks/D3D12/D3D12Hooks.h"
#endif

#define DO_THREAD_SLEEP 0
#define THREAD_SLEEP_TIME 100

namespace Cheat
{
	bool Init()
	{
	#if FRAMEWORK_RENDER_D3D11
		if (kiero::init(kiero::RenderType::D3D11) == kiero::Status::Success)
		{
			if (kiero::bind(8, reinterpret_cast<void**>(&oPresent), hkPresent) != kiero::Status::Success)
				return false;
		}
		else
			return false;
	#endif

	#if FRAMEWORK_UNREAL // If the framework is Unreal initalize the SDK assuming the SDK was generated with CheatGeat by Cormm
		if (!CG::InitSdk())
			return false;

		if (!(*CG::UWorld::GWorld))
			Utils::LogError(Utils::GetLocation(CurrentLoc), "Waiting for GWorld to initalize");

		while (!(*CG::UWorld::GWorld))
			continue;
	#endif

	#if FRAMEWORK_RENDER_D3D12
		if (kiero::init(kiero::RenderType::D3D12) == kiero::Status::Success)
		{
			if (kiero::bind(54, (void**)&oExecuteCommandLists, hkExecuteCommandLists) != kiero::Status::Success)
				return false;

			if (kiero::bind(140, (void**)&oPresent, hkPresent) != kiero::Status::Success)
				return false;
		}
		else
			return false;
	#endif

		Utils::LogDebug(Utils::GetLocation(CurrentLoc), "Initalizing Globals, this can take a bit"); // Log that the globals are being initalized

		MH_STATUS Status = MH_Initialize();
		if (Status == MH_ERROR_ALREADY_INITIALIZED)
			Utils::LogError(Utils::GetLocation(CurrentLoc), "MinHook already initalized");

	#if FRAMEWORK_UNREAL // If using the Unreal framework print the pointer to the Unreal class to make sure it was initalized
		Utils::LogDebug(Utils::GetLocation(CurrentLoc), (std::stringstream() << "Unreal: 0x" << unreal.get()).str());
	#endif

		// Add other globals that need to be initalized here

		Utils::LogDebug(Utils::GetLocation(CurrentLoc), "Globals Initalized"); // Log that the globals have been initalized

		// https://stackoverflow.com/questions/16711697/is-there-any-use-for-unique-ptr-with-array
		// Features
		//Features.push_back(std::make_unique<ESP>());
		Features.push_back(std::make_unique<Quit>());
		Features.push_back(std::make_unique<GameSpeed>());
		Features.push_back(std::make_unique<GiveStuff>());
		Features.push_back(std::make_unique<ModGifts>());
		Features.push_back(std::make_unique<Misc>());

		for (size_t i = 0; i < Features.size(); i++) // A loop to grap the feature pointers and call their respective setup functions
		{
			Features[i].get()->Setup();
		}

		return true; // Return true if the initalization was successful
	}

	void HandleKeys() // A function to handle the keys of both the menu and the features
	{
		if (GetAsyncKeyState(dwMenuKey) & 0x1)
		{
			GUI::bMenuOpen = !GUI::bMenuOpen;

			ImGui::GetIO().MouseDrawCursor = GUI::bMenuOpen;

			if (ImGui::GetIO().MouseDrawCursor)
				SetCursor(NULL);
		}

		if (GetAsyncKeyState(dwConsoleKey) & 0x1)
			console.get()->ToggleVisibility();

		if (GetAsyncKeyState(dwUnloadKey))
			bShouldRun = false;

		for (size_t i = 0; i < Features.size(); i++)
		{
			Features[i].get()->HandleKeys(); // Call the handle keys function for each feature

			// This is mostly outdated but is still useful for some things, using the ImGui::Hotkey function is better which is located in GUI/Custom.h
		}
	}

	DWORD __stdcall CheatThread(LPVOID lpParam)
	{
		hModule = reinterpret_cast<HMODULE>(lpParam); // Store the module handle which is used for unloading the module

#ifdef _DEBUG
		console.get()->SetVisibility(true); // Set the console to be visible if the framework is in debug mode
#endif
		if (!Init())
		{
			// If the initalization failed log an error and set the boolean to false to stop the cheat from running
			Utils::LogError(Utils::GetLocation(CurrentLoc), Cheat::Title + ": Failed to initalize");
			bShouldRun = false;
		}
		else // If the initalization was successful log that the cheat was initalized
			Utils::LogDebug(Utils::GetLocation(CurrentLoc), Cheat::Title + ": Initalized");

		while (bShouldRun) // the main process loop used to asynchonously run the features and handle the keys independently from the game
		{
			HandleKeys();

			for (size_t i = 0; i < Features.size(); i++)
			{
				Features[i].get()->Run();
			}

// If the thread sleep is enabled sleep for the specified amount of time
// This is used to reduce the CPU usage of the module, I would recommend keeping this enabled but added the option to disable it if needed for testing and when messing with less CPU intensive games
#if DO_THREAD_SLEEP
			std::this_thread::sleep_for(std::chrono::milliseconds(THREAD_SLEEP_TIME));
#endif
		}

		console.get()->SetVisibility(true); // Set the console to be visible when the cheat is unloading
		Utils::LogDebug(Utils::GetLocation(CurrentLoc), Cheat::Title + ": Unloading..."); // Log that the cheat is unloading

		#if FRAMEWORK_RENDER_D3D12 // If the framework is using D3D12 unbind the hooks and shutdown kiero, we do this here because the game might crash if we do it in the D3D12Hooks.cpp file like we do with D3D11
		D3D12Release();
		kiero::shutdown();
		#endif

		// Destroy features
		for (size_t i = 0; i < Features.size(); i++) // A loop to grab the feature pointers and call their respective destroy functions to clean up any resources that were used and restore any settings that were changed
		{
			Features[i].get()->Destroy();
		}

		console.get()->Destroy(); // Destroy/Free the console because if we don't the console window will stay open after the cheat is unloaded and can also cause a crash in rare cases

		std::this_thread::sleep_for(std::chrono::seconds(3)); // Sleep for 3 seconds to make sure the console is destroyed and that the hooks are released before unloading the module

		FreeLibraryAndExitThread(hModule, EXIT_SUCCESS); // Unload the module and exit the thread
		return TRUE; // Return true not sure if this is needed at all TBH but it's here
	}
}

// Simple and barebones DllMain to initalize the main thread
// Really the only thing that should be in DllMain is the thread creation
BOOL APIENTRY DllMain(HMODULE hModule, DWORD ulReasonForCall, LPVOID lpReserved)
{
	DisableThreadLibraryCalls(hModule); // Disable unwanted and unneeded thread calls

	if (ulReasonForCall != DLL_PROCESS_ATTACH)
		return TRUE;

	CreateThread(nullptr, NULL, Cheat::CheatThread, hModule, NULL, &Cheat::dwThreadID);

	return TRUE;
}
