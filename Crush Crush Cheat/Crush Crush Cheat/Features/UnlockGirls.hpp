#pragma once
#include "../Includes.hpp"

// Girls
// Token: 0x06000395 RID: 917 RVA: 0x0001CE84 File Offset: 0x0001B084
// private void UnlockGirl(int girl)
// Girls::UnlockGirl(int girl)

// Girls
// Token: 0x0600038A RID: 906 RVA: 0x0001B8D4 File Offset: 0x00019AD4
// private void Update()
// Girls::Update()

inline void* pGirlsClassInstance = nullptr;

class UnlockGirls
{
private:
	bool hookEnabled = false;

	void* Girls_Update = nullptr;

public:
	UnlockGirls() {};

	void Render()
	{
		if (ImGui::Button("Unlock All Girls"))
			Unlock();
	}

	void Create()
	{
		Girls_Update = Mono::instance().GetCompiledMethod("Girls", "Update", 0);
		if (Girls_Update == nullptr)
			return;

		std::cout << "[OmegaWare.xyz]::[Hooks]::Girls_Update Created & Enabled" << std::endl;
		CreateHook(Girls_Update);
		EnableHook(Girls_Update);
	}

	void Destroy()
	{
		std::cout << "[OmegaWare.xyz]::[Hooks]::Girls_Update Destroyed" << std::endl;
		DisableHook(Girls_Update);
	}

	void Unlock()
	{
		if (pGirlsClassInstance == nullptr)
			return;

		MonoMethod* UnlockGirl = Mono::instance().GetMethod("Girls", "UnlockGirl", 1);
		if (UnlockGirl == nullptr)
			return;

		for (unsigned int i = 0; i < Girls.size(); i++)
		{
			void* args[1] = { &Girls[i].id };
			MonoObject* result = Mono::instance().Invoke(UnlockGirl, pGirlsClassInstance, args);
			std::cout << "[OmegaWare.xyz]::Invoke(UnlockGirl)(Itter = " << i << ") = " << result << std::endl;
		}
	}

	HOOK_DEF(void, Girls_Update, (void* __this))
	{
		pGirlsClassInstance = __this;
		return oGirls_Update(__this);
	}
};
