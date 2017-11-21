using System;
using UnityEngine;

namespace Assets.MatchThreeUnity.Scripts.Tiles
{
	/// <summary>
	/// Match behaviour for <see cref="Tile"/> objects.
	/// </summary>
	[Serializable]
	[DisallowMultipleComponent]
	public abstract class TileMatcher : MonoBehaviour
	{
		/// <summary>
		/// Checks two tiles to determine if this TileMatcher considers them a match.
		/// </summary>
		public abstract bool IsMatch(Tile first, Tile second);
	}
}
