#pragma once
#include "../Includes.hpp"

// Gift
// Token: 0x0600030E RID: 782 RVA: 0x00016BD8 File Offset: 0x00014DD8
// public void OnGift(int quantity)
// Gift::OnGift(int quantity)

inline int modQuantity = 1000;

class ModifyGiftQuantity
{
private:
	bool hookEnabled = false;
	bool toggle = false;
	
	void* Gift_OnGift = nullptr;

public:
	ModifyGiftQuantity() {};

	void Render()
	{
		ImGui::BeginChild("##ModifyGiftQuantity", ImVec2(ImGui::GetContentRegionAvail().x / 2, ImGui::GetContentRegionAvail().y), true);
		{
			ImGui::Text("Gifts");
			ImGui::InputInt("##Gift Quantity", &modQuantity);
			ImGui::Checkbox("Override Gift Quantity", &toggle);
		}
		ImGui::EndChild();

		Toggle();
	}

	void Create()
	{
		Gift_OnGift = Mono::instance().GetCompiledMethod("Gift", "OnGift", 1);
		if (Gift_OnGift == nullptr)
			return;

		LogHook(HookLogReason::Create, "Gift_OnGift");
		CreateHook(Gift_OnGift);
	}

	void Toggle()
	{
		if (Gift_OnGift == nullptr)
			return;

		if (toggle && !hookEnabled)
		{
			hookEnabled = true;
			LogHook(HookLogReason::Enable, "Gift_OnGift");
			EnableHook(Gift_OnGift);
			return;
		}

		if (!toggle && hookEnabled)
		{
			hookEnabled = false;
			LogHook(HookLogReason::Disable, "Gift_OnGift");
			DisableHook(Gift_OnGift);
			return;
		}
	}

	void Destroy()
	{
		LogHook(HookLogReason::Destroy, "Gift_OnGift");
		DisableHook(Gift_OnGift);
	}

	HOOK_DEF(void, Gift_OnGift, (void* __this, INT quantity))
	{
		if (bExtraDebug)
			LogHook(HookLogReason::Called, "Gift_OnGift", "quantity = " + std::to_string(quantity) + " overrideQuantity = " + std::to_string(modQuantity));

		quantity = modQuantity;

		return oGift_OnGift(__this, quantity);
	}
};