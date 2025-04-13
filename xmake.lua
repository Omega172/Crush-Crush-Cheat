add_rules("mode.debug", "mode.release")

target("Omegaware CrushCrush")
    set_arch("x86")
    set_languages("c++latest")
    set_kind("shared")
    set_pcxxheader("OmegaWare Framework/pch.h")

    add_includedirs("OmegaWare Framework")

   add_linkdirs("OmegaWare Framework/Kiero/minhook/include")

   add_links(
        "kernel32", "user32", "gdi32", "winspool", "comdlg32",
        "advapi32", "shell32", "ole32", "oleaut32", "uuid",
        "odbc32", "odbccp32"
    )

    if is_mode("debug") then
        add_links("minhook.x86d.lib")
    else
        add_links("minhook.x86.lib")
    end

    if is_mode("debug") then
        set_runtimes("MDd")
        set_targetdir("Build/Debug/Internal")
        add_defines("_DEBUG")

        add_cxflags(
            "/permissive-",   -- Enforce standard C++ compliance
            "/GS",            -- Buffer security check
            "/W3",            -- Warning level 3
            "/Zc:wchar_t",    -- Treat wchar_t as a built-in type
            --"/ZI",            -- Edit-and-continue debug information
            "/Od",            -- Disable optimizations for debugging
            "/sdl",           -- Enable additional security checks
            "/Zc:inline",     -- Remove unused inline functions
            "/fp:precise",    -- Floating-point model: precise
            "/WX-",           -- Do not treat warnings as errors
            "/Zc:forScope",   -- Enforce standard C++ for-loop scope
            "/RTC1",          -- Enable runtime error checks (stack frame consistency)
            "/Gd",            -- Use __cdecl calling convention
            "/MDd",           -- Use multi-threaded **debug** DLL runtime
            "/FC",            -- Show full path in diagnostics
            "/EHsc",          -- Enable C++ exception handling
            "/nologo"         -- Suppress startup banner
        )

        add_ldflags(
            "/MANIFEST",       -- Generate a side-by-side manifest
            "/NXCOMPAT",       -- Data Execution Prevention (DEP)
            "/DYNAMICBASE",    -- Enable ASLR (Address Space Layout Randomization)
            "/DEBUG",          -- Generate debugging information
            "/DLL",            -- Build as a DLL
            "/MACHINE:X86",    -- Target x64 architecture
            "/INCREMENTAL",    -- Enable incremental linking
            "/SUBSYSTEM:CONSOLE", -- Console application
            "/MANIFESTUAC:\"level='asInvoker' uiAccess='false'\"", -- UAC manifest settings
            "/NOLOGO",         -- Suppress linker startup banner
            "/TLBID:1"         -- Type library ID
        )
    else
        set_runtimes("MD")
        set_targetdir("Build/Release/Internal")
        add_undefines("_DEBUG")

        add_cxflags(
            "/permissive-",   -- Enforce standard C++ compliance
            "/GS",            -- Buffer security check
            "/GL",            -- Whole program optimization
            "/W3",            -- Warning level 3
            "/Gy",            -- Function-level linking
            "/Zc:wchar_t",    -- Treat wchar_t as a built-in type
            "/Zi",            -- Generate debug information
            "/O2",            -- Optimize for speed
            "/sdl",           -- Enable additional security checks
            "/Zc:inline",     -- Remove unused inline functions
            "/fp:precise",    -- Floating-point model: precise
            "/WX-",           -- Do not treat warnings as errors
            "/Zc:forScope",   -- Enforce standard C++ for-loop scope
            "/Gd",            -- Use __cdecl calling convention
            "/Oi",            -- Enable intrinsic functions
            "/MD",            -- Use multi-threaded DLL runtime
            "/FC",            -- Show full path in diagnostics
            "/EHsc",          -- Enable C++ exception handling
            "/FS",            -- Full program optimization
            "/nologo"         -- Suppress startup banner
        )

        add_ldflags(
            "/MANIFEST",         -- Generate a side-by-side manifest
            "/LTCG:incremental", -- Enable incremental link-time code generation
            "/NXCOMPAT",         -- Enable Data Execution Prevention (DEP)
            "/DYNAMICBASE",      -- Enable ASLR (Address Space Layout Randomization)
            "/DEBUG",            -- Generate debugging information (Release PDBs)
            "/DLL",              -- Build as a DLL
            "/MACHINE:X86",      -- Target x64 architecture
            "/OPT:REF",          -- Remove unreferenced functions/data
            "/OPT:ICF",          -- Perform identical COMDAT folding
            "/SUBSYSTEM:CONSOLE", -- Console application
            "/MANIFESTUAC:\"level='asInvoker' uiAccess='false'\"", -- UAC settings
            "/NOLOGO",           -- Suppress linker startup banner
            "/TLBID:1"           -- Type library ID
        )
    end

    add_files("OmegaWare Framework/dllmain.cpp")
    add_files("OmegaWare Framework/pch.cpp")
    add_files("OmegaWare Framework/GUI/**.cpp")
    add_files("OmegaWare Framework/ImGUI/**.cpp")
    add_files("OmegaWare Framework/Kiero/**.cpp")
    add_files("OmegaWare Framework/Utils/**.cpp")