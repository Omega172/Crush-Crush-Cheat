#pragma once
#include "../../includes.hpp"

// Gift
// Token: 0x0600030E RID: 782 RVA: 0x00016BD8 File Offset: 0x00014DD8
// public void OnGift(int quantity)
// Gift::OnGift(int quantity)

class ModifyGiftQuantity
{
private:
	bool hookEnabled = false;

	void* Gift_OnGift = nullptr;

public:
	ModifyGiftQuantity() {};

	void Create()
	{
		Gift_OnGift = Mono::instance().GetMethod("Gift", "OnGift", 1);
		if (Gift_OnGift == nullptr)
			return;

		CreateHook(Gift_OnGift);
	}

	void Toggle()
	{
		if (Gift_OnGift == nullptr)
			return;

		if (!hookEnabled)
		{
			hookEnabled = true;
			std::cout << "Gift_OnGift Hook Enabled!\n";
			EnableHook(Gift_OnGift);
			return;
		}

		if (hookEnabled)
		{
			hookEnabled = false;
			std::cout << "Gift_OnGift Hook Disabled!\n";
			DisableHook(Gift_OnGift);
			return;
		}
	}

	void Destroy()
	{
		DisableHook(Gift_OnGift);
	}

	HOOK_DEF(void, Gift_OnGift, (void* __this, INT quantity))
	{
		std::cout << "Gift_OnGift Called: " << quantity << std::endl;

		return oGift_OnGift(__this, quantity);
	}
};