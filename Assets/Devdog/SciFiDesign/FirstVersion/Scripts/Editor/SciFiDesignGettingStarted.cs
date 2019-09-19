using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Devdog.General.Editors;
using UnityEditor.Callbacks;

namespace Devdog.SciFiDesign.Editors
{
    [InitializeOnLoad]
    public class SciFiDesignGettingStarted : GettingStartedEditorBase
    {
        private const string MenuItemPath = SciFiDesignConstants.ToolsMenuPath + "Getting started";
        private static bool _doInit = false;

        public SciFiDesignGettingStarted()
        {
            version = SciFiDesignConstants.Version;
            productName = SciFiDesignConstants.ProductName;
            documentationUrl = SciFiDesignConstants.ProductUrl;
            productsFetchApiUrl = "http://devdog.io/unity/editorproducts.php?product=" + SciFiDesignConstants.ProductName;
            reviewProductUrl = "https://www.assetstore.unity3d.com/en/#!/content/55270";
        }

        static SciFiDesignGettingStarted()
        {
            // Init
            _doInit = true;
        }

        private void OnEnable()
        {
            if (_doInit)
            {
                if (EditorPrefs.GetBool(editorPrefsKey))
                {
                    ShowWindowInternal();
                }
            }

            _doInit = false;
        }

        [MenuItem(MenuItemPath, false, 1)] // Always at bottom
        protected static void ShowWindowInternal()
        {
            window = GetWindow<SciFiDesignGettingStarted>();
            window.GetImages();
            window.ShowUtility();
        }

        public override void ShowWindow()
        {
            ShowWindowInternal();
        }

        protected override void DrawGettingStarted()
        {
            DrawBox(0, 0, "Documentation", "The official documentation has a detailed description of all components and code examples.", documentationIcon, () =>
            {
                Application.OpenURL(documentationUrl);
            });

            DrawBox(1, 0, "Discord", "Join the community on Discord for support.", discordIcon, () =>
            {
                Application.OpenURL(discordUrl);
            });

            DrawBox(2, 0, "Integrations", "Combine the power of assets and enable integrations.", integrationsIcon, () =>
            {
                IntegrationHelperEditor.ShowWindow();
            });

            DrawBox(3, 0, "Rate / Review", "Like " + productName + "? Share the experience :)", reviewIcon, () =>
            {
                Application.OpenURL(reviewProductUrl);
            });

            base.DrawGettingStarted();
        }
    }
}