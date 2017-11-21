using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.MatchThreeUnity.Scripts.Audio
{
	/// <summary>
	/// Automatically creates small variations in the volume, pitch, and start delay of a <see cref="AudioSource"/>.
	/// </summary>
	[RequireComponent(typeof(AudioSource))]
	public class SFXVariations : MonoBehaviour
	{
		[SerializeField]
		private MinMaxValue startDelay;

		[SerializeField]
		private MinMaxValue volumeVariation;

		[SerializeField]
		private MinMaxValue pitchVariation;

		private AudioSource audioSource;

		private void Awake()
		{
			audioSource = GetComponent<AudioSource>();
		}

		private void OnEnable()
		{
			audioSource.volume = volumeVariation.Value;
			audioSource.pitch = pitchVariation.Value;
			audioSource.PlayDelayed(startDelay.Value);
		}

		/// <summary>
		/// Has a value based on a random amount between the min and max.
		/// </summary>
		[Serializable]
		public struct MinMaxValue
		{
			[SerializeField]
			private float min;
			public float Min { get { return min; } }

			[SerializeField]
			private float max;
			public float Max { get { return max; } }

			private bool valueSet;

			private float value;
			public float Value
			{
				get
				{
					if (!valueSet)
					{
						Recalculate();
					}
					return value;
				}
			}

			/// <summary>
			/// Recalculates the <see cref="Value"/> of this <see cref="MinMaxValue"/>.
			/// </summary>
			/// <returns>The new <see cref="Value"/>.</returns>
			public float Recalculate()
			{
				value = Random.Range(min, max);
				valueSet = true;
				return value;
			}
		}

	}
}
