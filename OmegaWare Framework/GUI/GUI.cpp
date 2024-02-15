#include "pch.h"
#include "Watermark.h"

bool bWatermark = true;
bool bWatermarkFPS = true;

void GUI::Render()
{
	if (bWatermark)
		showWatermark(bWatermarkFPS, Cheat::Title.c_str(), ImVec4(255, 255, 255, 255), ImVec4(255, 255, 255, 0));

	if (bMenuOpen)
	{
		ImGui::SetNextWindowSize(ImVec2(WIDTH, HEIGHT));
		ImGui::Begin(Cheat::Title.c_str(), NULL, ImGuiWindowFlags_NoResize | ImGuiWindowFlags_NoSavedSettings | ImGuiWindowFlags_NoCollapse);

		ImGui::BeginChild("Cheat", ImVec2(ImGui::GetContentRegionAvail().x / 3, ImGui::GetContentRegionAvail().y / 2), true);
		{
			ImGui::Text("Cheat");
			ImGui::Spacing();

			if (ImGui::Button("Unload"))
				Cheat::bShouldRun = false;
			ImGui::SameLine();
			if (ImGui::Button(Cheat::console.get()->GetVisibility() ? "Hide Console" : "Show Console"))
				Cheat::console.get()->ToggleVisibility();

			if (Cheat::console.get()->GetVisibility())
			{
				ImGui::SameLine();
				if (ImGui::Button("Clear Console"))
				{
					system("cls");
					Utils::LogDebug(Utils::GetLocation(CurrentLoc), "Console cleared.");
				}
			}

			ImGui::Checkbox("Watermark", &bWatermark);
			if (bWatermark)
				ImGui::Checkbox("Watermark FPS", &bWatermarkFPS);
		}
		ImGui::EndChild();

		for (size_t i = 0; i < Features.size(); i++)
		{
			Features[i].get()->DrawMenuItems();
		}

		ImGui::End();
	}

	//
	//	Other Render Stuff
	//

#if FRAMEWORK_UNREAL
	auto pUnreal = Cheat::unreal.get();
	pUnreal->RefreshActorList();
#endif

	for (size_t i = 0; i < Features.size(); i++)
	{
		Features[i].get()->Render();
	}

	//
	// End Other Render Stuff
	//
}