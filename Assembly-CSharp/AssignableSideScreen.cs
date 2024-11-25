using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000D47 RID: 3399
public class AssignableSideScreen : SideScreenContent
{
	// Token: 0x17000781 RID: 1921
	// (get) Token: 0x06006AE7 RID: 27367 RVA: 0x002840DB File Offset: 0x002822DB
	// (set) Token: 0x06006AE8 RID: 27368 RVA: 0x002840E3 File Offset: 0x002822E3
	public Assignable targetAssignable { get; private set; }

	// Token: 0x06006AE9 RID: 27369 RVA: 0x002840EC File Offset: 0x002822EC
	public override string GetTitle()
	{
		if (this.targetAssignable != null)
		{
			return string.Format(base.GetTitle(), this.targetAssignable.GetProperName());
		}
		return base.GetTitle();
	}

	// Token: 0x06006AEA RID: 27370 RVA: 0x0028411C File Offset: 0x0028231C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		MultiToggle multiToggle = this.dupeSortingToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.SortByName(true);
		}));
		MultiToggle multiToggle2 = this.generalSortingToggle;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(delegate()
		{
			this.SortByAssignment(true);
		}));
		base.Subscribe(Game.Instance.gameObject, 875045922, new Action<object>(this.OnRefreshData));
	}

	// Token: 0x06006AEB RID: 27371 RVA: 0x0028419F File Offset: 0x0028239F
	private void OnRefreshData(object obj)
	{
		this.SetTarget(this.targetAssignable.gameObject);
	}

	// Token: 0x06006AEC RID: 27372 RVA: 0x002841B4 File Offset: 0x002823B4
	public override void ClearTarget()
	{
		if (this.targetAssignableSubscriptionHandle != -1 && this.targetAssignable != null)
		{
			this.targetAssignable.Unsubscribe(this.targetAssignableSubscriptionHandle);
			this.targetAssignableSubscriptionHandle = -1;
		}
		this.targetAssignable = null;
		Components.LiveMinionIdentities.OnAdd -= this.OnMinionIdentitiesChanged;
		Components.LiveMinionIdentities.OnRemove -= this.OnMinionIdentitiesChanged;
		base.ClearTarget();
	}

	// Token: 0x06006AED RID: 27373 RVA: 0x00284229 File Offset: 0x00282429
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Assignable>() != null && target.GetComponent<Assignable>().CanBeAssigned && target.GetComponent<AssignmentGroupController>() == null;
	}

	// Token: 0x06006AEE RID: 27374 RVA: 0x00284254 File Offset: 0x00282454
	public override void SetTarget(GameObject target)
	{
		Components.LiveMinionIdentities.OnAdd += this.OnMinionIdentitiesChanged;
		Components.LiveMinionIdentities.OnRemove += this.OnMinionIdentitiesChanged;
		if (this.targetAssignableSubscriptionHandle != -1 && this.targetAssignable != null)
		{
			this.targetAssignable.Unsubscribe(this.targetAssignableSubscriptionHandle);
		}
		this.targetAssignable = target.GetComponent<Assignable>();
		if (this.targetAssignable == null)
		{
			global::Debug.LogError(string.Format("{0} selected has no Assignable component.", target.GetProperName()));
			return;
		}
		if (this.rowPool == null)
		{
			this.rowPool = new UIPool<AssignableSideScreenRow>(this.rowPrefab);
		}
		base.gameObject.SetActive(true);
		this.identityList = new List<MinionAssignablesProxy>(Components.MinionAssignablesProxy.Items);
		this.dupeSortingToggle.ChangeState(0);
		this.generalSortingToggle.ChangeState(0);
		this.activeSortToggle = null;
		this.activeSortFunction = null;
		if (!this.targetAssignable.CanBeAssigned)
		{
			this.HideScreen(true);
		}
		else
		{
			this.HideScreen(false);
		}
		this.targetAssignableSubscriptionHandle = this.targetAssignable.Subscribe(684616645, new Action<object>(this.OnAssigneeChanged));
		this.Refresh(this.identityList);
		this.SortByAssignment(false);
	}

	// Token: 0x06006AEF RID: 27375 RVA: 0x00284397 File Offset: 0x00282597
	private void OnMinionIdentitiesChanged(MinionIdentity change)
	{
		this.identityList = new List<MinionAssignablesProxy>(Components.MinionAssignablesProxy.Items);
		this.Refresh(this.identityList);
	}

	// Token: 0x06006AF0 RID: 27376 RVA: 0x002843BC File Offset: 0x002825BC
	private void OnAssigneeChanged(object data = null)
	{
		foreach (KeyValuePair<IAssignableIdentity, AssignableSideScreenRow> keyValuePair in this.identityRowMap)
		{
			keyValuePair.Value.Refresh(null);
		}
	}

	// Token: 0x06006AF1 RID: 27377 RVA: 0x00284418 File Offset: 0x00282618
	private void Refresh(List<MinionAssignablesProxy> identities)
	{
		this.ClearContent();
		this.currentOwnerText.text = string.Format(UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.UNASSIGNED, Array.Empty<object>());
		if (this.targetAssignable == null)
		{
			return;
		}
		if (this.targetAssignable.GetComponent<Equippable>() == null && !this.targetAssignable.HasTag(GameTags.NotRoomAssignable))
		{
			Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(this.targetAssignable.gameObject);
			if (roomOfGameObject != null)
			{
				RoomType roomType = roomOfGameObject.roomType;
				if (roomType.primary_constraint != null && !roomType.primary_constraint.building_criteria(this.targetAssignable.GetComponent<KPrefabID>()))
				{
					AssignableSideScreenRow freeElement = this.rowPool.GetFreeElement(this.rowGroup, true);
					freeElement.sideScreen = this;
					this.identityRowMap.Add(roomOfGameObject, freeElement);
					freeElement.SetContent(roomOfGameObject, new Action<IAssignableIdentity>(this.OnRowClicked), this);
					return;
				}
			}
		}
		if (this.targetAssignable.canBePublic)
		{
			AssignableSideScreenRow freeElement2 = this.rowPool.GetFreeElement(this.rowGroup, true);
			freeElement2.sideScreen = this;
			freeElement2.transform.SetAsFirstSibling();
			this.identityRowMap.Add(Game.Instance.assignmentManager.assignment_groups["public"], freeElement2);
			freeElement2.SetContent(Game.Instance.assignmentManager.assignment_groups["public"], new Action<IAssignableIdentity>(this.OnRowClicked), this);
		}
		foreach (MinionAssignablesProxy minionAssignablesProxy in identities)
		{
			AssignableSideScreenRow freeElement3 = this.rowPool.GetFreeElement(this.rowGroup, true);
			freeElement3.sideScreen = this;
			this.identityRowMap.Add(minionAssignablesProxy, freeElement3);
			freeElement3.SetContent(minionAssignablesProxy, new Action<IAssignableIdentity>(this.OnRowClicked), this);
		}
		this.ExecuteSort(this.activeSortFunction);
	}

	// Token: 0x06006AF2 RID: 27378 RVA: 0x00284618 File Offset: 0x00282818
	private void SortByName(bool reselect)
	{
		this.SelectSortToggle(this.dupeSortingToggle, reselect);
		this.ExecuteSort((IAssignableIdentity i1, IAssignableIdentity i2) => i1.GetProperName().CompareTo(i2.GetProperName()) * (this.sortReversed ? -1 : 1));
	}

	// Token: 0x06006AF3 RID: 27379 RVA: 0x0028463C File Offset: 0x0028283C
	private void SortByAssignment(bool reselect)
	{
		this.SelectSortToggle(this.generalSortingToggle, reselect);
		Comparison<IAssignableIdentity> sortFunction = delegate(IAssignableIdentity i1, IAssignableIdentity i2)
		{
			int num = this.targetAssignable.CanAssignTo(i1).CompareTo(this.targetAssignable.CanAssignTo(i2));
			if (num != 0)
			{
				return num * -1;
			}
			num = this.identityRowMap[i1].currentState.CompareTo(this.identityRowMap[i2].currentState);
			if (num != 0)
			{
				return num * (this.sortReversed ? -1 : 1);
			}
			return i1.GetProperName().CompareTo(i2.GetProperName());
		};
		this.ExecuteSort(sortFunction);
	}

	// Token: 0x06006AF4 RID: 27380 RVA: 0x0028466C File Offset: 0x0028286C
	private void SelectSortToggle(MultiToggle toggle, bool reselect)
	{
		this.dupeSortingToggle.ChangeState(0);
		this.generalSortingToggle.ChangeState(0);
		if (toggle != null)
		{
			if (reselect && this.activeSortToggle == toggle)
			{
				this.sortReversed = !this.sortReversed;
			}
			this.activeSortToggle = toggle;
		}
		this.activeSortToggle.ChangeState(this.sortReversed ? 2 : 1);
	}

	// Token: 0x06006AF5 RID: 27381 RVA: 0x002846D8 File Offset: 0x002828D8
	private void ExecuteSort(Comparison<IAssignableIdentity> sortFunction)
	{
		if (sortFunction != null)
		{
			List<IAssignableIdentity> list = new List<IAssignableIdentity>(this.identityRowMap.Keys);
			list.Sort(sortFunction);
			for (int i = 0; i < list.Count; i++)
			{
				this.identityRowMap[list[i]].transform.SetSiblingIndex(i);
			}
			this.activeSortFunction = sortFunction;
		}
	}

	// Token: 0x06006AF6 RID: 27382 RVA: 0x00284738 File Offset: 0x00282938
	private void ClearContent()
	{
		if (this.rowPool != null)
		{
			this.rowPool.DestroyAll();
		}
		foreach (KeyValuePair<IAssignableIdentity, AssignableSideScreenRow> keyValuePair in this.identityRowMap)
		{
			keyValuePair.Value.targetIdentity = null;
		}
		this.identityRowMap.Clear();
	}

	// Token: 0x06006AF7 RID: 27383 RVA: 0x002847B0 File Offset: 0x002829B0
	private void HideScreen(bool hide)
	{
		if (hide)
		{
			base.transform.localScale = Vector3.zero;
			return;
		}
		if (base.transform.localScale != Vector3.one)
		{
			base.transform.localScale = Vector3.one;
		}
	}

	// Token: 0x06006AF8 RID: 27384 RVA: 0x002847ED File Offset: 0x002829ED
	private void OnRowClicked(IAssignableIdentity identity)
	{
		if (this.targetAssignable.assignee != identity)
		{
			this.ChangeAssignment(identity);
			return;
		}
		if (this.CanDeselect(identity))
		{
			this.ChangeAssignment(null);
		}
	}

	// Token: 0x06006AF9 RID: 27385 RVA: 0x00284815 File Offset: 0x00282A15
	private bool CanDeselect(IAssignableIdentity identity)
	{
		return identity is MinionAssignablesProxy;
	}

	// Token: 0x06006AFA RID: 27386 RVA: 0x00284820 File Offset: 0x00282A20
	private void ChangeAssignment(IAssignableIdentity new_identity)
	{
		this.targetAssignable.Unassign();
		if (!new_identity.IsNullOrDestroyed())
		{
			this.targetAssignable.Assign(new_identity);
		}
	}

	// Token: 0x06006AFB RID: 27387 RVA: 0x00284841 File Offset: 0x00282A41
	private void OnValidStateChanged(bool state)
	{
		if (base.gameObject.activeInHierarchy)
		{
			this.Refresh(this.identityList);
		}
	}

	// Token: 0x040048E0 RID: 18656
	[SerializeField]
	private AssignableSideScreenRow rowPrefab;

	// Token: 0x040048E1 RID: 18657
	[SerializeField]
	private GameObject rowGroup;

	// Token: 0x040048E2 RID: 18658
	[SerializeField]
	private LocText currentOwnerText;

	// Token: 0x040048E3 RID: 18659
	[SerializeField]
	private MultiToggle dupeSortingToggle;

	// Token: 0x040048E4 RID: 18660
	[SerializeField]
	private MultiToggle generalSortingToggle;

	// Token: 0x040048E5 RID: 18661
	private MultiToggle activeSortToggle;

	// Token: 0x040048E6 RID: 18662
	private Comparison<IAssignableIdentity> activeSortFunction;

	// Token: 0x040048E7 RID: 18663
	private bool sortReversed;

	// Token: 0x040048E8 RID: 18664
	private int targetAssignableSubscriptionHandle = -1;

	// Token: 0x040048EA RID: 18666
	private UIPool<AssignableSideScreenRow> rowPool;

	// Token: 0x040048EB RID: 18667
	private Dictionary<IAssignableIdentity, AssignableSideScreenRow> identityRowMap = new Dictionary<IAssignableIdentity, AssignableSideScreenRow>();

	// Token: 0x040048EC RID: 18668
	private List<MinionAssignablesProxy> identityList = new List<MinionAssignablesProxy>();
}
