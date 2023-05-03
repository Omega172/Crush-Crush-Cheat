#pragma once
#include "../Includes.hpp"

// Playfab
// Token: 0x060006B3 RID: 1715 RVA: 0x00035C0C File Offset: 0x00033E0C
// public static bool HasItem(Playfab.PlayfabItem item)
// Playfab::HasItem(Playfab.PlayfabItem item)

// Playfab
// Token: 0x060006A9 RID: 1705 RVA: 0x000359B4 File Offset: 0x00033BB4
// public static bool ParticipatedIn(Playfab.EventParticipation e)
// Playfab::ParticipatedIn

class Unlocker
{
private:
	bool hookEnabled = false;
	bool toggle = false;

	void* Playfab_HasItem = nullptr;
	void* Playfab_ParticipatedIn = nullptr;

public:

	void Render()
	{

	}

	void Create()
	{
		Playfab_HasItem = Mono::Instance().GetCompiledMethod("Playfab", "HasItem", 1);
		if (Playfab_HasItem == nullptr)
			return;

		LogHook(HookLogReason::Create, "Playfab_HasItem");
		CreateHook(Playfab_HasItem);

		LogHook(HookLogReason::Enable, "Playfab_HasItem");
		EnableHook(Playfab_HasItem);

		Playfab_ParticipatedIn = Mono::Instance().GetCompiledMethod("Playfab", "ParticipatedIn", 1);
		if (Playfab_ParticipatedIn == nullptr)
			return;

		LogHook(HookLogReason::Create, "Playfab_ParticipatedIn");
		CreateHook(Playfab_ParticipatedIn);

		LogHook(HookLogReason::Enable, "Playfab_ParticipatedIn");
		EnableHook(Playfab_ParticipatedIn);
	}

	void Toggle()
	{
		if (toggle && !hookEnabled)
		{
			hookEnabled = true;

			LogHook(HookLogReason::Enable, "Playfab_HasItem");
			EnableHook(Playfab_HasItem);
		}

		if (!toggle && hookEnabled)
		{
			hookEnabled = false;

			LogHook(HookLogReason::Disable, "Playfab_HasItem");
			DisableHook(Playfab_HasItem);
		}
	}

	void Destory()
	{
		LogHook(HookLogReason::Destroy, "Playfab_HasItem");
		DisableHook(Playfab_HasItem);
	}

	HOOK_DEF(bool, Playfab_HasItem, (void* __this, PlayfabEnums::PlayfabItem item))
	{
		if (bExtraDebug)
			LogHook(HookLogReason::Called, "Playfab_HasItem", "item = " + std::to_string(item));

		return true;
	}

	HOOK_DEF(bool, Playfab_ParticipatedIn, (void* __this, PlayfabEnums::EventParticipation e))
	{
		if (bExtraDebug)
			LogHook(HookLogReason::Called, "Playfab_ParticipatedIn", "e = " + std::to_string(e));
		
		return true;
	}
};