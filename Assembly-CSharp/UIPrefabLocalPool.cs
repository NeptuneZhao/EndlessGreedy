using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200041C RID: 1052
public class UIPrefabLocalPool
{
	// Token: 0x06001655 RID: 5717 RVA: 0x000785DC File Offset: 0x000767DC
	public UIPrefabLocalPool(GameObject sourcePrefab, GameObject parent)
	{
		this.sourcePrefab = sourcePrefab;
		this.parent = parent;
	}

	// Token: 0x06001656 RID: 5718 RVA: 0x00078608 File Offset: 0x00076808
	public GameObject Borrow()
	{
		GameObject gameObject;
		if (this.availableInstances.Count == 0)
		{
			gameObject = Util.KInstantiateUI(this.sourcePrefab, this.parent, true);
		}
		else
		{
			gameObject = this.availableInstances.First<KeyValuePair<int, GameObject>>().Value;
			this.availableInstances.Remove(gameObject.GetInstanceID());
		}
		this.checkedOutInstances.Add(gameObject.GetInstanceID(), gameObject);
		gameObject.SetActive(true);
		gameObject.transform.SetAsLastSibling();
		return gameObject;
	}

	// Token: 0x06001657 RID: 5719 RVA: 0x00078682 File Offset: 0x00076882
	public void Return(GameObject instance)
	{
		this.checkedOutInstances.Remove(instance.GetInstanceID());
		this.availableInstances.Add(instance.GetInstanceID(), instance);
		instance.SetActive(false);
	}

	// Token: 0x06001658 RID: 5720 RVA: 0x000786B0 File Offset: 0x000768B0
	public void ReturnAll()
	{
		foreach (KeyValuePair<int, GameObject> keyValuePair in this.checkedOutInstances)
		{
			int num;
			GameObject gameObject;
			keyValuePair.Deconstruct(out num, out gameObject);
			int key = num;
			GameObject gameObject2 = gameObject;
			this.availableInstances.Add(key, gameObject2);
			gameObject2.SetActive(false);
		}
		this.checkedOutInstances.Clear();
	}

	// Token: 0x06001659 RID: 5721 RVA: 0x0007872C File Offset: 0x0007692C
	public IEnumerable<GameObject> GetBorrowedObjects()
	{
		return this.checkedOutInstances.Values;
	}

	// Token: 0x04000C8B RID: 3211
	public readonly GameObject sourcePrefab;

	// Token: 0x04000C8C RID: 3212
	public readonly GameObject parent;

	// Token: 0x04000C8D RID: 3213
	private Dictionary<int, GameObject> checkedOutInstances = new Dictionary<int, GameObject>();

	// Token: 0x04000C8E RID: 3214
	private Dictionary<int, GameObject> availableInstances = new Dictionary<int, GameObject>();
}
