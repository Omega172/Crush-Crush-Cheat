#pragma once
#include "../Includes.hpp"

namespace GUI
{
	inline bool bMenuOpen = false;
	inline int WIDTH = 600;
	inline int HEIGHT = 300;
	
	inline void BeginRender()
	{
		ImGui_ImplDX11_NewFrame();
		ImGui_ImplWin32_NewFrame();
		ImGui::NewFrame();

		ImGuiIO& io = ImGui::GetIO();
	}

	void Render();

	inline void EndRender()
	{
		ImGui::EndFrame();
		ImGui::Render();
	}
}