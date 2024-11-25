using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000D3F RID: 3391
public class AccessControlSideScreen : SideScreenContent
{
	// Token: 0x06006A91 RID: 27281 RVA: 0x0028252A File Offset: 0x0028072A
	public override string GetTitle()
	{
		if (this.target != null)
		{
			return string.Format(base.GetTitle(), this.target.GetProperName());
		}
		return base.GetTitle();
	}

	// Token: 0x06006A92 RID: 27282 RVA: 0x00282558 File Offset: 0x00280758
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.sortByNameToggle.onValueChanged.AddListener(delegate(bool reverse_sort)
		{
			this.SortEntries(reverse_sort, new Comparison<MinionAssignablesProxy>(AccessControlSideScreen.MinionIdentitySort.CompareByName));
		});
		this.sortByRoleToggle.onValueChanged.AddListener(delegate(bool reverse_sort)
		{
			this.SortEntries(reverse_sort, new Comparison<MinionAssignablesProxy>(AccessControlSideScreen.MinionIdentitySort.CompareByRole));
		});
		this.sortByPermissionToggle.onValueChanged.AddListener(new UnityAction<bool>(this.SortByPermission));
	}

	// Token: 0x06006A93 RID: 27283 RVA: 0x002825BF File Offset: 0x002807BF
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<AccessControl>() != null && target.GetComponent<AccessControl>().controlEnabled;
	}

	// Token: 0x06006A94 RID: 27284 RVA: 0x002825DC File Offset: 0x002807DC
	public override void SetTarget(GameObject target)
	{
		if (this.target != null)
		{
			this.ClearTarget();
		}
		this.target = target.GetComponent<AccessControl>();
		this.doorTarget = target.GetComponent<Door>();
		if (this.target == null)
		{
			return;
		}
		target.Subscribe(1734268753, new Action<object>(this.OnDoorStateChanged));
		target.Subscribe(-1525636549, new Action<object>(this.OnAccessControlChanged));
		if (this.rowPool == null)
		{
			this.rowPool = new UIPool<AccessControlSideScreenRow>(this.rowPrefab);
		}
		base.gameObject.SetActive(true);
		this.identityList = new List<MinionAssignablesProxy>(Components.MinionAssignablesProxy.Items);
		this.Refresh(this.identityList, true);
	}

	// Token: 0x06006A95 RID: 27285 RVA: 0x0028269C File Offset: 0x0028089C
	public override void ClearTarget()
	{
		base.ClearTarget();
		if (this.target != null)
		{
			this.target.Unsubscribe(1734268753, new Action<object>(this.OnDoorStateChanged));
			this.target.Unsubscribe(-1525636549, new Action<object>(this.OnAccessControlChanged));
		}
	}

	// Token: 0x06006A96 RID: 27286 RVA: 0x002826F8 File Offset: 0x002808F8
	private void Refresh(List<MinionAssignablesProxy> identities, bool rebuild)
	{
		Rotatable component = this.target.GetComponent<Rotatable>();
		bool rotated = component != null && component.IsRotated;
		this.defaultsRow.SetRotated(rotated);
		this.defaultsRow.SetContent(this.target.DefaultPermission, new Action<MinionAssignablesProxy, AccessControl.Permission>(this.OnDefaultPermissionChanged));
		if (rebuild)
		{
			this.ClearContent();
		}
		foreach (MinionAssignablesProxy minionAssignablesProxy in identities)
		{
			AccessControlSideScreenRow accessControlSideScreenRow;
			if (rebuild)
			{
				accessControlSideScreenRow = this.rowPool.GetFreeElement(this.rowGroup, true);
				this.identityRowMap.Add(minionAssignablesProxy, accessControlSideScreenRow);
			}
			else
			{
				accessControlSideScreenRow = this.identityRowMap[minionAssignablesProxy];
			}
			AccessControl.Permission setPermission = this.target.GetSetPermission(minionAssignablesProxy);
			bool isDefault = this.target.IsDefaultPermission(minionAssignablesProxy);
			accessControlSideScreenRow.SetRotated(rotated);
			accessControlSideScreenRow.SetMinionContent(minionAssignablesProxy, setPermission, isDefault, new Action<MinionAssignablesProxy, AccessControl.Permission>(this.OnPermissionChanged), new Action<MinionAssignablesProxy, bool>(this.OnPermissionDefault));
		}
		this.RefreshOnline();
		this.ContentContainer.SetActive(this.target.controlEnabled);
	}

	// Token: 0x06006A97 RID: 27287 RVA: 0x00282834 File Offset: 0x00280A34
	private void RefreshOnline()
	{
		bool flag = this.target.Online && (this.doorTarget == null || this.doorTarget.CurrentState == Door.ControlState.Auto);
		this.disabledOverlay.SetActive(!flag);
		this.headerBG.ColorState = (flag ? KImage.ColorSelector.Active : KImage.ColorSelector.Inactive);
	}

	// Token: 0x06006A98 RID: 27288 RVA: 0x00282892 File Offset: 0x00280A92
	private void SortByPermission(bool state)
	{
		this.ExecuteSort<int>(this.sortByPermissionToggle, state, delegate(MinionAssignablesProxy identity)
		{
			if (!this.target.IsDefaultPermission(identity))
			{
				return (int)this.target.GetSetPermission(identity);
			}
			return -1;
		}, false);
	}

	// Token: 0x06006A99 RID: 27289 RVA: 0x002828B0 File Offset: 0x00280AB0
	private void ExecuteSort<T>(Toggle toggle, bool state, Func<MinionAssignablesProxy, T> sortFunction, bool refresh = false)
	{
		toggle.GetComponent<ImageToggleState>().SetActiveState(state);
		if (!state)
		{
			return;
		}
		this.identityList = (state ? this.identityList.OrderBy(sortFunction).ToList<MinionAssignablesProxy>() : this.identityList.OrderByDescending(sortFunction).ToList<MinionAssignablesProxy>());
		if (refresh)
		{
			this.Refresh(this.identityList, false);
			return;
		}
		for (int i = 0; i < this.identityList.Count; i++)
		{
			if (this.identityRowMap.ContainsKey(this.identityList[i]))
			{
				this.identityRowMap[this.identityList[i]].transform.SetSiblingIndex(i);
			}
		}
	}

	// Token: 0x06006A9A RID: 27290 RVA: 0x00282960 File Offset: 0x00280B60
	private void SortEntries(bool reverse_sort, Comparison<MinionAssignablesProxy> compare)
	{
		this.identityList.Sort(compare);
		if (reverse_sort)
		{
			this.identityList.Reverse();
		}
		for (int i = 0; i < this.identityList.Count; i++)
		{
			if (this.identityRowMap.ContainsKey(this.identityList[i]))
			{
				this.identityRowMap[this.identityList[i]].transform.SetSiblingIndex(i);
			}
		}
	}

	// Token: 0x06006A9B RID: 27291 RVA: 0x002829D8 File Offset: 0x00280BD8
	private void ClearContent()
	{
		if (this.rowPool != null)
		{
			this.rowPool.ClearAll();
		}
		this.identityRowMap.Clear();
	}

	// Token: 0x06006A9C RID: 27292 RVA: 0x002829F8 File Offset: 0x00280BF8
	private void OnDefaultPermissionChanged(MinionAssignablesProxy identity, AccessControl.Permission permission)
	{
		this.target.DefaultPermission = permission;
		this.Refresh(this.identityList, false);
		foreach (MinionAssignablesProxy key in this.identityList)
		{
			if (this.target.IsDefaultPermission(key))
			{
				this.target.ClearPermission(key);
			}
		}
	}

	// Token: 0x06006A9D RID: 27293 RVA: 0x00282A78 File Offset: 0x00280C78
	private void OnPermissionChanged(MinionAssignablesProxy identity, AccessControl.Permission permission)
	{
		this.target.SetPermission(identity, permission);
	}

	// Token: 0x06006A9E RID: 27294 RVA: 0x00282A87 File Offset: 0x00280C87
	private void OnPermissionDefault(MinionAssignablesProxy identity, bool isDefault)
	{
		if (isDefault)
		{
			this.target.ClearPermission(identity);
		}
		else
		{
			this.target.SetPermission(identity, this.target.DefaultPermission);
		}
		this.Refresh(this.identityList, false);
	}

	// Token: 0x06006A9F RID: 27295 RVA: 0x00282ABE File Offset: 0x00280CBE
	private void OnAccessControlChanged(object data)
	{
		this.RefreshOnline();
	}

	// Token: 0x06006AA0 RID: 27296 RVA: 0x00282AC6 File Offset: 0x00280CC6
	private void OnDoorStateChanged(object data)
	{
		this.RefreshOnline();
	}

	// Token: 0x06006AA1 RID: 27297 RVA: 0x00282AD0 File Offset: 0x00280CD0
	private void OnSelectSortFunc(IListableOption role, object data)
	{
		if (role != null)
		{
			foreach (AccessControlSideScreen.MinionIdentitySort.SortInfo sortInfo in AccessControlSideScreen.MinionIdentitySort.SortInfos)
			{
				if (sortInfo.name == role.GetProperName())
				{
					this.sortInfo = sortInfo;
					this.identityList.Sort(this.sortInfo.compare);
					for (int j = 0; j < this.identityList.Count; j++)
					{
						if (this.identityRowMap.ContainsKey(this.identityList[j]))
						{
							this.identityRowMap[this.identityList[j]].transform.SetSiblingIndex(j);
						}
					}
					return;
				}
			}
		}
	}

	// Token: 0x040048A7 RID: 18599
	[SerializeField]
	private AccessControlSideScreenRow rowPrefab;

	// Token: 0x040048A8 RID: 18600
	[SerializeField]
	private GameObject rowGroup;

	// Token: 0x040048A9 RID: 18601
	[SerializeField]
	private AccessControlSideScreenDoor defaultsRow;

	// Token: 0x040048AA RID: 18602
	[SerializeField]
	private Toggle sortByNameToggle;

	// Token: 0x040048AB RID: 18603
	[SerializeField]
	private Toggle sortByPermissionToggle;

	// Token: 0x040048AC RID: 18604
	[SerializeField]
	private Toggle sortByRoleToggle;

	// Token: 0x040048AD RID: 18605
	[SerializeField]
	private GameObject disabledOverlay;

	// Token: 0x040048AE RID: 18606
	[SerializeField]
	private KImage headerBG;

	// Token: 0x040048AF RID: 18607
	private AccessControl target;

	// Token: 0x040048B0 RID: 18608
	private Door doorTarget;

	// Token: 0x040048B1 RID: 18609
	private UIPool<AccessControlSideScreenRow> rowPool;

	// Token: 0x040048B2 RID: 18610
	private AccessControlSideScreen.MinionIdentitySort.SortInfo sortInfo = AccessControlSideScreen.MinionIdentitySort.SortInfos[0];

	// Token: 0x040048B3 RID: 18611
	private Dictionary<MinionAssignablesProxy, AccessControlSideScreenRow> identityRowMap = new Dictionary<MinionAssignablesProxy, AccessControlSideScreenRow>();

	// Token: 0x040048B4 RID: 18612
	private List<MinionAssignablesProxy> identityList = new List<MinionAssignablesProxy>();

	// Token: 0x02001E77 RID: 7799
	private static class MinionIdentitySort
	{
		// Token: 0x0600AB9D RID: 43933 RVA: 0x003A56EF File Offset: 0x003A38EF
		public static int CompareByName(MinionAssignablesProxy a, MinionAssignablesProxy b)
		{
			return a.GetProperName().CompareTo(b.GetProperName());
		}

		// Token: 0x0600AB9E RID: 43934 RVA: 0x003A5704 File Offset: 0x003A3904
		public static int CompareByRole(MinionAssignablesProxy a, MinionAssignablesProxy b)
		{
			global::Debug.Assert(a, "a was null");
			global::Debug.Assert(b, "b was null");
			GameObject targetGameObject = a.GetTargetGameObject();
			GameObject targetGameObject2 = b.GetTargetGameObject();
			MinionResume minionResume = targetGameObject ? targetGameObject.GetComponent<MinionResume>() : null;
			MinionResume minionResume2 = targetGameObject2 ? targetGameObject2.GetComponent<MinionResume>() : null;
			if (minionResume2 == null)
			{
				return 1;
			}
			if (minionResume == null)
			{
				return -1;
			}
			int num = minionResume.CurrentRole.CompareTo(minionResume2.CurrentRole);
			if (num != 0)
			{
				return num;
			}
			return AccessControlSideScreen.MinionIdentitySort.CompareByName(a, b);
		}

		// Token: 0x04008AAB RID: 35499
		public static readonly AccessControlSideScreen.MinionIdentitySort.SortInfo[] SortInfos = new AccessControlSideScreen.MinionIdentitySort.SortInfo[]
		{
			new AccessControlSideScreen.MinionIdentitySort.SortInfo
			{
				name = UI.MINION_IDENTITY_SORT.NAME,
				compare = new Comparison<MinionAssignablesProxy>(AccessControlSideScreen.MinionIdentitySort.CompareByName)
			},
			new AccessControlSideScreen.MinionIdentitySort.SortInfo
			{
				name = UI.MINION_IDENTITY_SORT.ROLE,
				compare = new Comparison<MinionAssignablesProxy>(AccessControlSideScreen.MinionIdentitySort.CompareByRole)
			}
		};

		// Token: 0x0200265E RID: 9822
		public class SortInfo : IListableOption
		{
			// Token: 0x0600C234 RID: 49716 RVA: 0x003E00DF File Offset: 0x003DE2DF
			public string GetProperName()
			{
				return this.name;
			}

			// Token: 0x0400AA70 RID: 43632
			public LocString name;

			// Token: 0x0400AA71 RID: 43633
			public Comparison<MinionAssignablesProxy> compare;
		}
	}
}
