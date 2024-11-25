using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B81 RID: 2945
public class UIGameObjectPool
{
	// Token: 0x170006B0 RID: 1712
	// (get) Token: 0x06005882 RID: 22658 RVA: 0x001FEB8D File Offset: 0x001FCD8D
	public int ActiveElementsCount
	{
		get
		{
			return this.activeElements.Count;
		}
	}

	// Token: 0x170006B1 RID: 1713
	// (get) Token: 0x06005883 RID: 22659 RVA: 0x001FEB9A File Offset: 0x001FCD9A
	public int FreeElementsCount
	{
		get
		{
			return this.freeElements.Count;
		}
	}

	// Token: 0x170006B2 RID: 1714
	// (get) Token: 0x06005884 RID: 22660 RVA: 0x001FEBA7 File Offset: 0x001FCDA7
	public int TotalElementsCount
	{
		get
		{
			return this.ActiveElementsCount + this.FreeElementsCount;
		}
	}

	// Token: 0x06005885 RID: 22661 RVA: 0x001FEBB6 File Offset: 0x001FCDB6
	public UIGameObjectPool(GameObject prefab)
	{
		this.prefab = prefab;
		this.freeElements = new List<GameObject>();
		this.activeElements = new List<GameObject>();
	}

	// Token: 0x06005886 RID: 22662 RVA: 0x001FEBF4 File Offset: 0x001FCDF4
	public GameObject GetFreeElement(GameObject instantiateParent = null, bool forceActive = false)
	{
		if (this.freeElements.Count == 0)
		{
			this.activeElements.Add(Util.KInstantiateUI(this.prefab.gameObject, instantiateParent, false));
		}
		else
		{
			GameObject gameObject = this.freeElements[0];
			this.activeElements.Add(gameObject);
			if (gameObject.transform.parent != instantiateParent)
			{
				gameObject.transform.SetParent(instantiateParent.transform);
			}
			this.freeElements.RemoveAt(0);
		}
		GameObject gameObject2 = this.activeElements[this.activeElements.Count - 1];
		if (gameObject2.gameObject.activeInHierarchy != forceActive)
		{
			gameObject2.gameObject.SetActive(forceActive);
		}
		return gameObject2;
	}

	// Token: 0x06005887 RID: 22663 RVA: 0x001FECAC File Offset: 0x001FCEAC
	public void ClearElement(GameObject element)
	{
		if (!this.activeElements.Contains(element))
		{
			object obj = this.freeElements.Contains(element) ? (element.name + ": The element provided is already inactive") : (element.name + ": The element provided does not belong to this pool");
			element.SetActive(false);
			if (this.disabledElementParent != null)
			{
				element.transform.SetParent(this.disabledElementParent);
			}
			global::Debug.LogError(obj);
			return;
		}
		if (this.disabledElementParent != null)
		{
			element.transform.SetParent(this.disabledElementParent);
		}
		element.SetActive(false);
		this.freeElements.Add(element);
		this.activeElements.Remove(element);
	}

	// Token: 0x06005888 RID: 22664 RVA: 0x001FED64 File Offset: 0x001FCF64
	public void ClearAll()
	{
		while (this.activeElements.Count > 0)
		{
			if (this.disabledElementParent != null)
			{
				this.activeElements[0].transform.SetParent(this.disabledElementParent);
			}
			this.activeElements[0].SetActive(false);
			this.freeElements.Add(this.activeElements[0]);
			this.activeElements.RemoveAt(0);
		}
	}

	// Token: 0x06005889 RID: 22665 RVA: 0x001FEDE0 File Offset: 0x001FCFE0
	public void DestroyAll()
	{
		this.DestroyAllActive();
		this.DestroyAllFree();
	}

	// Token: 0x0600588A RID: 22666 RVA: 0x001FEDEE File Offset: 0x001FCFEE
	public void DestroyAllActive()
	{
		this.activeElements.ForEach(delegate(GameObject ae)
		{
			UnityEngine.Object.Destroy(ae);
		});
		this.activeElements.Clear();
	}

	// Token: 0x0600588B RID: 22667 RVA: 0x001FEE25 File Offset: 0x001FD025
	public void DestroyAllFree()
	{
		this.freeElements.ForEach(delegate(GameObject ae)
		{
			UnityEngine.Object.Destroy(ae);
		});
		this.freeElements.Clear();
	}

	// Token: 0x0600588C RID: 22668 RVA: 0x001FEE5C File Offset: 0x001FD05C
	public void ForEachActiveElement(Action<GameObject> predicate)
	{
		this.activeElements.ForEach(predicate);
	}

	// Token: 0x0600588D RID: 22669 RVA: 0x001FEE6A File Offset: 0x001FD06A
	public void ForEachFreeElement(Action<GameObject> predicate)
	{
		this.freeElements.ForEach(predicate);
	}

	// Token: 0x04003A28 RID: 14888
	private GameObject prefab;

	// Token: 0x04003A29 RID: 14889
	private List<GameObject> freeElements = new List<GameObject>();

	// Token: 0x04003A2A RID: 14890
	private List<GameObject> activeElements = new List<GameObject>();

	// Token: 0x04003A2B RID: 14891
	public Transform disabledElementParent;
}
