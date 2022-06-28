#pragma once
#define WIN32_LEAN_AND_MEAN

#include <Windows.h>
#include <iostream>
#include <stdexcept>
#include <thread>

// Utilities
#include "Console/Console.hpp"
#include "minhook/include/MinHook.h"
#include "Hook/hook.h"

// Mono
#include <mono/metadata/threads.h>
#include <mono/metadata/object.h>

// SDK
#include "SDK/Girls.h"
#include "SDK/mono.h"

// Features
//#include "Features/UnlockGirls/UnlockGirls.h"
