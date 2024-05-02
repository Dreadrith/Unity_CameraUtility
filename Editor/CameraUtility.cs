using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DreadScripts.CameraUtility
{
    public class CameraUtility
    {
        private const float SCENE_PIVOT_OFFSET = 1;

        [MenuItem("DreadTools/Utility/Camera/Snap Scene To Game")]
        public static void SnapSceneViewToGame()
        {
            if (!TryGetGameCamera(out Camera gc)) return;

            SceneView view = SceneView.lastActiveSceneView;
            if (YellowLog(view == null, "No Scene View found")) return;

            Undo.RecordObject(view, "Snap STG");
            view.LookAtDirect(gc.transform.position, gc.transform.rotation, SCENE_PIVOT_OFFSET/2);
            view.pivot = gc.transform.position + gc.transform.forward * SCENE_PIVOT_OFFSET;
        }

        [MenuItem("DreadTools/Utility/Camera/Snap Game To Scene")]
        public static void SnapGameViewToScene()
        {
            if (!TryGetGameCamera(out Camera gc)) return;
            if (!TryGetSceneCamera(out Camera sc)) return;

            Undo.RecordObject(gc.transform, "Snap GTS");
            gc.transform.SetPositionAndRotation(sc.transform.position, sc.transform.rotation);
        }

        private static bool TryGetGameCamera(out Camera gameCamera)
        {
            gameCamera = Camera.main ?? Object.FindObjectOfType<Camera>();
            return !YellowLog(!gameCamera, "No Camera found in scene");
        }
        private static bool TryGetSceneCamera(out Camera sceneCamera)
        {
            sceneCamera = SceneView.lastActiveSceneView?.camera;
            return !YellowLog(!sceneCamera, "No Scene View found");
        }

        private static bool YellowLog(bool condition, string msg)
        {
            if (condition) Debug.LogWarning($"<color=yellow>[CameraUtility] {msg}</color>");
            return condition;
        }
    }
}
