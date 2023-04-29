#pragma once
#include "../../Includes.hpp"

enum class HookLogReason
{
	Create,
	Enable,
	Disable,
	Destroy,
	Error,
	Called,
	Info,
	None
};

inline std::string LR2S(HookLogReason reason)
{
	switch (reason)
	{
	case HookLogReason::Create:
		return "Created";
	case HookLogReason::Enable:
		return "Enabled";
	case HookLogReason::Disable:
		return "Disabled";
	case HookLogReason::Destroy:
		return "Destroyed";
	case HookLogReason::Error:
		return "Errored";
	case HookLogReason::Called:
		return "Called";
	case HookLogReason::Info:
		return "Info";
	case HookLogReason::None:
		return "None";
	}
	
	return "Unknown";
}

inline void LogHook(HookLogReason reason, std::string hook, std::string info = "")
{
	if (info == "MH_ERROR_DISABLED" && bExit)
		return; 
	
	std::cout << "[" << dye::aqua("OmegaWare.xyz") << "]::[" << dye::green("Hooks") << "]::[" << dye::light_red(hook) << "] ";

	if (reason != HookLogReason::Error)
		std::cout << "Reason: " << dye::yellow(LR2S(reason));
	else
		std::cout << "Reason: " << dye::red(LR2S(reason));
	
	if (!info.empty())
	{
		if (reason != HookLogReason::Error)
			std::cout << " Info: " << dye::purple(info);
		else
			std::cout << " Info: " << dye::red(info);
	}

	std::cout << std::endl;
}

inline void Log(std::string hook, std::string info)
{
	std::cout << "[" << dye::aqua("OmegaWare.xyz") << "]::[" << dye::green("Info") << "]::[" << dye::light_red(hook) << "] ";
	std::cout << " Info: " << dye::purple(info);
	std::cout << std::endl;
}

inline void LogInvoke(std::string method, std::string info)
{
	std::cout << "[" << dye::aqua("OmegaWare.xyz") << "]::[" << dye::green("Invoke") << "]::[" << dye::light_red(method) << "]";

	if (info.length() > 0)
	{
		std::cout << "Info: " << dye::purple(info);
	}

	std::cout << std::endl;
}
