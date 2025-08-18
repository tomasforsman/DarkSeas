using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using DarkSeas.Gameplay.Boat;
using DarkSeas.Gameplay.Interaction;
using DarkSeas.Data;

namespace DarkSeas.Editor
{
    public static class CreateTestScene
    {
        private const string ScenePath = "Assets/_DarkSeas/Scenes/Test/TestScene.unity";

        [MenuItem("DarkSeas/Create Test Scene")]
        public static void Create()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = "TestScene";

            // Lighting
            var lightGO = new GameObject("Directional Light");
            var light = lightGO.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.0f;
            lightGO.transform.rotation = Quaternion.Euler(50, -30, 0);

            // Water plane
            var water = GameObject.CreatePrimitive(PrimitiveType.Plane);
            water.name = "Water";
            water.transform.localScale = new Vector3(10, 1, 10);

            // Harbor dock marker
            var dock = GameObject.CreatePrimitive(PrimitiveType.Cube);
            dock.name = "HarborDock";
            dock.transform.position = new Vector3(0, 0.5f, -40);
            dock.transform.localScale = new Vector3(4, 1, 8);

            // Boat
            var boat = new GameObject("Boat");
            boat.transform.position = new Vector3(0, 0.5f, 0);
            var rb = boat.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            boat.AddComponent<CapsuleCollider>();

            var fuel = boat.AddComponent<BoatFuel>();
            var controller = boat.AddComponent<BoatController>();
            var interactor = boat.AddComponent<RescueInteractor>();

            // Assign configs from Resources
            var boatConfig = Resources.Load<BoatConfig>("DefaultBoatConfig");
            if (boatConfig != null)
            {
                controller.SetBoatConfig(boatConfig);
                // mirror seats into interactor until meta/harbor systems wire this
                interactor.SetMaxPassengers(Mathf.Max(1, boatConfig.seats));
            }

            // Try to assign Input Actions asset if present on disk
            var inputAsset = AssetDatabase.LoadAssetAtPath<InputActionAsset>("Assets/InputSystem_Actions.inputactions");
            if (inputAsset != null)
            {
                // Assign to components via serialized objects (fields are private)
                var soCtrl = new SerializedObject(controller);
                soCtrl.FindProperty("_inputActions").objectReferenceValue = inputAsset;
                soCtrl.ApplyModifiedProperties();

                var soInteract = new SerializedObject(interactor);
                soInteract.FindProperty("_inputActions").objectReferenceValue = inputAsset;
                soInteract.ApplyModifiedProperties();
            }

            // Rescue target
            var rescue = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            rescue.name = "RescueTarget";
            rescue.transform.position = new Vector3(8, 0.5f, 8);
            rescue.AddComponent<RescueTarget>();

            // Save
            System.IO.Directory.CreateDirectory("Assets/_DarkSeas/Scenes/Test");
            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"DarkSeas test scene created at {ScenePath}");
        }
    }
}

