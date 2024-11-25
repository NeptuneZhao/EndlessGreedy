using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020007CC RID: 1996
[AddComponentMenu("KMonoBehaviour/scripts/FactionAlignment")]
public class FactionAlignment : KMonoBehaviour
{
	// Token: 0x170003CE RID: 974
	// (get) Token: 0x060036FD RID: 14077 RVA: 0x0012B2FD File Offset: 0x001294FD
	// (set) Token: 0x060036FE RID: 14078 RVA: 0x0012B305 File Offset: 0x00129505
	[MyCmpAdd]
	public Health health { get; private set; }

	// Token: 0x170003CF RID: 975
	// (get) Token: 0x060036FF RID: 14079 RVA: 0x0012B30E File Offset: 0x0012950E
	// (set) Token: 0x06003700 RID: 14080 RVA: 0x0012B316 File Offset: 0x00129516
	public AttackableBase attackable { get; private set; }

	// Token: 0x06003701 RID: 14081 RVA: 0x0012B320 File Offset: 0x00129520
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.health = base.GetComponent<Health>();
		this.attackable = base.GetComponent<AttackableBase>();
		Components.FactionAlignments.Add(this);
		base.Subscribe<FactionAlignment>(493375141, FactionAlignment.OnRefreshUserMenuDelegate);
		base.Subscribe<FactionAlignment>(2127324410, FactionAlignment.SetPlayerTargetedFalseDelegate);
		base.Subscribe<FactionAlignment>(1502190696, FactionAlignment.OnQueueDestroyObjectDelegate);
		if (this.alignmentActive)
		{
			FactionManager.Instance.GetFaction(this.Alignment).Members.Add(this);
		}
		GameUtil.SubscribeToTags<FactionAlignment>(this, FactionAlignment.OnDeadTagAddedDelegate, true);
		this.SetPlayerTargeted(this.targeted);
		this.UpdateStatusItem();
	}

	// Token: 0x06003702 RID: 14082 RVA: 0x0012B3CB File Offset: 0x001295CB
	protected override void OnPrefabInit()
	{
	}

	// Token: 0x06003703 RID: 14083 RVA: 0x0012B3CD File Offset: 0x001295CD
	private void OnDeath(object data)
	{
		this.SetAlignmentActive(false);
	}

	// Token: 0x06003704 RID: 14084 RVA: 0x0012B3D8 File Offset: 0x001295D8
	public void SetAlignmentActive(bool active)
	{
		this.SetPlayerTargetable(active);
		this.alignmentActive = active;
		if (active)
		{
			FactionManager.Instance.GetFaction(this.Alignment).Members.Add(this);
			return;
		}
		FactionManager.Instance.GetFaction(this.Alignment).Members.Remove(this);
	}

	// Token: 0x06003705 RID: 14085 RVA: 0x0012B42F File Offset: 0x0012962F
	public bool IsAlignmentActive()
	{
		return FactionManager.Instance.GetFaction(this.Alignment).Members.Contains(this);
	}

	// Token: 0x06003706 RID: 14086 RVA: 0x0012B44C File Offset: 0x0012964C
	public bool IsPlayerTargeted()
	{
		return this.targeted;
	}

	// Token: 0x06003707 RID: 14087 RVA: 0x0012B454 File Offset: 0x00129654
	public void SetPlayerTargetable(bool state)
	{
		this.targetable = (state && this.canBePlayerTargeted);
		if (!state)
		{
			this.SetPlayerTargeted(false);
		}
	}

	// Token: 0x06003708 RID: 14088 RVA: 0x0012B474 File Offset: 0x00129674
	public void SetPlayerTargeted(bool state)
	{
		this.targeted = (this.canBePlayerTargeted && state && this.targetable);
		if (state)
		{
			if (!Components.PlayerTargeted.Items.Contains(this))
			{
				Components.PlayerTargeted.Add(this);
			}
			this.SetPrioritizable(true);
		}
		else
		{
			Components.PlayerTargeted.Remove(this);
			this.SetPrioritizable(false);
		}
		this.UpdateStatusItem();
	}

	// Token: 0x06003709 RID: 14089 RVA: 0x0012B4DC File Offset: 0x001296DC
	private void UpdateStatusItem()
	{
		if (this.targeted)
		{
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.OrderAttack, null);
			return;
		}
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.OrderAttack, false);
	}

	// Token: 0x0600370A RID: 14090 RVA: 0x0012B52C File Offset: 0x0012972C
	private void SetPrioritizable(bool enable)
	{
		Prioritizable component = base.GetComponent<Prioritizable>();
		if (component == null || !this.updatePrioritizable)
		{
			return;
		}
		if (enable && !this.hasBeenRegisterInPriority)
		{
			Prioritizable.AddRef(base.gameObject);
			this.hasBeenRegisterInPriority = true;
			return;
		}
		if (!enable && component.IsPrioritizable() && this.hasBeenRegisterInPriority)
		{
			Prioritizable.RemoveRef(base.gameObject);
			this.hasBeenRegisterInPriority = false;
		}
	}

	// Token: 0x0600370B RID: 14091 RVA: 0x0012B595 File Offset: 0x00129795
	public void SwitchAlignment(FactionManager.FactionID newAlignment)
	{
		this.SetAlignmentActive(false);
		this.Alignment = newAlignment;
		this.SetAlignmentActive(true);
		base.Trigger(-971105736, newAlignment);
	}

	// Token: 0x0600370C RID: 14092 RVA: 0x0012B5BD File Offset: 0x001297BD
	private void OnQueueDestroyObject()
	{
		FactionManager.Instance.GetFaction(this.Alignment).Members.Remove(this);
		Components.FactionAlignments.Remove(this);
	}

	// Token: 0x0600370D RID: 14093 RVA: 0x0012B5E8 File Offset: 0x001297E8
	private void OnRefreshUserMenu(object data)
	{
		if (this.Alignment == FactionManager.FactionID.Duplicant)
		{
			return;
		}
		if (!this.canBePlayerTargeted)
		{
			return;
		}
		if (!this.IsAlignmentActive())
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (!this.targeted) ? new KIconButtonMenu.ButtonInfo("action_attack", UI.USERMENUACTIONS.ATTACK.NAME, delegate()
		{
			this.SetPlayerTargeted(true);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.ATTACK.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_attack", UI.USERMENUACTIONS.CANCELATTACK.NAME, delegate()
		{
			this.SetPlayerTargeted(false);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCELATTACK.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x0400208D RID: 8333
	[MyCmpReq]
	public KPrefabID kprefabID;

	// Token: 0x0400208E RID: 8334
	[SerializeField]
	public bool canBePlayerTargeted = true;

	// Token: 0x0400208F RID: 8335
	[SerializeField]
	public bool updatePrioritizable = true;

	// Token: 0x04002090 RID: 8336
	[Serialize]
	private bool alignmentActive = true;

	// Token: 0x04002091 RID: 8337
	public FactionManager.FactionID Alignment;

	// Token: 0x04002092 RID: 8338
	[Serialize]
	private bool targeted;

	// Token: 0x04002093 RID: 8339
	[Serialize]
	private bool targetable = true;

	// Token: 0x04002094 RID: 8340
	private bool hasBeenRegisterInPriority;

	// Token: 0x04002095 RID: 8341
	private static readonly EventSystem.IntraObjectHandler<FactionAlignment> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<FactionAlignment>(GameTags.Dead, delegate(FactionAlignment component, object data)
	{
		component.OnDeath(data);
	});

	// Token: 0x04002096 RID: 8342
	private static readonly EventSystem.IntraObjectHandler<FactionAlignment> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<FactionAlignment>(delegate(FactionAlignment component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04002097 RID: 8343
	private static readonly EventSystem.IntraObjectHandler<FactionAlignment> SetPlayerTargetedFalseDelegate = new EventSystem.IntraObjectHandler<FactionAlignment>(delegate(FactionAlignment component, object data)
	{
		component.SetPlayerTargeted(false);
	});

	// Token: 0x04002098 RID: 8344
	private static readonly EventSystem.IntraObjectHandler<FactionAlignment> OnQueueDestroyObjectDelegate = new EventSystem.IntraObjectHandler<FactionAlignment>(delegate(FactionAlignment component, object data)
	{
		component.OnQueueDestroyObject();
	});
}
