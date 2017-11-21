using UnityEngine;

namespace Assets.Scripts.Pooling
{
	public class PoolableObject : MonoBehaviour, IPoolableObject
	{
		public IPoolableObject Prefab { get; set; }

		public virtual void Destroy()
		{
			PrefabPool.Return(this);
		}
	}
}
