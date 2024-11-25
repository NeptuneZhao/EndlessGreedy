using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D38 RID: 3384
[AddComponentMenu("KMonoBehaviour/scripts/ScheduledUIInstantiation")]
public class ScheduledUIInstantiation : KMonoBehaviour
{
	// Token: 0x06006A75 RID: 27253 RVA: 0x00281A7E File Offset: 0x0027FC7E
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.InstantiateOnAwake)
		{
			this.InstantiateElements(null);
			return;
		}
		Game.Instance.Subscribe((int)this.InstantiationEvent, new Action<object>(this.InstantiateElements));
	}

	// Token: 0x06006A76 RID: 27254 RVA: 0x00281AB4 File Offset: 0x0027FCB4
	public void InstantiateElements(object data)
	{
		if (this.completed)
		{
			return;
		}
		this.completed = true;
		foreach (ScheduledUIInstantiation.Instantiation instantiation in this.UIElements)
		{
			if (SaveLoader.Instance.IsDLCActiveForCurrentSave(instantiation.RequiredDlcId))
			{
				foreach (GameObject gameObject in instantiation.prefabs)
				{
					Vector3 v = gameObject.rectTransform().anchoredPosition;
					GameObject gameObject2 = Util.KInstantiateUI(gameObject, instantiation.parent.gameObject, false);
					gameObject2.rectTransform().anchoredPosition = v;
					gameObject2.rectTransform().localScale = Vector3.one;
					this.instantiatedObjects.Add(gameObject2);
				}
			}
		}
		if (!this.InstantiateOnAwake)
		{
			base.Unsubscribe((int)this.InstantiationEvent, new Action<object>(this.InstantiateElements));
		}
	}

	// Token: 0x06006A77 RID: 27255 RVA: 0x00281B9C File Offset: 0x0027FD9C
	public T GetInstantiatedObject<T>() where T : Component
	{
		for (int i = 0; i < this.instantiatedObjects.Count; i++)
		{
			if (this.instantiatedObjects[i].GetComponent(typeof(T)) != null)
			{
				return this.instantiatedObjects[i].GetComponent(typeof(T)) as T;
			}
		}
		return default(T);
	}

	// Token: 0x0400488D RID: 18573
	public ScheduledUIInstantiation.Instantiation[] UIElements;

	// Token: 0x0400488E RID: 18574
	public bool InstantiateOnAwake;

	// Token: 0x0400488F RID: 18575
	public GameHashes InstantiationEvent = GameHashes.StartGameUser;

	// Token: 0x04004890 RID: 18576
	private bool completed;

	// Token: 0x04004891 RID: 18577
	private List<GameObject> instantiatedObjects = new List<GameObject>();

	// Token: 0x02001E74 RID: 7796
	[Serializable]
	public struct Instantiation
	{
		// Token: 0x04008AA1 RID: 35489
		public string Name;

		// Token: 0x04008AA2 RID: 35490
		public string Comment;

		// Token: 0x04008AA3 RID: 35491
		public GameObject[] prefabs;

		// Token: 0x04008AA4 RID: 35492
		public Transform parent;

		// Token: 0x04008AA5 RID: 35493
		public string RequiredDlcId;
	}
}
