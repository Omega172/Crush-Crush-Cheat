#pragma once
#include "../Includes.hpp"

namespace GUI
{
	inline bool bMenuOpen = false;
	inline int WIDTH = 800;
	inline int HEIGHT = 600;

	inline void BeginRender()
	{
		ImGui_ImplDX11_NewFrame();
		ImGui_ImplWin32_NewFrame();
		ImGui::NewFrame();

		ImGuiIO& io = ImGui::GetIO();
	}

	inline void Render()
	{
		if (bWatermark)
			showWatermark(bWatermarkFPS, "OmegaWare.xyz (Crush Crush)", ImVec4(255, 255, 255, 255), ImVec4(255, 255, 255, 0));

		if (!bMenuOpen)
			return;

		ImGui::SetNextWindowSize(ImVec2(WIDTH, HEIGHT));
		ImGui::Begin("OmegaWare.xyz (Crush Crush)", NULL, ImGuiWindowFlags_NoResize | ImGuiWindowFlags_NoSavedSettings | ImGuiWindowFlags_NoCollapse);
		
		if (ImGui::Button("Unload"))
			bExit = true;

		unlockGirls.Render();
		speedHack.Render();
		modifyGiftQuantity.Render();
		infiniteDiamonds.Render();
		phoneSkip.Render();

		ImGui::End();
	}

	inline void EndRender()
	{
		ImGui::EndFrame();
		ImGui::Render();
	}
}