using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Pooling
{
	/// <summary>
	/// Used to easily pool Gameobjects based on prefabs.
	/// </summary>
	public class PrefabPool
	{
		private static Dictionary<IPoolableObject, Queue<IPoolableObject>> pools = new Dictionary<IPoolableObject, Queue<IPoolableObject>>();

		private static Transform poolContainer;

		public static void Reset()
		{
			if (poolContainer != null)
				Object.Destroy(poolContainer.gameObject);
			poolContainer = null;
			pools = new Dictionary<IPoolableObject, Queue<IPoolableObject>>();
		}

		public static IPoolableObject Get(IPoolableObject prefab)
		{
			IPoolableObject item;
			if (!pools.ContainsKey(prefab))
			{
				pools.Add(prefab, new Queue<IPoolableObject>());
			}

			if (pools[prefab].Count > 0)
			{
				item = pools[prefab].Dequeue();
				item.gameObject.transform.parent = null;
			}
			else
			{
				item = GameObject.Instantiate(prefab.gameObject).GetComponent<IPoolableObject>();
				item.Prefab = prefab;
			}

			return item;
		}

		public static void Return(IPoolableObject itemToReturn)
		{
			if (poolContainer == null)
			{
				poolContainer = new GameObject("Pool container").transform;
				poolContainer.gameObject.SetActive(false);
			}

			if (!pools.ContainsKey(itemToReturn.Prefab))
				pools.Add(itemToReturn.Prefab, new Queue<IPoolableObject>());

			itemToReturn.gameObject.transform.SetParent(poolContainer);
			itemToReturn.gameObject.SetActive(false);
			pools[itemToReturn.Prefab].Enqueue(itemToReturn);
		}


	}
}