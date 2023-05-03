#include "GUI.hpp"
#include "Custom.hpp"

int testHotkey;

void GUI::Render()
{
	if (bWatermark)
		showWatermark(bWatermarkFPS, "OmegaWare.xyz (Crush Crush)", ImVec4(255, 255, 255, 255), ImVec4(255, 255, 255, 0));

	phoneSkip.Update();
	
	if (!bMenuOpen)
		return;

	//ImGui::ShowStyleEditor();

	ImGui::SetNextWindowSize(ImVec2(WIDTH, HEIGHT));
	ImGui::Begin("OmegaWare.xyz (Crush Crush)", NULL, ImGuiWindowFlags_NoResize | ImGuiWindowFlags_NoSavedSettings | ImGuiWindowFlags_NoCollapse);

	//	ImGui::SetCursorPos(ImVec2(6, 20));
	ImGui::BeginChild("##Cheat", ImVec2(ImGui::GetContentRegionAvail().x / 3, ImGui::GetContentRegionAvail().y / 2), true);
	{
		ImGui::Text("Cheat");
		if (ImGui::Button("Unload"))
			bExit = true;
		ImGui::SameLine();
		if (ImGui::Button(con.GetVisibility() ? "Hide Console" : "Show Console"))
			con.ToggleVisibility();
		ImGui::Checkbox("Extra Debug Info", &bExtraDebug);
		ImGui::Checkbox("Watermark", &bWatermark);
		if (bWatermark)
			ImGui::Checkbox("Watermark FPS", &bWatermarkFPS);
	}
	ImGui::EndChild();
	ImGui::SameLine();

	speedHack.Render();
	ImGui::SameLine();
	
	infiniteDiamonds.Render();
	
	modifyGiftQuantity.Render();
	ImGui::SameLine();

	ImGui::BeginChild("##Misc", ImVec2(ImGui::GetContentRegionAvail().x, ImGui::GetContentRegionAvail().y), true);
	{
		ImGui::Text("Misc");
		
		unlockGirls.Render();
		modGirls.Render();
		phoneSkip.Render();
		albumUnlock.Render();
		jobUnlock.Render();
		ImGui::SameLine();
		hobbiesUnlock.Render();
		nsfw.Render();
	}
	ImGui::EndChild();

	ImGui::End();
}
