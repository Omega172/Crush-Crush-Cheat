#pragma once
#include "pch.h"

inline void* pGirlsClassInstance = nullptr;
inline void* pCellphoneClassInstance = nullptr;
inline void* pAlbumClassInstance = nullptr;

class Misc : public Feature
{
private:
	bool Initalized = false;
	bool* bGameIsInitalized = nullptr;

	void* Girls_Update = nullptr;
	void* Cellphone_Update = nullptr;
	void* Cellphone_IsUnlocked = nullptr;
	void* Album_IsPinupUnlocked = nullptr;
	void* Album_Update = nullptr;

	KeyBindToggle SkipPhoneTimerKey = KeyBindToggle(ImGuiKey_MouseX1);
	bool bSkipKeySetting = false;

	bool bUnlockPhoneConversations = false;
	bool bUnlockPhoneConversationsHookSet = false;

	bool bUnlockPinupsToggle = false;
	bool bUnlockPinupsHookSet = false;

	bool obNSFW = false;
	bool bNSFWToggle = false;
	bool* bNSFW = nullptr;

	void UnlockGirls()
	{
		if (pGirlsClassInstance == nullptr)
			return;

		MonoMethod* UnlockGirl = Mono::Instance().GetMethod("Girls", "UnlockGirl", 1);
		if (UnlockGirl == nullptr)
			return;

		for (unsigned int i = 0; i < Girls.size(); i++)
		{
			void* args[1] = { &Girls[i].id };
			MonoObject* pResult = Mono::Instance().Invoke(UnlockGirl, pGirlsClassInstance, args);

			std::stringstream SS("UnlockGirl = ");
			SS << std::hex << pResult << std::dec << " | Unlocked Girl: " << Girls[i].name;

			Utils::LogDebug(Utils::GetLocation(CurrentLoc), SS.str());
		}
	}

	void SetLover()
	{
		uintptr_t* pCurrentGirl = reinterpret_cast<uintptr_t*>(Mono::Instance().GetStaticFieldValue("Girls", "CurrentGirl"));
		if (pCurrentGirl == nullptr)
		{
			Utils::LogError(Utils::GetLocation(CurrentLoc), "pCurrentGirl = nullptr");
			return;
		}

		MonoMethod* Girl_SetLove = Mono::Instance().GetMethod("Girl", "SetLove", 1);
		if (Girl_SetLove == nullptr)
			return;

		int loveLevel = LoveLevel::Lover;
		void* args[1] = { &loveLevel };
		MonoObject* pResult = Mono::Instance().Invoke(Girl_SetLove, (uintptr_t*)*pCurrentGirl, args);

		std::stringstream SS("SetLove = ");
		SS << std::hex << pResult;
	}

	void SkipPhoneTimer()
	{
		if (pCellphoneClassInstance == nullptr)
			return;

		MonoMethod* TimeSkip = Mono::Instance().GetMethod("Cellphone", "Debug_SkipMessage", 0);
		if (TimeSkip == nullptr)
			return;

		MonoObject* pResult = Mono::Instance().Invoke(TimeSkip, pCellphoneClassInstance, nullptr);

		std::stringstream SS("Debug_SkipMessage = ");
		SS << std::hex << pResult;

		Utils::LogDebug(Utils::GetLocation(CurrentLoc), SS.str());
	}

