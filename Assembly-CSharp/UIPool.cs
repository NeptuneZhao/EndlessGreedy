using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B82 RID: 2946
public class UIPool<T> where T : MonoBehaviour
{
	// Token: 0x170006B3 RID: 1715
	// (get) Token: 0x0600588E RID: 22670 RVA: 0x001FEE78 File Offset: 0x001FD078
	public int ActiveElementsCount
	{
		get
		{
			return this.activeElements.Count;
		}
	}

	// Token: 0x170006B4 RID: 1716
	// (get) Token: 0x0600588F RID: 22671 RVA: 0x001FEE85 File Offset: 0x001FD085
	public int FreeElementsCount
	{
		get
		{
			return this.freeElements.Count;
		}
	}

	// Token: 0x170006B5 RID: 1717
	// (get) Token: 0x06005890 RID: 22672 RVA: 0x001FEE92 File Offset: 0x001FD092
	public int TotalElementsCount
	{
		get
		{
			return this.ActiveElementsCount + this.FreeElementsCount;
		}
	}

	// Token: 0x06005891 RID: 22673 RVA: 0x001FEEA1 File Offset: 0x001FD0A1
	public UIPool(T prefab)
	{
		this.prefab = prefab;
		this.freeElements = new List<T>();
		this.activeElements = new List<T>();
	}

	// Token: 0x06005892 RID: 22674 RVA: 0x001FEEDC File Offset: 0x001FD0DC
	public T GetFreeElement(GameObject instantiateParent = null, bool forceActive = false)
	{
		if (this.freeElements.Count == 0)
		{
			this.activeElements.Add(Util.KInstantiateUI<T>(this.prefab.gameObject, instantiateParent, false));
		}
		else
		{
			T t = this.freeElements[0];
			this.activeElements.Add(t);
			if (t.transform.parent != instantiateParent)
			{
				t.transform.SetParent(instantiateParent.transform);
			}
			this.freeElements.RemoveAt(0);
		}
		T t2 = this.activeElements[this.activeElements.Count - 1];
		if (t2.gameObject.activeInHierarchy != forceActive)
		{
			t2.gameObject.SetActive(forceActive);
		}
		return t2;
	}

	// Token: 0x06005893 RID: 22675 RVA: 0x001FEFAC File Offset: 0x001FD1AC
	public void ClearElement(T element)
	{
		if (!this.activeElements.Contains(element))
		{
			global::Debug.LogError(this.freeElements.Contains(element) ? "The element provided is already inactive" : "The element provided does not belong to this pool");
			return;
		}
		if (this.disabledElementParent != null)
		{
			element.gameObject.transform.SetParent(this.disabledElementParent);
		}
		element.gameObject.SetActive(false);
		this.freeElements.Add(element);
		this.activeElements.Remove(element);
	}

	// Token: 0x06005894 RID: 22676 RVA: 0x001FF03C File Offset: 0x001FD23C
	public void ClearAll()
	{
		while (this.activeElements.Count > 0)
		{
			if (this.disabledElementParent != null)
			{
				this.activeElements[0].gameObject.transform.SetParent(this.disabledElementParent);
			}
			this.activeElements[0].gameObject.SetActive(false);
			this.freeElements.Add(this.activeElements[0]);
			this.activeElements.RemoveAt(0);
		}
	}

	// Token: 0x06005895 RID: 22677 RVA: 0x001FF0CF File Offset: 0x001FD2CF
	public void DestroyAll()
	{
		this.DestroyAllActive();
		this.DestroyAllFree();
	}

	// Token: 0x06005896 RID: 22678 RVA: 0x001FF0DD File Offset: 0x001FD2DD
	public void DestroyAllActive()
	{
		this.activeElements.ForEach(delegate(T ae)
		{
			UnityEngine.Object.Destroy(ae.gameObject);
		});
		this.activeElements.Clear();
	}

	// Token: 0x06005897 RID: 22679 RVA: 0x001FF114 File Offset: 0x001FD314
	public void DestroyAllFree()
	{
		this.freeElements.ForEach(delegate(T ae)
		{
			UnityEngine.Object.Destroy(ae.gameObject);
		});
		this.freeElements.Clear();
	}

	// Token: 0x06005898 RID: 22680 RVA: 0x001FF14B File Offset: 0x001FD34B
	public void ForEachActiveElement(Action<T> predicate)
	{
		this.activeElements.ForEach(predicate);
	}

	// Token: 0x06005899 RID: 22681 RVA: 0x001FF159 File Offset: 0x001FD359
	public void ForEachFreeElement(Action<T> predicate)
	{
		this.freeElements.ForEach(predicate);
	}

	// Token: 0x04003A2C RID: 14892
	private T prefab;

	// Token: 0x04003A2D RID: 14893
	private List<T> freeElements = new List<T>();

	// Token: 0x04003A2E RID: 14894
	private List<T> activeElements = new List<T>();

	// Token: 0x04003A2F RID: 14895
	public Transform disabledElementParent;
}
