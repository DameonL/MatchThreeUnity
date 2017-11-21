using Assets.MatchThreeUnity.Scripts.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MatchThreeUnity.Scripts.Tiles
{
	public class EndGameTileDetector : MonoBehaviour
	{
		[SerializeField]
		private float detectionTime = 1;
		private float detectionTimer;

		private HashSet<Tile> containedTiles = new HashSet<Tile>();

		private void OnTriggerEnter2D(Collider2D other)
		{
			Tile tile = other.GetComponent<Tile>();
			if (tile != null)
			{
				containedTiles.Add(tile);
			}
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			Tile tile = other.GetComponent<Tile>();
			if (tile != null)
			{
				while (containedTiles.Contains(tile))
					containedTiles.Remove(tile);
			}
		}

		private void OnEnable()
		{
			detectionTimer = detectionTime;
		}

		private void Update()
		{
			bool endGame = false;
			foreach (var tile in containedTiles)
			{
				if (!tile.IsFalling)
				{
					endGame = true;
					break;
				}
			}

			if (endGame)
			{
				detectionTimer -= Time.deltaTime;
				if (detectionTimer <= 0)
				{
					containedTiles.Clear();
					gameObject.SetActive(false);
					GameManager.Instance.EndGame();
				}
			}
			else
			{
				detectionTimer = detectionTime;
			}
		}
	}
}
