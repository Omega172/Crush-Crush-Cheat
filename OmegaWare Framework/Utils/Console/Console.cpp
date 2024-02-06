#include "pch.h"

Console::Console(bool bVisibility, std::string sConsoleTitle)
{
	if (this->bInitalized)
		return;

	if (!AllocConsole())
	{
		Utils::LogError(Utils::GetLocation(CurrentLoc), GetLastError());
		this->bInitalized = false;
		return;
	}

	errno_t errSTDOut = freopen_s(&m_pSTDOutDummy, "CONOUT$", "w", stdout);
	if (errSTDOut != NULL)
	{
		Utils::LogError(Utils::GetLocation(CurrentLoc), errSTDOut);
		this->bInitalized = false;
		return;
	}

	errno_t errSTDIn = freopen_s(&m_pSTDInDummy, "CONIN$", "w", stdin);
	if (errSTDIn != NULL)
	{
		Utils::LogError(Utils::GetLocation(CurrentLoc), errSTDIn);
		this->bInitalized = false;
		return;
	}

	std::cout.clear();
	std::cin.clear();
	
	SetVisibility(bVisibility);
	SetTitle(sConsoleTitle);

	this->bInitalized = true;
	return;
}

void Console::Destroy()
{
	if (!this->bInitalized)
		return;

	if (m_pSTDOutDummy)
		fclose(m_pSTDOutDummy);

	if (m_pSTDInDummy)
		fclose(m_pSTDInDummy);

	if (!FreeConsole())
		Utils::LogError(Utils::GetLocation(CurrentLoc), GetLastError());

	this->bInitalized = false;
	return;
}

void Console::SetVisibility(bool bVisibility)
{
	this->bVisible = bVisibility;
	ShowWindow(GetConsoleWindow(), (bVisibility) ? SW_SHOW : SW_HIDE);
	return;
}

void Console::ToggleVisibility()
{
	SetVisibility(!this->bVisible);
	return;
}