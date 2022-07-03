#pragma once
#include "../Includes.hpp"

// Cellphone
// Token: 0x0600063B RID: 1595 RVA: 0x00033F78 File Offset: 0x00032178
// public static void TimeSkip(int seconds)
// Cellphone::TimeSkip(int seconds)

inline void* pCellphoneClassInstance = nullptr;

class PhoneSkip
{
private:
	void* Cellphone_Update = nullptr;
	void* Cellphone_IsUnlocked = nullptr;

public:
	PhoneSkip() {};

	void Render()
	{\
		if (ImGui::Button("Phone Skip"))
			Skip();
	}
	
	void Create()
	{
		Cellphone_Update = Mono::instance().GetCompiledMethod("Cellphone", "Update", 0);
		if (Cellphone_Update == nullptr)
			return;

		CreateHook(Cellphone_Update);
		EnableHook(Cellphone_Update);

		Cellphone_IsUnlocked = Mono::instance().GetCompiledMethod("Cellphone", "IsUnlocked", 1);
		if (Cellphone_IsUnlocked == nullptr)
			return;
		
		CreateHook(Cellphone_IsUnlocked);
		EnableHook(Cellphone_IsUnlocked);
	}

	void Destroy()
	{
		DisableHook(Cellphone_Update);
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