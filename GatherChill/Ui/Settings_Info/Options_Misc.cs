using Dalamud.Interface.Utility;
using ECommons.Logging;
using GatherChill.Ui.RouteWindowTabs;

namespace GatherChill.Ui.Settings_Info;

public static partial class SettingsUi
{
    private static readonly string MiscOption = "Misc Options";
    private static SettingEntry ColorTheme = new()
    {
        Label = "Use Custom Theme",
        Category = MiscOption,
        Keywords = new[] { "Theme", "Color", "Ice Theme" },
        Draw = () =>
        {
            var v = C.UseIceTheme;
            if (ImGui.Checkbox("Use Ice Theme", ref v))
            {
                C.UseIceTheme = v;
                C.Save();
            }
        }
    };

    private static SettingEntry CompactOverlay = new()
    {
        Label = "Compact Sidebar",
        Category = MiscOption,
        Keywords = new[] { "Main Window", "Main", "Window", "Compact" },
        Draw = () =>
        {
            var v = C.Compact_Ui;
            if (ImGui.Checkbox("Compact Tabs on Sidebar", ref v))
            {
                C.Compact_Ui = v;
                C.Save();
            }
        }
    };

    private static SettingEntry Overlay_AutoShow = new()
    {
        Label = "Open Overlay upon start",
        Category = MiscOption,
        Keywords = new[] { "Overlay", "Auto Open", "Start" },
        Draw = () =>
        {
            var v = C.Overlay_OpenUponRun;
            if (ImGui.Checkbox("Open overlay when running", ref v))
            {
                C.Overlay_OpenUponRun = v;
                C.Save();
            }
        }
    };

    private static SettingEntry Overlay_AutoClose = new()
    {
        Label = "Close Overlay upon idle",
        Category = MiscOption,
        Keywords = new[] { "Overlay", "Auto Open", "Start" },
        Draw = () =>
        {
            var v = C.Overlay_CloseUponStop;
            if (ImGui.Checkbox("Close overlay when not running", ref v))
            {
                C.Overlay_CloseUponStop = v;
                C.Save();
            }
        }
    };

    private static SettingEntry SafetyGatherDelay = new()
    {
        Label = "Safety Delay [Gather]",
        Category = MiscOption,
        Keywords = new[] { "Safety", "Delay", "Gathering" },
        Draw = () =>
        {
            var v = C.SafetyDelay_Gather;
            if (ImGui.Checkbox("Add a delay between gathering attempts", ref v))
            {
                C.SafetyDelay_Gather = v;
                C.Save();
            }

            if (v)
            {
                var d = C.Gather_DelayTimer;
                ImGui.SetNextItemWidth(150 * ImGuiHelpers.GlobalScale);
                if (ImGui.SliderInt("Gathering delay", ref d, 250, 2500))
                {
                    C.Gather_DelayTimer = d;
                    C.SaveDebounced();
                }
            }
        }
    };

    private static void DrawRandomDelay(string checkboxLabel, string id, bool enabled, Action<bool> setEnabled, int min, Action<int> setMin, int max, Action<int> setMax)
    {
        if (ImGui.Checkbox(checkboxLabel, ref enabled))
        {
            setEnabled(enabled);
            C.Save();
        }

        if (!enabled)
            return;

        ImGui.SetNextItemWidth(150 * ImGuiHelpers.GlobalScale);
        if (ImGui.SliderInt($"Min (ms)##{id}", ref min, 0, 5000))
        {
            setMin(min);
            if (max < min)
                setMax(min);
            C.SaveDebounced();
        }

        ImGui.SetNextItemWidth(150 * ImGuiHelpers.GlobalScale);
        if (ImGui.SliderInt($"Max (ms)##{id}", ref max, 0, 5000))
        {
            setMax(max);
            if (min > max)
                setMin(max);
            C.SaveDebounced();
        }
    }

    private static SettingEntry RandomMountDelay = new()
    {
        Label = "Random Delay Mount",
        Category = MiscOption,
        Keywords = new[] { "Safety", "Delay", "Random", "Mount", "Human" },
        Draw = () => DrawRandomDelay("Random delay before mounting", "MountDelay",
            C.RandomDelay_Mount, v => C.RandomDelay_Mount = v,
            C.MountDelay_Min, v => C.MountDelay_Min = v,
            C.MountDelay_Max, v => C.MountDelay_Max = v)
    };

    private static SettingEntry RandomNodeDelay = new()
    {
        Label = "Random Delay Node",
        Category = MiscOption,
        Keywords = new[] { "Safety", "Delay", "Random", "Node", "Interact", "Human" },
        Draw = () => DrawRandomDelay("Random delay before opening a node", "NodeDelay",
            C.RandomDelay_Node, v => C.RandomDelay_Node = v,
            C.NodeDelay_Min, v => C.NodeDelay_Min = v,
            C.NodeDelay_Max, v => C.NodeDelay_Max = v)
    };
}
