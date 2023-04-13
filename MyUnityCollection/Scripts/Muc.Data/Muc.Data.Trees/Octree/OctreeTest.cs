
namespace Unitylity.Data.Trees.Tests {

	using UnityEngine;

	internal class OctreeTest : MonoBehaviour {

		public bool toggle;

		[Range(0, 1)]
		public float splitChance;
		public int maxDebth;
		public int maxRendered = 5000;
		public float minLineWidth = 2;
		public float maxLineWidth = 3;

		public Octree<Data> tree;

		public void OnValidate() {
			tree = new Octree<Data>(); // 0

			int i = 0;
			var e = tree.GetEnumerator();
			Random.InitState((int)(splitChance * 1000));
			while (e.MoveNext() && i++ < 1_000_000) {
				if (maxDebth > e.depth && (Random.value < splitChance || e.depth == 0)) e.Current.Split();
			}
		}

		public class Data { }

	}

}


#if UNITY_EDITOR
namespace Unitylity.Data.Trees.Tests.Editor {

	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomEditor(typeof(OctreeTest))]
	internal class OctreeTestEditor : Editor {

		private OctreeTest t => (OctreeTest)target;

		protected virtual void OnSceneGUI() {
			if (Event.current.GetTypeForControl(0) == EventType.Repaint) {
				Draw();
			}
		}

		void Draw() {
			var e = t.tree.GetDetailedEnumerator();
			int i = 0;
			while (e.MoveNext() && i++ < t.maxRendered) {

				var color = Color.white;
				color.a /= (e.depth * 2 + 1);
				var width = Mathf.Lerp(t.maxLineWidth, t.minLineWidth, e.depth / (t.maxDebth + 1));

				var origin = t.transform.position + new Vector3(e.currentOrigin.x * t.transform.lossyScale.x, e.currentOrigin.y * t.transform.lossyScale.y, e.currentOrigin.z * t.transform.lossyScale.z);
				var size = e.currentSize * t.transform.lossyScale;

				if (e.Current.isLeaf) {
					DrawLeaf(origin, size, color, width);
				} else {
					if (i == 1) DrawLeaf(origin, size, color, width);
					DrawParent(origin, size, color, width);
				}
			}
		}

		void DrawLeaf(Vector3 origin, Vector3 size, Color color, float width) {
			var prevColor = Handles.color;
			Handles.color = color;

			Handles.DrawAAPolyLine(width,
				origin + new Vector3(size.x * 0, size.y * 0, size.z * 0),
				origin + new Vector3(size.x * 1, size.y * 0, size.z * 0),
				origin + new Vector3(size.x * 1, size.y * 1, size.z * 0),
				origin + new Vector3(size.x * 0, size.y * 1, size.z * 0),
				origin + new Vector3(size.x * 0, size.y * 0, size.z * 0),
				origin + new Vector3(size.x * 0, size.y * 0, size.z * 1),
				origin + new Vector3(size.x * 0, size.y * 1, size.z * 1),
				origin + new Vector3(size.x * 1, size.y * 1, size.z * 1),
				origin + new Vector3(size.x * 1, size.y * 0, size.z * 1),
				origin + new Vector3(size.x * 0, size.y * 0, size.z * 1)
			);

			Handles.DrawAAPolyLine(width,
				origin + new Vector3(size.x * 1, size.y * 0, size.z * 1),
				origin + new Vector3(size.x * 1, size.y * 0, size.z * 0)
			);

			Handles.DrawAAPolyLine(width,
				origin + new Vector3(size.x * 0, size.y * 1, size.z * 1),
				origin + new Vector3(size.x * 0, size.y * 1, size.z * 0)
			);

			Handles.DrawAAPolyLine(width,
				origin + new Vector3(size.x * 1, size.y * 1, size.z * 1),
				origin + new Vector3(size.x * 1, size.y * 1, size.z * 0)
			);

			Handles.color = prevColor;
		}

		void DrawParent(Vector3 origin, Vector3 size, Color color, float width) {
			var prevColor = Handles.color;
			Handles.color = color;

			Handles.DrawAAPolyLine(width,
				origin + new Vector3(size.x * 0.5f, size.y * 0, size.z * 0),
				origin + new Vector3(size.x * 0.5f, size.y * 1, size.z * 0),
				origin + new Vector3(size.x * 0.5f, size.y * 1, size.z * 1),
				origin + new Vector3(size.x * 0.5f, size.y * 0, size.z * 1),
				origin + new Vector3(size.x * 0.5f, size.y * 0, size.z * 0)
			);

			Handles.DrawAAPolyLine(width,
				origin + new Vector3(size.x * 0, size.y * 0.5f, size.z * 0),
				origin + new Vector3(size.x * 1, size.y * 0.5f, size.z * 0),
				origin + new Vector3(size.x * 1, size.y * 0.5f, size.z * 1),
				origin + new Vector3(size.x * 0, size.y * 0.5f, size.z * 1),
				origin + new Vector3(size.x * 0, size.y * 0.5f, size.z * 0)
			);

			Handles.DrawAAPolyLine(width,
				origin + new Vector3(size.x * 0, size.y * 0, size.z * 0.5f),
				origin + new Vector3(size.x * 1, size.y * 0, size.z * 0.5f),
				origin + new Vector3(size.x * 1, size.y * 1, size.z * 0.5f),
				origin + new Vector3(size.x * 0, size.y * 1, size.z * 0.5f),
				origin + new Vector3(size.x * 0, size.y * 0, size.z * 0.5f)
			);

			Handles.color = prevColor;
		}

	}

}
#endif