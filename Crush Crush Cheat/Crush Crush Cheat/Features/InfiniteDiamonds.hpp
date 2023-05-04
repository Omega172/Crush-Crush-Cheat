#pragma once
#include "../Includes.hpp"

// Utilities
// Token: 0x060009D0 RID: 2512 RVA: 0x00051AE4 File Offset: 0x0004FCE4
// public static bool AwardDiamonds(int amount)
// Utilities::AwardDiamonds(int amount)

class InfiniteDiamonds
{
private:
	int amount = 1000000;
	bool force = true;

public:
	InfiniteDiamonds() {};
	
	void Render()
	{
		ImGui::BeginChild("##InfiniteDiamonds", ImVec2(ImGui::GetContentRegionAvail().x, ImGui::GetContentRegionAvail().y / 2), true);
		{
			ImGui::Text("Diamonds");
			ImGui::InputInt("##Diamond Amount", &amount);
			if (ImGui::Button("Give Diamonds"))
				Give();
		}
		ImGui::EndChild();
	}

	void Give()
	{
		MonoMethod* AwardDiamonds = Mono::Instance().GetMethod("Utilities", "AwardDiamonds", 2);
		if (AwardDiamonds == nullptr)
			return;

		void* args[2] = { &amount, &force };
		MonoObject* result = Mono::Instance().Invoke(AwardDiamonds, nullptr, args);

		if (bExtraDebug)
			LogInvoke("AwardDiamonds", "Result = " + (std::stringstream()<<result).str());
	}
};