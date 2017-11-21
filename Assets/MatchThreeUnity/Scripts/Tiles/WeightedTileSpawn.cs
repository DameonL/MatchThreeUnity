using System;
using UnityEngine;

namespace Assets.MatchThreeUnity.Scripts.Tiles
{
	/// <summary>
	/// A weighted reference to a Tile prefab.
	/// </summary>
	[Serializable]
	public class WeightedTileSpawn
	{
		[SerializeField]
		private Tile _prefab;
		public Tile Prefab { get { return _prefab; } }

		[SerializeField]
		private float _weight = 1;
		public float Weight { get { return _weight; } }
	}
}
