
namespace Unitylity.Systems.Popups {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using TMPro;
	using UnityEngine;
	using UnityEngine.SceneManagement;
	using UnityEngine.UI;
	using Unitylity.Data;
	using Object = UnityEngine.Object;

#if UNITYLITY_SYSTEMS_POPUPS_HIDDEN
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/" + nameof(Unitylity.Systems.Popups) + "/" + nameof(Popup))]
#endif
	public class Popup : MonoBehaviour {

		[SerializeField] private TMP_Text title;
		[SerializeField] private TMP_Text message;
		[SerializeField] private RectTransform optionsParent;

		[SerializeField, HideInInspector] internal PopupOption defaultOptionPrefab;

		[SerializeField, HideInInspector] private List<PopupOption> _options;
		public ReadOnlyCollection<PopupOption> options => _options.AsReadOnly();

		protected void Awake() {
			Popups.instance.popups.Add(this);
		}

		protected void Start() {
			transform.localPosition = default;
		}

		protected void OnDestroy() {
			if (Popups.instance) Popups.instance.popups.RemoveAll(v => v == this);
		}

		public virtual void SetTitle(string title) {
			this.title.text = title;
			this.title.gameObject.SetActive(!string.IsNullOrWhiteSpace(title));
		}
		public virtual void SetMessage(string message) {
			this.message.text = message;
			this.message.gameObject.SetActive(!string.IsNullOrWhiteSpace(message));
		}

		/// <summary> Adds the Object after the message UI element </summary>
		public virtual void AddCustomObject(GameObject go) {
			go.transform.SetParent(message.transform.parent);
			go.transform.SetSiblingIndex(2);
		}

		public PopupOption AddOption(Action action = null) => AddOption(defaultOptionPrefab, action);
		public virtual PopupOption AddOption(PopupOption optionPrefab, Action action = null) {
			var option = Instantiate(optionPrefab, Vector3.zero, Quaternion.identity, optionsParent);
			option.AddAction(action);
			_options.Add(option);
			if (optionPrefab.flags.HasFlag(PopupOption.Flags.Default)) {
				UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(option.gameObject, null);
			}
			return option;
		}

		public virtual void Hide() {
			Popups.instance.popups.Remove(this);

			foreach (var option in options) option.RemoveActions();

			if (TryGetComponent<Animator>(out var animator)) {
				animator.SetTrigger("Hide");
			} else {
				Destroy(gameObject);
			}
		}

		public void FinalizeHideAnim() {
			Destroy(gameObject);
		}

	}

}