#pragma once
#include "../../Includes.hpp"

class Console
{
private:
	FILE* stdoutDummy = nullptr;
	FILE* stdinDummy = nullptr;
	bool bAllocated = FALSE;
	bool bInitalized = false;
	bool bVisible = true;
	void Initalize();
	Console() {};

public:
	static Console& Instance();
	static Console& Instance(bool bVisibility);
	bool Alloc();
	bool Free();
	void SetVisibility(bool bVisibility);
	void ToggleVisibility();
	bool GetVisibility() { return this->bVisible; };
	void SetTitle(std::string title) { SetConsoleTitleA(title.c_str()); };
	bool IsAllocated() { return this->bAllocated; };
};

inline Console con = Console::Instance(true);