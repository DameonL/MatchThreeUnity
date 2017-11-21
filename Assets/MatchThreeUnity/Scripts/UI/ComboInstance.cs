using UnityEngine;
using UnityEngine.UI;

namespace Assets.MatchThreeUnity.Scripts.UI
{
	/// <summary>
	/// Briefly displays infomration on a combo, then disappears.
	/// </summary>
	public class ComboInstance : MonoBehaviour
	{
		[SerializeField]
		private Text comboType;
		public Text ComboType { get { return comboType; } }

		[SerializeField]
		private Text bonusSize;
		public Text BonusSize { get { return bonusSize; } }

		[SerializeField]
		private float decayTime = 1;

		private void Update()
		{
			decayTime -= Time.deltaTime;
			if (decayTime <= 0)
			{
				gameObject.SetActive(false);
				Destroy(gameObject);
			}
		}
	}
}
