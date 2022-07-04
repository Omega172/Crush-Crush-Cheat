#pragma once
#include "../Includes.hpp"

// Cellphone
// Token: 0x0600063B RID: 1595 RVA: 0x00033F78 File Offset: 0x00032178
// public static void TimeSkip(int seconds)
// Cellphone::TimeSkip(int seconds)

// Cellphone
// Token: 0x0600064F RID: 1615 RVA: 0x00034F80 File Offset: 0x00033180
// private void Update()
// Cellphone::Update()

inline void* pCellphoneClassInstance = nullptr;

class PhoneSkip
{
private:
	bool hookEnabled = false;
	bool toggle = false;
	int phoneSkipKey = VK_XBUTTON1;

	void* Cellphone_Update = nullptr;
	void* Cellphone_IsUnlocked = nullptr;

public:
	PhoneSkip() {};

	void Render()
	{
		if (ImGui::Button("Phone Skip Wait"))
			Skip();

		ImGui::SameLine();

		ImGui::Checkbox("Unlock Phone Convos", &toggle);

		Toggle();
	}

	void Update()
	{
		if (GetAsyncKeyState(phoneSkipKey))
			Skip();
	}
	
	void Create()
	{
		Cellphone_Update = Mono::instance().GetCompiledMethod("Cellphone", "Update", 0);
		if (Cellphone_Update == nullptr)
			return;

		LogHook(HookLogReason::Create, "Cellphone_Update");
		LogHook(HookLogReason::Enable, "Cellphone_Update");
		CreateHook(Cellphone_Update);
		EnableHook(Cellphone_Update);

		Cellphone_IsUnlocked = Mono::instance().GetCompiledMethod("Cellphone", "IsUnlocked", 1);
		if (Cellphone_IsUnlocked == nullptr)
			return;
		
		LogHook(HookLogReason::Create, "Cellphone_IsUnlocked");
		CreateHook(Cellphone_IsUnlocked);
	}

	void Toggle()
	{
		if (toggle && !hookEnabled)
		{
			hookEnabled = true;
			LogHook(HookLogReason::Enable, "Cellphone_IsUnlocked");
			EnableHook(Cellphone_IsUnlocked);
		}

		if (!toggle && hookEnabled)
		{
			hookEnabled = false;
			LogHook(HookLogReason::Disable, "Cellphone_IsUnlocked");
			DisableHook(Cellphone_IsUnlocked);
		}
	}

	void Destroy()
	{
		LogHook(HookLogReason::Destroy, "Cellphone_Update");
		DisableHook(Cellphone_Update);
		
		LogHook(HookLogReason::Destroy, "Cellphone_IsUnlocked");
		DisableHook(Cellphone_IsUnlocked);
	}

	void Skip()
	{
		if (pCellphoneClassInstance == nullptr)
			return;
		
		MonoMethod* TimeSkip = Mono::instance().GetMethod("Cellphone", "Debug_SkipMessage", 0);
		if (TimeSkip == nullptr)
			return;

		MonoObject* result = Mono::instance().Invoke(TimeSkip, pCellphoneClassInstance, nullptr);
		if (bExtraDebug)
			LogInvoke("Debug_SkipMessage", "Result = " + (std::stringstream()<<result).str());
	}

	HOOK_DEF(void, Cellphone_Update, (void* __this))
	{
		pCellphoneClassInstance = __this;
		return oCellphone_Update(__this);
	}
	
	HOOK_DEF(bool, Cellphone_IsUnlocked, (void* __this, short id))
	{
		return true;
	}
};