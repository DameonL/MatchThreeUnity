using UnityEngine;

namespace Assets.MatchThreeUnity.Scripts.Tiles
{
	/// <summary>
	/// Detects the use of a mouse to "swipe" a tile in a direction.
	/// </summary>
	[RequireComponent(typeof(LineRenderer))]
	public class TileSwitcher : MonoBehaviour
	{
		[SerializeField]
		private float minDelta = 25;

		private Vector2 downCoordinates;
		private Tile firstTile;
		private Vector3[] positions = new Vector3[2];

		private LineRenderer lineRenderer;

		private void Awake()
		{
			lineRenderer = GetComponent<LineRenderer>();
			lineRenderer.enabled = false;
		}

		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				downCoordinates = Input.mousePosition;
				Vector3 clickPosition = Camera.main.ScreenToWorldPoint(new Vector3(downCoordinates.x, downCoordinates.y, -Camera.main.transform.position.z));
				var hit = Physics2D.OverlapBox(clickPosition, new Vector2(0.1f, 0.1f), 0, LayerMask.GetMask("Tiles"));
				clickPosition.z = -1;
				positions[0] = clickPosition;
				lineRenderer.enabled = true;
				firstTile = null;
				Tile hitTile = hit?.GetComponent<Tile>();

				if (hitTile == null || hitTile.IsFalling)
					return;

				firstTile = hitTile;
			}

			if (Input.GetMouseButton(0))
			{
				Vector3 clickPosition = Camera.main.ScreenToWorldPoint(new Vector3(downCoordinates.x, downCoordinates.y, -Camera.main.transform.position.z - 1));
				positions[1] = clickPosition;
				lineRenderer.SetPositions(positions);
			}

			if (Input.GetMouseButtonUp(0))
			{
				lineRenderer.enabled = false;

				if (firstTile == null)
					return;

				int direction = 0;
				Vector2 delta = (Vector2)Input.mousePosition - downCoordinates;
				if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
				{
					if (Mathf.Abs(delta.x) < minDelta)
						return;

					if (delta.x < 0)
						direction = (int)Tile.Direction.Left;
					else
						direction = (int)Tile.Direction.Right;
				}
				else
				{
					if (Mathf.Abs(delta.y) < minDelta)
						return;

					if (delta.y < 0)
						direction = (int)Tile.Direction.Down;
					else
						direction = (int)Tile.Direction.Up;
				}

				Tile secondTile = firstTile.Neighbors[direction].ContainedLockedTile;
				if (secondTile == null)
					return;

				firstTile.ClearNeighbors();
				secondTile.ClearNeighbors();
				Vector3 cachedPosition = firstTile.transform.position;
				firstTile.transform.position = secondTile.transform.position;
				secondTile.transform.position = cachedPosition;
				firstTile = null;
			}
		}

	}
}
