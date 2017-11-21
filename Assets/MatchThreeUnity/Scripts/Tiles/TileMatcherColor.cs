using System;

namespace Assets.MatchThreeUnity.Scripts.Tiles
{
	/// <summary>
	/// Matches a <see cref="Tile"/> based on its color.
	/// </summary>
	[Serializable]
	public class TileMatcherColor : TileMatcher
	{
		/// <summary>
		/// Determines if two Tiles share the same color.
		/// </summary>
		public override bool IsMatch(Tile first, Tile second)
		{
			if (first == null || second == null)
				return false;

			return (first.TileColor == second.TileColor);
		}
	}
}
