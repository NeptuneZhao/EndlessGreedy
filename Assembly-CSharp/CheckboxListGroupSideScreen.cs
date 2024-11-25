using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D54 RID: 3412
public class CheckboxListGroupSideScreen : SideScreenContent
{
	// Token: 0x06006B64 RID: 27492 RVA: 0x002864D3 File Offset: 0x002846D3
	private CheckboxListGroupSideScreen.CheckboxContainer InstantiateCheckboxContainer()
	{
		return new CheckboxListGroupSideScreen.CheckboxContainer(Util.KInstantiateUI(this.checkboxGroupPrefab, this.groupParent.gameObject, true).GetComponent<HierarchyReferences>());
	}

	// Token: 0x06006B65 RID: 27493 RVA: 0x002864F6 File Offset: 0x002846F6
	private GameObject InstantiateCheckbox()
	{
		return Util.KInstantiateUI(this.checkboxPrefab, this.checkboxParent.gameObject, false);
	}

	// Token: 0x06006B66 RID: 27494 RVA: 0x0028650F File Offset: 0x0028470F
	protected override void OnSpawn()
	{
		this.checkboxPrefab.SetActive(false);
		this.checkboxGroupPrefab.SetActive(false);
		base.OnSpawn();
	}

	// Token: 0x06006B67 RID: 27495 RVA: 0x00286530 File Offset: 0x00284730
	public override bool IsValidForTarget(GameObject target)
	{
		ICheckboxListGroupControl[] components = target.GetComponents<ICheckboxListGroupControl>();
		if (components != null)
		{
			ICheckboxListGroupControl[] array = components;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].SidescreenEnabled())
				{
					return true;
				}
			}
		}
		using (List<ICheckboxListGroupControl>.Enumerator enumerator = target.GetAllSMI<ICheckboxListGroupControl>().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.SidescreenEnabled())
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06006B68 RID: 27496 RVA: 0x002865B4 File Offset: 0x002847B4
	public override int GetSideScreenSortOrder()
	{
		if (this.targets == null)
		{
			return 20;
		}
		return this.targets[0].CheckboxSideScreenSortOrder();
	}

	// Token: 0x06006B69 RID: 27497 RVA: 0x002865D4 File Offset: 0x002847D4
	public override void SetTarget(GameObject target)
	{
		if (target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.targets = target.GetAllSMI<ICheckboxListGroupControl>();
		this.targets.AddRange(target.GetComponents<ICheckboxListGroupControl>());
		this.Rebuild(target);
		this.uiRefreshSubHandle = this.currentBuildTarget.Subscribe(1980521255, new Action<object>(this.Refresh));
	}

	// Token: 0x06006B6A RID: 27498 RVA: 0x0028663C File Offset: 0x0028483C
	public override void ClearTarget()
	{
		if (this.uiRefreshSubHandle != -1 && this.currentBuildTarget != null)
		{
			this.currentBuildTarget.Unsubscribe(this.uiRefreshSubHandle);
			this.uiRefreshSubHandle = -1;
		}
		this.ReleaseContainers(this.activeChecklistGroups.Count);
	}

	// Token: 0x06006B6B RID: 27499 RVA: 0x00286689 File Offset: 0x00284889
	public override string GetTitle()
	{
		if (this.targets != null && this.targets.Count > 0 && this.targets[0] != null)
		{
			return this.targets[0].Title;
		}
		return base.GetTitle();
	}

	// Token: 0x06006B6C RID: 27500 RVA: 0x002866C8 File Offset: 0x002848C8
	private void Rebuild(GameObject buildTarget)
	{
		if (this.checkboxContainerPool == null)
		{
			this.checkboxContainerPool = new ObjectPool<CheckboxListGroupSideScreen.CheckboxContainer>(new Func<CheckboxListGroupSideScreen.CheckboxContainer>(this.InstantiateCheckboxContainer), 0);
			this.checkboxPool = new GameObjectPool(new Func<GameObject>(this.InstantiateCheckbox), 0);
		}
		this.descriptionLabel.enabled = !this.targets[0].Description.IsNullOrWhiteSpace();
		if (!this.targets[0].Description.IsNullOrWhiteSpace())
		{
			this.descriptionLabel.SetText(this.targets[0].Description);
		}
		if (buildTarget == this.currentBuildTarget)
		{
			this.Refresh(null);
			return;
		}
		this.currentBuildTarget = buildTarget;
		foreach (ICheckboxListGroupControl checkboxListGroupControl in this.targets)
		{
			foreach (ICheckboxListGroupControl.ListGroup group in checkboxListGroupControl.GetData())
			{
				CheckboxListGroupSideScreen.CheckboxContainer instance = this.checkboxContainerPool.GetInstance();
				this.InitContainer(checkboxListGroupControl, group, instance);
			}
		}
	}

	// Token: 0x06006B6D RID: 27501 RVA: 0x002867F8 File Offset: 0x002849F8
	[ContextMenu("Force refresh")]
	private void Test()
	{
		this.Refresh(null);
	}

	// Token: 0x06006B6E RID: 27502 RVA: 0x00286804 File Offset: 0x00284A04
	private void Refresh(object data = null)
	{
		int num = 0;
		foreach (ICheckboxListGroupControl checkboxListGroupControl in this.targets)
		{
			foreach (ICheckboxListGroupControl.ListGroup listGroup in checkboxListGroupControl.GetData())
			{
				if (++num > this.activeChecklistGroups.Count)
				{
					this.InitContainer(checkboxListGroupControl, listGroup, this.checkboxContainerPool.GetInstance());
				}
				CheckboxListGroupSideScreen.CheckboxContainer checkboxContainer = this.activeChecklistGroups[num - 1];
				if (listGroup.resolveTitleCallback != null)
				{
					checkboxContainer.container.GetReference<LocText>("Text").SetText(listGroup.resolveTitleCallback(listGroup.title));
				}
				for (int j = 0; j < listGroup.checkboxItems.Length; j++)
				{
					ICheckboxListGroupControl.CheckboxItem data3 = listGroup.checkboxItems[j];
					if (checkboxContainer.checkboxUIItems.Count <= j)
					{
						this.CreateSingleCheckBoxForGroupUI(checkboxContainer);
					}
					HierarchyReferences checkboxUI = checkboxContainer.checkboxUIItems[j];
					this.SetCheckboxData(checkboxUI, data3, checkboxListGroupControl);
				}
				while (checkboxContainer.checkboxUIItems.Count > listGroup.checkboxItems.Length)
				{
					HierarchyReferences checkbox = checkboxContainer.checkboxUIItems[checkboxContainer.checkboxUIItems.Count - 1];
					this.RemoveSingleCheckboxFromContainer(checkbox, checkboxContainer);
				}
			}
		}
		this.ReleaseContainers(this.activeChecklistGroups.Count - num);
	}

	// Token: 0x06006B6F RID: 27503 RVA: 0x002869A4 File Offset: 0x00284BA4
	private void ReleaseContainers(int count)
	{
		int count2 = this.activeChecklistGroups.Count;
		for (int i = 1; i <= count; i++)
		{
			int index = count2 - i;
			CheckboxListGroupSideScreen.CheckboxContainer checkboxContainer = this.activeChecklistGroups[index];
			this.activeChecklistGroups.RemoveAt(index);
			for (int j = checkboxContainer.checkboxUIItems.Count - 1; j >= 0; j--)
			{
				HierarchyReferences checkbox = checkboxContainer.checkboxUIItems[j];
				this.RemoveSingleCheckboxFromContainer(checkbox, checkboxContainer);
			}
			checkboxContainer.container.gameObject.SetActive(false);
			this.checkboxContainerPool.ReleaseInstance(checkboxContainer);
		}
	}

	// Token: 0x06006B70 RID: 27504 RVA: 0x00286A38 File Offset: 0x00284C38
	private void InitContainer(ICheckboxListGroupControl target, ICheckboxListGroupControl.ListGroup group, CheckboxListGroupSideScreen.CheckboxContainer groupUI)
	{
		this.activeChecklistGroups.Add(groupUI);
		groupUI.container.gameObject.SetActive(true);
		string text = group.title;
		if (group.resolveTitleCallback != null)
		{
			text = group.resolveTitleCallback(text);
		}
		groupUI.container.GetReference<LocText>("Text").SetText(text);
		foreach (ICheckboxListGroupControl.CheckboxItem data in group.checkboxItems)
		{
			this.CreateSingleCheckBoxForGroupUI(data, target, groupUI);
		}
	}

	// Token: 0x06006B71 RID: 27505 RVA: 0x00286ABB File Offset: 0x00284CBB
	public void RemoveSingleCheckboxFromContainer(HierarchyReferences checkbox, CheckboxListGroupSideScreen.CheckboxContainer container)
	{
		container.checkboxUIItems.Remove(checkbox);
		checkbox.gameObject.SetActive(false);
		checkbox.transform.SetParent(this.checkboxParent);
		this.checkboxPool.ReleaseInstance(checkbox.gameObject);
	}

	// Token: 0x06006B72 RID: 27506 RVA: 0x00286AF8 File Offset: 0x00284CF8
	public HierarchyReferences CreateSingleCheckBoxForGroupUI(CheckboxListGroupSideScreen.CheckboxContainer container)
	{
		HierarchyReferences component = this.checkboxPool.GetInstance().GetComponent<HierarchyReferences>();
		component.gameObject.SetActive(true);
		container.checkboxUIItems.Add(component);
		component.transform.SetParent(container.container.transform);
		return component;
	}

	// Token: 0x06006B73 RID: 27507 RVA: 0x00286B48 File Offset: 0x00284D48
	public HierarchyReferences CreateSingleCheckBoxForGroupUI(ICheckboxListGroupControl.CheckboxItem data, ICheckboxListGroupControl target, CheckboxListGroupSideScreen.CheckboxContainer container)
	{
		HierarchyReferences hierarchyReferences = this.CreateSingleCheckBoxForGroupUI(container);
		this.SetCheckboxData(hierarchyReferences, data, target);
		return hierarchyReferences;
	}

	// Token: 0x06006B74 RID: 27508 RVA: 0x00286B68 File Offset: 0x00284D68
	public void SetCheckboxData(HierarchyReferences checkboxUI, ICheckboxListGroupControl.CheckboxItem data, ICheckboxListGroupControl target)
	{
		LocText reference = checkboxUI.GetReference<LocText>("Text");
		reference.SetText(data.text);
		reference.SetLinkOverrideAction(data.overrideLinkActions);
		checkboxUI.GetReference<Image>("Check").enabled = data.isOn;
		ToolTip reference2 = checkboxUI.GetReference<ToolTip>("Tooltip");
		reference2.SetSimpleTooltip(data.tooltip);
		reference2.refreshWhileHovering = (data.resolveTooltipCallback != null);
		reference2.OnToolTip = delegate()
		{
			if (data.resolveTooltipCallback == null)
			{
				return data.tooltip;
			}
			return data.resolveTooltipCallback(data.tooltip, target);
		};
	}

	// Token: 0x04004931 RID: 18737
	public const int DefaultCheckboxListSideScreenSortOrder = 20;

	// Token: 0x04004932 RID: 18738
	private ObjectPool<CheckboxListGroupSideScreen.CheckboxContainer> checkboxContainerPool;

	// Token: 0x04004933 RID: 18739
	private GameObjectPool checkboxPool;

	// Token: 0x04004934 RID: 18740
	[SerializeField]
	private GameObject checkboxGroupPrefab;

	// Token: 0x04004935 RID: 18741
	[SerializeField]
	private GameObject checkboxPrefab;

	// Token: 0x04004936 RID: 18742
	[SerializeField]
	private RectTransform groupParent;

	// Token: 0x04004937 RID: 18743
	[SerializeField]
	private RectTransform checkboxParent;

	// Token: 0x04004938 RID: 18744
	[SerializeField]
	private LocText descriptionLabel;

	// Token: 0x04004939 RID: 18745
	private List<ICheckboxListGroupControl> targets;

	// Token: 0x0400493A RID: 18746
	private GameObject currentBuildTarget;

	// Token: 0x0400493B RID: 18747
	private int uiRefreshSubHandle = -1;

	// Token: 0x0400493C RID: 18748
	private List<CheckboxListGroupSideScreen.CheckboxContainer> activeChecklistGroups = new List<CheckboxListGroupSideScreen.CheckboxContainer>();

	// Token: 0x02001E85 RID: 7813
	public class CheckboxContainer
	{
		// Token: 0x0600ABBB RID: 43963 RVA: 0x003A5AE2 File Offset: 0x003A3CE2
		public CheckboxContainer(HierarchyReferences container)
		{
			this.container = container;
		}

		// Token: 0x04008AD2 RID: 35538
		public HierarchyReferences container;

		// Token: 0x04008AD3 RID: 35539
		public List<HierarchyReferences> checkboxUIItems = new List<HierarchyReferences>();
	}
}
