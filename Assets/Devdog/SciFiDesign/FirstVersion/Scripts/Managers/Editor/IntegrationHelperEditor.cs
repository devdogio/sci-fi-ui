using UnityEngine;
using System.Collections;
using UnityEditor;
using Devdog.General.Editors;

namespace Devdog.SciFiDesign.Editors
{
    public class IntegrationHelperEditor : IntegrationHelperEditorBase
    {
        [MenuItem(SciFiDesignConstants.ToolsMenuPath + "Integrations", false, 0)]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<IntegrationHelperEditor>(true, "Integrations", true);
        }

        protected override void DrawIntegrations()
        {

            ShowIntegration("Inventory Pro", "Inventory Pro is a highly flexible and easy to use inventory, that can be used for all game types. ", GetUrlForProductWithID("66801"), "INVENTORY_PRO");
            ShowIntegration("Inventory Pro Legacy", "Using V2.4 or older? Use enable the legacy version. (when using legacy check Inventory pro AND legacy)", GetUrlForProductWithID("31226"), "INVENTORY_PRO_LEGACY");

        }
    }
}