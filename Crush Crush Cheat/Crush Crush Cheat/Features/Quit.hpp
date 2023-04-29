#pragma once
#include "../includes.hpp"

// GameState
// Token: 0x0600038F RID: 911 RVA: 0x0001AFB0 File Offset: 0x000191B0
// public void Quit()
// GameState::Quit()

// UnityEngine
// Application
// Token: 0x06000A42 RID: 2626
// public static extern void Quit();
// Application::Quit();

class Quit
{
private:
	bool hookEnabled = false;
	bool toggle = false;

	void* GameState_Quit = nullptr;
	bool* isInitialized = nullptr;

public:
	Quit() {};

	void Create()
	{
		isInitialized = (bool*)Mono::Instance().GetStaticFieldValue("GameState", "Initialized");
		if (isInitialized == nullptr)
		{
			LogHook(HookLogReason::Error, "GameState_Initialized", "isInitialized == nullptr");
			return;
		}

		std::string state = (*isInitialized) ? "True" : "False";

		(*isInitialized) ? Log("GameState_Initialized", "GameState::Initialized = " + state + " | Continuing with initialization") : Log("GameState_Initialized", "GameState::Initialized = " + state + " | Waiting for game to initialize");
		
		while (!*isInitialized)
			std::this_thread::sleep_for(std::chrono::milliseconds(100));;
		
		GameState_Quit = Mono::Instance().GetCompiledMethod("GameState", "Quit", 0);
		if (GameState_Quit == nullptr)
		{
			LogHook(HookLogReason::Error, "GameState_Quit", "GameState_Quit == nullptr");
			return;
		}

		LogHook(HookLogReason::Create, "GameState_Quit");
		LogHook(HookLogReason::Enable, "GameState_Quit");
		CreateHook(GameState_Quit);
		EnableHook(GameState_Quit);
	}

	void Destroy()
	{
		LogHook(HookLogReason::Destroy, "GameState_Quit");
		DisableHook(GameState_Quit);
	}

	HOOK_DEF(void, GameState_Quit, (void* __this))
	{
		bExit = true;
		return oGameState_Quit(__this);
	}
};