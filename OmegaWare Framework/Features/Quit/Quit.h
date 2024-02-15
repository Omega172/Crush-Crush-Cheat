#pragma once
#include "pch.h"

class Quit : public Feature
{
private:
	bool Initalized = false;
	bool* bGameIsInitalized = nullptr;

	void* GameState_Quit = nullptr;

public:
	Quit() {};

	virtual bool Setup()
	{
		bGameIsInitalized = (bool*)Mono::Instance().GetStaticFieldValue("GameState", "Initialized");
		std::string state = (*bGameIsInitalized) ? "True" : "False";

		(*bGameIsInitalized) ? Utils::LogDebug(Utils::GetLocation(CurrentLoc), "GameState::Initialized = " + state + " | Continuing with initialization") : Utils::LogDebug(Utils::GetLocation(CurrentLoc), "GameState::Initialized = " + state + " | Waiting for game to initialize");

		while (!*bGameIsInitalized)
			std::this_thread::sleep_for(std::chrono::milliseconds(100));

		GameState_Quit = Mono::Instance().GetCompiledMethod("GameState", "Quit", 0);
		if (GameState_Quit == nullptr)
			return false;

		Utils::LogHook(Utils::GetLocation(CurrentLoc), "GameState_Quit", "Create", "Hook Created");
		CreateHook(GameState_Quit);

		Utils::LogHook(Utils::GetLocation(CurrentLoc), "GameState_Quit", "Enable", "Hook Enabled");
		EnableHook(GameState_Quit);

		Initalized = true;
		return true;
	}

	virtual void Destroy()
	{
		if (!Initalized)
			return;

		Utils::LogHook(Utils::GetLocation(CurrentLoc), "GameState_Quit", "Destroy", "Hook Destroyed");
		DisableHook(GameState_Quit);
	}

	virtual void HandleKeys() {}

	virtual void DrawMenuItems() {}

	virtual void Render() {}

	virtual void Run() {}

	HOOK_DEF(void, GameState_Quit, (void* __this))
	{
		Cheat::bShouldRun = false;
		return oGameState_Quit(__this);
	}
};