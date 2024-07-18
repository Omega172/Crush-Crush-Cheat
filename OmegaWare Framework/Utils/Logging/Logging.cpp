#include "pch.h"

// These functions might be ugly but they are used to make the console text look pretty and make debugging easier

Utils::Location Utils::GetLocation(std::source_location stLocation)
{
	return { std::filesystem::path(stLocation.file_name()).filename().string(), stLocation.function_name(), stLocation.line(), stLocation.column() };
}

void Utils::LogHook(Location stLocation, std::string sHookName, std::string sReason, std::string sMessage)
{
	// Hook[HookName]: Filename | Function() -> Ln: 1 Col: 1 | Reason: Message
	std::cout << colors::cyan << "Hook[" << sHookName << "]" << colors::white << ": ";

	std::cout << colors::green << stLocation.m_sFilename << colors::white << " | " << colors::green << stLocation.m_sFunction << colors::white;

	std::cout << " -> Ln: " << colors::magenta << stLocation.m_iLine << colors::white << " Col: " << colors::magenta << stLocation.m_iColumn << colors::white;

	std::cout << " | " << colors::yellow << sReason << colors::white << ": " << sMessage << std::endl;
}

void Utils::LogError(Location stLocation, int iErrorCode)
{
	// Error: Filename | Function() -> Ln: 1 Col: 1 | Info: Message
	std::cout << colors::red << "Error" << colors::white << ": ";

	std::cout << colors::green << stLocation.m_sFilename << colors::white << " | " << colors::green << stLocation.m_sFunction << colors::white;

	std::cout << " -> Ln: " << colors::magenta << stLocation.m_iLine << colors::white << " Col: " << colors::magenta << stLocation.m_iColumn << colors::white;

	std::cout << " | " << colors::yellow << "Info" << colors::white << ": " << std::system_category().message(iErrorCode) << std::endl;
}

void Utils::LogError(Location stLocation, std::string sErrorMessage)
{
	// Error: Filename | Function() -> Ln: 1 Col: 1 | Info: Message
	std::cout << colors::red << "Error" << colors::white << ": ";

	std::cout << colors::green << stLocation.m_sFilename << colors::white << " | " << colors::green << stLocation.m_sFunction << colors::white;

	std::cout << " -> Ln: " << colors::magenta << stLocation.m_iLine << colors::white << " Col: " << colors::magenta << stLocation.m_iColumn << colors::white;

	std::cout << " | " << colors::yellow << "Info" << colors::white << ": " << sErrorMessage << std::endl;
}

void Utils::LogDebug(Location stLocation, std::string sDebugMessage)
{
	// Debug: Filename | Function() -> Ln: 1 Col: 1 | Info: Message
	std::cout << colors::cyan << "Debug" << colors::white << ": ";

	std::cout << colors::green << stLocation.m_sFilename << colors::white << " | " << colors::green << stLocation.m_sFunction << colors::white;

	std::cout << " -> Ln: " << colors::magenta << stLocation.m_iLine << colors::white << " Col: " << colors::magenta << stLocation.m_iColumn << colors::white;

	std::cout << " | " << colors::yellow << "Info" << colors::white << ": " << sDebugMessage << std::endl;
}