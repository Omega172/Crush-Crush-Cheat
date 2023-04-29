#pragma once
#include "../Includes.hpp"

// Job2
// Token: 0x0600053D RID: 1341 RVA: 0x00029E4C File Offset: 0x0002804C
// private void Unlock()
// Job2::Unlock()

// Job2
// Token: 0x06000541 RID: 1345 RVA: 0x0002A1C4 File Offset: 0x000283C4
// private void Update()
// Job2::Update()

class JobUnlock
{
private:
	bool hookEnabled = false;
	bool toggle = false;

	void* Job_Update = nullptr;

public:
	JobUnlock() {};

	void Render()
	{
		ImGui::Checkbox("Unlock All Jobs", &toggle);

		Toggle();
	}

	void Create()
	{
		Job_Update = Mono::Instance().GetCompiledMethod("Job2", "Update", 0);
		if (Job_Update == nullptr)
			return;

		LogHook(HookLogReason::Create, "Job_Update");
		CreateHook(Job_Update);
	}

	void Toggle()
	{
		if (toggle && !hookEnabled)
		{
			hookEnabled = true;
			LogHook(HookLogReason::Enable, "Job_Update");
			EnableHook(Job_Update);
		}

		if (!toggle && hookEnabled)
		{
			hookEnabled = false;
			LogHook(HookLogReason::Disable, "Job_Update");
			DisableHook(Job_Update);
		}
	}

	void Destroy()
	{
		LogHook(HookLogReason::Destroy, "Job_Update");
		DisableHook(Job_Update);
	}

	HOOK_DEF(void, Job_Update, (void* __this))
	{
		if (__this == nullptr)
			return oJob_Update(__this);

		MonoMethod* Job_Unlock = Mono::Instance().GetMethod("Job2", "Unlock", 0);
		if (Job_Unlock == nullptr)
			return oJob_Update(__this);

		MonoObject* result = Mono::Instance().Invoke(Job_Unlock, __this, nullptr);

		/*bool bGilded = true;	
		MonoClassField* pField = Mono::instance().GetField("Job2", "Gilded");
		if (pField == nullptr)
			return oJob_Update(__this);
		
		Mono::instance().SetFieldValue((MonoObject*)__this, pField, &bGilded);

		bool bIsActive = true;
		pField = Mono::instance().GetField("Job2", "IsActive");
		if (pField == nullptr)
			return oJob_Update(__this);
		
		Mono::instance().SetFieldValue((MonoObject*)__this, pField, &bIsActive);
		
		bool bLocked = false;
		pField = Mono::instance().GetField("Job2", "locked");
		if (pField == nullptr)
			return oJob_Update(__this);
		
		Mono::instance().SetFieldValue((MonoObject*)__this, pField, &bLocked);*/
		
		if (bExtraDebug)
			LogInvoke("Job_Unlock", "Result = " + (std::stringstream() << result).str());

		return oJob_Update(__this);
	}
};