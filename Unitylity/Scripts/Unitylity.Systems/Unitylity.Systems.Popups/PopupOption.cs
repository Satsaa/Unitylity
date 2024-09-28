
namespace Unitylity.Systems.Popups {

	using System.Collections;
	using System.Collections.Generic;
	using TMPro;
	using UnityEngine;
	using UnityEngine.SceneManagement;
	using UnityEngine.UI;
	using Unitylity.Data;

#if UNITYLITY_SYSTEMS_POPUPS_HIDDEN
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/" + nameof(Unitylity.Systems.Popups) + "/" + nameof(PopupOption))]
#endif
	public class PopupOption : MonoBehaviour {

		public Button button;
		public TMP_Text text;
		public Flags flags;

		[System.Flags]
		public enum Flags {
			None = 0,
			Default = 1,
			Cancel = 2,
		}

		protected void Awake() {
			if (!button) Debug.Assert(button = GetComponentInChildren<Button>());
			if (!text) Debug.Assert(text = GetComponentInChildren<TMP_Text>());
			button.onClick.AddListener(() => GetComponentInParent<Popup>().Hide());
		}

		public virtual void SetInteractable(bool interactable) {
			button.interactable = interactable;
		}

		public virtual void SetText(string text) {
			this.text.text = text;
		}

		public virtual void Invoke() {
			button.onClick.Invoke();
		}

		public virtual void AddAction(System.Action action) {
			if (action != null) button.onClick.AddListener(() => action());
		}

		public virtual void RemoveActions() {
			button.onClick.RemoveAllListeners();
		}
	}

}