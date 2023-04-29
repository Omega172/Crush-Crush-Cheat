#pragma once
#include "../includes.hpp"

// Hobby2
// Token: 0x0600046E RID: 1134 RVA: 0x00026538 File Offset: 0x00024738
// private void Update()
// Hobby2::Update()

// Hobby2
// Token: 0x0600046D RID: 1133 RVA: 0x000263B8 File Offset: 0x000245B8
// public void Unlock(bool spawnParticles = true)
// Hobby2::Unlock()

class HobbiesUnlock
{
private:
	bool hookEnabled = false;
	bool toggle = false;

	void* Hobby_Update = nullptr;

public:
	HobbiesUnlock() {};

	void Render()
	{
		ImGui::Checkbox("Unlock All Hobbies", &toggle);

		Toggle();
	}

	void Create()
	{
		Hobby_Update = Mono::instance().GetCompiledMethod("Hobby2", "Update", 0);
		if (Hobby_Update == nullptr)
			return;

		LogHook(HookLogReason::Create, "Hobby_Update");
		CreateHook(Hobby_Update);
	}
	
	void Toggle()
	{
		if (toggle && !hookEnabled)
		{
			hookEnabled = true;
			LogHook(HookLogReason::Enable, "Hobby_Update");
			EnableHook(Hobby_Update);
		}

		if (!toggle && hookEnabled)
		{
			hookEnabled = false;
			LogHook(HookLogReason::Disable, "Hobby_Update");
			DisableHook(Hobby_Update);
		}
	}

	void Destroy()
	{
		LogHook(HookLogReason::Destroy, "Hobby_Update");
		DisableHook(Hobby_Update);
	}

	HOOK_DEF(void, Hobby_Update, (void* __this))
	{
		if (__this == nullptr)
			return oHobby_Update(__this);

		MonoMethod* Hobby_Unlock = Mono::instance().GetMethod("Hobby2", "Unlock", 1);
		if (Hobby_Unlock == nullptr)
			return oHobby_Update(__this);

		bool value = true;

		void* args[1] = { &value };
		MonoObject* result = Mono::instance().Invoke(Hobby_Unlock, __this, args);

		if (bExtraDebug)
			LogInvoke("Hobby_Unlock", "Result = " + (std::stringstream() << result).str());

		return oHobby_Update(__this);
	}
};