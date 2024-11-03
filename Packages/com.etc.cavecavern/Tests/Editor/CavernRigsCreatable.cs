using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace ETC.CaveCavern.Tests
{
    [TestFixture]
    [Description("Verifies that the Cavern Rig prefab can be created from the game object menu")]
    public class CavernRigsCreatable
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // propAudioContainer = ScriptableObject.CreateInstance<PropAudioContainer>();
            // const string assetPath = "Assets/NewScriptableObject.asset";
            // AssetDatabase.CreateAsset(propAudioContainer, assetPath);
            // AssetDatabase.SaveAssets();
            // _containerGUID = AssetDatabase.GUIDFromAssetPath(assetPath);

            // EditorUtility.FocusProjectWindow();

            // Selection.activeObject = propAudioContainer;
        }



        [Test]
        public void CanCreateRigsFromContextMenu()
        {
            // Run the CreateCavernRigsPrefab.CreateCavernMainRig method, and if any errors are thrown, fail the test
            try
            {
                CreateCavernRigsPrefab.CreateCavernMainRig();
            }
            catch (Exception e)
            {
                Assert.Fail($"Failed to create Cavern Main Rig prefab: {e.Message}");
            }
        }
    }
}
