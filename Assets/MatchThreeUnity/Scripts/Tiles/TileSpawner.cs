using Assets.Scripts.Pooling;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.MatchThreeUnity.Scripts.Tiles
{
	/// <summary>
	/// Spawns Tiles at random from a list.
	/// </summary>
	public class TileSpawner : MonoBehaviour
	{
		[SerializeField]
		private float spawnInterval = 1;

		[SerializeField]
		private List<WeightedTileSpawn> tilesToSpawn = new List<WeightedTileSpawn>();

		[SerializeField]
		private AnimationCurve gameSpeedCurve;

		private List<Transform> spawnPositions = new List<Transform>();
		private float spawnCountdown;
		private int spawnedTiles = 0;

		private void Start()
		{
			for (var i = 0; i < transform.childCount; i++)
			{
				spawnPositions.Add(transform.GetChild(i));
			}
		}

		private void Update()
		{
			spawnCountdown -= Time.deltaTime;
			if (spawnCountdown <= 0)
			{
				Time.timeScale = gameSpeedCurve.Evaluate(Time.time);
				var spawnPoint = spawnPositions[Random.Range(0, spawnPositions.Count)];
				var spawnTile = tilesToSpawn[Random.Range(0, tilesToSpawn.Count)];

				var tile = PrefabPool.Get(spawnTile.Prefab) as Tile;
				spawnedTiles++;
				tile.name = "Tile " + spawnedTiles;
				tile.transform.position = spawnPoint.transform.position;
				tile.gameObject.SetActive(true);
				spawnCountdown = spawnInterval;
			}
		}
	}
}
