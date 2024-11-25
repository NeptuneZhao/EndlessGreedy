using System;
using System.Collections.Generic;
using Klei.AI;
using ProcGen;
using STRINGS;
using UnityEngine;

// Token: 0x02000424 RID: 1060
public class MinionBrain : Brain
{
	// Token: 0x0600169D RID: 5789 RVA: 0x00079260 File Offset: 0x00077460
	public bool IsCellClear(int cell)
	{
		if (Grid.Reserved[cell])
		{
			return false;
		}
		GameObject gameObject = Grid.Objects[cell, 0];
		return !(gameObject != null) || !(base.gameObject != gameObject) || gameObject.GetComponent<Navigator>().IsMoving();
	}

	// Token: 0x0600169E RID: 5790 RVA: 0x000792B6 File Offset: 0x000774B6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.Navigator.SetAbilities(new MinionPathFinderAbilities(this.Navigator));
		base.Subscribe<MinionBrain>(-1697596308, MinionBrain.AnimTrackStoredItemDelegate);
		base.Subscribe<MinionBrain>(-975551167, MinionBrain.OnUnstableGroundImpactDelegate);
	}

	// Token: 0x0600169F RID: 5791 RVA: 0x000792F8 File Offset: 0x000774F8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		foreach (GameObject go in base.GetComponent<Storage>().items)
		{
			this.AddAnimTracker(go);
		}
		Game.Instance.Subscribe(-107300940, new Action<object>(this.OnResearchComplete));
	}

	// Token: 0x060016A0 RID: 5792 RVA: 0x00079374 File Offset: 0x00077574
	private void AnimTrackStoredItem(object data)
	{
		Storage component = base.GetComponent<Storage>();
		GameObject gameObject = (GameObject)data;
		this.RemoveTracker(gameObject);
		if (component.items.Contains(gameObject))
		{
			this.AddAnimTracker(gameObject);
		}
	}

	// Token: 0x060016A1 RID: 5793 RVA: 0x000793AC File Offset: 0x000775AC
	private void AddAnimTracker(GameObject go)
	{
		KAnimControllerBase component = go.GetComponent<KAnimControllerBase>();
		if (component == null)
		{
			return;
		}
		if (component.AnimFiles != null && component.AnimFiles.Length != 0 && component.AnimFiles[0] != null && component.GetComponent<Pickupable>().trackOnPickup)
		{
			KBatchedAnimTracker kbatchedAnimTracker = go.AddComponent<KBatchedAnimTracker>();
			kbatchedAnimTracker.useTargetPoint = false;
			kbatchedAnimTracker.fadeOut = false;
			kbatchedAnimTracker.symbol = new HashedString("snapTo_chest");
			kbatchedAnimTracker.forceAlwaysVisible = true;
		}
	}

	// Token: 0x060016A2 RID: 5794 RVA: 0x00079424 File Offset: 0x00077624
	private void RemoveTracker(GameObject go)
	{
		KBatchedAnimTracker component = go.GetComponent<KBatchedAnimTracker>();
		if (component != null)
		{
			UnityEngine.Object.Destroy(component);
		}
	}

	// Token: 0x060016A3 RID: 5795 RVA: 0x00079448 File Offset: 0x00077648
	public override void UpdateBrain()
	{
		base.UpdateBrain();
		if (Game.Instance == null)
		{
			return;
		}
		if (!Game.Instance.savedInfo.discoveredSurface)
		{
			int cell = Grid.PosToCell(base.gameObject);
			if (global::World.Instance.zoneRenderData.GetSubWorldZoneType(cell) == SubWorld.ZoneType.Space)
			{
				Game.Instance.savedInfo.discoveredSurface = true;
				DiscoveredSpaceMessage message = new DiscoveredSpaceMessage(base.gameObject.transform.GetPosition());
				Messenger.Instance.QueueMessage(message);
				Game.Instance.Trigger(-818188514, base.gameObject);
			}
		}
		if (!Game.Instance.savedInfo.discoveredOilField)
		{
			int cell2 = Grid.PosToCell(base.gameObject);
			if (global::World.Instance.zoneRenderData.GetSubWorldZoneType(cell2) == SubWorld.ZoneType.OilField)
			{
				Game.Instance.savedInfo.discoveredOilField = true;
			}
		}
	}

	// Token: 0x060016A4 RID: 5796 RVA: 0x00079520 File Offset: 0x00077720
	private void RegisterReactEmotePair(string reactable_id, Emote emote, float max_trigger_time)
	{
		if (base.gameObject == null)
		{
			return;
		}
		ReactionMonitor.Instance smi = base.gameObject.GetSMI<ReactionMonitor.Instance>();
		if (smi != null)
		{
			EmoteChore emoteChore = new EmoteChore(base.gameObject.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteIdle, emote, 1, null);
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(base.gameObject, reactable_id, Db.Get().ChoreTypes.Cough, max_trigger_time, 20f, float.PositiveInfinity, 0f);
			emoteChore.PairReactable(selfEmoteReactable);
			selfEmoteReactable.SetEmote(emote);
			selfEmoteReactable.PairEmote(emoteChore);
			smi.AddOneshotReactable(selfEmoteReactable);
		}
	}

	// Token: 0x060016A5 RID: 5797 RVA: 0x000795BC File Offset: 0x000777BC
	private void OnResearchComplete(object data)
	{
		if (Time.time - this.lastResearchCompleteEmoteTime > 1f)
		{
			this.RegisterReactEmotePair("ResearchComplete", Db.Get().Emotes.Minion.ResearchComplete, 3f);
			this.lastResearchCompleteEmoteTime = Time.time;
		}
	}

	// Token: 0x060016A6 RID: 5798 RVA: 0x0007960C File Offset: 0x0007780C
	public Notification CreateCollapseNotification()
	{
		MinionIdentity component = base.GetComponent<MinionIdentity>();
		return new Notification(MISC.NOTIFICATIONS.TILECOLLAPSE.NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.TILECOLLAPSE.TOOLTIP + notificationList.ReduceMessages(false), "/t• " + component.GetProperName(), true, 0f, null, null, null, true, false, false);
	}

	// Token: 0x060016A7 RID: 5799 RVA: 0x0007966C File Offset: 0x0007786C
	public void RemoveCollapseNotification(Notification notification)
	{
		Vector3 position = notification.clickFocus.GetPosition();
		position.z = -40f;
		WorldContainer myWorld = notification.clickFocus.gameObject.GetMyWorld();
		if (myWorld != null && myWorld.IsDiscovered)
		{
			CameraController.Instance.ActiveWorldStarWipe(myWorld.id, position, 10f, null);
		}
		base.gameObject.AddOrGet<Notifier>().Remove(notification);
	}

	// Token: 0x060016A8 RID: 5800 RVA: 0x000796DC File Offset: 0x000778DC
	private void OnUnstableGroundImpact(object data)
	{
		GameObject telepad = GameUtil.GetTelepad(base.gameObject.GetMyWorld().id);
		Navigator component = base.GetComponent<Navigator>();
		Assignable assignable = base.GetComponent<MinionIdentity>().GetSoleOwner().GetAssignable(Db.Get().AssignableSlots.Bed);
		bool flag = assignable != null && component.CanReach(Grid.PosToCell(assignable.transform.GetPosition()));
		bool flag2 = telepad != null && component.CanReach(Grid.PosToCell(telepad.transform.GetPosition()));
		if (!flag && !flag2)
		{
			this.RegisterReactEmotePair("UnstableGroundShock", Db.Get().Emotes.Minion.Shock, 1f);
			Notification notification = this.CreateCollapseNotification();
			notification.customClickCallback = delegate(object o)
			{
				this.RemoveCollapseNotification(notification);
			};
			base.gameObject.AddOrGet<Notifier>().Add(notification, "");
		}
	}

	// Token: 0x060016A9 RID: 5801 RVA: 0x000797E1 File Offset: 0x000779E1
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Game.Instance.Unsubscribe(-107300940, new Action<object>(this.OnResearchComplete));
	}

	// Token: 0x04000CA3 RID: 3235
	[MyCmpReq]
	public Navigator Navigator;

	// Token: 0x04000CA4 RID: 3236
	[MyCmpGet]
	public OxygenBreather OxygenBreather;

	// Token: 0x04000CA5 RID: 3237
	private float lastResearchCompleteEmoteTime;

	// Token: 0x04000CA6 RID: 3238
	private static readonly EventSystem.IntraObjectHandler<MinionBrain> AnimTrackStoredItemDelegate = new EventSystem.IntraObjectHandler<MinionBrain>(delegate(MinionBrain component, object data)
	{
		component.AnimTrackStoredItem(data);
	});

	// Token: 0x04000CA7 RID: 3239
	private static readonly EventSystem.IntraObjectHandler<MinionBrain> OnUnstableGroundImpactDelegate = new EventSystem.IntraObjectHandler<MinionBrain>(delegate(MinionBrain component, object data)
	{
		component.OnUnstableGroundImpact(data);
	});
}
