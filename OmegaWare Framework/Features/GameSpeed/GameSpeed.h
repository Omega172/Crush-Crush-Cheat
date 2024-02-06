#pragma once
#include "pch.h"
#include <sstream>
#include <iostream>

class GameSpeed : public Feature
{
private:
	bool Initalized = false;

	KeyBindToggle TimeScaleToggle = KeyBindToggle(ImGuiKey_S);
	bool bSetting = false;
	float flTimeScale = 3.f;
	bool bSet = false;

	void SetTimeScale(float flTargetTimeScale)
	{
		MonoMethod* set_timeScale = Mono::Instance().GetMethod("Time", "set_timeScale", 1, "UnityEngine", "UnityEngine");
		if (!set_timeScale)
		{
			Utils::LogError(Utils::GetLocation(CurrentLoc), "Failed to get a pointer to set_timeScale");
			return;
		}

		void* pArgs[1] = { &flTargetTimeScale };
		MonoObject* pResult = Mono::Instance().Invoke(set_timeScale, nullptr, pArgs);

		std::stringstream SS("set_timeScale = ");
		SS << std::hex << pResult << std::dec << " | Time Scale set to: " << flTargetTimeScale;

		Utils::LogDebug(Utils::GetLocation(CurrentLoc), SS.str());
	}

public:
	GameSpeed() {}

	virtual bool Setup() 
	{
		Initalized = true;
		return Initalized;
	}

	virtual void Destroy() { SetTimeScale(1.f); }

	virtual void HandleKeys() { TimeScaleToggle.HandleToggle(); }

	virtual void DrawMenuItems()
	{
		ImGui::SameLine();
		ImGui::BeginChild("#GameSpeed", ImVec2(ImGui::GetContentRegionAvail().x / 2, ImGui::GetContentRegionAvail().y / 2), ImGuiChildFlags_Border);
		{
			ImGui::Text("Game Speed");
			ImGui::InputFloat("Time Scale Factor", &flTimeScale, .1f, 1.f, "%.1f");
			ImGui::Text("Time Scale Manipulation");
			ImGui::SameLine();
			ImGui::Hotkey("#Time Scale Manipulation", TimeScaleToggle, &bSetting);
		}
		ImGui::EndChild();
	}

	virtual void Render() {}

	virtual void Run()
	{
		if (TimeScaleToggle.IsToggled() && !bSet)
		{
			bSet = true;
			SetTimeScale(flTimeScale);
		}

		if (!TimeScaleToggle.IsToggled() && bSet)
		{
			bSet = false;
			SetTimeScale(1.f);
		}
	}
};

