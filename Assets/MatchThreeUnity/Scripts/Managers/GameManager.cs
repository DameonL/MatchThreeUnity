using Assets.MatchThreeUnity.Scripts.Tiles;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.MatchThreeUnity.Scripts.Managers
{
	/// <summary>
	/// Tracks scoring, and ends the game (if appropriate).
	/// </summary>
	public class GameManager : MonoBehaviour
	{
		private static GameManager instance;
		public static GameManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = GameObject.FindObjectOfType<GameManager>();
				}
				return instance;
			}
		}

		/// <summary>
		/// Called when the score increases. Passes the amount of points scored as a parameter.
		/// </summary>
		public event Action<ulong> ScoreChangedHandler = (ulong incomingPoints) => { };

		[SerializeField]
		private int startingScene;

		[SerializeField]
		private Text scoreText;

		[SerializeField]
		private AnimationCurve bonusCurve;

		[SerializeField]
		private AudioClip largeScoreAmount;

		private ulong score;

		private void Awake()
		{
			if ((instance != null) && (instance != this))
			{
				Destroy(instance.gameObject);
			}

			instance = this;
		}

		/// <summary>
		/// Adds an amount of points to the player's score, with a bonus based on the points scored using the <see cref="bonusCurve"/>.
		/// </summary>
		/// <param name="points"></param>
		public void AddScore(ulong points)
		{
			float bonus = bonusCurve.Evaluate(Convert.ToSingle(points));
			points = Convert.ToUInt64(points + bonus);
			score += points;
			if ((points > 3) && (largeScoreAmount != null))
			{
				AudioSource.PlayClipAtPoint(largeScoreAmount, Vector3.zero, 4);
			}
			scoreText.text = score.ToString();
			ScoreChangedHandler.Invoke(points);
		}

		/// <summary>
		/// Adds points without triggering any bonus-related handling, or event triggering.
		/// </summary>
		/// <param name="points"></param>
		public void AddScoreSilently(ulong points)
		{
			score += points;
			scoreText.text = score.ToString();
		}

		/// <summary>
		/// Ends the game and returns to the starting scene.
		/// </summary>
		public void EndGame()
		{
			var spawners = FindObjectsOfType<TileSpawner>();
			foreach (var spawner in spawners)
			{
				spawner.enabled = false;
			}

			var tiles = FindObjectsOfType<Tile>();
			foreach (var tile in tiles)
			{
				Destroy(tile.gameObject);
			}
		}

	}
}
