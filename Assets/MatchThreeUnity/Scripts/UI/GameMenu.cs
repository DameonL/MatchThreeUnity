using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.MatchThreeUnity.Scripts.UI
{
	public class GameMenu : MonoBehaviour
	{
		[SerializeField]
		private int gameScene;

		public void NewGameButton()
		{
			SceneManager.LoadScene(gameScene);
		}
	}
}
