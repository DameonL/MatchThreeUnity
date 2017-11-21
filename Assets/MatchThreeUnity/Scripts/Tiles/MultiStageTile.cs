using System.Collections.Generic;
using UnityEngine;

namespace Assets.MatchThreeUnity.Scripts.Tiles
{
	/// <summary>
	/// A tile that must be matched multiple times to destroy it.
	/// </summary>
	public class MultiStageTile : Tile
	{
		[SerializeField]
		private List<Material> stages = new List<Material>();

		private int currentStage;

		/// <summary>
		/// If the tile has passed its stage, destroys it.
		/// Otherwise, progresses the tile to the next stage.
		/// </summary>
		public override void Destroy()
		{
			if (currentStage < stages.Count)
			{
				GetComponent<Renderer>().material = stages[currentStage];
				TileColor = stages[currentStage].color;
				currentStage++;
			}
			else
			{
				base.Destroy();
			}
		}
	}
}
