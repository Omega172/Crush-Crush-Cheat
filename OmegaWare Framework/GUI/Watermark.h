#pragma once
#include "pch.h"

// This function is scuffed and doesn't work properly when the mouse is over the watermark
// If someone could fix it that would be great I'll give you a cookie and 5 schmeckles

inline void showWatermark(bool showFPS, const char* text, ImVec4 color, ImVec4 hoverColor)
{
	bool open = true;
	const float distance = 10.0f;
	static int corner = 0;
	ImGuiIO& io = ImGui::GetIO();
	ImGuiWindowFlags windowFlags = ImGuiWindowFlags_NoDecoration | ImGuiWindowFlags_AlwaysAutoResize | ImGuiWindowFlags_NoSavedSettings | ImGuiWindowFlags_NoFocusOnAppearing | ImGuiWindowFlags_NoMove;

	if (io.MousePos.x < 278.0f && io.MousePos.y < 42.0f && io.MousePos.x > 0.0f && io.MousePos.y > 0.0f)
	{
		if (corner != -1)
		{
			windowFlags |= ImGuiWindowFlags_NoMove | ImGuiWindowFlags_NoBackground;
			ImVec2 windowPosPivot = ImVec2((corner & 1) ? 1.0f : 0.0f, (corner & 2) ? 1.0f : 0.0f);
			ImVec2 windowPos = ImVec2((corner & 1) ? io.DisplaySize.x - distance : distance, (corner & 2) ? io.DisplaySize.y - distance : distance);
			ImGui::SetNextWindowPos(windowPos, ImGuiCond_Always, windowPosPivot);
		}

		ImGui::SetNextWindowBgAlpha(0.05f);
		if (ImGui::Begin("Watermark", &open, windowFlags))
		{
			ImGui::TextColored(hoverColor, text);
			ImGui::End();
		}
	}
	else
	{
		if (corner != -1)
		{
			windowFlags |= ImGuiWindowFlags_NoMove;
			ImVec2 windowPosPivot = ImVec2((corner & 1) ? 1.0f : 0.0f, (corner & 2) ? 1.0f : 0.0f);
			ImVec2 windowPos = ImVec2((corner & 1) ? io.DisplaySize.x - distance : distance, (corner & 2) ? io.DisplaySize.y - distance : distance);
			ImGui::SetNextWindowPos(windowPos, ImGuiCond_Always, windowPosPivot);
		}

		ImGui::SetNextWindowBgAlpha(0.50f);
		if (ImGui::Begin("Watermark", &open, windowFlags))
		{
			if (showFPS)
				ImGui::TextColored(color, "%s FPS: %i", text, static_cast<int>(io.Framerate));
			else
				ImGui::TextColored(color, text);

			ImGui::End();
		}
	}
}