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
		ImGui::Checkbox("Unlock Pinups", &toggle);

		if (ImGui::Button("Unlock Date Pics"))
			UnlockDatePhotos();

		Toggle();
	}

	void Create()
	{
		Album_IsPinupUnlocked = Mono::instance().GetCompiledMethod("Album", "IsPinupUnlocked", 2);
		if (Album_IsPinupUnlocked == nullptr)
			return;
		
		std::cout << "[OmegaWare.xyz]::[Hooks]::Album_IsPinupUnlocked Created" << std::endl;
		CreateHook(Album_IsPinupUnlocked);

		Album_Update = Mono::instance().GetCompiledMethod("Album", "Update", 0);
		if (Album_Update == nullptr)
			return;
		
		std::cout << "[OmegaWare.xyz]::[Hooks]::Album_Update Created & Enabled" << std::endl;
		CreateHook(Album_Update);
		EnableHook(Album_Update);
	}

	void Toggle()
	{
		if (toggle && !hookEnabled)
		{
			hookEnabled = true;
			
			std::cout << "[OmegaWare.xyz]::[Hooks]::Album_IsPinupUnlocked Enabled" << std::endl;
			EnableHook(Album_IsPinupUnlocked);
		}
		
		if (!toggle && hookEnabled)
		{
			hookEnabled = false;
			
			std::cout << "[OmegaWare.xyz]::[Hooks]::Album_IsPinupUnlocked Disabled" << std::endl;
			DisableHook(Album_IsPinupUnlocked);
		}
	}

	void Destory()
	{
		std::cout << "[OmegaWare.xyz]::[Hooks]::Album_IsPinupUnlocked Destroyed" << std::endl;
		DisableHook(Album_IsPinupUnlocked);

		std::cout << "[OmegaWare.xyz]::[Hooks]::Album_Update Destroyed" << std::endl;
		DisableHook(Album_Update);
	}

	void UnlockDatePhotos()
	{
		if (pAlbumClassInstance == nullptr)
		{
			std::cout << "[OmegaWare.xyz]::[Hooks]::Album Class instance null" << std::endl;
			return;
		}

		MonoMethod* Album_AddDate = Mono::instance().GetMethod("Album", "AddDate", 2);
		if (Album_AddDate == nullptr)
		{
			std::cout << "[OmegaWare.xyz]::[Hooks]::Album_AddDate Method null" << std::endl;
			return;
		}

		for (unsigned int i = 0; i < Girls.size(); i++)
		{
			int dateType = DateTypes::DateType::Beach;
			void* args[2] = { &dateType, &Girls[i].id };
			MonoObject* result = Mono::instance().Invoke(Album_AddDate, pAlbumClassInstance, args);
			//std::cout << "[OmegaWare.xyz]::Invoke(Album_AddDate)(Itter = " << i << " " << dateType << ") = " << result << std::endl;
			
			
			dateType = DateTypes::DateType::MoonlightStroll;
			args[0] = &dateType;
			args[1] = &Girls[i].id;
			result = Mono::instance().Invoke(Album_AddDate, pAlbumClassInstance, args);
			//std::cout << "[OmegaWare.xyz]::Invoke(Album_AddDate)(Itter = " << i << " " << dateType << ") = " << result << std::endl;
			
			
			dateType = DateTypes::DateType::MovieTheater;
			args[0] = &dateType;
			args[1] = &Girls[i].id;
			result = Mono::instance().Invoke(Album_AddDate, pAlbumClassInstance, args);
			//std::cout << "[OmegaWare.xyz]::Invoke(Album_AddDate)(Itter = " << i << " " << dateType << ") = " << result << std::endl;
			
			
			dateType = DateTypes::DateType::Sightseeing;
			args[0] = &dateType;
			args[1] = &Girls[i].id;
			result = Mono::instance().Invoke(Album_AddDate, pAlbumClassInstance, args);
			std::cout << "[OmegaWare.xyz]::Invoke(Album_AddDate)(Itter = " << i << " " << dateType << ") = " << result << std::endl;
		}
	}

	HOOK_DEF(bool, Album_IsPinupUnlocked, (void* __this, int pinupPage, int image))
	{
		if (image != 0)
			std::cout << "[OmegaWare.xyz]::[Hooks]::Album_IsPinupUnlocked called = " << pinupPage << " " << image << std::endl;

		return true;
	}

	HOOK_DEF(void, Album_Update, (void* __this))
	{
		//std::cout << "[OmegaWare.xyz]::[Hooks]::Album_Update called = " << __this << std::endl;
		pAlbumClassInstance = __this;
		return oAlbum_Update(__this);
	}
};