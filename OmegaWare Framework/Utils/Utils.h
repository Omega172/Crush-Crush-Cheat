#pragma once
#include "Logging/Logging.h" // Include the logging header file
#include "Console/Console.h" // Include the console header file which contains the console class used to create a console window

namespace Utils
{
	// Functions to check if memory is readable
	bool IsReadableMemory(void* ptr, size_t byteCount);
	bool IsReadableMemory(const void* ptr, size_t byteCount); 
}