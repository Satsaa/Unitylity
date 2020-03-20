using UnityEngine;
using Unity.Mathematics;
using MUC.Types.Extensions;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace MUC.Components {

  public class DisplayRect : MonoBehaviour {
    public Rect rect;
    public Color color = Color.magenta;
    [Range(0, 1)]
    public float fillAlpha = 0.2f;
    public bool useExperimentalHandles = false;

    void OnDrawGizmos() {
      Gizmos.color = color;
      Gizmos.DrawWireCube(rect.center, rect.size);

      if (fillAlpha == 0) return;
      Gizmos.color = color * fillAlpha;
      Gizmos.DrawCube(rect.center, rect.size);
    }
  }


#if UNITY_EDITOR
  [CustomEditor(typeof(DisplayRect), true)]
  public class DisplayRectEditor : Editor {

    private DisplayRect t { get => (DisplayRect)target; }

    private Camera cam { get => Camera.current; }

#pragma warning disable CS0414
    private bool mouse = false;
    private Vector2 mousePos;
    private bool shift { get => Event.current.shift; }

    private bool control { get => controlRight || controlLeft; }
    private bool controlRight = false;
    private bool controlLeft = false;

    private bool alt { get => altRight || altLeft; }
    private bool altRight = false;
    private bool altLeft = false;

    private Vector2 prevTarget;

    void OnSceneGUI() {
      switch (Event.current.type) {
        case EventType.Repaint:
          break;

        case EventType.MouseMove:
          mousePos = Event.current.mousePosition;
          break;

        case EventType.MouseDown:
          if (Event.current.button == 0) mouse = true;
          break;

        case EventType.MouseUp:
          if (Event.current.button == 0) mouse = false;
          break;

        case EventType.KeyDown:
          if (Event.current.keyCode == KeyCode.RightAlt) altRight = true;
          else if (Event.current.keyCode == KeyCode.LeftAlt) altLeft = true;

          if (Event.current.keyCode == KeyCode.RightControl) controlRight = true;
          else if (Event.current.keyCode == KeyCode.LeftControl) controlLeft = true;
          break;

        case EventType.KeyUp:
          if (Event.current.keyCode == KeyCode.RightAlt) altRight = false;
          else if (Event.current.keyCode == KeyCode.LeftAlt) altLeft = false;

          if (Event.current.keyCode == KeyCode.RightControl) controlRight = false;
          else if (Event.current.keyCode == KeyCode.LeftControl) controlLeft = false;
          break;
      }
      Draw();
    }

    void Draw() {

      var botRight = new Vector2(t.rect.xMax, t.rect.yMin);
      var botLeft = new Vector2(t.rect.xMin, t.rect.yMin);
      var topRight = new Vector2(t.rect.xMax, t.rect.yMax);
      var topLeft = new Vector2(t.rect.xMin, t.rect.yMax);

      if (t.useExperimentalHandles) {
        var ray = cam.ScreenPointToRay(new Vector2(mousePos.x, cam.pixelHeight - mousePos.y));
        var plane = new Plane(Vector3.forward, 0);
        if (plane.Raycast(ray, out var distance)) {

          var target = ray.GetPoint(distance).xy();
          var diff = target - prevTarget.Add(0.01f);

          var size = math.min(t.rect.size.x, t.rect.size.y) / 10;

          if (Handles.Button(topLeft.AddX(size).AddY(-size), Quaternion.identity, size, size, Handles.RectangleHandleCap)) {
            Undo.RegisterCompleteObjectUndo(t, "Modify rect");
            t.rect.xMin += diff.x;
            t.rect.yMin += diff.y;
            Dirty();
          }
          if (Handles.Button(topRight.AddX(-size).AddY(-size), Quaternion.identity, size, size, Handles.RectangleHandleCap)) {

            Undo.RegisterCompleteObjectUndo(t, "Modify rect");
            t.rect.xMax += diff.x;
            t.rect.yMin += diff.y;
            Dirty();

          }

          if (Handles.Button(botLeft.AddX(size).AddY(size), Quaternion.identity, size, size, Handles.RectangleHandleCap)) {
            Undo.RegisterCompleteObjectUndo(t, "Modify rect");
            t.rect.xMin += diff.x;
            t.rect.yMax += diff.y;
            Dirty();
          }
          if (Handles.Button(botRight.AddX(-size).AddY(size), Quaternion.identity, size, size, Handles.RectangleHandleCap)) {
            Undo.RegisterCompleteObjectUndo(t, "Modify rect");
            t.rect.xMax += diff.x;
            t.rect.yMax += diff.y;
            Dirty();
          }

          prevTarget = target;
        }

      } else {

        EditorGUI.BeginChangeCheck();
        float3 newBotRight = Handles.PositionHandle(botRight, Quaternion.identity);

        if (EditorGUI.EndChangeCheck()) {
          Undo.RegisterCompleteObjectUndo(t, "Modify rect");
          t.rect.xMax = newBotRight.x;
          t.rect.yMin = newBotRight.y;
          Dirty();
        }

        EditorGUI.BeginChangeCheck();
        float3 newBotLeft = Handles.PositionHandle(botLeft, Quaternion.identity);

        if (EditorGUI.EndChangeCheck()) {
          Undo.RegisterCompleteObjectUndo(t, "Modify rect");
          t.rect.xMin = newBotLeft.x;
          t.rect.yMin = newBotLeft.y;
          Dirty();
        }

        EditorGUI.BeginChangeCheck();
        float3 newTopRight = Handles.PositionHandle(topRight, Quaternion.identity);

        if (EditorGUI.EndChangeCheck()) {
          Undo.RegisterCompleteObjectUndo(t, "Modify rect");
          t.rect.xMax = newTopRight.x;
          t.rect.yMax = newTopRight.y;
          Dirty();
        }

        EditorGUI.BeginChangeCheck();
        float3 newTopLeft = Handles.PositionHandle(topLeft, Quaternion.identity);

        if (EditorGUI.EndChangeCheck()) {
          Undo.RegisterCompleteObjectUndo(t, "Modify rect");
          t.rect.xMin = newTopLeft.x;
          t.rect.yMax = newTopLeft.y;
          Dirty();
        }
      }


    }

    void Dirty() {
      if (!Application.isPlaying) {
        EditorUtility.SetDirty(t);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
      }
    }
  }
#endif

}