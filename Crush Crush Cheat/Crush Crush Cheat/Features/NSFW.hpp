#pragma once
#include "../includes.hpp"

// GameState
// Token: 0x040003FF RID: 1023
// public static bool NSFW
// GameState::NSFW

class NSFW
{
private:
	bool originalValue = false;
	bool* nsfw = nullptr;
	bool toggle = false;
	
public:
	NSFW() {};

	void Render()
	{
		ImGui::Checkbox("Toggle NSFW", &toggle);

		Toggle();
	}

	void Create()
	{	
		nsfw = (bool*)Mono::instance().GetStaticFieldValue("GameState", "NSFW");
		if (nsfw == nullptr)
		{
			LogHook(HookLogReason::Error, "GameState_NSFW", "nsfw == nullptr");
			return;
		}
		
		originalValue = *nsfw;
		toggle = *nsfw;
		
		std::string state = (*nsfw) ? "True" : "False";
		LogHook(HookLogReason::Create, "GameState_NSFW", "GameState.NSFW = " + state + " Original: " + std::to_string(originalValue) + " Toggle: " + std::to_string(toggle));
	}

	void Toggle()
	{
		if (nsfw == nullptr)
		{
			LogHook(HookLogReason::Error, "GameState_NSFW", "nsfw == nullptr");
			return;
		}
			

		if (toggle && !*nsfw)
		{
			*nsfw = true;
			LogHook(HookLogReason::Enable, "GameState_NSFW");
		}

		if (!toggle && *nsfw)
		{
			*nsfw = false;
			LogHook(HookLogReason::Disable, "GameState_NSFW");
		}
	}

	void Destroy()
	{
		*nsfw = originalValue;
		LogHook(HookLogReason::Destroy, "GameState_NSFW");
	}
};