	void UnlockDatePhotos()
	{
		if (pAlbumClassInstance == nullptr)
			return;

		MonoMethod* Album_AddDate = Mono::Instance().GetMethod("Album", "AddDate", 2);
		if (Album_AddDate == nullptr)
			return;

		int dateType = NULL;
		MonoObject* pResult;
		for (unsigned int i = 0; i < Girls.size(); i++)
		{
			dateType = DateType::Beach;
			void* args[2] = { &dateType, &Girls[i].id };
			pResult = Mono::Instance().Invoke(Album_AddDate, pAlbumClassInstance, args);

			std::stringstream SS("Album_AddDate - Itter = " + std::to_string(i) + " Result = ");
			SS << std::hex << pResult;
			Utils::LogDebug(Utils::GetLocation(CurrentLoc), SS.str());

			dateType = DateType::MoonlightStroll;
			args[0] = &dateType;
			args[1] = &Girls[i].id;
			pResult = Mono::Instance().Invoke(Album_AddDate, pAlbumClassInstance, args);

			std::stringstream SS1("Album_AddDate - Itter = " + std::to_string(i) + " Result = ");
			SS1 << std::hex << pResult;
			Utils::LogDebug(Utils::GetLocation(CurrentLoc), SS1.str());

			dateType = DateType::MovieTheater;
			args[0] = &dateType;
			args[1] = &Girls[i].id;
			pResult = Mono::Instance().Invoke(Album_AddDate, pAlbumClassInstance, args);

			std::stringstream SS2("Album_AddDate - Itter = " + std::to_string(i) + " Result = ");
			SS2 << std::hex << pResult;
			Utils::LogDebug(Utils::GetLocation(CurrentLoc), SS2.str());

			dateType = DateType::Sightseeing;
			args[0] = &dateType;
			args[1] = &Girls[i].id;
			pResult = Mono::Instance().Invoke(Album_AddDate, pAlbumClassInstance, args);

			std::stringstream SS3("Album_AddDate - Itter = " + std::to_string(i) + " Result = ");
			SS3 << std::hex << pResult;
			Utils::LogDebug(Utils::GetLocation(CurrentLoc), SS3.str());
		}
	}

public:
	Misc() {};

	virtual bool Setup()
	{
		bNSFW = reinterpret_cast<bool*>(Mono::Instance().GetStaticFieldValue("GameState", "NSFW"));
		obNSFW = *bNSFW;
		bNSFWToggle = *bNSFW;

		std::stringstream SS("Value on initialization: ");
		SS << (obNSFW ? "True" : "False");
		Utils::LogDebug(Utils::GetLocation(CurrentLoc), SS.str());

		Girls_Update = Mono::Instance().GetCompiledMethod("Girls", "Update", 0);
		if (Girls_Update == nullptr)
			return false;

		Utils::LogHook(Utils::GetLocation(CurrentLoc), "Girls_Update", "Create", "Hook Created");
		CreateHook(Girls_Update);
		
		Utils::LogHook(Utils::GetLocation(CurrentLoc), "Girls_Update", "Enabled", "Hook Enabled");
		EnableHook(Girls_Update);

		Cellphone_Update = Mono::Instance().GetCompiledMethod("Cellphone", "Update", 0);
		if (Cellphone_Update == nullptr)
			return false;

		Utils::LogHook(Utils::GetLocation(CurrentLoc), "Cellphone_Update", "Create", "Hook Created");
		CreateHook(Cellphone_Update);

		Utils::LogHook(Utils::GetLocation(CurrentLoc), "Cellphone_Update", "Enabled", "Hook Enabled");
		EnableHook(Cellphone_Update);

		Cellphone_IsUnlocked = Mono::Instance().GetCompiledMethod("Cellphone", "IsUnlocked", 1);
		if (Cellphone_IsUnlocked == nullptr)
			return false;

		Utils::LogHook(Utils::GetLocation(CurrentLoc), "Cellphone_IsUnlocked", "Create", "Hook Created");
		CreateHook(Cellphone_IsUnlocked);

		Album_Update = Mono::Instance().GetCompiledMethod("Album", "Update", 0);
		if (Album_Update == nullptr)
			return false;

		Utils::LogHook(Utils::GetLocation(CurrentLoc), "Album_Update", "Create", "Hook Created");
		CreateHook(Album_Update);

		Utils::LogHook(Utils::GetLocation(CurrentLoc), "Album_Update", "Enabled", "Hook Enabled");
		EnableHook(Album_Update);

		Album_IsPinupUnlocked = Mono::Instance().GetCompiledMethod("Album", "IsPinupUnlocked", 1);
		if (Album_IsPinupUnlocked == nullptr)
			return false;

		Utils::LogHook(Utils::GetLocation(CurrentLoc), "Album_IsPinupUnlocked", "Create", "Hook Created");
		CreateHook(Album_IsPinupUnlocked);

		Initalized = true;
		return true;
	}

