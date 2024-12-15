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

	void GivePurchases()
	{
		MonoMethod* AwardBlayFapItem = Mono::Instance().GetMethod("BlayFapClient", "AwardBlayFapItem", 2, "Assembly-CSharp", "BlayFap");
		if (!AwardBlayFapItem)
		{
			Utils::LogError(Utils::GetLocation(CurrentLoc), "Failed to get a pointer to AwardBlayFapItem");
			return;
		}

		MonoMethod* BlayFapClientInstance = Mono::Instance().GetMethod("BlayFapClient", "get_Instance", 0, "Assembly-CSharp", "BlayFap");
		MonoObject* Instance = Mono::Instance().Invoke(BlayFapClientInstance, nullptr, nullptr);
		if (!Instance)
		{
			Utils::LogError(Utils::GetLocation(CurrentLoc), "Failed to get an instance pointer to BlayFapClient");
			return;
		}

		std::cout << Instance << std::endl;
		for (int i = 0; i < BlayFapItems::MAX; i++)
		{
			void* pArgs[2] = { &i, nullptr };
			MonoObject* pResult = Mono::Instance().Invoke(AwardBlayFapItem, Instance, pArgs);

			std::stringstream SS("AwardBlayFapItem = ");
			SS << std::hex << pResult << std::dec << " | Added item: " << i;

			Utils::LogDebug(Utils::GetLocation(CurrentLoc), SS.str());
		}
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
			if (ImGui::Button("Add All Purchases"))
				GivePurchases();
		}
		ImGui::EndChild();
	}

	virtual void Render() {}

	virtual void Run() {}
};