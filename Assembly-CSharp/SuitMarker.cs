using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200077A RID: 1914
[AddComponentMenu("KMonoBehaviour/scripts/SuitMarker")]
public class SuitMarker : KMonoBehaviour
{
	// Token: 0x17000379 RID: 889
	// (get) Token: 0x060033DA RID: 13274 RVA: 0x0011BE20 File Offset: 0x0011A020
	// (set) Token: 0x060033DB RID: 13275 RVA: 0x0011BE40 File Offset: 0x0011A040
	private bool OnlyTraverseIfUnequipAvailable
	{
		get
		{
			DebugUtil.Assert(this.onlyTraverseIfUnequipAvailable == (this.gridFlags & Grid.SuitMarker.Flags.OnlyTraverseIfUnequipAvailable) > (Grid.SuitMarker.Flags)0);
			return this.onlyTraverseIfUnequipAvailable;
		}
		set
		{
			this.onlyTraverseIfUnequipAvailable = value;
			this.UpdateGridFlag(Grid.SuitMarker.Flags.OnlyTraverseIfUnequipAvailable, this.onlyTraverseIfUnequipAvailable);
		}
	}

	// Token: 0x1700037A RID: 890
	// (get) Token: 0x060033DC RID: 13276 RVA: 0x0011BE56 File Offset: 0x0011A056
	// (set) Token: 0x060033DD RID: 13277 RVA: 0x0011BE63 File Offset: 0x0011A063
	private bool isRotated
	{
		get
		{
			return (this.gridFlags & Grid.SuitMarker.Flags.Rotated) > (Grid.SuitMarker.Flags)0;
		}
		set
		{
			this.UpdateGridFlag(Grid.SuitMarker.Flags.Rotated, value);
		}
	}

	// Token: 0x1700037B RID: 891
	// (get) Token: 0x060033DE RID: 13278 RVA: 0x0011BE6D File Offset: 0x0011A06D
	// (set) Token: 0x060033DF RID: 13279 RVA: 0x0011BE7A File Offset: 0x0011A07A
	private bool isOperational
	{
		get
		{
			return (this.gridFlags & Grid.SuitMarker.Flags.Operational) > (Grid.SuitMarker.Flags)0;
		}
		set
		{
			this.UpdateGridFlag(Grid.SuitMarker.Flags.Operational, value);
		}
	}

