using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200058E RID: 1422
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Movable")]
public class Movable : Workable
{
	// Token: 0x17000162 RID: 354
	// (get) Token: 0x06002137 RID: 8503 RVA: 0x000BA268 File Offset: 0x000B8468
	public bool IsMarkedForMove
	{
		get
		{
			return this.isMarkedForMove;
		}
	}

	// Token: 0x17000163 RID: 355
	// (get) Token: 0x06002138 RID: 8504 RVA: 0x000BA270 File Offset: 0x000B8470
	public Storage StorageProxy
	{
		get
		{
			if (this.storageProxy == null)
			{
				return null;
			}
			return this.storageProxy.Get();
		}
	}

	// Token: 0x06002139 RID: 8505 RVA: 0x000BA287 File Offset: 0x000B8487
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe(493375141, new Action<object>(this.OnRefreshUserMenu));
		base.Subscribe(1335436905, new Action<object>(this.OnSplitFromChunk));
	}

	// Token: 0x0600213A RID: 8506 RVA: 0x000BA2C0 File Offset: 0x000B84C0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.isMarkedForMove)
		{
			if (this.StorageProxy != null)
			{
				if (this.reachableChangedHandle < 0)
				{
					this.reachableChangedHandle = base.Subscribe(-1432940121, new Action<object>(this.OnReachableChanged));
				}
				if (this.storageReachableChangedHandle < 0)
				{
					this.storageReachableChangedHandle = this.StorageProxy.Subscribe(-1432940121, new Action<object>(this.OnReachableChanged));
				}
				if (this.cancelHandle < 0)
				{
					this.cancelHandle = base.Subscribe(2127324410, new Action<object>(this.CleanupMove));
				}
				base.gameObject.AddTag(GameTags.MarkedForMove);
			}
			else
			{
				this.isMarkedForMove = false;
			}
		}
		if (this.IsCritter())
		{
			this.skillsUpdateHandle = Game.Instance.Subscribe(-1523247426, new Action<object>(this.UpdateStatusItem));
			this.shouldShowSkillPerkStatusItem = this.isMarkedForMove;
			this.requiredSkillPerk = Db.Get().SkillPerks.CanWrangleCreatures.Id;
			this.UpdateStatusItem();
		}
	}

	// Token: 0x0600213B RID: 8507 RVA: 0x000BA3D8 File Offset: 0x000B85D8
	private void OnReachableChanged(object data)
	{
		if (this.isMarkedForMove)
		{
			if (this.StorageProxy != null)
			{
				int num = Grid.PosToCell(this.pickupable);
				int num2 = Grid.PosToCell(this.StorageProxy);
				if (num != num2)
				{
					bool flag = MinionGroupProber.Get().IsReachable(num, OffsetGroups.Standard) && MinionGroupProber.Get().IsReachable(num2, OffsetGroups.Standard);
					if (this.pickupable.KPrefabID.HasTag(GameTags.Creatures.Confined))
					{
						flag = false;
					}
					KSelectable component = base.GetComponent<KSelectable>();
					this.pendingMoveGuid = component.ToggleStatusItem(Db.Get().MiscStatusItems.MarkedForMove, this.pendingMoveGuid, flag, this);
					this.storageUnreachableGuid = component.ToggleStatusItem(Db.Get().MiscStatusItems.MoveStorageUnreachable, this.storageUnreachableGuid, !flag, this);
					return;
				}
			}
			else
			{
				this.ClearMove();
			}
		}
	}

	// Token: 0x0600213C RID: 8508 RVA: 0x000BA4B8 File Offset: 0x000B86B8
	private void OnSplitFromChunk(object data)
	{
		Pickupable pickupable = data as Pickupable;
		if (pickupable != null)
		{
			Movable component = pickupable.GetComponent<Movable>();
			if (component.isMarkedForMove)
			{
				this.storageProxy = new Ref<Storage>(component.StorageProxy);
				this.MarkForMove();
			}
		}
	}

	// Token: 0x0600213D RID: 8509 RVA: 0x000BA4FB File Offset: 0x000B86FB
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.isMarkedForMove && this.StorageProxy != null)
		{
			this.StorageProxy.GetComponent<CancellableMove>().RemoveMovable(this);
			this.ClearStorageProxy();
		}
	}

	// Token: 0x0600213E RID: 8510 RVA: 0x000BA530 File Offset: 0x000B8730
	private void CleanupMove(object data)
	{
		if (this.StorageProxy != null)
		{
			this.StorageProxy.GetComponent<CancellableMove>().OnCancel(this);
		}
	}

	// Token: 0x0600213F RID: 8511 RVA: 0x000BA554 File Offset: 0x000B8754
	public void ClearMove()
	{
		if (this.isMarkedForMove)
		{
			this.isMarkedForMove = false;
			KSelectable component = base.GetComponent<KSelectable>();
			this.pendingMoveGuid = component.RemoveStatusItem(this.pendingMoveGuid, false);
			this.storageUnreachableGuid = component.RemoveStatusItem(this.storageUnreachableGuid, false);
			this.ClearStorageProxy();
			base.gameObject.RemoveTag(GameTags.MarkedForMove);
			if (this.reachableChangedHandle != -1)
			{
				base.Unsubscribe(-1432940121, new Action<object>(this.OnReachableChanged));
				this.reachableChangedHandle = -1;
			}
			if (this.cancelHandle != -1)
			{
				base.Unsubscribe(2127324410, new Action<object>(this.CleanupMove));
				this.cancelHandle = -1;
			}
		}
		this.UpdateStatusItem();
	}

	// Token: 0x06002140 RID: 8512 RVA: 0x000BA60A File Offset: 0x000B880A
	private void ClearStorageProxy()
	{
		if (this.storageReachableChangedHandle != -1)
		{
			this.StorageProxy.Unsubscribe(-1432940121, new Action<object>(this.OnReachableChanged));
			this.storageReachableChangedHandle = -1;
		}
		this.storageProxy = null;
	}

	// Token: 0x06002141 RID: 8513 RVA: 0x000BA63F File Offset: 0x000B883F
	private void OnClickMove()
	{
		MoveToLocationTool.Instance.Activate(this);
	}

	// Token: 0x06002142 RID: 8514 RVA: 0x000BA64C File Offset: 0x000B884C
	private void OnClickCancel()
	{
		if (this.StorageProxy != null)
		{
			this.StorageProxy.GetComponent<CancellableMove>().OnCancel(this);
		}
	}

	// Token: 0x06002143 RID: 8515 RVA: 0x000BA670 File Offset: 0x000B8870
	private void OnRefreshUserMenu(object data)
	{
		if (this.pickupable.KPrefabID.HasTag(GameTags.Stored))
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = this.isMarkedForMove ? new KIconButtonMenu.ButtonInfo("action_control", UI.USERMENUACTIONS.PICKUPABLEMOVE.NAME_OFF, new System.Action(this.OnClickCancel), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.PICKUPABLEMOVE.TOOLTIP_OFF, true) : new KIconButtonMenu.ButtonInfo("action_control", UI.USERMENUACTIONS.PICKUPABLEMOVE.NAME, new System.Action(this.OnClickMove), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.PICKUPABLEMOVE.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x06002144 RID: 8516 RVA: 0x000BA721 File Offset: 0x000B8921
	public void MoveToLocation(int cell)
	{
		this.CreateStorageProxy(cell);
		this.MarkForMove();
		base.gameObject.Trigger(1122777325, base.gameObject);
	}

	// Token: 0x06002145 RID: 8517 RVA: 0x000BA748 File Offset: 0x000B8948
	private void MarkForMove()
	{
		base.Trigger(2127324410, null);
		this.isMarkedForMove = true;
		this.OnReachableChanged(null);
		this.storageReachableChangedHandle = this.StorageProxy.Subscribe(-1432940121, new Action<object>(this.OnReachableChanged));
		this.reachableChangedHandle = base.Subscribe(-1432940121, new Action<object>(this.OnReachableChanged));
		this.StorageProxy.GetComponent<CancellableMove>().SetMovable(this);
		base.gameObject.AddTag(GameTags.MarkedForMove);
		this.cancelHandle = base.Subscribe(2127324410, new Action<object>(this.CleanupMove));
		this.UpdateStatusItem();
	}

	// Token: 0x06002146 RID: 8518 RVA: 0x000BA7F2 File Offset: 0x000B89F2
	private void UpdateStatusItem()
	{
		if (this.IsCritter())
		{
			this.shouldShowSkillPerkStatusItem = this.isMarkedForMove;
			base.UpdateStatusItem(null);
		}
	}

	// Token: 0x06002147 RID: 8519 RVA: 0x000BA80F File Offset: 0x000B8A0F
	private bool IsCritter()
	{
		return base.GetComponent<Capturable>() != null;
	}

	// Token: 0x06002148 RID: 8520 RVA: 0x000BA81D File Offset: 0x000B8A1D
	public bool CanMoveTo(int cell)
	{
		return !Grid.IsSolidCell(cell) && Grid.IsWorldValidCell(cell) && base.gameObject.IsMyParentWorld(cell);
	}

	// Token: 0x06002149 RID: 8521 RVA: 0x000BA840 File Offset: 0x000B8A40
	private void CreateStorageProxy(int cell)
	{
		if (this.storageProxy == null || this.storageProxy.Get() == null)
		{
			if (Grid.Objects[cell, 44] != null)
			{
				Storage component = Grid.Objects[cell, 44].GetComponent<Storage>();
				this.storageProxy = new Ref<Storage>(component);
				return;
			}
			Vector3 position = Grid.CellToPosCBC(cell, MoveToLocationTool.Instance.visualizerLayer);
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(MovePickupablePlacerConfig.ID), position);
			Storage component2 = gameObject.GetComponent<Storage>();
			gameObject.SetActive(true);
			this.storageProxy = new Ref<Storage>(component2);
		}
	}

	// Token: 0x040012A3 RID: 4771
	[MyCmpReq]
	private Pickupable pickupable;

	// Token: 0x040012A4 RID: 4772
	[Serialize]
	private bool isMarkedForMove;

	// Token: 0x040012A5 RID: 4773
	[Serialize]
	private Ref<Storage> storageProxy;

	// Token: 0x040012A6 RID: 4774
	private int storageReachableChangedHandle = -1;

	// Token: 0x040012A7 RID: 4775
	private int reachableChangedHandle = -1;

	// Token: 0x040012A8 RID: 4776
	private int cancelHandle = -1;

	// Token: 0x040012A9 RID: 4777
	private Guid pendingMoveGuid;

	// Token: 0x040012AA RID: 4778
	private Guid storageUnreachableGuid;
}
