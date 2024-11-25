using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020009E1 RID: 2529
public class POITechItemUnlocks : GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>
{
	// Token: 0x06004965 RID: 18789 RVA: 0x001A4580 File Offset: 0x001A2780
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.locked;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.locked.PlayAnim("on", KAnim.PlayMode.Loop).ParamTransition<bool>(this.isUnlocked, this.unlocked, GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.IsTrue);
		this.unlocked.ParamTransition<bool>(this.seenNotification, this.unlocked.notify, GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.IsFalse).ParamTransition<bool>(this.seenNotification, this.unlocked.done, GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.IsTrue);
		this.unlocked.notify.PlayAnim("notify", KAnim.PlayMode.Loop).ToggleStatusItem(Db.Get().MiscStatusItems.AttentionRequired, null).ToggleNotification(delegate(POITechItemUnlocks.Instance smi)
		{
			smi.notificationReference = EventInfoScreen.CreateNotification(POITechItemUnlocks.GenerateEventPopupData(smi), null);
			smi.notificationReference.Type = NotificationType.MessageImportant;
			return smi.notificationReference;
		});
		this.unlocked.done.PlayAnim("off");
	}

	// Token: 0x06004966 RID: 18790 RVA: 0x001A4668 File Offset: 0x001A2868
	private static string GetMessageBody(POITechItemUnlocks.Instance smi)
	{
		string text = "";
		foreach (TechItem techItem in smi.unlockTechItems)
		{
			text = text + "\n    • " + techItem.Name;
		}
		return string.Format(MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.MESSAGEBODY, text);
	}

	// Token: 0x06004967 RID: 18791 RVA: 0x001A46DC File Offset: 0x001A28DC
	private static EventInfoData GenerateEventPopupData(POITechItemUnlocks.Instance smi)
	{
		EventInfoData eventInfoData = new EventInfoData(MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.NAME, POITechItemUnlocks.GetMessageBody(smi), smi.def.animName);
		int num = Mathf.Max(2, Components.LiveMinionIdentities.Count);
		GameObject[] array = new GameObject[num];
		using (IEnumerator<MinionIdentity> enumerator = Components.LiveMinionIdentities.Shuffle<MinionIdentity>().GetEnumerator())
		{
			for (int i = 0; i < num; i++)
			{
				if (!enumerator.MoveNext())
				{
					num = 0;
					array = new GameObject[num];
					break;
				}
				array[i] = enumerator.Current.gameObject;
			}
		}
		eventInfoData.minions = array;
		if (smi.def.loreUnlockId != null)
		{
			eventInfoData.AddOption(MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.BUTTON_VIEW_LORE, null).callback = delegate()
			{
				smi.sm.seenNotification.Set(true, smi, false);
				smi.notificationReference = null;
				Game.Instance.unlocks.Unlock(smi.def.loreUnlockId, true);
				ManagementMenu.Instance.OpenCodexToLockId(smi.def.loreUnlockId, false);
			};
		}
		eventInfoData.AddDefaultOption(delegate
		{
			smi.sm.seenNotification.Set(true, smi, false);
			smi.notificationReference = null;
		});
		eventInfoData.clickFocus = smi.gameObject.transform;
		return eventInfoData;
	}

	// Token: 0x04003004 RID: 12292
	public GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.State locked;

	// Token: 0x04003005 RID: 12293
	public POITechItemUnlocks.UnlockedStates unlocked;

	// Token: 0x04003006 RID: 12294
	public StateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.BoolParameter isUnlocked;

	// Token: 0x04003007 RID: 12295
	public StateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.BoolParameter pendingChore;

	// Token: 0x04003008 RID: 12296
	public StateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.BoolParameter seenNotification;

	// Token: 0x020019E2 RID: 6626
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007AC4 RID: 31428
		public List<string> POITechUnlockIDs;

		// Token: 0x04007AC5 RID: 31429
		public LocString PopUpName;

		// Token: 0x04007AC6 RID: 31430
		public string animName;

		// Token: 0x04007AC7 RID: 31431
		public string loreUnlockId;
	}

	// Token: 0x020019E3 RID: 6627
	public new class Instance : GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.GameInstance, ISidescreenButtonControl
	{
		// Token: 0x06009E51 RID: 40529 RVA: 0x003776E8 File Offset: 0x003758E8
		public Instance(IStateMachineTarget master, POITechItemUnlocks.Def def) : base(master, def)
		{
			this.unlockTechItems = new List<TechItem>(def.POITechUnlockIDs.Count);
			foreach (string text in def.POITechUnlockIDs)
			{
				TechItem techItem = Db.Get().TechItems.TryGet(text);
				if (techItem != null)
				{
					this.unlockTechItems.Add(techItem);
				}
				else
				{
					DebugUtil.DevAssert(false, "Invalid tech item " + text + " for POI Tech Unlock", null);
				}
			}
		}

		// Token: 0x06009E52 RID: 40530 RVA: 0x0037778C File Offset: 0x0037598C
		public override void StartSM()
		{
			base.Subscribe(-1503271301, new Action<object>(this.OnBuildingSelect));
			this.UpdateUnlocked();
			base.StartSM();
			if (base.sm.pendingChore.Get(this) && this.unlockChore == null)
			{
				this.CreateChore();
			}
		}

		// Token: 0x06009E53 RID: 40531 RVA: 0x003777DD File Offset: 0x003759DD
		public override void StopSM(string reason)
		{
			base.Unsubscribe(-1503271301, new Action<object>(this.OnBuildingSelect));
			base.StopSM(reason);
		}

		// Token: 0x06009E54 RID: 40532 RVA: 0x00377800 File Offset: 0x00375A00
		public void OnBuildingSelect(object obj)
		{
			if (!(bool)obj)
			{
				return;
			}
			if (!base.sm.seenNotification.Get(this) && this.notificationReference != null)
			{
				this.notificationReference.customClickCallback(this.notificationReference.customClickData);
			}
		}

		// Token: 0x06009E55 RID: 40533 RVA: 0x0037784C File Offset: 0x00375A4C
		private void ShowPopup()
		{
		}

		// Token: 0x06009E56 RID: 40534 RVA: 0x00377850 File Offset: 0x00375A50
		public void UnlockTechItems()
		{
			foreach (TechItem techItem in this.unlockTechItems)
			{
				if (techItem != null)
				{
					techItem.POIUnlocked();
				}
			}
			MusicManager.instance.PlaySong("Stinger_ResearchComplete", false);
			this.UpdateUnlocked();
		}

		// Token: 0x06009E57 RID: 40535 RVA: 0x003778BC File Offset: 0x00375ABC
		private void UpdateUnlocked()
		{
			bool value = true;
			using (List<TechItem>.Enumerator enumerator = this.unlockTechItems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.IsComplete())
					{
						value = false;
						break;
					}
				}
			}
			base.sm.isUnlocked.Set(value, base.smi, false);
		}

		// Token: 0x17000AFB RID: 2811
		// (get) Token: 0x06009E58 RID: 40536 RVA: 0x00377930 File Offset: 0x00375B30
		public string SidescreenButtonText
		{
			get
			{
				if (base.sm.isUnlocked.Get(base.smi))
				{
					return UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.ALREADY_RUMMAGED;
				}
				if (this.unlockChore != null)
				{
					return UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.NAME_OFF;
				}
				return UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.NAME;
			}
		}

		// Token: 0x17000AFC RID: 2812
		// (get) Token: 0x06009E59 RID: 40537 RVA: 0x00377980 File Offset: 0x00375B80
		public string SidescreenButtonTooltip
		{
			get
			{
				if (base.sm.isUnlocked.Get(base.smi))
				{
					return UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.TOOLTIP_ALREADYRUMMAGED;
				}
				if (this.unlockChore != null)
				{
					return UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.TOOLTIP_OFF;
				}
				return UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.TOOLTIP;
			}
		}

		// Token: 0x06009E5A RID: 40538 RVA: 0x003779CD File Offset: 0x00375BCD
		public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06009E5B RID: 40539 RVA: 0x003779D4 File Offset: 0x00375BD4
		public bool SidescreenEnabled()
		{
			return base.smi.IsInsideState(base.sm.locked);
		}

		// Token: 0x06009E5C RID: 40540 RVA: 0x003779EC File Offset: 0x00375BEC
		public bool SidescreenButtonInteractable()
		{
			return base.smi.IsInsideState(base.sm.locked);
		}

		// Token: 0x06009E5D RID: 40541 RVA: 0x00377A04 File Offset: 0x00375C04
		public void OnSidescreenButtonPressed()
		{
			if (this.unlockChore == null)
			{
				base.smi.sm.pendingChore.Set(true, base.smi, false);
				base.smi.CreateChore();
				return;
			}
			base.smi.sm.pendingChore.Set(false, base.smi, false);
			base.smi.CancelChore();
		}

		// Token: 0x06009E5E RID: 40542 RVA: 0x00377A6C File Offset: 0x00375C6C
		private void CreateChore()
		{
			Workable component = base.smi.master.GetComponent<POITechItemUnlockWorkable>();
			Prioritizable.AddRef(base.gameObject);
			base.Trigger(1980521255, null);
			this.unlockChore = new WorkChore<POITechItemUnlockWorkable>(Db.Get().ChoreTypes.Research, component, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}

		// Token: 0x06009E5F RID: 40543 RVA: 0x00377ACD File Offset: 0x00375CCD
		private void CancelChore()
		{
			this.unlockChore.Cancel("UserCancel");
			this.unlockChore = null;
			Prioritizable.RemoveRef(base.gameObject);
			base.Trigger(1980521255, null);
		}

		// Token: 0x06009E60 RID: 40544 RVA: 0x00377AFD File Offset: 0x00375CFD
		public int HorizontalGroupID()
		{
			return -1;
		}

		// Token: 0x06009E61 RID: 40545 RVA: 0x00377B00 File Offset: 0x00375D00
		public int ButtonSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x04007AC8 RID: 31432
		public List<TechItem> unlockTechItems;

		// Token: 0x04007AC9 RID: 31433
		public Notification notificationReference;

		// Token: 0x04007ACA RID: 31434
		private Chore unlockChore;
	}

	// Token: 0x020019E4 RID: 6628
	public class UnlockedStates : GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.State
	{
		// Token: 0x04007ACB RID: 31435
		public GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.State notify;

		// Token: 0x04007ACC RID: 31436
		public GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.State done;
	}
}
