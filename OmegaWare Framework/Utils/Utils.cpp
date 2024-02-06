#include "pch.h"

#include <stdexcept>

// https://stackoverflow.com/questions/48708440/check-if-i-can-write-to-memory
// https://stackoverflow.com/questions/18394647/can-i-check-if-memory-block-is-readable-without-raising-exception-with-c
bool Utils::IsReadableMemory(void* lpAddress, size_t dwLength)
{
	MEMORY_BASIC_INFORMATION MemInfo;
	if (VirtualQuery(lpAddress, &MemInfo, sizeof(MEMORY_BASIC_INFORMATION)) == NULL)
	{
#ifdef EXCEPT_ON_VQUERY_ERR
		throw std::runtime_error(std::system_category().message(GetLastError()));
#endif
		return false;
	}

	if (MemInfo.State != MEM_COMMIT)
	{
#ifdef EXCEPT_ON_MEM_ERR
		throw std::runtime_error("State != MEM_COMMIT");
#endif
		return false;
	}

	if (MemInfo.Protect == PAGE_NOACCESS || MemInfo.Protect == PAGE_EXECUTE)
	{
#ifdef EXCEPT_ON_MEM_ERR
		throw std::runtime_error("Protect is not Readable");
#endif
		return false;
	}

	SIZE_T dwRemainingRegionSize = MemInfo.RegionSize + ((char*)lpAddress - (char*)MemInfo.AllocationBase);
	if (dwRemainingRegionSize < dwLength)
		return IsReadableMemory((char*)lpAddress + dwRemainingRegionSize, dwLength - dwRemainingRegionSize);

	return true;
}

bool Utils::IsReadableMemory(const void* lpAddress, size_t dwLength)
{
	MEMORY_BASIC_INFORMATION MemInfo;
	if (VirtualQuery(lpAddress, &MemInfo, sizeof(MEMORY_BASIC_INFORMATION)) == NULL)
	{
#ifdef EXCEPT_ON_VQUERY_ERR
		throw std::runtime_error(std::system_category().message(GetLastError()));
#endif
		return false;
	}

	if (MemInfo.State != MEM_COMMIT)
	{
#ifdef EXCEPT_ON_MEM_ERR
		throw std::runtime_error("State != MEM_COMMIT");
#endif
		return false;
	}

	if (MemInfo.Protect == PAGE_NOACCESS || MemInfo.Protect == PAGE_EXECUTE)
	{
#ifdef EXCEPT_ON_MEM_ERR
		throw std::runtime_error("Protect is not Readable");
#endif
		return false;
	}

	SIZE_T dwRemainingRegionSize = MemInfo.RegionSize + ((char*)lpAddress - (char*)MemInfo.AllocationBase);
	if (dwRemainingRegionSize < dwLength)
		return IsReadableMemory((char*)lpAddress + dwRemainingRegionSize, dwLength - dwRemainingRegionSize);

	return true;
}