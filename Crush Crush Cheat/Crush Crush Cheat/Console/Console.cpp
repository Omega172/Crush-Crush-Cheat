#include "Console.hpp"

void Console::initalize()
{
    if (this->initalized == true || this->allocated == TRUE)
        return;

    if (this->stdoutDummy == nullptr || this->stdinDummy == nullptr)
        this->alloc();

    this->initalized = true;
}

Console& Console::instance(bool show)
{
    static Console _instance;

    if (!_instance.initalized)
        _instance.initalize();

    _instance.setVisibility(show);

    return _instance;
}

BOOL Console::alloc()
{
    if (this->allocated)
    {
        std::cout << "Console already allocated!\n";
        return TRUE;
    }

    if (!AllocConsole()) {
        std::cout << "AllocConsole() Error: " << GetLastError() << std::endl;
        return FALSE;
    }

    freopen_s(&stdoutDummy, "CONOUT$", "w", stdout);
    freopen_s(&stdinDummy, "CONIN$", "r", stdin);
    std::cout.clear();
    std::cin.clear();

    this->allocated = true;
    
    std::cout << "Console allocated!\n";

    return TRUE;
}

BOOL Console::free()
{
    if (!this->allocated)
    {
        std::cout << "No console allocated!\n";
        return FALSE;
    }

    if (stdoutDummy)
        fclose(stdoutDummy);
    if (stdinDummy)
        fclose(stdinDummy);

    if (!FreeConsole())
    {
        std::cout << "FreeConsole() Error: " << GetLastError() << std::endl;
        return FALSE;
    }

    this->allocated = false;
    return TRUE;
}

void Console::setVisibility(bool visibility)
{
    this->visible = visibility;
    ShowWindow(GetConsoleWindow(), (visibility) ? SW_SHOW : SW_HIDE);
    return;
}

void Console::toggleVisibility()
{
    setVisibility(!this->visible);
    return;
}