#pragma once
#include "../includes.hpp"

// GameState
// Token: 0x0600038F RID: 911 RVA: 0x0001AFB0 File Offset: 0x000191B0
// public void Quit()
// GameState::Quit()

class Quit
{
private:
	bool hookEnabled = false;
	bool toggle = false;

	void* GameState_Quit = nullptr;

public:
	Quit() {};

	void Create()
	{
		GameState_Quit = Mono::Instance().GetCompiledMethod("GameState", "Quit", 0);
		if (GameState_Quit == nullptr)
			return;

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