#include "GUI.hpp"

void GUI::Render()
{
	if (bWatermark)
		showWatermark(bWatermarkFPS, "OmegaWare.xyz (Crush Crush)", ImVec4(255, 255, 255, 255), ImVec4(255, 255, 255, 0));

	if (!bMenuOpen)
		return;

	ImGui::SetNextWindowSize(ImVec2(WIDTH, HEIGHT));
	ImGui::Begin("OmegaWare.xyz (Crush Crush)", NULL, ImGuiWindowFlags_NoResize | ImGuiWindowFlags_NoSavedSettings | ImGuiWindowFlags_NoCollapse);

	//	ImGui::SetCursorPos(ImVec2(6, 20));
	ImGui::BeginChild("##Misc", ImVec2(ImGui::GetContentRegionAvail().x / 3, ImGui::GetContentRegionAvail().y / 2), true);
	{
		ImGui::Text("Misc");
		if (ImGui::Button("Unload"))
			bExit = true;
		ImGui::SameLine();
		if (ImGui::Button(con.getVisibility() ? "Hide Console" : "Show Console"))
			con.toggleVisibility();
		unlockGirls.Render();
		phoneSkip.Render();
	}
	ImGui::EndChild();
	ImGui::SameLine();

	speedHack.Render();
	ImGui::SameLine();
	
	infiniteDiamonds.Render();
	
	modifyGiftQuantity.Render();

	ImGui::End();
}
