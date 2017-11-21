using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MatchThreeUnity.Scripts.Tiles
{
	[Serializable]
	[RequireComponent(typeof(BoxCollider2D))]
	public class TileNeighborCollider : MonoBehaviour
	{
		[SerializeField]
		private List<Tile> containedTiles = new List<Tile>();

		[SerializeField]
		private Tile attachedTile;

		[SerializeField]
		private Tile containedLockedTile;
		/// <summary>
		/// The tile which is locked into place in this collider.
		/// </summary>
		public Tile ContainedLockedTile { get { return containedLockedTile; } }

		private void Awake()
		{
			attachedTile = GetComponentInParent<Tile>();
			if (attachedTile == null)
			{
				throw new NullReferenceException("TileNeighborCollider must have its attachedTile set manually, or be the child of a Tile.");
			}
		}

		private void FixedUpdate()
		{
			if (attachedTile.IsFalling)
			{
				containedLockedTile = null;
				return;
			}

			if (containedLockedTile != null && containedLockedTile.IsFalling)
			{
				containedLockedTile = null;
			}

			foreach (var tile in containedTiles)
			{
				if (!tile.IsFalling)
				{
					containedLockedTile = tile;
					break;
				}
			}
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			var tile = collision.GetComponent<Tile>();
			if (tile != null)
			{
				containedTiles.Add(tile);
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			var tile = collision.GetComponent<Tile>();
			if (tile != null)
			{
				containedTiles.Remove(tile);
			}
		}

		/// <summary>
		/// Clears the locked tile from this collider, and clears information about any contained tiles.
		/// </summary>
		public void ClearTileInformation()
		{
			containedLockedTile = null;
			containedTiles.Clear();
		}
	}
}
