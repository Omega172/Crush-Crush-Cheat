#pragma once
#include "imgui.h"

inline ImFont* defaultFont; // The default font is this even needed?
inline ImFont* tahomaFont;
inline ImFont* tahomaFontESP; // A font with extra spacing for ESP

inline void SetupStyle()
{
	ImGuiStyle& style = ImGui::GetStyle();

	// Main
	style.WindowPadding = ImVec2(8, 8);
	style.FramePadding = ImVec2(4, 3);
	style.ItemSpacing = ImVec2(8, 4);
	style.ItemInnerSpacing = ImVec2(4, 4);
	style.TouchExtraPadding = ImVec2(0, 0);
	style.IndentSpacing = 15;
	style.ScrollbarSize = 15;

	// Borders
	style.WindowBorderSize = 1;
	style.ChildBorderSize = 1;
	style.PopupBorderSize = 1;
	style.FrameBorderSize = 1;
	style.TabBorderSize = 1;

	// Rounding
	style.WindowRounding = 0;
	style.ChildRounding = 0;
	style.FrameRounding = 0;
	style.PopupRounding = 0;
	style.ScrollbarRounding = 0;
	style.GrabRounding = 0;
	style.TabRounding = 0;

	// Alignment
	style.WindowTitleAlign = ImVec2(0.0, 0.50);
	style.WindowMenuButtonPosition = ImGuiDir_Right;
	style.ColorButtonPosition = ImGuiDir_Right;
	style.ButtonTextAlign = ImVec2(0.50, 0.50);
	style.SelectableTextAlign = ImVec2(0.0, 0.0);

	// Safe Area Padding
	style.DisplaySafeAreaPadding = ImVec2(3, 3);

	// Colors
	ImVec4* colors = ImGui::GetStyle().Colors;
	colors[ImGuiCol_Text] = ImVec4(1.00f, 1.00f, 1.00f, 1.00f);
	colors[ImGuiCol_TextDisabled] = ImVec4(0.62f, 0.62f, 0.62f, 0.50f);
	colors[ImGuiCol_WindowBg] = ImVec4(0.06f, 0.06f, 0.06f, 1.00f);
	colors[ImGuiCol_ChildBg] = ImVec4(0.13f, 0.13f, 0.13f, 1.00f);
	colors[ImGuiCol_PopupBg] = ImVec4(0.08f, 0.08f, 0.08f, 0.94f);
	colors[ImGuiCol_Border] = ImVec4(0.62f, 0.62f, 0.62f, 0.50f);
	colors[ImGuiCol_BorderShadow] = ImVec4(0.00f, 0.00f, 0.00f, 0.00f);
	colors[ImGuiCol_FrameBg] = ImVec4(0.61f, 0.39f, 0.66f, 0.54f);
	colors[ImGuiCol_FrameBgHovered] = ImVec4(0.83f, 0.00f, 1.00f, 1.00f);
	colors[ImGuiCol_FrameBgActive] = ImVec4(0.83f, 0.00f, 1.00f, 0.50f);
	colors[ImGuiCol_TitleBg] = ImVec4(0.04f, 0.04f, 0.04f, 1.00f);
	colors[ImGuiCol_TitleBgActive] = ImVec4(0.04f, 0.04f, 0.04f, 1.00f);
	colors[ImGuiCol_TitleBgCollapsed] = ImVec4(0.04f, 0.04f, 0.04f, 1.00f);
	colors[ImGuiCol_MenuBarBg] = ImVec4(0.14f, 0.14f, 0.14f, 1.00f);
	colors[ImGuiCol_ScrollbarBg] = ImVec4(0.02f, 0.02f, 0.02f, 0.53f);
	colors[ImGuiCol_ScrollbarGrab] = ImVec4(0.31f, 0.31f, 0.31f, 1.00f);
	colors[ImGuiCol_ScrollbarGrabHovered] = ImVec4(0.41f, 0.41f, 0.41f, 1.00f);
	colors[ImGuiCol_ScrollbarGrabActive] = ImVec4(0.51f, 0.51f, 0.51f, 1.00f);
	colors[ImGuiCol_CheckMark] = ImVec4(0.83f, 0.00f, 1.00f, 1.00f);
	colors[ImGuiCol_SliderGrab] = ImVec4(0.62f, 0.62f, 0.62f, 0.50f);
	colors[ImGuiCol_SliderGrabActive] = ImVec4(0.73f, 0.73f, 0.73f, 0.50f);
	colors[ImGuiCol_Button] = ImVec4(0.62f, 0.38f, 0.66f, 0.54f);
	colors[ImGuiCol_ButtonHovered] = ImVec4(0.84f, 0.00f, 1.00f, 1.00f);
	colors[ImGuiCol_ButtonActive] = ImVec4(0.84f, 0.00f, 1.00f, 0.54f);
	colors[ImGuiCol_Header] = ImVec4(0.62f, 0.38f, 0.66f, 0.54f);
	colors[ImGuiCol_HeaderHovered] = ImVec4(0.84f, 0.00f, 1.00f, 1.00f);
	colors[ImGuiCol_HeaderActive] = ImVec4(0.84f, 0.00f, 1.00f, 0.54f);
	colors[ImGuiCol_Separator] = ImVec4(0.43f, 0.43f, 0.50f, 0.50f);
	colors[ImGuiCol_SeparatorHovered] = ImVec4(0.84f, 0.00f, 1.00f, 1.00f);
	colors[ImGuiCol_SeparatorActive] = ImVec4(0.84f, 0.00f, 1.00f, 0.54f);
	colors[ImGuiCol_ResizeGrip] = ImVec4(0.62f, 0.38f, 0.66f, 0.54f);
	colors[ImGuiCol_ResizeGripHovered] = ImVec4(0.84f, 0.00f, 1.00f, 1.00f);
	colors[ImGuiCol_ResizeGripActive] = ImVec4(0.84f, 0.00f, 1.00f, 0.54f);
	colors[ImGuiCol_Tab] = ImVec4(0.62f, 0.38f, 0.66f, 0.54f);
	colors[ImGuiCol_TabHovered] = ImVec4(0.84f, 0.00f, 1.00f, 1.00f);
	colors[ImGuiCol_TabActive] = ImVec4(0.84f, 0.00f, 1.00f, 0.54f);
	colors[ImGuiCol_TabUnfocused] = ImVec4(0.24f, 0.17f, 0.24f, 0.97f);
	colors[ImGuiCol_TabUnfocusedActive] = ImVec4(0.29f, 0.19f, 0.31f, 1.00f);
	colors[ImGuiCol_PlotLines] = ImVec4(1.00f, 1.00f, 1.00f, 1.00f);
	colors[ImGuiCol_PlotLinesHovered] = ImVec4(0.84f, 0.00f, 1.00f, 1.00f);
	colors[ImGuiCol_PlotHistogram] = ImVec4(0.91f, 0.56f, 0.97f, 1.00f);
	colors[ImGuiCol_PlotHistogramHovered] = ImVec4(0.11f, 0.00f, 1.00f, 1.00f);
	colors[ImGuiCol_TextSelectedBg] = ImVec4(0.84f, 0.00f, 1.00f, 0.54f);
	colors[ImGuiCol_DragDropTarget] = ImVec4(0.91f, 0.63f, 0.97f, 1.00f);
	colors[ImGuiCol_NavHighlight] = ImVec4(0.62f, 0.38f, 0.66f, 0.54f);
	colors[ImGuiCol_NavWindowingHighlight] = ImVec4(1.00f, 1.00f, 1.00f, 0.70f);
	colors[ImGuiCol_NavWindowingDimBg] = ImVec4(0.80f, 0.80f, 0.80f, 0.20f);
	colors[ImGuiCol_ModalWindowDimBg] = ImVec4(0.80f, 0.80f, 0.80f, 0.35f);

}

inline void ImportFonts()
{
	ImGuiIO& io = ImGui::GetIO();
	defaultFont = io.Fonts->AddFontDefault();
	tahomaFont = io.Fonts->AddFontFromFileTTF("C:\\Windows\\Fonts\\Tahoma.ttf", 14.0f);

	ImFontConfig Config;
	Config.GlyphExtraSpacing.x = 1.f;
	tahomaFontESP = io.Fonts->AddFontFromFileTTF("C:\\Windows\\Fonts\\Tahoma.ttf", 14.0f, &Config);
}