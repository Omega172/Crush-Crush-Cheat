#pragma once
#include "../../Includes.hpp"

class Console
{
private:
	FILE* stdoutDummy = nullptr;
	FILE* stdinDummy = nullptr;
	BOOL allocated = FALSE;
	bool initalized = false;
	bool visible = true;
	void initalize();
	Console() {};

public:
	static Console& instance(bool visibility);
	BOOL alloc();
	BOOL free();
	void setVisibility(bool visibility);
	void toggleVisibility();
	bool getVisibility() { return this->visible; };
};