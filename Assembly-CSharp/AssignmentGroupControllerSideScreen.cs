using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000D49 RID: 3401
public class AssignmentGroupControllerSideScreen : KScreen
{
	// Token: 0x06006B06 RID: 27398 RVA: 0x00284D11 File Offset: 0x00282F11
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.RefreshRows();
		}
	}

	// Token: 0x06006B07 RID: 27399 RVA: 0x00284D24 File Offset: 0x00282F24
	protected override void OnCmpDisable()
	{
		for (int i = 0; i < this.identityRowMap.Count; i++)
		{
			UnityEngine.Object.Destroy(this.identityRowMap[i]);
		}
		this.identityRowMap.Clear();
		base.OnCmpDisable();
	}

	// Token: 0x06006B08 RID: 27400 RVA: 0x00284D69 File Offset: 0x00282F69
	public void SetTarget(GameObject target)
	{
		this.target = target.GetComponent<AssignmentGroupController>();
		this.RefreshRows();
	}

	// Token: 0x06006B09 RID: 27401 RVA: 0x00284D80 File Offset: 0x00282F80
	private void RefreshRows()
	{
		int num = 0;
		WorldContainer myWorld = this.target.GetMyWorld();
		ClustercraftExteriorDoor component = this.target.GetComponent<ClustercraftExteriorDoor>();
		if (component != null)
		{
			myWorld = component.GetInteriorDoor().GetMyWorld();
		}
		List<AssignmentGroupControllerSideScreen.RowSortHelper> list = new List<AssignmentGroupControllerSideScreen.RowSortHelper>();
		for (int i = 0; i < Components.MinionAssignablesProxy.Count; i++)
		{
			MinionAssignablesProxy minionAssignablesProxy = Components.MinionAssignablesProxy[i];
			GameObject targetGameObject = minionAssignablesProxy.GetTargetGameObject();
			WorldContainer myWorld2 = targetGameObject.GetMyWorld();
			if (!(targetGameObject == null) && !targetGameObject.HasTag(GameTags.Dead))
			{
				MinionResume component2 = minionAssignablesProxy.GetTargetGameObject().GetComponent<MinionResume>();
				bool isPilot = false;
				if (component2 != null && component2.HasPerk(Db.Get().SkillPerks.CanUseRocketControlStation))
				{
					isPilot = true;
				}
				bool isSameWorld = myWorld2.ParentWorldId == myWorld.ParentWorldId;
				list.Add(new AssignmentGroupControllerSideScreen.RowSortHelper
				{
					minion = minionAssignablesProxy,
					isPilot = isPilot,
					isSameWorld = isSameWorld
				});
			}
		}
		list.Sort(delegate(AssignmentGroupControllerSideScreen.RowSortHelper a, AssignmentGroupControllerSideScreen.RowSortHelper b)
		{
			int num2 = b.isSameWorld.CompareTo(a.isSameWorld);
			if (num2 != 0)
			{
				return num2;
			}
			return b.isPilot.CompareTo(a.isPilot);
		});
		foreach (AssignmentGroupControllerSideScreen.RowSortHelper rowSortHelper in list)
		{
			MinionAssignablesProxy minion = rowSortHelper.minion;
			GameObject gameObject;
			if (num >= this.identityRowMap.Count)
			{
				gameObject = Util.KInstantiateUI(this.minionRowPrefab, this.minionRowContainer, true);
				this.identityRowMap.Add(gameObject);
			}
			else
			{
				gameObject = this.identityRowMap[num];
				gameObject.SetActive(true);
			}
			num++;
			HierarchyReferences component3 = gameObject.GetComponent<HierarchyReferences>();
			MultiToggle toggle = component3.GetReference<MultiToggle>("Toggle");
			toggle.ChangeState(this.target.CheckMinionIsMember(minion) ? 1 : 0);
			component3.GetReference<CrewPortrait>("Portrait").SetIdentityObject(minion, false);
			LocText reference = component3.GetReference<LocText>("Label");
			LocText reference2 = component3.GetReference<LocText>("Designation");
			if (rowSortHelper.isSameWorld)
			{
				if (rowSortHelper.isPilot)
				{
					reference2.text = UI.UISIDESCREENS.ASSIGNMENTGROUPCONTROLLER.PILOT;
				}
				else
				{
					reference2.text = "";
				}
				reference.color = Color.black;
				reference2.color = Color.black;
			}
			else
			{
				reference.color = Color.grey;
				reference2.color = Color.grey;
				reference2.text = UI.UISIDESCREENS.ASSIGNMENTGROUPCONTROLLER.OFFWORLD;
				gameObject.transform.SetAsLastSibling();
			}
			toggle.onClick = delegate()
			{
				this.target.SetMember(minion, !this.target.CheckMinionIsMember(minion));
				toggle.ChangeState(this.target.CheckMinionIsMember(minion) ? 1 : 0);
				this.RefreshRows();
			};
			string simpleTooltip = this.UpdateToolTip(minion, !rowSortHelper.isSameWorld);
			toggle.GetComponent<ToolTip>().SetSimpleTooltip(simpleTooltip);
		}
		for (int j = num; j < this.identityRowMap.Count; j++)
		{
			this.identityRowMap[j].SetActive(false);
		}
		this.minionRowContainer.GetComponent<QuickLayout>().ForceUpdate();
	}

	// Token: 0x06006B0A RID: 27402 RVA: 0x002850E4 File Offset: 0x002832E4
	private string UpdateToolTip(MinionAssignablesProxy minion, bool offworld)
	{
		string text = this.target.CheckMinionIsMember(minion) ? UI.UISIDESCREENS.ASSIGNMENTGROUPCONTROLLER.TOOLTIPS.UNASSIGN : UI.UISIDESCREENS.ASSIGNMENTGROUPCONTROLLER.TOOLTIPS.ASSIGN;
		if (offworld)
		{
			text = string.Concat(new string[]
			{
				text,
				"\n\n",
				UIConstants.ColorPrefixYellow,
				UI.UISIDESCREENS.ASSIGNMENTGROUPCONTROLLER.TOOLTIPS.DIFFERENT_WORLD,
				UIConstants.ColorSuffix
			});
		}
		return text;
	}

	// Token: 0x040048F5 RID: 18677
	[SerializeField]
	private GameObject header;

	// Token: 0x040048F6 RID: 18678
	[SerializeField]
	private GameObject minionRowPrefab;

	// Token: 0x040048F7 RID: 18679
	[SerializeField]
	private GameObject footer;

	// Token: 0x040048F8 RID: 18680
	[SerializeField]
	private GameObject minionRowContainer;

	// Token: 0x040048F9 RID: 18681
	private AssignmentGroupController target;

	// Token: 0x040048FA RID: 18682
	private List<GameObject> identityRowMap = new List<GameObject>();

	// Token: 0x02001E7F RID: 7807
	private struct RowSortHelper
	{
		// Token: 0x04008ABE RID: 35518
		public MinionAssignablesProxy minion;

		// Token: 0x04008ABF RID: 35519
		public bool isPilot;

		// Token: 0x04008AC0 RID: 35520
		public bool isSameWorld;
	}
}
