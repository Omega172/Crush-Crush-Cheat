#pragma once
#include "pch.h"

// Colors for ImGui
inline ImU32 Black = ImGui::ColorConvertFloat4ToU32({ 0.f, 0.f, 0.f, 1.f });
inline ImU32 White = ImGui::ColorConvertFloat4ToU32({ 1.f, 1.f, 1.f, 1.f });

inline ImU32 Red = ImGui::ColorConvertFloat4ToU32({ 1.f, 0.f, 0.f, 1.f });
inline ImU32 Green = ImGui::ColorConvertFloat4ToU32({ 0.f, 1.f, 0.f, 1.f });
inline ImU32 Blue = ImGui::ColorConvertFloat4ToU32({ 0.f, 0.f, 1.f, 1.f });

inline ImU32 Cyan = ImGui::ColorConvertFloat4ToU32({ 0.f, 1.f, 1.f, 1.f });
inline ImU32 Gold = ImGui::ColorConvertFloat4ToU32({ 1.f, 0.84f, 0.f, 1.f });
inline ImU32 Orange = ImGui::ColorConvertFloat4ToU32({ 1.f, 0.65f, 0.f, 1.f });
inline ImU32 Purple = ImGui::ColorConvertFloat4ToU32({ 0.5f, 0.f, 0.5f, 1.f });
inline ImU32 Magenta = ImGui::ColorConvertFloat4ToU32({ 1.f, 0.f, 1.f, 1.f });

namespace GUI
{
	inline bool bMenuOpen = false;
	constexpr float WIDTH = 850;
	constexpr float HEIGHT = 550;

	inline float sWIDTH = float(GetSystemMetrics(SM_CXSCREEN));
	inline float sHEIGHT = float(GetSystemMetrics(SM_CYSCREEN));

	inline void BeginRender()
	{
		#if FRAMEWORK_RENDER_D3D11
		ImGui_ImplDX11_NewFrame();
		#endif

		#if FRAMEWORK_RENDER_D3D12
		ImGui_ImplDX12_NewFrame();
		#endif

		ImGui_ImplWin32_NewFrame();
		ImGui::NewFrame();
	}

	void Render();

	inline void EndRender()
	{
		ImGui::EndFrame();
		ImGui::Render();
	}
}