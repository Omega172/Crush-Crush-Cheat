#pragma once
#include "../../includes.hpp"

// Utilities
// Token: 0x060009D0 RID: 2512 RVA: 0x00051AE4 File Offset: 0x0004FCE4
// public static bool AwardDiamonds(int amount)
// Utilities::AwardDiamonds(int amount)

// Utilities
// Token: 0x060009D2 RID: 2514 RVA: 0x00051BC8 File Offset: 0x0004FDC8
// public static void PurchaseDiamonds(int count)
// Utilities::PurchaseDiamonds(int count)

class InfiniteDiamonds
{
private:
	bool hookEnabled = false;

	void* Utilities_AwardDiamonds = nullptr;

public:
	InfiniteDiamonds() {};

	void Create()
	{
		Utilities_AwardDiamonds = Mono::instance().GetMethod("Utilities", "AwardDiamonds", 1);
		if (Utilities_AwardDiamonds == nullptr)
			return;

		CreateHook(Utilities_AwardDiamonds);
	}

	void Toggle()
	{
		if (Utilities_AwardDiamonds == nullptr)
			return;

		if (!hookEnabled)
		{
			hookEnabled = true;
			std::cout << "Utilities_AwardDiamonds Hook Enabled!\n";
			EnableHook(Utilities_AwardDiamonds);
			return;
		}

		if (hookEnabled)
		{
			hookEnabled = false;
			std::cout << "Utilities_AwardDiamonds Hook Disabled!\n";
			DisableHook(Utilities_AwardDiamonds);
			return;
		}
	}

	void Destroy()
	{
		DisableHook(Utilities_AwardDiamonds);
	}

	HOOK_DEF(bool, Utilities_AwardDiamonds, (void* __this, INT amount))
	{
		amount = 1000000;

		return oUtilities_AwardDiamonds(__this, amount);
	}
};