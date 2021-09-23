

namespace Muc.Systems.Values {

	public class Lock : Modifier<float> {

		public override Handler onSet => OnSet;
		protected float OnSet(float arg1) => target.GetRaw();

		public override Handler onAdd => OnArithmetic;
		public override Handler onSub => OnArithmetic;

		protected float OnArithmetic(float arg1) {
			Ignore();
			return 0;
		}

	}

}