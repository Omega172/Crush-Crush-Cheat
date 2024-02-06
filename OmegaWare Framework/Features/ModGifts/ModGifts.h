#pragma once
#include "pch.h"

inline int iModQuantity = 1000;

class ModGifts : public Feature
{
private:
	bool Initalized = false;

	bool bEnabled = false;;
	bool bHookSet = false;

	void* Gift_OnGift = nullptr;

public:
	ModGifts() {};

	virtual bool Setup()
	{
		Gift_OnGift = Mono::Instance().GetCompiledMethod("Gift", "OnGift", 1);
		if (Gift_OnGift == nullptr)
			return false;

		Utils::LogHook(Utils::GetLocation(CurrentLoc), "Gift_OnGift", "Create", "");

		CreateHook(Gift_OnGift);

		Initalized = true;
		return true;
	}

	virtual void Destroy()
	{
		if (!Initalized)
			return;

		Utils::LogHook(Utils::GetLocation(CurrentLoc), "Gift_OnGift", "Destroy", "");

		DisableHook(Gift_OnGift);
	}

	virtual void HandleKeys() {}

	virtual void DrawMenuItems()
	{
		if (!Initalized)
			return;

		ImGui::BeginChild("##ModifyGiftQuantity", ImVec2(ImGui::GetContentRegionAvail().x / 2, ImGui::GetContentRegionAvail().y), true);
		{
			ImGui::Text("Gifts");
			ImGui::InputInt("##Gift Quantity", &iModQuantity);
			ImGui::Checkbox("Override Gift Quantity", &bEnabled);
		}
		ImGui::EndChild();
	}

	virtual void Render() {}

	virtual void Run()
	{
		if (!Initalized)
			return;

		if (bEnabled && !bHookSet)
		{
			bHookSet = true;
			Utils::LogHook(Utils::GetLocation(CurrentLoc), "Gift_OnGift", "Enable", "");

			EnableHook(Gift_OnGift);
			return;
		}

		if (!bEnabled && bHookSet)
		{
			bHookSet = false;
			Utils::LogHook(Utils::GetLocation(CurrentLoc), "Gift_OnGift", "Disable", "");

			DisableHook(Gift_OnGift);
			return;
		}
	}

	HOOK_DEF(void, Gift_OnGift, (void* __this, INT iQuantity))
	{
		Utils::LogHook(Utils::GetLocation(CurrentLoc), "Gift_OnGift", "Called", "quantity = " + std::to_string(iQuantity) + " overrideQuantity = " + std::to_string(iModQuantity));

		iQuantity = iModQuantity;

		return oGift_OnGift(__this, iQuantity);
	}
};