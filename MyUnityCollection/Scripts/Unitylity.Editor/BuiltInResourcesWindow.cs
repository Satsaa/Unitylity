
#if UNITY_EDITOR
namespace Unitylity.Editor {

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEditor;
	using UnityEngine;

	public class GUIResourcesWindow : EditorWindow {

		[MenuItem("Window/GUI Resources")]
		public static void ShowWindow() {
			GUIResourcesWindow w = (GUIResourcesWindow)EditorWindow.GetWindow<GUIResourcesWindow>();
			w.Show();

		}

		private struct Drawer {
			public Rect rect;
			public Action draw;
			public bool active;
		}

		private List<Drawer> drawers;

		private List<Texture2D> textures;

		private float maxY;
		private Rect oldPosition;

		private Tab tab = Tab.Styles;
		private enum Tab { Styles, Textures }

		private string search = "";
		private float scrollVal;

		private const int PADDING_X = 5;
		private const int PADDING_Y = 5;

		private const int SCROLL_W = 16;

		private string[] FILTER_TEXTURE_NAMES { get; } = new string[] { "", "CurveTexture", "Font Texture", "Clear Texture", "MagentaTexture" };

		void OnGUI() {
			if (position.width != oldPosition.width && Event.current.type == EventType.Layout) {
				drawers = null;
				oldPosition = position;
			}

			using (new GUILayout.HorizontalScope()) {

				if (GUILayout.Toggle(tab == Tab.Styles, "Styles", GUI.skin.FindStyle("Tab first")) && tab != Tab.Styles) {
					tab = Tab.Styles;
					drawers = null;
				}

				if (GUILayout.Toggle(tab == Tab.Textures, "Textures", GUI.skin.FindStyle("Tab last")) && tab != Tab.Textures) {
					tab = Tab.Textures;
					textures = null;
					drawers = null;
				}
			}


			string newSearch = GUILayout.TextField(search);
			if (newSearch != search) {
				search = newSearch;
				drawers = null;
			}


			if (drawers == null) {

				drawers = new List<Drawer>();

				var nameStyle = GUI.skin.FindStyle("MiniToolbarButton");

				var normalText = new GUIContent("normal");
				var activeText = new GUIContent("active");

				float x = PADDING_X;
				float y = PADDING_Y;

				float maxHeight = 0;

				string searchLow = search.ToLower();


				switch (tab) {

					case Tab.Styles:

						foreach (GUIStyle style in GUI.skin.customStyles) {
							if (searchLow != "" && !style.name.ToLower().Contains(searchLow))
								continue;

							var drawer = new Drawer();

							var size = style.CalcSize(normalText);
							if (size.x < 8) size.x = 80;
							if (size.y < 8) size.y = 40;

							var nameSize = nameStyle.CalcSize(new GUIContent(style.name));

							var width = Mathf.Max(
									nameSize.x,
									size.x * 2
							) + 16;

							var height = nameSize.y + size.y + PADDING_X * 2;

							if (x + width > position.width - SCROLL_W - PADDING_X && x > PADDING_X) {
								x = PADDING_X;
								y += maxHeight + PADDING_Y * 2;
								maxHeight = height;
							}

							maxHeight = Mathf.Max(maxHeight, height);

							drawer.rect = new Rect(x, y, width, height);

							drawer.draw = () => {
								EditorGUILayout.SelectableLabel(style.name, nameStyle, GUILayout.Height(16));

								using (new GUILayout.HorizontalScope()) {
									drawer.active =
									GUILayout.Toggle(drawer.active, normalText, style, GUILayout.Width(width / 2));
									GUILayout.Toggle(true, activeText, style, GUILayout.Width(width / 2));
								}
							};

							x += width + PADDING_X * 2;

							drawers.Add(drawer);
						}

						break;


					case Tab.Textures:

						if (textures == null) {
							textures = Resources.FindObjectsOfTypeAll<Texture2D>().ToList();
							textures.Sort((a, b) => System.String.Compare(a.name, b.name, System.StringComparison.OrdinalIgnoreCase));
						}

						foreach (var texture in textures) {

							if (FILTER_TEXTURE_NAMES.Contains(texture.name))
								continue;
							if (searchLow != "" && !texture.name.ToLower().Contains(searchLow))
								continue;

							var drawer = new Drawer();

							var buttonSize = GUI.skin.button.CalcSize(new GUIContent(texture.name));

							var width = Mathf.Max(
									buttonSize.x,
									texture.width
							) + 8;

							var height = buttonSize.y + texture.height + 4;


							if (x + width > position.width - SCROLL_W - PADDING_X && x > PADDING_X) {
								x = PADDING_X;
								y += maxHeight + PADDING_Y * 2;
								maxHeight = height;
							}

							maxHeight = Mathf.Max(maxHeight, height);

							drawer.rect = new Rect(x, y, width, height);

							drawer.draw = () => {
								EditorGUILayout.SelectableLabel(texture.name, nameStyle, GUILayout.Height(16));

								var clampW = Mathf.Min(texture.width, position.width - 16);
								var clampH = Mathf.Min(texture.height, position.height - 16);

								Rect textureRect = GUILayoutUtility.GetRect(
										texture.width, texture.width, texture.height, texture.height,
										GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false)
								);

								textureRect.x += 4;
								textureRect.y += 3;

								EditorGUI.DrawTextureTransparent(textureRect, texture);
							};

							x += width + PADDING_X * 2;

							drawers.Add(drawer);
						}

						break;

				}

				maxY = y;
			}

			float top = EditorGUIUtility.singleLineHeight * 2 - 1;

			var scrollRect = new Rect(
					x: position.width - SCROLL_W,
					y: top,
					width: SCROLL_W,
					height: position.height - top
			);

			float contentHeight = position.height - top; // 696
			scrollVal = GUI.VerticalScrollbar(scrollRect, scrollVal, contentHeight, 0, Mathf.Max(position.height - top, maxY)); // 585

			var area = new Rect(0, top, position.width - 16, contentHeight);
			using (new GUILayout.AreaScope(area)) {

				foreach (var drawing in drawers) {
					var clampRect = drawing.rect;
					clampRect.y -= scrollVal;

					if (clampRect.y + clampRect.height > 0 && clampRect.y < contentHeight) {
						using (new GUILayout.AreaScope(clampRect, "", GUI.skin.FindStyle("grey_border"))) {
							drawing.draw();
						}
					}
				}
			}
		}

	}

}
#endif