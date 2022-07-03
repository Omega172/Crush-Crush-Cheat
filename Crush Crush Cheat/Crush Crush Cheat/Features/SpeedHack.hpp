#pragma once
#include "../Includes.hpp"

// UnityEngine.Time
// Token: 0x06000DC2 RID: 3522
// public static extern void set_timeScale(float value)
// UnityEngine.Time::set_timeScale(float value)

class SpeedHack
{
private:
	bool enabled = false;
	bool toggle = false;
	float timeScale = 3.0f;

	void SetTimeScale(float scale)
	{
		MonoMethod* set_timeScale = Mono::instance().GetMethod("Time", "set_timeScale", 1, "UnityEngine", "UnityEngine");
		if (set_timeScale == nullptr)
			return;

		void* args[1] = { &scale };
		MonoObject* result = Mono::instance().Invoke(set_timeScale, nullptr, args);
		
		if (bExtraDebug)
			LogInvoke("set_timeScale", "Result = " + (std::stringstream() << result).str());
	}

public:
	SpeedHack() {};

	void Render()
	{
		ImGui::BeginChild("##SpeedHack", ImVec2(ImGui::GetContentRegionAvail().x / 2, ImGui::GetContentRegionAvail().y / 2), true);
		{
			ImGui::Text("Speed Hack");
			ImGui::InputFloat("##Speed Hack Speed", &timeScale, 0.1f, 1.0f, 1);
			ImGui::Checkbox("Speed Hack", &toggle);
		}
		ImGui::EndChild();

		Toggle();
	}

	void Enable()
	{
		enabled = true;
		SetTimeScale(timeScale);
	}

	void Toggle()
	{
		if (toggle && !enabled)
			Enable();
		
		if (!toggle && enabled)
			Disable();
	}

	void Disable()
	{
		enabled = false;
		SetTimeScale(1.0f);
	}
};
