

namespace Muc.Systems.Camera {

  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;
#if UNITY_EDITOR
  using UnityEditor;
#endif
  using Muc.Editor;
  using Muc.Extensions;

#if (MUC_HIDE_COMPONENTS || MUC_HIDE_SYSTEM_COMPONENTS)
  [AddComponentMenu("")]
#else
  [AddComponentMenu("MyUnityCollection/" + nameof(Muc.Systems.Camera) + "/" + nameof(MyUnityCameraDrag))]
#endif
  [RequireComponent(typeof(MyUnityCamera))]
  public class MyUnityCameraDrag : MonoBehaviour {

    public KeyCode key = KeyCode.Mouse2;


    public LayerMask mask;

    public bool raycastPlaneNormal;
    public Vector3 planeNormal = Vector3.up;

    public bool raycastPlanePoint;
    public Vector3 planePoint;


    MyUnityCamera pc;
    Vector3 rayOrigin;
    Vector3 prev;
    Plane plane => new Plane(planeNormal, planePoint);

    void Start() {
      pc = gameObject.GetComponent<MyUnityCamera>();
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected() {
      if (!Application.isPlaying || !Input.GetKey(key)) return;

      var startPoint = planePoint;
      var endPoint = planePoint + planeNormal * Mathf.Min(1, Vector3.Distance(planePoint, planePoint + planeNormal) * 0.8f);

      Handles.color = Color.red;
      DrawDirArrow(startPoint, endPoint);
    }

    void DrawDirArrow(Vector3 sourcePoint, Vector3 endPoint) {
      var dist = Vector3.Distance(endPoint, sourcePoint);
      if (dist < 0.0001f) return;
      var maxSize = 0.1f;
      var size = Mathf.Min(maxSize, dist / 10);
      Handles.DrawLine(sourcePoint, endPoint);
      Handles.ConeHandleCap(0, endPoint - ((endPoint - sourcePoint).normalized * size) * 0.7f, Quaternion.LookRotation(endPoint - sourcePoint), size, EventType.Repaint);
    }
#endif

    // Update is called once per frame
    void Update() {
      if (Input.GetKeyDown(key)) Init();
      if (Input.GetKey(key)) UpdateDrag();
    }

    /// <summary> Starts dragging. Can be called externally </summary> 
    public void Init() {
      rayOrigin = Camera.main.gameObject.transform.position;

      if (raycastPlaneNormal || raycastPlanePoint) {

        var ray = pc.cam.ScreenPointToRay(Input.mousePosition);
        ray.origin = rayOrigin;

        if (Physics.Raycast(ray, out var hit, mask)) {
          if (raycastPlaneNormal) planeNormal = hit.normal;
          if (raycastPlanePoint) planePoint = hit.point;
        }
      }

      GetMousePoint(plane, out prev);
    }

    /// <summary> Continue drag. Can be called externally </summary> 
    public void UpdateDrag() {
      if (!GetMousePoint(plane, out var current)) return;

      var dif = prev - current;
      pc.displacement += dif;
      prev = current;
    }


    private bool GetMousePoint(Plane plane, out Vector3 point) {
      var ray = pc.cam.ScreenPointToRay(Input.mousePosition);
      ray.origin = rayOrigin;

      var res = (plane.Raycast(ray, out float enter));
      if (res) point = ray.origin + ray.direction * enter;
      else point = Vector3.zero;
      return res;
    }
  }

}


#if UNITY_EDITOR
namespace Muc.Systems.Camera {

  using System.Reflection;
  using System.Collections;
  using System.Collections.Generic;

  using UnityEngine;
  using UnityEditor;
  using UnityEditorInternal;


  [CustomEditor(typeof(MyUnityCameraDrag))]
  internal class MyUnityCameraPlaneEditor : Editor {

    public override void OnInspectorGUI() {
      serializedObject.Update();

      var target = this.target as MyUnityCameraDrag;

      target.key = (KeyCode)EditorGUILayout.EnumPopup(new GUIContent(ObjectNames.NicifyVariableName(nameof(target.key))), target.key);

      // Normals
      target.raycastPlaneNormal = EditorGUILayout.Toggle(new GUIContent(ObjectNames.NicifyVariableName(nameof(target.raycastPlaneNormal))), target.raycastPlaneNormal);
      if (!target.raycastPlaneNormal) target.planeNormal = EditorGUILayout.Vector3Field(new GUIContent(ObjectNames.NicifyVariableName(nameof(target.planeNormal))), target.planeNormal);


      // Points
      target.raycastPlanePoint = EditorGUILayout.Toggle(new GUIContent(ObjectNames.NicifyVariableName(nameof(target.raycastPlanePoint))), target.raycastPlanePoint);
      if (!target.raycastPlanePoint) target.planePoint = EditorGUILayout.Vector3Field(new GUIContent(ObjectNames.NicifyVariableName(nameof(target.planePoint))), target.planePoint);

      // Shared
      if (target.raycastPlaneNormal || target.raycastPlanePoint) {
        LayerMask tempMask = EditorGUILayout.MaskField(InternalEditorUtility.LayerMaskToConcatenatedLayersMask(target.mask), InternalEditorUtility.layers);
        target.mask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);
      }


      serializedObject.ApplyModifiedProperties();
    }

  }
}
#endif