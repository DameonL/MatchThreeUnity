using Assets.MatchThreeUnity.Scripts.Managers;
using Assets.Scripts.Pooling;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MatchThreeUnity.Scripts.Tiles
{
	/// <summary>
	/// A matchable tile.
	/// </summary>
	public class Tile : MonoBehaviour, IPoolableObject
	{
		[SerializeField]
		private GameObject destroyEffect;

		[SerializeField]
		private float destroyTime = 1;

		[SerializeField]
		new private Renderer renderer;

		[SerializeField]
		private bool isFalling = true;
		public bool IsFalling { get { return isFalling; } }

		[SerializeField]
		private TileNeighborCollider[] neighborColliders = new TileNeighborCollider[4]; // Clockwise, 0 = top, 1 = right, 2 = bottom, 3 = left
		public TileNeighborCollider[] Neighbors { get { return neighborColliders; } }

		private Color tileColor = Color.blue;
		public Color TileColor { get { return tileColor; } protected set { tileColor = value; } }

		private IPoolableObject prefab;
		public IPoolableObject Prefab { get { return prefab; } set { prefab = value; } }

		private float velocity;
		private float lastY;
		private TileMatcher matcher;
		private float destroyTimer = 0;

		private void Awake()
		{
			matcher = GetComponent<TileMatcher>();
			if (matcher == null)
			{
				matcher = gameObject.AddComponent<TileMatcherColor>();
			}
			TileColor = renderer.material.color;
		}

		private void OnEnable()
		{
			lastY = transform.position.y;
			destroyTimer = destroyTime;
		}

		private void FixedUpdate()
		{
			velocity = lastY - transform.position.y;

			if (velocity < 0.01)
			{
				isFalling = false;
			}
			else
			{
				isFalling = true;
			}

			lastY = transform.position.y;

			if (!isFalling)
			{
				destroyTimer -= Time.fixedDeltaTime;
				var vertMatches = 1 + CheckMatchesInDirection(Direction.Up) + CheckMatchesInDirection(Direction.Down);
				var horizMatches = 1 + CheckMatchesInDirection(Direction.Right) + CheckMatchesInDirection(Direction.Left);

				if ((vertMatches > 2 || horizMatches > 2))
				{
					if (destroyTimer <= 0)
					{
						var tilesToDestroy = new Stack<Tile>();
						tilesToDestroy.Push(this);
						if (horizMatches > 2)
						{
							if (CheckMatchesInDirection(Direction.Right) > 0)
								neighborColliders[(int)Direction.Right].ContainedLockedTile?.CrawlAndDestroy(Direction.Right, tilesToDestroy);
								
							if (CheckMatchesInDirection(Direction.Left) > 0)
								neighborColliders[(int)Direction.Left].ContainedLockedTile?.CrawlAndDestroy(Direction.Left, tilesToDestroy);
						}

						if (vertMatches > 2)
						{
							if (CheckMatchesInDirection(Direction.Up) > 0)
								neighborColliders[(int)Direction.Up].ContainedLockedTile?.CrawlAndDestroy(Direction.Up, tilesToDestroy);

							if (CheckMatchesInDirection(Direction.Down) > 0)
								neighborColliders[(int)Direction.Down].ContainedLockedTile?.CrawlAndDestroy(Direction.Down, tilesToDestroy);
						}
						GameManager.Instance.AddScore(Convert.ToUInt64(tilesToDestroy.Count));

						while (tilesToDestroy.Count > 0)
						{
							var tile = tilesToDestroy.Pop();
							Instantiate(destroyEffect).transform.position = tile.transform.position;
							tile.Destroy();
						}
					}
				}
				else
				{
					destroyTimer = destroyTime;
				}
			}
		}

		/// <summary>
		/// Checks tiles in the specified direction, using this tile's TileMatcher component. (Recursive)
		/// </summary>
		/// <param name="direction">A <see cref="Direction"/> to search in.</param>
		/// <param name="currentMatches">The current number of matches in this series.</param>
		/// <returns>The number of matches in the series plus this, if it's a match.</returns>
		public int CheckMatchesInDirection(Direction direction, int currentMatches = 0)
		{
			if (neighborColliders[(int)direction].ContainedLockedTile == null)
				return currentMatches;

			if (matcher.IsMatch(this, neighborColliders[(int)direction].ContainedLockedTile))
			{
				currentMatches++;
				return neighborColliders[(int)direction].ContainedLockedTile.CheckMatchesInDirection(direction, currentMatches);
			}

			return currentMatches;
		}

		/// <summary>
		/// Adds itself to a Stack of Tiles marked for destruction, and looks for more tiles to destroy. (Recursive)
		/// </summary>
		/// <param name="direction">A <see cref="Direction"/> direction to search in.</param>
		/// <param name="tiles">The Stack of Tiles to be destroyed. This Tile will add itself to the Stack.</param>
		public void CrawlAndDestroy(Direction direction, Stack<Tile> tiles)
		{
			Direction cross1 = Direction.Right;
			Direction cross2 = Direction.Left;

			if (direction == Direction.Right || direction == Direction.Left)
			{
				cross1 = Direction.Up;
				cross2 = Direction.Down;
			}

			int cross1Matches = 0;
			if (!tiles.Contains(neighborColliders[(int)cross1]?.ContainedLockedTile))
			{
				cross1Matches = CheckMatchesInDirection(cross1);
			}

			int cross2Matches = 0;
			if (!tiles.Contains(neighborColliders[(int)cross2]?.ContainedLockedTile))
			{
				cross2Matches = CheckMatchesInDirection(cross2);
			}

			if ((1 + cross1Matches + cross2Matches) >= 3)
			{
				if (cross1Matches > 0)
				{
					neighborColliders[(int)cross1].ContainedLockedTile?.CrawlAndDestroy(cross1, tiles);
				}
				if (cross2Matches > 0)
				{
					neighborColliders[(int)cross2].ContainedLockedTile?.CrawlAndDestroy(cross2, tiles);
				}
			}

			if (matcher.IsMatch(this, neighborColliders[(int)direction].ContainedLockedTile) && !tiles.Contains(neighborColliders[(int)direction].ContainedLockedTile))
			{
				neighborColliders[(int)direction].ContainedLockedTile?.CrawlAndDestroy(direction, tiles);
			}
			tiles.Push(this);
		}

		private void RemoveTileFromNeighbors(Tile tile)
		{
			foreach (var neighbor in neighborColliders)
			{
				if (neighbor.ContainedLockedTile == tile)
				{
					neighbor.ClearTileInformation();
				}
			}
		}

		/// <summary>
		/// Clears all contained tiles from this Tile's <see cref="TileNeighborCollider"/>s.
		/// </summary>
		public void ClearNeighbors()
		{
			foreach (var neighbor in neighborColliders)
			{
				if (neighbor.ContainedLockedTile != null)
				{
					neighbor.ContainedLockedTile.RemoveTileFromNeighbors(this);
				}
				neighbor.ClearTileInformation();
			}
		}

		/// <summary>
		/// Destroy this Tile.
		/// </summary>
		public virtual void Destroy()
		{
			PrefabPool.Return(this);
		}

		/// <summary>
		/// A 2D direction.
		/// </summary>
		public enum Direction
		{
			Up,
			Right,
			Down,
			Left
		}
	}
}
