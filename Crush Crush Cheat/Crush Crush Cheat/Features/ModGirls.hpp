#pragma once
#include "../Includes.hpp"

// Girl
// Token: 0x06000357 RID: 855 RVA: 0x00018B6C File Offset: 0x00016D6C
// public void SetLove(Girl.LoveLevel value)
// Girl::SetLove(Girl.LoveLevel value)

// Girl
// Token: 0x0600035F RID: 863 RVA: 0x00018F2C File Offset: 0x0001712C
// private void Update()
// Girl::Update()

class ModGirls
{
private:
	bool hookEnabled = false;
	bool toggle = false;

	void* Girl_Update = nullptr;

public:
	ModGirls() {};

	void Render()
	{
		ImGui::Checkbox("All Girls lover", &toggle);

		Toggle();
	}

	void Create()
	{
		Girl_Update = Mono::instance().GetCompiledMethod("Girl", "Update", 0);
		if (Girl_Update == nullptr)
			return;

		LogHook(HookLogReason::Create, "Girl_Update");
		CreateHook(Girl_Update);
	}

	void Toggle()
	{
		if (toggle && !hookEnabled)
		{
			hookEnabled = true;
			LogHook(HookLogReason::Enable, "Girl_Update");
			EnableHook(Girl_Update);
		}

		if (!toggle && hookEnabled)
		{
			hookEnabled = false;
			LogHook(HookLogReason::Disable, "Girl_Update");
			DisableHook(Girl_Update);
		}
	}

	void Destroy()
	{
		LogHook(HookLogReason::Destroy, "Girl_Update");
		DisableHook(Girl_Update);
	}

	HOOK_DEF(void, Girl_Update, (void* __this))
	{
		if (__this == nullptr)
			return oGirl_Update(__this);

		MonoMethod* Girl_SetLove = Mono::instance().GetMethod("Girl", "SetLove", 1);
		if (Girl_SetLove == nullptr)
			return oGirl_Update(__this);

		int loveLevel = LoveLevels::LoveLevel::Lover;
		void* args[1] = { &loveLevel };
		MonoObject* result = Mono::instance().Invoke(Girl_SetLove, __this, args);
		
		if (bExtraDebug)
			LogInvoke("Girl_SetLove", "Result = " + (std::stringstream() << result).str());
		
		return oGirl_Update(__this);
	}
};