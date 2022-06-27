#pragma once
#include "../../includes.hpp"

typedef void *t_Girls_UnlockGirl(INT32 girl);

/*
void my_int_func(int x)
{
    printf( "%d\n", x );
}
 
int main()
{
    void (*foo)(int);
    // the ampersand is actually optional
    foo = &my_int_func;
 
    return 0;
}
*/

/*
fullname=Girls::UnlockGirl
namespace=
classname=Girls
methodname=UnlockGirl
Method.Parameters=(int girl)
parameterstring=int girl 
*/

inline void UnlockGirls()
{
	void (*Girls_UnlockGirl)(INT32) = (void(*)(INT32))(Mono::instance().GetMethod("Girls", "UnlockGirl", 1));
	if (Girls_UnlockGirl == nullptr)
	{
		std::cout << "Girls_UnlockGirl was NULL\n" << std::endl;
		return;
	}

	std::cout << "Girls_UnlockGirl Addr: " << std::hex << Girls_UnlockGirl << std::dec << std::endl;

	for (INT32 i = 0; i < 46; i++)
		Girls_UnlockGirl(i);

	return;
}

/*
HOOK_DEF(void, Girls_UnlockGirl, (void* __this, UINT32 girl))
{
	
}
*/