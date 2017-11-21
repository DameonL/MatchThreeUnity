using Assets.MatchThreeUnity.Scripts.Managers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MatchThreeUnity.Scripts.UI
{
	/// <summary>
	/// Tracks and displays combos.
	/// </summary>
	public class ComboDisplay : MonoBehaviour
	{
		[SerializeField]
		private List<ComboDefinition> comboDefinitions = new List<ComboDefinition>();

		[SerializeField]
		private float comboTimeAllowed = 1;

		[SerializeField]
		private AnimationCurve comboBonusPoints;

		[SerializeField]
		private ComboInstance comboPrefab;

		private float comboTimer;
		private ulong currentComboSize;

		private void OnEnable()
		{
			GameManager.Instance.ScoreChangedHandler += OnScoreChanged;
			comboDefinitions.Sort((ComboDefinition definition1, ComboDefinition definition2) =>
			{ return definition1.MinimumSize.CompareTo(definition2.MinimumSize); });
		}

		private void OnDisable()
		{
			if (GameManager.Instance != null)
				GameManager.Instance.ScoreChangedHandler -= OnScoreChanged;
		}

		private void OnScoreChanged(ulong points)
		{
			currentComboSize += points;
			comboTimer = comboTimeAllowed;
		}

		private void Update()
		{
			if (comboTimer > 0)
				comboTimer -= Time.deltaTime;

			if (comboTimer <= 0)
			{
				if (currentComboSize > 6)
				{
					var combo = Instantiate(comboPrefab);
					combo.transform.SetParent(transform, false);
					combo.transform.localPosition = Vector3.zero;
					ulong bonus = Convert.ToUInt64(comboBonusPoints.Evaluate(currentComboSize));
					combo.ComboType.text = GetComboLabel(bonus) + " Combo";
					combo.BonusSize.text = "+" + bonus.ToString() + " points!";
					GameManager.Instance.AddScoreSilently(bonus);
				}
				currentComboSize = 0;
			}
		}

		private string GetComboLabel(ulong bonus)
		{
			string label = null;
			for (var i = 0; i < comboDefinitions.Count; i++)
			{
				if (comboDefinitions[i].MinimumSize > bonus)
					return label;

				label = comboDefinitions[i].Name;
			}
			return label;
		}

		/// <summary>
		/// Associates the names of combos with the minimum score required to attain them.
		/// </summary>
		[Serializable]
		public class ComboDefinition
		{
			[SerializeField]
			private string name;
			public string Name { get { return name; } }

			[SerializeField]
			private ulong minimumSize;
			public ulong MinimumSize { get { return minimumSize; } }
		}

	}
}
