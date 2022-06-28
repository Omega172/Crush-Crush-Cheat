#pragma once
#include "../../includes.hpp"

// Girls
// Token: 0x06000395 RID: 917 RVA: 0x0001CE84 File Offset: 0x0001B084
// private void UnlockGirl(int girl)
// Girls::UnlockGirl(int girl)

class UnlockGirls
{
private:
	bool hookEnabled = false;

	void* Girls_UnlockGirl = nullptr;

public:
	UnlockGirls() {};

	void Create()
	{
		Girls_UnlockGirl = Mono::instance().GetMethod("Girls", "UnlockGirl", 1);
		if (Girls_UnlockGirl == nullptr)
			return;

		CreateHook(Girls_UnlockGirl);
	}

	void Toggle()
	{
		if (Girls_UnlockGirl == nullptr)
			return;

		if (!hookEnabled)
		{
			hookEnabled = true;
			std::cout << "Girls_UnlockGirl Hook Enabled!\n";
			EnableHook(Girls_UnlockGirl);
			return;
		}

		if (hookEnabled)
		{
			hookEnabled = false;
			std::cout << "Girls_UnlockGirl Hook Disabled!\n";
			DisableHook(Girls_UnlockGirl);
			return;
		}
	}

	void Destroy()
	{
		DisableHook(Girls_UnlockGirl);
	}

	HOOK_DEF(void, Girls_UnlockGirl, (void* __this, INT girl))
	{
		for (INT i = 0; i < 46 + 1; i++)
			oGirls_UnlockGirl(__this, i);

		return;
	}
};
