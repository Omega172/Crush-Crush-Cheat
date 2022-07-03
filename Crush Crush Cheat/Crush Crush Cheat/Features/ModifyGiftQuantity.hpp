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
		ImGui::BeginChild("##ModifyGiftQuantity", ImVec2(ImGui::GetContentRegionAvail().x, ImGui::GetContentRegionAvail().y), true);
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

		CreateHook(Gift_OnGift);
	}

	void Toggle()
	{
		if (Gift_OnGift == nullptr)
			return;

		if (toggle && !hookEnabled)
		{
			hookEnabled = true;
			//std::cout << "Gift_OnGift Hook Enabled!\n";
			EnableHook(Gift_OnGift);
			return;
		}

		if (!toggle && hookEnabled)
		{
			hookEnabled = false;
			//std::cout << "Gift_OnGift Hook Disabled!\n";
			DisableHook(Gift_OnGift);
			return;
		}
	}

	void Destroy()
	{
		DisableHook(Gift_OnGift);
	}

	int GetModQuantity()
	{
		return modQuantity;
	}

	HOOK_DEF(void, Gift_OnGift, (void* __this, INT quantity))
	{
		//std::cout << "Gift_OnGift Called: " << quantity << std::endl;

		quantity = modQuantity;

		return oGift_OnGift(__this, quantity);
	}
};