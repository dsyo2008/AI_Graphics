﻿using UnityEngine;
using static AIGraphics.Inspector.Util;

namespace AIGraphics.Inspector
{
    internal class Inspector
    {
        private static Rect _windowRect;
        private readonly int _windowID = 0;
        private enum Tab { Lighting, Lights, PostProcessing, Presets, Settings };
        private Tab SelectedTab { get; set; }
        internal AIGraphics Parent { get; set; }

        internal Inspector(AIGraphics parent)
        {
            Parent = parent;
            _windowRect = new Rect(StartOffsetX, StartOffsetY, Width, Height);
        }

        internal static int Width
        {
            get => AIGraphics.ConfigWindowWidth.Value;
            set
            {
                AIGraphics.ConfigWindowWidth.Value = value;
                _windowRect.width = value;
            }
        }

        internal static int Height
        {
            get => AIGraphics.ConfigWindowHeight.Value;
            set
            {
                AIGraphics.ConfigWindowHeight.Value = value;
                _windowRect.height = value;
            }
        }

        internal static int StartOffsetX
        {
            get => AIGraphics.ConfigWindowOffsetX.Value;
            set => AIGraphics.ConfigWindowOffsetX.Value = value;
        }

        internal static int StartOffsetY
        {
            get => AIGraphics.ConfigWindowOffsetY.Value;
            set => AIGraphics.ConfigWindowOffsetY.Value = value;
        }

        internal void DrawWindow()
        {
            _windowRect = GUILayout.Window(_windowID, _windowRect, WindowFunction, "");
            EatInputInRect(_windowRect);
            StartOffsetX = (int)_windowRect.x;
            StartOffsetY = (int)_windowRect.y;
        }

        private void WindowFunction(int thisWindowID)
        {
            GUILayout.BeginVertical(GUIStyles.Skin.box);
            SelectedTab = Toolbar(SelectedTab);
            DrawTabs(SelectedTab);
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        private void DrawTabs(Tab tabSelected)
        {
            GUILayout.Space(10);
            switch (tabSelected)
            {
                case Tab.Lighting:
                    LightingInspector.Draw(Parent.LightingSettings, Parent.SkyboxManager, Parent.Settings.ShowAdvancedSettings);
                    break;
                case Tab.Lights:
                    LightInspector.Draw(Parent.Settings, Parent.LightManager, Parent.Settings.ShowAdvancedSettings);
                    break;
                case Tab.PostProcessing:
                    PostProcessingInspector.Draw(Parent.PostProcessingSettings, Parent.PostProcessingManager, Parent.FocusPuller, Parent.Settings.ShowAdvancedSettings);
                    break;
                case Tab.Presets:
                    PresetInspector.Draw(Parent.PresetManager);
                    break;
                case Tab.Settings:
                    SettingsInspector.Draw(Parent.CameraSettings, Parent.Settings, Parent.Settings.ShowAdvancedSettings);
                    break;
            }
        }

        private static void EatInputInRect(Rect eatRect)
        {
            if (eatRect.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)))
            {
                Input.ResetInputAxes();
            }
        }
    }
}
