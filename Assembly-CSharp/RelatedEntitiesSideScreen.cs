using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D95 RID: 3477
public class RelatedEntitiesSideScreen : SideScreenContent, ISim1000ms
{
	// Token: 0x06006DA6 RID: 28070 RVA: 0x00293C10 File Offset: 0x00291E10
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		this.rowPrefab.SetActive(false);
		if (show)
		{
			this.RefreshOptions(null);
		}
	}

	// Token: 0x06006DA7 RID: 28071 RVA: 0x00293C2F File Offset: 0x00291E2F
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IRelatedEntities>() != null;
	}

	// Token: 0x06006DA8 RID: 28072 RVA: 0x00293C3A File Offset: 0x00291E3A
	public override void SetTarget(GameObject target)
	{
		this.target = target;
		this.targetRelatedEntitiesComponent = target.GetComponent<IRelatedEntities>();
		this.RefreshOptions(null);
		this.uiRefreshSubHandle = Game.Instance.Subscribe(1980521255, new Action<object>(this.RefreshOptions));
	}

	// Token: 0x06006DA9 RID: 28073 RVA: 0x00293C77 File Offset: 0x00291E77
	public override void ClearTarget()
	{
		if (this.uiRefreshSubHandle != -1 && this.targetRelatedEntitiesComponent != null)
		{
			Game.Instance.Unsubscribe(this.uiRefreshSubHandle);
			this.uiRefreshSubHandle = -1;
		}
	}

	// Token: 0x06006DAA RID: 28074 RVA: 0x00293CA4 File Offset: 0x00291EA4
	private void RefreshOptions(object data = null)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.ClearRows();
		foreach (KSelectable entity in this.targetRelatedEntitiesComponent.GetRelatedEntities())
		{
			this.AddRow(entity);
		}
	}

	// Token: 0x06006DAB RID: 28075 RVA: 0x00293D10 File Offset: 0x00291F10
	private void ClearRows()
	{
		for (int i = this.rowContainer.childCount - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.rowContainer.GetChild(i));
		}
		this.rows.Clear();
	}

	// Token: 0x06006DAC RID: 28076 RVA: 0x00293D54 File Offset: 0x00291F54
	private void AddRow(KSelectable entity)
	{
		GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.rowContainer.gameObject, true);
		gameObject.GetComponent<KButton>().onClick += delegate()
		{
			SelectTool.Instance.SelectAndFocus(entity.transform.position, entity);
		};
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		component.GetReference<LocText>("label").SetText((SelectTool.Instance.selected == entity) ? ("<b>" + entity.GetProperName() + "</b>") : entity.GetProperName());
		component.GetReference<Image>("icon").sprite = Def.GetUISprite(entity.gameObject, "ui", false).first;
		this.rows.Add(entity, gameObject);
		this.RefreshMainStatus(entity);
	}

	// Token: 0x06006DAD RID: 28077 RVA: 0x00293E3C File Offset: 0x0029203C
	private void RefreshMainStatus(KSelectable entity)
	{
		if (entity.IsNullOrDestroyed())
		{
			return;
		}
		if (!this.rows.ContainsKey(entity))
		{
			return;
		}
		HierarchyReferences component = this.rows[entity].GetComponent<HierarchyReferences>();
		StatusItemGroup.Entry statusItem = entity.GetStatusItem(Db.Get().StatusItemCategories.Main);
		LocText reference = component.GetReference<LocText>("status");
		if (statusItem.data != null)
		{
			reference.gameObject.SetActive(true);
			reference.SetText(statusItem.item.GetName(statusItem.data));
			return;
		}
		reference.gameObject.SetActive(false);
		reference.SetText("");
	}

	// Token: 0x06006DAE RID: 28078 RVA: 0x00293ED8 File Offset: 0x002920D8
	public void Sim1000ms(float dt)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		foreach (KeyValuePair<KSelectable, GameObject> keyValuePair in this.rows)
		{
			this.RefreshMainStatus(keyValuePair.Key);
		}
	}

	// Token: 0x04004AD5 RID: 19157
	private GameObject target;

	// Token: 0x04004AD6 RID: 19158
	private IRelatedEntities targetRelatedEntitiesComponent;

	// Token: 0x04004AD7 RID: 19159
	public GameObject rowPrefab;

	// Token: 0x04004AD8 RID: 19160
	public RectTransform rowContainer;

	// Token: 0x04004AD9 RID: 19161
	public Dictionary<KSelectable, GameObject> rows = new Dictionary<KSelectable, GameObject>();

	// Token: 0x04004ADA RID: 19162
	private int uiRefreshSubHandle = -1;
}
