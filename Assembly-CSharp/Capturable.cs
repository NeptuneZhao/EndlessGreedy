using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020007F5 RID: 2037
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Capturable")]
public class Capturable : Workable, IGameObjectEffectDescriptor
{
	// Token: 0x170003F0 RID: 1008
	// (get) Token: 0x0600384B RID: 14411 RVA: 0x001335D3 File Offset: 0x001317D3
	public bool IsMarkedForCapture
	{
		get
		{
			return this.markedForCapture;
		}
	}

	// Token: 0x0600384C RID: 14412 RVA: 0x001335DC File Offset: 0x001317DC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.Capturables.Add(this);
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.attributeConverter = Db.Get().AttributeConverters.CapturableSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Ranching.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.requiredSkillPerk = Db.Get().SkillPerks.CanWrangleCreatures.Id;
		this.resetProgressOnStop = true;
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
		this.multitoolContext = "capture";
		this.multitoolHitEffectTag = "fx_capture_splash";
	}

	// Token: 0x0600384D RID: 14413 RVA: 0x0013369C File Offset: 0x0013189C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Capturable>(1623392196, Capturable.OnDeathDelegate);
		base.Subscribe<Capturable>(493375141, Capturable.OnRefreshUserMenuDelegate);
		base.Subscribe<Capturable>(-1582839653, Capturable.OnTagsChangedDelegate);
		if (this.markedForCapture)
		{
			Prioritizable.AddRef(base.gameObject);
		}
		this.UpdateStatusItem();
		this.UpdateChore();
		base.SetWorkTime(10f);
	}

	// Token: 0x0600384E RID: 14414 RVA: 0x0013370C File Offset: 0x0013190C
	protected override void OnCleanUp()
	{
		Components.Capturables.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x0600384F RID: 14415 RVA: 0x00133720 File Offset: 0x00131920
	public override Vector3 GetTargetPoint()
	{
		Vector3 result = base.transform.GetPosition();
		KBoxCollider2D component = base.GetComponent<KBoxCollider2D>();
		if (component != null)
		{
			result = component.bounds.center;
		}
		result.z = 0f;
		return result;
	}

	// Token: 0x06003850 RID: 14416 RVA: 0x00133765 File Offset: 0x00131965
	private void OnDeath(object data)
	{
		this.allowCapture = false;
		this.markedForCapture = false;
		this.UpdateChore();
	}

	// Token: 0x06003851 RID: 14417 RVA: 0x0013377B File Offset: 0x0013197B
	private void OnTagsChanged(object data)
	{
		this.MarkForCapture(this.markedForCapture);
	}

	// Token: 0x06003852 RID: 14418 RVA: 0x0013378C File Offset: 0x0013198C
	public void MarkForCapture(bool mark)
	{
		PrioritySetting priority = new PrioritySetting(PriorityScreen.PriorityClass.basic, 5);
		this.MarkForCapture(mark, priority, false);
	}

	// Token: 0x06003853 RID: 14419 RVA: 0x001337AC File Offset: 0x001319AC
	public void MarkForCapture(bool mark, PrioritySetting priority, bool updateMarkedPriority = false)
	{
		mark = (mark && this.IsCapturable());
		if (this.markedForCapture && !mark)
		{
			Prioritizable.RemoveRef(base.gameObject);
		}
		else if (!this.markedForCapture && mark)
		{
			Prioritizable.AddRef(base.gameObject);
			Prioritizable component = base.GetComponent<Prioritizable>();
			if (component)
			{
				component.SetMasterPriority(priority);
			}
		}
		else if (updateMarkedPriority && this.markedForCapture && mark)
		{
			Prioritizable component2 = base.GetComponent<Prioritizable>();
			if (component2)
			{
				component2.SetMasterPriority(priority);
			}
		}
		this.markedForCapture = mark;
		this.UpdateStatusItem();
		this.UpdateChore();
	}

	// Token: 0x06003854 RID: 14420 RVA: 0x00133848 File Offset: 0x00131A48
	public bool IsCapturable()
	{
		return this.allowCapture && !base.gameObject.HasTag(GameTags.Trapped) && !base.gameObject.HasTag(GameTags.Stored) && !base.gameObject.HasTag(GameTags.Creatures.Bagged);
	}

	// Token: 0x06003855 RID: 14421 RVA: 0x0013389C File Offset: 0x00131A9C
	private void OnRefreshUserMenu(object data)
	{
		if (!this.IsCapturable())
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (!this.markedForCapture) ? new KIconButtonMenu.ButtonInfo("action_capture", UI.USERMENUACTIONS.CAPTURE.NAME, delegate()
		{
			this.MarkForCapture(true);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CAPTURE.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_capture", UI.USERMENUACTIONS.CANCELCAPTURE.NAME, delegate()
		{
			this.MarkForCapture(false);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCELCAPTURE.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x06003856 RID: 14422 RVA: 0x00133940 File Offset: 0x00131B40
	private void UpdateStatusItem()
	{
		this.shouldShowSkillPerkStatusItem = this.markedForCapture;
		base.UpdateStatusItem(null);
		if (this.markedForCapture)
		{
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.OrderCapture, this);
			return;
		}
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.OrderCapture, false);
	}

	// Token: 0x06003857 RID: 14423 RVA: 0x001339A4 File Offset: 0x00131BA4
	private void UpdateChore()
	{
		if (this.markedForCapture && this.chore == null)
		{
			this.chore = new WorkChore<Capturable>(Db.Get().ChoreTypes.Capture, this, null, true, null, new Action<Chore>(this.OnChoreBegins), new Action<Chore>(this.OnChoreEnds), true, null, false, true, null, true, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			return;
		}
		if (!this.markedForCapture && this.chore != null)
		{
			this.chore.Cancel("not marked for capture");
			this.chore = null;
		}
	}

	// Token: 0x06003858 RID: 14424 RVA: 0x00133A2C File Offset: 0x00131C2C
	private void OnChoreBegins(Chore chore)
	{
		IdleStates.Instance smi = base.gameObject.GetSMI<IdleStates.Instance>();
		if (smi != null)
		{
			smi.GoTo(smi.sm.root);
			smi.GetComponent<Navigator>().Stop(false, true);
		}
	}

	// Token: 0x06003859 RID: 14425 RVA: 0x00133A68 File Offset: 0x00131C68
	private void OnChoreEnds(Chore chore)
	{
		IdleStates.Instance smi = base.gameObject.GetSMI<IdleStates.Instance>();
		if (smi != null)
		{
			smi.GoTo(smi.sm.GetDefaultState());
		}
	}

	// Token: 0x0600385A RID: 14426 RVA: 0x00133A95 File Offset: 0x00131C95
	protected override void OnStartWork(WorkerBase worker)
	{
		base.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.Stunned, false);
	}

	// Token: 0x0600385B RID: 14427 RVA: 0x00133AA8 File Offset: 0x00131CA8
	protected override void OnStopWork(WorkerBase worker)
	{
		base.GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.Stunned);
	}

	// Token: 0x0600385C RID: 14428 RVA: 0x00133ABC File Offset: 0x00131CBC
	protected override void OnCompleteWork(WorkerBase worker)
	{
		int num = this.NaturalBuildingCell();
		if (Grid.Solid[num])
		{
			int num2 = Grid.CellAbove(num);
			if (Grid.IsValidCell(num2) && !Grid.Solid[num2])
			{
				num = num2;
			}
		}
		this.MarkForCapture(false);
		this.baggable.SetWrangled();
		this.baggable.transform.SetPosition(Grid.CellToPosCCC(num, Grid.SceneLayer.Ore));
	}

	// Token: 0x0600385D RID: 14429 RVA: 0x00133B28 File Offset: 0x00131D28
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		if (this.allowCapture)
		{
			descriptors.Add(new Descriptor(UI.BUILDINGEFFECTS.CAPTURE_METHOD_WRANGLE, UI.BUILDINGEFFECTS.TOOLTIPS.CAPTURE_METHOD_WRANGLE, Descriptor.DescriptorType.Effect, false));
		}
		return descriptors;
	}

	// Token: 0x040021D0 RID: 8656
	[MyCmpAdd]
	private Baggable baggable;

	// Token: 0x040021D1 RID: 8657
	[MyCmpAdd]
	private Prioritizable prioritizable;

	// Token: 0x040021D2 RID: 8658
	public bool allowCapture = true;

	// Token: 0x040021D3 RID: 8659
	[Serialize]
	private bool markedForCapture;

	// Token: 0x040021D4 RID: 8660
	private Chore chore;

	// Token: 0x040021D5 RID: 8661
	private static readonly EventSystem.IntraObjectHandler<Capturable> OnDeathDelegate = new EventSystem.IntraObjectHandler<Capturable>(delegate(Capturable component, object data)
	{
		component.OnDeath(data);
	});

	// Token: 0x040021D6 RID: 8662
	private static readonly EventSystem.IntraObjectHandler<Capturable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Capturable>(delegate(Capturable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x040021D7 RID: 8663
	private static readonly EventSystem.IntraObjectHandler<Capturable> OnTagsChangedDelegate = new EventSystem.IntraObjectHandler<Capturable>(delegate(Capturable component, object data)
	{
		component.OnTagsChanged(data);
	});
}
