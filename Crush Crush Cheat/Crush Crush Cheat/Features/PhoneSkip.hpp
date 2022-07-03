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
	
	void* Cellphone_Update = nullptr;
	void* Cellphone_IsUnlocked = nullptr;

public:
	PhoneSkip() {};

	void Render()
	{
		if (ImGui::Button("Phone Skip"))
			Skip();
		
		ImGui::Checkbox("Unlock Phone Convos", &toggle);

		Toggle();
	}
	
	void Create()
	{
		Cellphone_Update = Mono::instance().GetCompiledMethod("Cellphone", "Update", 0);
		if (Cellphone_Update == nullptr)
			return;

		std::cout << "[OmegaWare.xyz]::[Hooks]::Cellphone_Update Created & Enabled" << std::endl;
		CreateHook(Cellphone_Update);
		EnableHook(Cellphone_Update);

		Cellphone_IsUnlocked = Mono::instance().GetCompiledMethod("Cellphone", "IsUnlocked", 1);
		if (Cellphone_IsUnlocked == nullptr)
			return;
		
		std::cout << "[OmegaWare.xyz]::[Hooks]::Cellphone_IsUnlocked Created" << std::endl;
		CreateHook(Cellphone_IsUnlocked);
	}

	void Toggle()
	{
		if (toggle && !hookEnabled)
		{
			hookEnabled = true;
			std::cout << "[OmegaWare.xyz]::[Hooks]::Cellphone_IsUnlocked Enabled" << std::endl;
			EnableHook(Cellphone_IsUnlocked);
		}

		if (!toggle && hookEnabled)
		{
			hookEnabled = false;
			std::cout << "[OmegaWare.xyz]::[Hooks]::Cellphone_IsUnlocked Disabled" << std::endl;
			DisableHook(Cellphone_IsUnlocked);
		}
	}

	void Destroy()
	{
		std::cout << "[OmegaWare.xyz]::[Hooks]::Cellphone_Update Destroyed" << std::endl;
		std::cout << "[OmegaWare.xyz]::[Hooks]::Cellphone_IsUnloaked Destroyed" << std::endl;
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
		std::cout << "[OmegaWare.xyz]::Invoke(Debug_SkipMessage) = " << result << std::endl;
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