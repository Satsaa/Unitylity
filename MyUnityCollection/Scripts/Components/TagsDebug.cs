
namespace Muc.Components {

  using UnityEngine;

  public class TagsDebug : MonoBehaviour { }

}


#if UNITY_EDITOR
namespace Muc.Components.Editor {

  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;

  using UnityEngine;
  using UnityEditor;
  using UnityEditor.AnimatedValues;

  [CustomEditor(typeof(TagsDebug))]
  public class TagsDebugEditor : Editor {

    private TagsDebug t => target as TagsDebug;
    private Dictionary<string, AnimBool> fadeAnims = new Dictionary<string, AnimBool>();
    private GUIStyle objectButtonStyle;


    public override bool RequiresConstantRepaint() => true;

    public override void OnInspectorGUI() {

      DrawDefaultInspector();

      if (objectButtonStyle == null) {
        objectButtonStyle = GUI.skin.FindStyle("ExposablePopupItem");
        objectButtonStyle.margin = new RectOffset(0, 0, 0, 0);
      }

      var prop = typeof(Tags).GetField("tagged", BindingFlags.NonPublic | BindingFlags.Static);
      var val = (Dictionary<string, HashSet<Tags>>)prop.GetValue(null);

      // Show/Hide button
      using (new EditorGUILayout.HorizontalScope()) {
        var allShown = val.Keys.All(tag => fadeAnims.ContainsKey(tag) && fadeAnims[tag].target);
        if (GUILayout.Button($"{(allShown ? "Hide" : "Show")} all"))
          foreach (var tag in val.Keys)
            fadeAnims[tag].target = !allShown;
      }

      // Tag list and tagged objects
      foreach (var tagSet in val) {
        var tag = tagSet.Key;
        var taggeds = tagSet.Value;

        if (!fadeAnims.ContainsKey(tag)) {
          var animBool = new AnimBool(true);
          fadeAnims.Add(tag, animBool);
        }

        var fadeAnim = fadeAnims[tag];
        fadeAnim.target = EditorGUILayout.Foldout(fadeAnim.target, tag);
        using (new EditorGUILayout.FadeGroupScope(fadeAnim.faded)) {
          if (fadeAnim.faded > 0) {
            foreach (var tagged in taggeds) {
              using (new EditorGUILayout.HorizontalScope()) {
                // Show tagged object name and allow clicking to select it
                var cont = new GUIContent(tagged ? tagged.name : "NULL", tagged ? "Select Object" : "Object is null!");
                if (GUILayout.Button(cont, objectButtonStyle) && tagged) {
                  Selection.activeGameObject = tagged.gameObject;
                  EditorGUIUtility.PingObject(tagged.gameObject);
                }
              }
            }
          }
        }
      }

    }

  }
}
#endif