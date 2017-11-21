using UnityEngine;

namespace Assets.MatchThreeUnity.Scripts.Particles
{
	/// <summary>
	/// Destroys the attached <see cref="ParticleSystem"/> when it's done playing.
	/// </summary>
	[RequireComponent(typeof(ParticleSystem))]
	[RequireComponent(typeof(AudioSource))]
	public class DestroyAfterPlay : MonoBehaviour
	{
		private ParticleSystem particles;
		private AudioSource audioSource;

		private void Awake()
		{
			particles = GetComponent<ParticleSystem>();
			audioSource = GetComponent<AudioSource>();
		}

		private void Update()
		{
			if (particles.isStopped && !audioSource.isPlaying)
			{
				Destroy(gameObject);
			}
		}
	}
}
