#pragma once
#include "pch.h"

class GiveStuff : public Feature
{
private:
	bool Initalized = false;

	int iAmount = 1000000;
	bool bForce = true;

	void GiveDiamonds(int iAmountToGive)
	{
		MonoMethod* AwardDiamonds = Mono::Instance().GetMethod("Utilities", "AwardDiamonds", 2);
		if (!AwardDiamonds)
		{
			Utils::LogError(Utils::GetLocation(CurrentLoc), "Failed to get a pointer to AwardDiamonds");
			return;
		}

		void* pArgs[2] = { &iAmountToGive, &bForce };
		MonoObject* pResult = Mono::Instance().Invoke(AwardDiamonds, nullptr, pArgs);

		std::stringstream SS("AwardDiamonds = ");
		SS << std::hex << pResult << std::dec << " | Added " << iAmountToGive << " Diamonds";

		Utils::LogDebug(Utils::GetLocation(CurrentLoc), SS.str());
	}

public:
	GiveStuff() {}

	virtual bool Setup()
	{
		Initalized = true;
		return Initalized;
	}

	virtual void Destroy() {}

	virtual void HandleKeys() {}

	virtual void DrawMenuItems()
	{
		ImGui::SameLine();
		ImGui::BeginChild("#Diamonds", ImVec2(ImGui::GetContentRegionAvail().x, ImGui::GetContentRegionAvail().y / 2), ImGuiChildFlags_Border);
		{
			ImGui::Text("Diamonds");
			ImGui::InputInt("##Diamond Amount", &iAmount);
			if (ImGui::Button("Add Diamonds"))
				GiveDiamonds(iAmount);
		}
		ImGui::EndChild();
	}

	virtual void Render() {}

	virtual void Run() {}
};