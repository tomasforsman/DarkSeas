using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using DarkSeas.Gameplay.Boat;
using DarkSeas.Gameplay.Interaction;
using DarkSeas.Data;
using DarkSeas.UI;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
            var dockScript = dock.AddComponent<DarkSeas.Meta.Harbor.HarborDock>();
            var dockCol = dock.GetComponent<Collider>();
            if (dockCol != null) dockCol.isTrigger = true;

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

            // UI Canvas
            var canvasGO = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var canvas = canvasGO.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));

            // Legacy HUD text (top-right)
            var legacyTextGO = new GameObject("LegacyText", typeof(Text), typeof(LegacyHUD));
            legacyTextGO.transform.SetParent(canvasGO.transform, false);
            var legacyText = legacyTextGO.GetComponent<Text>();
            legacyText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            legacyText.alignment = TextAnchor.UpperRight;
            legacyText.fontSize = 18;
            legacyText.rectTransform.anchorMin = new Vector2(1, 1);
            legacyText.rectTransform.anchorMax = new Vector2(1, 1);
            legacyText.rectTransform.pivot = new Vector2(1, 1);
            legacyText.rectTransform.anchoredPosition = new Vector2(-12, -12);
            legacyText.text = "Legacy: 0";

            // Delivery toast (bottom-center)
            var toastGO = new GameObject("DeliveryToast", typeof(CanvasGroup), typeof(Text), typeof(DeliveryToast));
            toastGO.transform.SetParent(canvasGO.transform, false);
            var toastText = toastGO.GetComponent<Text>();
            toastText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            toastText.alignment = TextAnchor.LowerCenter;
            toastText.fontSize = 20;
            toastText.rectTransform.anchorMin = new Vector2(0.5f, 0);
            toastText.rectTransform.anchorMax = new Vector2(0.5f, 0);
            toastText.rectTransform.pivot = new Vector2(0.5f, 0);
            toastText.rectTransform.anchoredPosition = new Vector2(0, 24);

            // Debrief panel
            var debriefGO = new GameObject("DebriefPanel", typeof(CanvasGroup), typeof(DebriefPanel));
            debriefGO.transform.SetParent(canvasGO.transform, false);
            var group = debriefGO.GetComponent<CanvasGroup>();
            group.alpha = 0f;
            var panelRT = debriefGO.AddComponent<Image>().rectTransform;
            panelRT.sizeDelta = new Vector2(360, 220);
            panelRT.anchoredPosition = Vector2.zero;

            // Title
            Text MakeText(string name, Vector2 pos, int size, TextAnchor align)
            {
                var go = new GameObject(name, typeof(Text));
                go.transform.SetParent(debriefGO.transform, false);
                var t = go.GetComponent<Text>();
                t.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                t.alignment = align;
                t.fontSize = size;
                t.rectTransform.anchoredPosition = pos;
                return t;
            }

            var title = MakeText("Title", new Vector2(0, 70), 24, TextAnchor.MiddleCenter);
            var rescued = MakeText("Rescued", new Vector2(0, 20), 18, TextAnchor.MiddleCenter);
            var earned = MakeText("Earned", new Vector2(0, -10), 18, TextAnchor.MiddleCenter);
            var total = MakeText("Total", new Vector2(0, -40), 18, TextAnchor.MiddleCenter);

            // Continue button
            var buttonGO = new GameObject("ContinueButton", typeof(Image), typeof(Button));
            buttonGO.transform.SetParent(debriefGO.transform, false);
            var btnRT = buttonGO.GetComponent<RectTransform>();
            btnRT.sizeDelta = new Vector2(160, 40);
            btnRT.anchoredPosition = new Vector2(0, -80);
            var btnText = MakeText("ContinueText", new Vector2(0, -80), 18, TextAnchor.MiddleCenter);
            btnText.text = "Continue";
            btnText.transform.SetParent(buttonGO.transform, true);

            var debrief = debriefGO.GetComponent<DebriefPanel>();
            var soDebrief = new SerializedObject(debrief);
            soDebrief.FindProperty("_group").objectReferenceValue = group;
            soDebrief.FindProperty("_title").objectReferenceValue = title;
            soDebrief.FindProperty("_rescuedText").objectReferenceValue = rescued;
            soDebrief.FindProperty("_earnedText").objectReferenceValue = earned;
            soDebrief.FindProperty("_totalText").objectReferenceValue = total;
            soDebrief.FindProperty("_continueButton").objectReferenceValue = buttonGO.GetComponent<Button>();
            // Attach a RunStateMachine to the scene root
            var runSM = new GameObject("RunStateMachine").AddComponent<DarkSeas.Gameplay.Run.RunStateMachine>();
            soDebrief.FindProperty("_runStateMachine").objectReferenceValue = runSM;
            soDebrief.ApplyModifiedProperties();

            // Save
            System.IO.Directory.CreateDirectory("Assets/_DarkSeas/Scenes/Test");
            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"DarkSeas test scene created at {ScenePath}");
        }
    }
}