	// Token: 0x060033E0 RID: 13280 RVA: 0x0011BE84 File Offset: 0x0011A084
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.OnlyTraverseIfUnequipAvailable = this.onlyTraverseIfUnequipAvailable;
		global::Debug.Assert(this.interactAnim != null, "interactAnim is null");
		base.Subscribe<SuitMarker>(493375141, SuitMarker.OnRefreshUserMenuDelegate);
		this.isOperational = base.GetComponent<Operational>().IsOperational;
		base.Subscribe<SuitMarker>(-592767678, SuitMarker.OnOperationalChangedDelegate);
		this.isRotated = base.GetComponent<Rotatable>().IsRotated;
		base.Subscribe<SuitMarker>(-1643076535, SuitMarker.OnRotatedDelegate);
		this.CreateNewEquipReactable();
		this.CreateNewUnequipReactable();
		this.cell = Grid.PosToCell(this);
		Grid.RegisterSuitMarker(this.cell);
		base.GetComponent<KAnimControllerBase>().Play("no_suit", KAnim.PlayMode.Once, 1f, 0f);
		Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Suits, true);
		this.RefreshTraverseIfUnequipStatusItem();
		SuitLocker.UpdateSuitMarkerStates(Grid.PosToCell(base.transform.position), base.gameObject);
	}

	// Token: 0x060033E1 RID: 13281 RVA: 0x0011BF80 File Offset: 0x0011A180
	private void CreateNewEquipReactable()
	{
		this.equipReactable = new SuitMarker.EquipSuitReactable(this);
	}

	// Token: 0x060033E2 RID: 13282 RVA: 0x0011BF8E File Offset: 0x0011A18E
	private void CreateNewUnequipReactable()
	{
		this.unequipReactable = new SuitMarker.UnequipSuitReactable(this);
	}

	// Token: 0x060033E3 RID: 13283 RVA: 0x0011BF9C File Offset: 0x0011A19C
	public void GetAttachedLockers(List<SuitLocker> suit_lockers)
	{
		int num = this.isRotated ? 1 : -1;
		int num2 = 1;
		for (;;)
		{
			int num3 = Grid.OffsetCell(this.cell, num2 * num, 0);
			GameObject gameObject = Grid.Objects[num3, 1];
			if (gameObject == null)
			{
				break;
			}
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (!(component == null))
			{
				if (!component.IsAnyPrefabID(this.LockerTags))
				{
					break;
				}
				SuitLocker component2 = gameObject.GetComponent<SuitLocker>();
				if (component2 == null)
				{
					break;
				}
				Operational component3 = gameObject.GetComponent<Operational>();
				if ((!(component3 != null) || component3.GetFlag(BuildingEnabledButton.EnabledFlag)) && !suit_lockers.Contains(component2))
				{
					suit_lockers.Add(component2);
				}
			}
			num2++;
		}
	}

	// Token: 0x060033E4 RID: 13284 RVA: 0x0011C04C File Offset: 0x0011A24C
	public static bool DoesTraversalDirectionRequireSuit(int source_cell, int dest_cell, Grid.SuitMarker.Flags flags)
	{
		return Grid.CellColumn(dest_cell) > Grid.CellColumn(source_cell) == ((flags & Grid.SuitMarker.Flags.Rotated) == (Grid.SuitMarker.Flags)0);
	}

	// Token: 0x060033E5 RID: 13285 RVA: 0x0011C064 File Offset: 0x0011A264
	public bool DoesTraversalDirectionRequireSuit(int source_cell, int dest_cell)
	{
		return SuitMarker.DoesTraversalDirectionRequireSuit(source_cell, dest_cell, this.gridFlags);
	}

	// Token: 0x060033E6 RID: 13286 RVA: 0x0011C074 File Offset: 0x0011A274
	private void Update()
	{
		ListPool<SuitLocker, SuitMarker>.PooledList pooledList = ListPool<SuitLocker, SuitMarker>.Allocate();
		this.GetAttachedLockers(pooledList);
		int num = 0;
		int num2 = 0;
		KPrefabID x = null;
		foreach (SuitLocker suitLocker in pooledList)
		{
			if (suitLocker.CanDropOffSuit())
			{
				num++;
			}
			if (suitLocker.GetPartiallyChargedOutfit() != null)
			{
				num2++;
			}
			if (x == null)
			{
				x = suitLocker.GetStoredOutfit();
			}
		}
		pooledList.Recycle();
		bool flag = x != null;
		if (flag != this.hasAvailableSuit)
		{
			base.GetComponent<KAnimControllerBase>().Play(flag ? "off" : "no_suit", KAnim.PlayMode.Once, 1f, 0f);
			this.hasAvailableSuit = flag;
		}
		Grid.UpdateSuitMarker(this.cell, num2, num, this.gridFlags, this.PathFlag);
	}

	// Token: 0x060033E7 RID: 13287 RVA: 0x0011C168 File Offset: 0x0011A368
	private void RefreshTraverseIfUnequipStatusItem()
	{
		if (this.OnlyTraverseIfUnequipAvailable)
		{
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.SuitMarkerTraversalOnlyWhenRoomAvailable, null);
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.SuitMarkerTraversalAnytime, false);
			return;
		}
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.SuitMarkerTraversalOnlyWhenRoomAvailable, false);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.SuitMarkerTraversalAnytime, null);
	}

	// Token: 0x060033E8 RID: 13288 RVA: 0x0011C1EE File Offset: 0x0011A3EE
	private void OnEnableTraverseIfUnequipAvailable()
	{
		this.OnlyTraverseIfUnequipAvailable = true;
		this.RefreshTraverseIfUnequipStatusItem();
	}

	// Token: 0x060033E9 RID: 13289 RVA: 0x0011C1FD File Offset: 0x0011A3FD
	private void OnDisableTraverseIfUnequipAvailable()
	{
		this.OnlyTraverseIfUnequipAvailable = false;
		this.RefreshTraverseIfUnequipStatusItem();
	}

	// Token: 0x060033EA RID: 13290 RVA: 0x0011C20C File Offset: 0x0011A40C
	private void UpdateGridFlag(Grid.SuitMarker.Flags flag, bool state)
	{
		if (state)
		{
			this.gridFlags |= flag;
			return;
		}
		this.gridFlags &= ~flag;
	}

	// Token: 0x060033EB RID: 13291 RVA: 0x0011C230 File Offset: 0x0011A430
	private void OnOperationalChanged(bool isOperational)
	{
		SuitLocker.UpdateSuitMarkerStates(Grid.PosToCell(base.transform.position), base.gameObject);
		this.isOperational = isOperational;
	}

	// Token: 0x060033EC RID: 13292 RVA: 0x0011C254 File Offset: 0x0011A454
	private void OnRefreshUserMenu(object data)
	{
		KIconButtonMenu.ButtonInfo button = (!this.OnlyTraverseIfUnequipAvailable) ? new KIconButtonMenu.ButtonInfo("action_clearance", UI.USERMENUACTIONS.SUIT_MARKER_TRAVERSAL.ONLY_WHEN_ROOM_AVAILABLE.NAME, new System.Action(this.OnEnableTraverseIfUnequipAvailable), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.SUIT_MARKER_TRAVERSAL.ONLY_WHEN_ROOM_AVAILABLE.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_clearance", UI.USERMENUACTIONS.SUIT_MARKER_TRAVERSAL.ALWAYS.NAME, new System.Action(this.OnDisableTraverseIfUnequipAvailable), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.SUIT_MARKER_TRAVERSAL.ALWAYS.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x060033ED RID: 13293 RVA: 0x0011C2F0 File Offset: 0x0011A4F0
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (base.isSpawned)
		{
			Grid.UnregisterSuitMarker(this.cell);
		}
		if (this.equipReactable != null)
		{
			this.equipReactable.Cleanup();
		}
		if (this.unequipReactable != null)
		{
			this.unequipReactable.Cleanup();
		}
		SuitLocker.UpdateSuitMarkerStates(Grid.PosToCell(base.transform.position), null);
	}

	// Token: 0x04001EB3 RID: 7859
	[MyCmpGet]
	private Building building;

	// Token: 0x04001EB4 RID: 7860
	private SuitMarker.SuitMarkerReactable equipReactable;

	// Token: 0x04001EB5 RID: 7861
	private SuitMarker.SuitMarkerReactable unequipReactable;

	// Token: 0x04001EB6 RID: 7862
	private bool hasAvailableSuit;

	// Token: 0x04001EB7 RID: 7863
	[Serialize]
	private bool onlyTraverseIfUnequipAvailable;

	// Token: 0x04001EB8 RID: 7864
	private Grid.SuitMarker.Flags gridFlags;

	// Token: 0x04001EB9 RID: 7865
	private int cell;

	// Token: 0x04001EBA RID: 7866
	public Tag[] LockerTags;

	// Token: 0x04001EBB RID: 7867
	public PathFinder.PotentialPath.Flags PathFlag;

	// Token: 0x04001EBC RID: 7868
	public KAnimFile interactAnim = Assets.GetAnim("anim_equip_clothing_kanim");

	// Token: 0x04001EBD RID: 7869
	private static readonly EventSystem.IntraObjectHandler<SuitMarker> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<SuitMarker>(delegate(SuitMarker component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04001EBE RID: 7870
	private static readonly EventSystem.IntraObjectHandler<SuitMarker> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<SuitMarker>(delegate(SuitMarker component, object data)
	{
		component.OnOperationalChanged((bool)data);
	});

	// Token: 0x04001EBF RID: 7871
	private static readonly EventSystem.IntraObjectHandler<SuitMarker> OnRotatedDelegate = new EventSystem.IntraObjectHandler<SuitMarker>(delegate(SuitMarker component, object data)
	{
		component.isRotated = ((Rotatable)data).IsRotated;
	});

	// Token: 0x0200161E RID: 5662
	private class EquipSuitReactable : SuitMarker.SuitMarkerReactable
	{
		// Token: 0x060090FA RID: 37114 RVA: 0x0034E8F7 File Offset: 0x0034CAF7
		public EquipSuitReactable(SuitMarker marker) : base("EquipSuitReactable", marker)
		{
		}

		// Token: 0x060090FB RID: 37115 RVA: 0x0034E90A File Offset: 0x0034CB0A
		public override bool InternalCanBegin(GameObject newReactor, Navigator.ActiveTransition transition)
		{
			return !newReactor.GetComponent<MinionIdentity>().GetEquipment().IsSlotOccupied(Db.Get().AssignableSlots.Suit) && base.InternalCanBegin(newReactor, transition);
		}

		// Token: 0x060090FC RID: 37116 RVA: 0x0034E93A File Offset: 0x0034CB3A
		protected override void InternalBegin()
		{
			base.InternalBegin();
			this.suitMarker.CreateNewEquipReactable();
		}

		// Token: 0x060090FD RID: 37117 RVA: 0x0034E950 File Offset: 0x0034CB50
		protected override bool MovingTheRightWay(GameObject newReactor, Navigator.ActiveTransition transition)
		{
			bool flag = transition.navGridTransition.x < 0;
			return this.IsRocketDoorExitEquip(newReactor, transition) || flag == this.suitMarker.isRotated;
		}

		// Token: 0x060090FE RID: 37118 RVA: 0x0034E988 File Offset: 0x0034CB88
		private bool IsRocketDoorExitEquip(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			bool flag = transition.end != NavType.Teleport && transition.start != NavType.Teleport;
			return transition.navGridTransition.x == 0 && new_reactor.GetMyWorld().IsModuleInterior && !flag;
		}

		// Token: 0x060090FF RID: 37119 RVA: 0x0034E9D0 File Offset: 0x0034CBD0
		protected override void Run()
		{
			ListPool<SuitLocker, SuitMarker>.PooledList pooledList = ListPool<SuitLocker, SuitMarker>.Allocate();
			this.suitMarker.GetAttachedLockers(pooledList);
			SuitLocker suitLocker = null;
			for (int i = 0; i < pooledList.Count; i++)
			{
				float suitScore = pooledList[i].GetSuitScore();
				if (suitScore >= 1f)
				{
					suitLocker = pooledList[i];
					break;
				}
				if (suitLocker == null || suitScore > suitLocker.GetSuitScore())
				{
					suitLocker = pooledList[i];
				}
			}
			pooledList.Recycle();
			if (suitLocker != null)
			{
				Equipment equipment = this.reactor.GetComponent<MinionIdentity>().GetEquipment();
				SuitWearer.Instance smi = this.reactor.GetSMI<SuitWearer.Instance>();
				suitLocker.EquipTo(equipment);
				smi.UnreserveSuits();
				this.suitMarker.Update();
			}
		}
	}

	// Token: 0x0200161F RID: 5663
	private class UnequipSuitReactable : SuitMarker.SuitMarkerReactable
	{
		// Token: 0x06009100 RID: 37120 RVA: 0x0034EA7F File Offset: 0x0034CC7F
		public UnequipSuitReactable(SuitMarker marker) : base("UnequipSuitReactable", marker)
		{
		}

		// Token: 0x06009101 RID: 37121 RVA: 0x0034EA94 File Offset: 0x0034CC94
		public override bool InternalCanBegin(GameObject newReactor, Navigator.ActiveTransition transition)
		{
			Navigator component = newReactor.GetComponent<Navigator>();
			return newReactor.GetComponent<MinionIdentity>().GetEquipment().IsSlotOccupied(Db.Get().AssignableSlots.Suit) && component != null && (component.flags & this.suitMarker.PathFlag) > PathFinder.PotentialPath.Flags.None && base.InternalCanBegin(newReactor, transition);
		}

		// Token: 0x06009102 RID: 37122 RVA: 0x0034EAFC File Offset: 0x0034CCFC
		protected override void InternalBegin()
		{
			base.InternalBegin();
			this.suitMarker.CreateNewUnequipReactable();
		}

		// Token: 0x06009103 RID: 37123 RVA: 0x0034EB10 File Offset: 0x0034CD10
		protected override bool MovingTheRightWay(GameObject newReactor, Navigator.ActiveTransition transition)
		{
			bool flag = transition.navGridTransition.x < 0;
			return transition.navGridTransition.x != 0 && flag != this.suitMarker.isRotated;
		}

		// Token: 0x06009104 RID: 37124 RVA: 0x0034EB4C File Offset: 0x0034CD4C
		protected override void Run()
		{
			Navigator component = this.reactor.GetComponent<Navigator>();
			Equipment equipment = this.reactor.GetComponent<MinionIdentity>().GetEquipment();
			if (component != null && (component.flags & this.suitMarker.PathFlag) > PathFinder.PotentialPath.Flags.None)
			{
				ListPool<SuitLocker, SuitMarker>.PooledList pooledList = ListPool<SuitLocker, SuitMarker>.Allocate();
				this.suitMarker.GetAttachedLockers(pooledList);
				SuitLocker suitLocker = null;
				int num = 0;
				while (suitLocker == null && num < pooledList.Count)
				{
					if (pooledList[num].CanDropOffSuit())
					{
						suitLocker = pooledList[num];
					}
					num++;
				}
				pooledList.Recycle();
				if (suitLocker != null)
				{
					suitLocker.UnequipFrom(equipment);
					component.GetSMI<SuitWearer.Instance>().UnreserveSuits();
					this.suitMarker.Update();
					return;
				}
			}
			Assignable assignable = equipment.GetAssignable(Db.Get().AssignableSlots.Suit);
			if (assignable != null)
			{
				assignable.Unassign();
				Notification notification = new Notification(MISC.NOTIFICATIONS.SUIT_DROPPED.NAME, NotificationType.BadMinor, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.SUIT_DROPPED.TOOLTIP, null, true, 0f, null, null, null, true, false, false);
				assignable.GetComponent<Notifier>().Add(notification, "");
			}
		}
	}

	// Token: 0x02001620 RID: 5664
	private abstract class SuitMarkerReactable : Reactable
	{
		// Token: 0x06009105 RID: 37125 RVA: 0x0034EC8C File Offset: 0x0034CE8C
		public SuitMarkerReactable(HashedString id, SuitMarker suit_marker) : base(suit_marker.gameObject, id, Db.Get().ChoreTypes.SuitMarker, 1, 1, false, 0f, 0f, float.PositiveInfinity, 0f, ObjectLayer.NumLayers)
		{
			this.suitMarker = suit_marker;
		}

		// Token: 0x06009106 RID: 37126 RVA: 0x0034ECD5 File Offset: 0x0034CED5
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (this.reactor != null)
			{
				return false;
			}
			if (this.suitMarker == null)
			{
				base.Cleanup();
				return false;
			}
			return this.suitMarker.isOperational && this.MovingTheRightWay(new_reactor, transition);
		}

		// Token: 0x06009107 RID: 37127 RVA: 0x0034ED14 File Offset: 0x0034CF14
		protected override void InternalBegin()
		{
			this.startTime = Time.time;
			KBatchedAnimController component = this.reactor.GetComponent<KBatchedAnimController>();
			component.AddAnimOverrides(this.suitMarker.interactAnim, 1f);
			component.Play("working_pre", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("working_loop", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("working_pst", KAnim.PlayMode.Once, 1f, 0f);
			if (this.suitMarker.HasTag(GameTags.JetSuitBlocker))
			{
				KBatchedAnimController component2 = this.suitMarker.GetComponent<KBatchedAnimController>();
				component2.Play("working_pre", KAnim.PlayMode.Once, 1f, 0f);
				component2.Queue("working_loop", KAnim.PlayMode.Once, 1f, 0f);
				component2.Queue("working_pst", KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x06009108 RID: 37128 RVA: 0x0034EE0C File Offset: 0x0034D00C
		public override void Update(float dt)
		{
			Facing facing = this.reactor ? this.reactor.GetComponent<Facing>() : null;
			if (facing && this.suitMarker)
			{
				facing.SetFacing(this.suitMarker.GetComponent<Rotatable>().GetOrientation() == Orientation.FlipH);
			}
			if (Time.time - this.startTime > 2.8f)
			{
				if (this.reactor != null && this.suitMarker != null)
				{
					this.reactor.GetComponent<KBatchedAnimController>().RemoveAnimOverrides(this.suitMarker.interactAnim);
					this.Run();
				}
				base.Cleanup();
			}
		}

		// Token: 0x06009109 RID: 37129 RVA: 0x0034EEB9 File Offset: 0x0034D0B9
		protected override void InternalEnd()
		{
			if (this.reactor != null)
			{
				this.reactor.GetComponent<KBatchedAnimController>().RemoveAnimOverrides(this.suitMarker.interactAnim);
			}
		}

		// Token: 0x0600910A RID: 37130 RVA: 0x0034EEE4 File Offset: 0x0034D0E4
		protected override void InternalCleanup()
		{
		}

		// Token: 0x0600910B RID: 37131
		protected abstract bool MovingTheRightWay(GameObject reactor, Navigator.ActiveTransition transition);

		// Token: 0x0600910C RID: 37132
		protected abstract void Run();

		// Token: 0x04006EB9 RID: 28345
		protected SuitMarker suitMarker;

		// Token: 0x04006EBA RID: 28346
		protected float startTime;
	}
}