	virtual void Destroy()
	{
		if (!Initalized)
			return;

		Utils::LogHook(Utils::GetLocation(CurrentLoc), "Girls_Update", "Destroy", "Hook Destroyed");
		DisableHook(Girls_Update);

		Utils::LogHook(Utils::GetLocation(CurrentLoc), "Cellphone_Update", "Destroy", "Hook Destroyed");
		DisableHook(Cellphone_Update);

		Utils::LogHook(Utils::GetLocation(CurrentLoc), "Cellphone_IsUnlocked", "Destroy", "Hook Destroyed");
		DisableHook(Cellphone_IsUnlocked);

		*bNSFW = obNSFW;
	}

	virtual void HandleKeys() {}

	virtual void DrawMenuItems() 
	{
		if (!Initalized)
			return;
		
		ImGui::SameLine();

		ImGui::BeginChild("##Misc", ImVec2(ImGui::GetContentRegionAvail().x, ImGui::GetContentRegionAvail().y), ImGuiChildFlags_Border);
		{
			ImGui::Text("Misc");

			if (ImGui::Button("Unlock all Girls"))
				UnlockGirls();

			if (ImGui::Button("Set Current Girl Lover"))
				SetLover();

			ImGui::Text("Skip Phone Timer");
			ImGui::SameLine();
			ImGui::Hotkey("#Skip Phone Timer", SkipPhoneTimerKey, &bSkipKeySetting);

			ImGui::Checkbox("Force Unlock Conversations", &bUnlockPhoneConversations);

			if(ImGui::Button("Unlock Date Pics"))
				UnlockDatePhotos();

			ImGui::SameLine();

			ImGui::Checkbox("Unlock Pinups", &bUnlockPinupsToggle);

			ImGui::Checkbox("Enable NSFW", &bNSFWToggle);
		}
		ImGui::EndChild();
	}

	virtual void Render() {}

	virtual void Run()
	{
		if (!Initalized)
			return;

		if (SkipPhoneTimerKey.IsDown())
			SkipPhoneTimer();

		if (bUnlockPhoneConversations && !bUnlockPhoneConversationsHookSet)
		{
			bUnlockPhoneConversationsHookSet = true;
			Utils::LogHook(Utils::GetLocation(CurrentLoc), "Cellphone_IsUnlocked", "Enable", "Hook Enabled");

			EnableHook(Cellphone_IsUnlocked);
			return;
		}

		if (!bUnlockPhoneConversations && bUnlockPhoneConversationsHookSet)
		{
			bUnlockPhoneConversationsHookSet = false;
			Utils::LogHook(Utils::GetLocation(CurrentLoc), "Cellphone_IsUnlocked", "Disable", "Hook Disabled");

			DisableHook(Cellphone_IsUnlocked);
			return;
		}

		if (bUnlockPinupsToggle && !bUnlockPinupsHookSet)
		{
			bUnlockPinupsHookSet = true;

			Utils::LogHook(Utils::GetLocation(CurrentLoc), "Album_IsPinupUnlocked", "Enable", "Hook Enabled");

			EnableHook(Album_IsPinupUnlocked);
		}

		if (!bUnlockPinupsToggle && bUnlockPinupsHookSet)
		{
			bUnlockPinupsHookSet = false;

			Utils::LogHook(Utils::GetLocation(CurrentLoc), "Album_IsPinupUnlocked", "Disable", "Hook Disabled");

			DisableHook(Album_IsPinupUnlocked);
		}

		if (bNSFWToggle && !*bNSFW)
		{
			*bNSFW = true;
			Utils::LogDebug(Utils::GetLocation(CurrentLoc), "NSFW Enabled");
		}
		else if (!bNSFWToggle && *bNSFW)
		{
			*bNSFW = false;
			Utils::LogDebug(Utils::GetLocation(CurrentLoc), "NSFW Disabled");
		}
	}

	HOOK_DEF(void, Girls_Update, (void* __this))
	{
		pGirlsClassInstance = __this;
		return oGirls_Update(__this);
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

	HOOK_DEF(bool, Album_IsPinupUnlocked, (void* __this, int pinupRewardAmount))
	{
		return true;
	}

	HOOK_DEF(void, Album_Update, (void* __this))
	{
		pAlbumClassInstance = __this;
		return oAlbum_Update(__this);
	}
};