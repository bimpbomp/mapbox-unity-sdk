using System;
using System.Collections.Generic;

namespace Mapbox.Unity.MeshGeneration.Data
{
	public class ObjectPool<T>
	{
		private readonly Func<T> _objectGenerator;
		private readonly Queue<T> _objects;

		public ObjectPool(Func<T> objectGenerator)
		{
			if (objectGenerator == null) throw new ArgumentNullException("objectGenerator");
			_objects = new Queue<T>();
			_objectGenerator = objectGenerator;
		}

		public T GetObject()
		{
			if (_objects.Count > 0)
				return _objects.Dequeue();
			return _objectGenerator();
		}

		public void Put(T item)
		{
			_objects.Enqueue(item);
		}

		public void Clear()
		{
			_objects.Clear();
		}

		public IEnumerable<T> GetQueue()
		{
			return _objects;
		}
	}
}
