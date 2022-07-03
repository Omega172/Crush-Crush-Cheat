#pragma once
#include "../Includes.hpp"

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

		std::cout << "Time Scale: " << scale << std::endl;

		void* args[1] = { &scale };
		Mono::instance().Invoke(set_timeScale, nullptr, args);
	}

public:
	SpeedHack() {};

	void Render()
	{
		ImGui::InputFloat("Speed Hack Speed", &timeScale);
		ImGui::Checkbox("Speed Hack", &toggle);

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
