#pragma once
#include "../includes.hpp"

#define CreateHook(a) \
	checkStatus(#a, MH_CreateHook(a, &h ## a, reinterpret_cast<LPVOID*>(&o ## a)));

#define EnableHook(a) \
	checkStatus(#a, MH_EnableHook(a));

#define DisableHook(a) \
	checkStatus(#a, MH_DisableHook(a));

#define HOOK_DEF(a, b, c) \
	typedef a (*t ## b) c; \
	static inline t ## b o ## b = NULL; \
	static a h ## b ## c

inline bool checkStatus(std::string name, MH_STATUS status)
{
	if (status != MH_OK && status != MH_ERROR_ALREADY_CREATED && status != MH_ERROR_ALREADY_INITIALIZED)
	{
		std::cout << "ERROR: Hook::" << name << " " << MH_StatusToString(status) << std::endl;
		return false;
	}

	return true;
}