#pragma once
#include "../Includes.hpp"

// Album
// Token: 0x0600060D RID: 1549 RVA: 0x00030258 File Offset: 0x0002E458
// public bool IsPinupUnlocked(int pinupPage, int image)
// Album::IsPinupUnlocked(int pinupPage, int image)

// Album
// Token: 0x06000602 RID: 1538 RVA: 0x0002FC9C File Offset: 0x0002DE9C
// public void AddDate(Requirement.DateType date, int girlIndex)
// Album::AddDate(Requirement.DateType date, int girlIndex)

// Album
// Token: 0x06000606 RID: 1542 RVA: 0x0002FE18 File Offset: 0x0002E018
// private void Update()
// Album::Update()

inline void* pAlbumClassInstance = nullptr;

class AlbumUnlock
{
private:
	bool hookEnabled = false;
	bool toggle = false;
	
	void* Album_IsPinupUnlocked = nullptr;
	void* Album_Update = nullptr;
	
public:
	
	void Render()
	{
		if (ImGui::Button("Unlock Date Pics"))
			UnlockDatePhotos();

		ImGui::SameLine();

		ImGui::Checkbox("Unlock Pinups", &toggle);

		Toggle();
	}

	void Create()
	{
		Album_IsPinupUnlocked = Mono::Instance().GetCompiledMethod("Album", "IsPinupUnlocked", 2);
		if (Album_IsPinupUnlocked == nullptr)
			return;
		
		LogHook(HookLogReason::Create, "Album_IsPinupUnlocked");
		CreateHook(Album_IsPinupUnlocked);

		Album_Update = Mono::Instance().GetCompiledMethod("Album", "Update", 0);
		if (Album_Update == nullptr)
			return;
		
		LogHook(HookLogReason::Create, "Album_Update");
		LogHook(HookLogReason::Enable, "Album_Update");
		CreateHook(Album_Update);
		EnableHook(Album_Update);
	}

	void Toggle()
	{
		if (toggle && !hookEnabled)
		{
			hookEnabled = true;
			
			LogHook(HookLogReason::Enable, "Album_IsPinupUnlocked");
			EnableHook(Album_IsPinupUnlocked);
		}
		
		if (!toggle && hookEnabled)
		{
			hookEnabled = false;
			
			LogHook(HookLogReason::Disable, "Album_IsPinupUnlocked");
			DisableHook(Album_IsPinupUnlocked);
		}
	}

	void Destory()
	{
		LogHook(HookLogReason::Destroy, "Album_IsPinupUnlocked");
		DisableHook(Album_IsPinupUnlocked);

		LogHook(HookLogReason::Enable, "Album_Update");
		DisableHook(Album_Update);
	}

	void UnlockDatePhotos()
	{
		if (pAlbumClassInstance == nullptr)
			return;

		MonoMethod* Album_AddDate = Mono::Instance().GetMethod("Album", "AddDate", 2);
		if (Album_AddDate == nullptr)
			return;

		int dateType = NULL;
		MonoObject* result;
		for (unsigned int i = 0; i < Girls.size(); i++)
		{
			dateType = DateTypes::DateType::Beach;
			void* args[2] = { &dateType, &Girls[i].id };
			result = Mono::Instance().Invoke(Album_AddDate, pAlbumClassInstance, args);
			
			dateType = DateTypes::DateType::MoonlightStroll;
			args[0] = &dateType;
			args[1] = &Girls[i].id;
			result = Mono::Instance().Invoke(Album_AddDate, pAlbumClassInstance, args);
			
			dateType = DateTypes::DateType::MovieTheater;
			args[0] = &dateType;
			args[1] = &Girls[i].id;
			result = Mono::Instance().Invoke(Album_AddDate, pAlbumClassInstance, args);
			
			dateType = DateTypes::DateType::Sightseeing;
			args[0] = &dateType;
			args[1] = &Girls[i].id;
			result = Mono::Instance().Invoke(Album_AddDate, pAlbumClassInstance, args);
			
			if (bExtraDebug)
				LogInvoke("Album_AddDate", "Itter = " + std::to_string(i) + " Result = " + (std::stringstream() << result).str());
		}
	}

	HOOK_DEF(bool, Album_IsPinupUnlocked, (void* __this, int pinupPage, int image))
	{
		if (image != 0 && bExtraDebug)
			LogHook(HookLogReason::Called, "Album_IsPinupUnlocked", "pinupPage = " + std::to_string(pinupPage) + " image = " + std::to_string(image));
		
		return true;
	}

	HOOK_DEF(void, Album_Update, (void* __this))
	{
		//if (bExtraDebug)
			//LogHook(HookLogReason::Called, "Album_Update", (std::stringstream() << "__this = " << __this).str());
		
		pAlbumClassInstance = __this;
		return oAlbum_Update(__this);
	}
};