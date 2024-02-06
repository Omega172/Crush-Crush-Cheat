#pragma once
#include "pch.h"
#include "../kiero/minhook/include/MinHook.h"

#define CreateHook(a) \
	checkStatus(#a, MH_CreateHook(a, &h ## a, reinterpret_cast<LPVOID*>(&o ## a)), "Created");

#define EnableHook(a) \
	checkStatus(#a, MH_EnableHook(a), "Enabled");

#define DisableHook(a) \
	checkStatus(#a, MH_DisableHook(a), "Disabled");

#define HOOK_DEF(a, b, c) \
	typedef a (*t ## b) c; \
	static inline t ## b o ## b = NULL; \
	static a h ## b ## c

inline bool checkStatus(std::string name, MH_STATUS status, std::string reason)
{
    if (status != MH_ERROR_ALREADY_CREATED && status != MH_ERROR_ALREADY_INITIALIZED)
        return true;

	if (status != MH_OK)
	{
		Utils::LogHook(Utils::GetLocation(CurrentLoc), name, "Error", MH_StatusToString(status));
		return false;
	}
    else
        Utils::LogHook(Utils::GetLocation(CurrentLoc), name, reason, MH_StatusToString(status));

	return true;
}

/* Example:
inline void* pAlbumClassInstance = nullptr;
void* Album_Update = nullptr;

Album_Update = mono.GetCompiledMethod("Album", "Update", 0);
if (Album_Update == nullptr)
    return;

CreateHook(Album_Update);
EnableHook(Album_Update);

HOOK_DEF(void, Album_Update, (void* __this))
{
    // Do stuff

    pAlbumClassInstance = __this;
    return oAlbum_Update(__this);
}
*/