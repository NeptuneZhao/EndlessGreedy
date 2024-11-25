using System;
using UnityEngine;

// Token: 0x02000C6C RID: 3180
[AddComponentMenu("KMonoBehaviour/scripts/InstantiateUIPrefabChild")]
public class InstantiateUIPrefabChild : KMonoBehaviour
{
	// Token: 0x06006193 RID: 24979 RVA: 0x002451EB File Offset: 0x002433EB
	protected override void OnPrefabInit()
	{
		if (this.InstantiateOnAwake)
		{
			this.Instantiate();
		}
	}

	// Token: 0x06006194 RID: 24980 RVA: 0x002451FC File Offset: 0x002433FC
	public void Instantiate()
	{
		if (this.alreadyInstantiated)
		{
			global::Debug.LogWarning(base.gameObject.name + "trying to instantiate UI prefabs multiple times.");
			return;
		}
		this.alreadyInstantiated = true;
		foreach (GameObject gameObject in this.prefabs)
		{
			if (!(gameObject == null))
			{
				Vector3 v = gameObject.rectTransform().anchoredPosition;
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
				gameObject2.transform.SetParent(base.transform);
				gameObject2.rectTransform().anchoredPosition = v;
				gameObject2.rectTransform().localScale = Vector3.one;
				if (this.setAsFirstSibling)
				{
					gameObject2.transform.SetAsFirstSibling();
				}
			}
		}
	}

	// Token: 0x04004227 RID: 16935
	public GameObject[] prefabs;

	// Token: 0x04004228 RID: 16936
	public bool InstantiateOnAwake = true;

	// Token: 0x04004229 RID: 16937
	private bool alreadyInstantiated;

	// Token: 0x0400422A RID: 16938
	public bool setAsFirstSibling;
}
