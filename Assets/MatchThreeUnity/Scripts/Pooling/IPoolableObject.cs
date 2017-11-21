using UnityEngine;

namespace Assets.Scripts.Pooling
{
	/// <summary>
	/// An object which can be stored in a <see cref="PrefabPool"/>.
	/// </summary>
	public interface IPoolableObject
	{
		/// <summary>
		/// The prefab this object is based on.
		/// </summary>
		IPoolableObject Prefab { get; set; }
		/// <summary>
		/// The gameObject this IPoolableObject is connected to.
		/// </summary>
		GameObject gameObject { get; }
	}
}
