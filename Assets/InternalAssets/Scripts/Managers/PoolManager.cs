using System.Collections.Generic;
using UnityEngine;
using Zenject;

public static class PoolManager
{
    private static List<Part> pools;

    struct Part 
	{
		public string name;
		public List<PoolComponent> prefab;
		public bool resize;
		public Transform parent;
	}

	public static void CreatePool(PoolComponent sample, string name, int count, bool autoResize)
	{
		if(pools == null || count <= 0 || name.Trim() == string.Empty || sample == null) return;

		Part p = new Part();
		p.prefab = new List<PoolComponent>();
		p.name = name;
		p.resize = autoResize;
		p.parent = new GameObject("Pool-" + name).transform;

		for(int i = 0; i < count; i++)
		{
			p.prefab.Add(AddObject(sample, name, i, p.parent));
		}

		pools.Add(p);
	}

	static PoolComponent AddObject(PoolComponent sample, string name, int index, Transform parent)
	{
		PoolComponent comp = GameObject.Instantiate(sample) as PoolComponent;
		comp.gameObject.name = name + "-" + index;
		comp.transform.parent = parent;
		comp.gameObject.SetActive(false);
		return comp;
	}

	static void AutoResize(Part part, int index)
	{
		part.prefab.Add(AddObject(part.prefab[0], part.name, index, part.parent));
	}

	public static PoolComponent GetPoolObject(string name, Vector3 position, Quaternion rotation)
	{
		if(pools == null) return null;

		foreach(Part part in pools)
		{
			if(string.Compare(part.name, name) == 0)
			{
				foreach(PoolComponent comp in part.prefab)
				{
					if (comp.isActiveAndEnabled) continue;
					comp.transform.rotation = rotation;
					comp.transform.position = position;
					comp.gameObject.SetActive(true);
					return comp;
				}

				if (!part.resize) continue;
				AutoResize(part, part.prefab.Count);
				return part.prefab[part.prefab.Count-1];
			}
		}

		return null;
	}

	public static void ReturnPoolObject(string name)
	{
		foreach (Part part in pools)
		{
			if (string.Compare(part.name, name) == 0)
			{
				foreach (PoolComponent poolComponent in part.prefab)
				{
					poolComponent.gameObject.SetActive(false);
				}
			}
		}
	}
	
	public static void DestroyPool(string name)
	{
		if(pools == null) return;

		var j = 0;

		foreach(Part p in pools)
		{
			if(string.Compare(p.name, name) == 0)
			{
				GameObject.Destroy(p.parent.gameObject);
				pools.RemoveAt(j);
				return;
			}

			j++;
		}
	}

	public static void Initialize()
	{
		pools = new List<Part>();
	}
}
