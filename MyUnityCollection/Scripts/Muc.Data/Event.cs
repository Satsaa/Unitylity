
namespace Unitylity.Data {

	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class Event {

		protected Action listeners;

		public void Invoke() {
			listeners?.Invoke();
		}

		public void ConfigureListener(bool add, Action listener) {
			if (add) AddListener(listener);
			else RemoveListener(listener);
		}

		public void AddListener(Action listener) {
			listeners += listener;
		}

		public void RemoveListener(Action listener) {
			listeners -= listener;
		}
	}

	public class Event<T> {

		protected Action<T> listeners;

		public void Invoke(T arg) {
			listeners?.Invoke(arg);
		}

		public void ConfigureListener(bool add, Action<T> listener) {
			if (add) AddListener(listener);
			else RemoveListener(listener);
		}

		public void AddListener(Action<T> listener) {
			listeners += listener;
		}

		public void RemoveListener(Action<T> listener) {
			listeners -= listener;
		}
	}

	public class Event<T1, T2> {

		protected Action<T1, T2> listeners;

		public void Invoke(T1 arg1, T2 arg2) {
			listeners?.Invoke(arg1, arg2);
		}

		public void ConfigureListener(bool add, Action<T1, T2> listener) {
			if (add) AddListener(listener);
			else RemoveListener(listener);
		}

		public void AddListener(Action<T1, T2> listener) {
			listeners += listener;
		}

		public void RemoveListener(Action<T1, T2> listener) {
			listeners -= listener;
		}
	}

	public class Event<T1, T2, T3> {

		protected Action<T1, T2, T3> listeners;

		public void Invoke(T1 arg1, T2 arg2, T3 arg3) {
			listeners?.Invoke(arg1, arg2, arg3);
		}

		public void ConfigureListener(bool add, Action<T1, T2, T3> listener) {
			if (add) AddListener(listener);
			else RemoveListener(listener);
		}

		public void AddListener(Action<T1, T2, T3> listener) {
			listeners += listener;
		}

		public void RemoveListener(Action<T1, T2, T3> listener) {
			listeners -= listener;
		}
	}

	public class Event<T1, T2, T3, T4> {

		protected Action<T1, T2, T3, T4> listeners;

		public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
			listeners?.Invoke(arg1, arg2, arg3, arg4);
		}

		public void ConfigureListener(bool add, Action<T1, T2, T3, T4> listener) {
			if (add) AddListener(listener);
			else RemoveListener(listener);
		}

		public void AddListener(Action<T1, T2, T3, T4> listener) {
			listeners += listener;
		}

		public void RemoveListener(Action<T1, T2, T3, T4> listener) {
			listeners -= listener;
		}
	}

	public class Event<T1, T2, T3, T4, T5> {

		protected Action<T1, T2, T3, T4, T5> listeners;

		public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) {
			listeners?.Invoke(arg1, arg2, arg3, arg4, arg5);
		}

		public void ConfigureListener(bool add, Action<T1, T2, T3, T4, T5> listener) {
			if (add) AddListener(listener);
			else RemoveListener(listener);
		}

		public void AddListener(Action<T1, T2, T3, T4, T5> listener) {
			listeners += listener;
		}

		public void RemoveListener(Action<T1, T2, T3, T4, T5> listener) {
			listeners -= listener;
		}
	}

	public class Event<T1, T2, T3, T4, T5, T6> {

		protected Action<T1, T2, T3, T4, T5, T6> listeners;

		public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) {
			listeners?.Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
		}

		public void ConfigureListener(bool add, Action<T1, T2, T3, T4, T5, T6> listener) {
			if (add) AddListener(listener);
			else RemoveListener(listener);
		}

		public void AddListener(Action<T1, T2, T3, T4, T5, T6> listener) {
			listeners += listener;
		}

		public void RemoveListener(Action<T1, T2, T3, T4, T5, T6> listener) {
			listeners -= listener;
		}
	}

	public class Event<T1, T2, T3, T4, T5, T6, T7> {

		protected Action<T1, T2, T3, T4, T5, T6, T7> listeners;

		public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) {
			listeners?.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}

		public void ConfigureListener(bool add, Action<T1, T2, T3, T4, T5, T6, T7> listener) {
			if (add) AddListener(listener);
			else RemoveListener(listener);
		}

		public void AddListener(Action<T1, T2, T3, T4, T5, T6, T7> listener) {
			listeners += listener;
		}

		public void RemoveListener(Action<T1, T2, T3, T4, T5, T6, T7> listener) {
			listeners -= listener;
		}
	}

	public class Event<T1, T2, T3, T4, T5, T6, T7, T8> {

		protected Action<T1, T2, T3, T4, T5, T6, T7, T8> listeners;

		public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) {
			listeners?.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
		}

		public void ConfigureListener(bool add, Action<T1, T2, T3, T4, T5, T6, T7, T8> listener) {
			if (add) AddListener(listener);
			else RemoveListener(listener);
		}

		public void AddListener(Action<T1, T2, T3, T4, T5, T6, T7, T8> listener) {
			listeners += listener;
		}

		public void RemoveListener(Action<T1, T2, T3, T4, T5, T6, T7, T8> listener) {
			listeners -= listener;
		}
	}

	public class Event<T1, T2, T3, T4, T5, T6, T7, T8, T9> {

		protected Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> listeners;

		public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) {
			listeners?.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
		}

		public void ConfigureListener(bool add, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> listener) {
			if (add) AddListener(listener);
			else RemoveListener(listener);
		}

		public void AddListener(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> listener) {
			listeners += listener;
		}

		public void RemoveListener(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> listener) {
			listeners -= listener;
		}
	}

	public class Event<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> {

		protected Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> listeners;

		public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) {
			listeners?.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
		}

		public void ConfigureListener(bool add, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> listener) {
			if (add) AddListener(listener);
			else RemoveListener(listener);
		}

		public void AddListener(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> listener) {
			listeners += listener;
		}

		public void RemoveListener(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> listener) {
			listeners -= listener;
		}
	}

}
