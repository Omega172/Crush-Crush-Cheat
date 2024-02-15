#pragma once
#include "pch.h"
#define IMGUI_DEFINE_MATH_OPERATORS
#include "../ImGui/imgui_internal.h"

#include <algorithm>
#include <array>
#include <string_view>

// I have no clue where I got this from but I fixed it up a bit to work with newer changes to the ImGui API, and cleaned it up a bit
class KeyBind
{
private:
    ImGuiKey key;

public:
    explicit KeyBind(ImGuiKey _key = ImGuiKey_None) noexcept : key{ _key } {};

    const char* ToString() const noexcept { return ImGui::GetKeyName(key); };

    bool IsPressed(bool repeat = false) const noexcept
    {
        if (!IsSet())
            return false;

        return ImGui::IsKeyPressed(key, repeat);
    }

    bool IsDown() const noexcept
    {
        if (!IsSet())
            return false;

        return ImGui::IsKeyDown(key);
    }

    bool IsSet() const noexcept { return key != ImGuiKey_None; }

    void Set(ImGuiKey _key) { key = _key; }

    bool Set() noexcept
    {
        for (int i = ImGuiKey_NamedKey_BEGIN; i < ImGuiKey_NamedKey_END; ++i) {
            ImGuiKey _key = static_cast<ImGuiKey>(i);
            if (!ImGui::IsKeyDown(_key, false))
                continue;

            key = _key;
            return true;
        }

        return false;
    }
};

class KeyBindToggle : public KeyBind {
public:
    using KeyBind::KeyBind;

    inline void HandleToggle() noexcept;
    bool IsToggled() const noexcept { return toggledOn; }
private:
    bool toggledOn = false;
};

inline void KeyBindToggle::HandleToggle() noexcept
{
    if (IsPressed())
        toggledOn = !toggledOn;
}

namespace ImGui
{
    inline void Hotkey(const char* label, KeyBind& key, bool* setting, const ImVec2& size = { 100.0f, 0.0f }) noexcept
    {
        const auto id = ImGui::GetID(label);
        ImGui::PushID(label);

        std::string BtnName = (*setting) ? "..." : key.ToString();

        if (ImGui::GetActiveID() == id) {
            ImGui::PushStyleColor(ImGuiCol_Button, ImGui::GetColorU32(ImGuiCol_ButtonActive));
            ImGui::Button("...", size);
            ImGui::PopStyleColor();

            ImGui::GetCurrentContext()->ActiveIdAllowOverlap = true;
            if ((!ImGui::IsItemHovered() && (ImGui::GetIO().MouseClicked[0]) || key.Set()))
            {
                ImGui::ClearActiveID();
                *setting = false;
            }
        }
        else if (ImGui::Button(BtnName.c_str(), size) || *setting) {
            ImGui::SetActiveID(id, ImGui::GetCurrentWindow());
            *setting = true;
        }

        ImGui::PopID();
    }

    inline void OutlinedText(ImVec2 Pos, ImU32 Color, std::string Text)
    {
        ImU32 Black = ImGui::ColorConvertFloat4ToU32({ 0.f, 0.f, 0.f, 1.f });
        auto* pDL = ImGui::GetBackgroundDrawList();

        pDL->AddText(tahomaFontESP, tahomaFontESP->FontSize, Pos + ImVec2(-1, -1), Black, Text.c_str());
        pDL->AddText(tahomaFontESP, tahomaFontESP->FontSize, Pos + ImVec2(1, -1), Black, Text.c_str());
        pDL->AddText(tahomaFontESP, tahomaFontESP->FontSize, Pos + ImVec2(-1, 1), Black, Text.c_str());
        pDL->AddText(tahomaFontESP, tahomaFontESP->FontSize, Pos + ImVec2(1, 1), Black, Text.c_str());

        pDL->AddText(tahomaFontESP, tahomaFontESP->FontSize, Pos, Color, Text.c_str());
    }
}