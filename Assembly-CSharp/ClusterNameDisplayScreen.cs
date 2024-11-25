using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BF4 RID: 3060
public class ClusterNameDisplayScreen : KScreen
{
	// Token: 0x06005D53 RID: 23891 RVA: 0x002252BA File Offset: 0x002234BA
	public static void DestroyInstance()
	{
		ClusterNameDisplayScreen.Instance = null;
	}

	// Token: 0x06005D54 RID: 23892 RVA: 0x002252C2 File Offset: 0x002234C2
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ClusterNameDisplayScreen.Instance = this;
	}

	// Token: 0x06005D55 RID: 23893 RVA: 0x002252D0 File Offset: 0x002234D0
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06005D56 RID: 23894 RVA: 0x002252D8 File Offset: 0x002234D8
	public void AddNewEntry(ClusterGridEntity representedObject)
	{
		if (this.GetEntry(representedObject) != null)
		{
			return;
		}
		ClusterNameDisplayScreen.Entry entry = new ClusterNameDisplayScreen.Entry();
		entry.grid_entity = representedObject;
		GameObject gameObject = Util.KInstantiateUI(this.nameAndBarsPrefab, base.gameObject, true);
		entry.display_go = gameObject;
		gameObject.name = representedObject.name + " cluster overlay";
		entry.Name = representedObject.name;
		entry.refs = gameObject.GetComponent<HierarchyReferences>();
		entry.bars_go = entry.refs.GetReference<RectTransform>("Bars").gameObject;
		this.m_entries.Add(entry);
		if (representedObject.GetComponent<KSelectable>() != null)
		{
			this.UpdateName(representedObject);
			this.UpdateBars(representedObject);
		}
	}

	// Token: 0x06005D57 RID: 23895 RVA: 0x00225388 File Offset: 0x00223588
	private void LateUpdate()
	{
		if (App.isLoading || App.IsExiting)
		{
			return;
		}
		int num = this.m_entries.Count;
		int i = 0;
		while (i < num)
		{
			if (this.m_entries[i].grid_entity != null && ClusterMapScreen.GetRevealLevel(this.m_entries[i].grid_entity) == ClusterRevealLevel.Visible)
			{
				Transform gridEntityNameTarget = ClusterMapScreen.Instance.GetGridEntityNameTarget(this.m_entries[i].grid_entity);
				if (gridEntityNameTarget != null)
				{
					Vector3 position = gridEntityNameTarget.GetPosition();
					this.m_entries[i].display_go.GetComponent<RectTransform>().SetPositionAndRotation(position, Quaternion.identity);
					this.m_entries[i].display_go.SetActive(this.m_entries[i].grid_entity.IsVisible && this.m_entries[i].grid_entity.ShowName());
				}
				else if (this.m_entries[i].display_go.activeSelf)
				{
					this.m_entries[i].display_go.SetActive(false);
				}
				this.UpdateBars(this.m_entries[i].grid_entity);
				if (this.m_entries[i].bars_go != null)
				{
					this.m_entries[i].bars_go.GetComponentsInChildren<KCollider2D>(false, this.workingList);
					foreach (KCollider2D kcollider2D in this.workingList)
					{
						kcollider2D.MarkDirty(false);
					}
				}
				i++;
			}
			else
			{
				UnityEngine.Object.Destroy(this.m_entries[i].display_go);
				num--;
				this.m_entries[i] = this.m_entries[num];
			}
		}
		this.m_entries.RemoveRange(num, this.m_entries.Count - num);
	}

	// Token: 0x06005D58 RID: 23896 RVA: 0x002255A0 File Offset: 0x002237A0
	public void UpdateName(ClusterGridEntity representedObject)
	{
		ClusterNameDisplayScreen.Entry entry = this.GetEntry(representedObject);
		if (entry == null)
		{
			return;
		}
		KSelectable component = representedObject.GetComponent<KSelectable>();
		entry.display_go.name = component.GetProperName() + " cluster overlay";
		LocText componentInChildren = entry.display_go.GetComponentInChildren<LocText>();
		if (componentInChildren != null)
		{
			componentInChildren.text = component.GetProperName();
		}
	}

	// Token: 0x06005D59 RID: 23897 RVA: 0x002255FC File Offset: 0x002237FC
	private void UpdateBars(ClusterGridEntity representedObject)
	{
		ClusterNameDisplayScreen.Entry entry = this.GetEntry(representedObject);
		if (entry == null)
		{
			return;
		}
		GenericUIProgressBar componentInChildren = entry.bars_go.GetComponentInChildren<GenericUIProgressBar>(true);
		if (entry.grid_entity.ShowProgressBar())
		{
			if (!componentInChildren.gameObject.activeSelf)
			{
				componentInChildren.gameObject.SetActive(true);
			}
			componentInChildren.SetFillPercentage(entry.grid_entity.GetProgress());
			return;
		}
		if (componentInChildren.gameObject.activeSelf)
		{
			componentInChildren.gameObject.SetActive(false);
		}
	}

	// Token: 0x06005D5A RID: 23898 RVA: 0x00225674 File Offset: 0x00223874
	private ClusterNameDisplayScreen.Entry GetEntry(ClusterGridEntity entity)
	{
		return this.m_entries.Find((ClusterNameDisplayScreen.Entry entry) => entry.grid_entity == entity);
	}

	// Token: 0x04003E92 RID: 16018
	public static ClusterNameDisplayScreen Instance;

	// Token: 0x04003E93 RID: 16019
	public GameObject nameAndBarsPrefab;

	// Token: 0x04003E94 RID: 16020
	[SerializeField]
	private Color selectedColor;

	// Token: 0x04003E95 RID: 16021
	[SerializeField]
	private Color defaultColor;

	// Token: 0x04003E96 RID: 16022
	private List<ClusterNameDisplayScreen.Entry> m_entries = new List<ClusterNameDisplayScreen.Entry>();

	// Token: 0x04003E97 RID: 16023
	private List<KCollider2D> workingList = new List<KCollider2D>();

	// Token: 0x02001CDA RID: 7386
	private class Entry
	{
		// Token: 0x0400854D RID: 34125
		public string Name;

		// Token: 0x0400854E RID: 34126
		public ClusterGridEntity grid_entity;

		// Token: 0x0400854F RID: 34127
		public GameObject display_go;

		// Token: 0x04008550 RID: 34128
		public GameObject bars_go;

		// Token: 0x04008551 RID: 34129
		public HierarchyReferences refs;
	}
}
