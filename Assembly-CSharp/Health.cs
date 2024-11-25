using System;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x020008E9 RID: 2281
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Health")]
public class Health : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x170004D0 RID: 1232
	// (get) Token: 0x0600417D RID: 16765 RVA: 0x00173C22 File Offset: 0x00171E22
	public AmountInstance GetAmountInstance
	{
		get
		{
			return this.amountInstance;
		}
	}

	// Token: 0x170004D1 RID: 1233
	// (get) Token: 0x0600417E RID: 16766 RVA: 0x00173C2A File Offset: 0x00171E2A
	// (set) Token: 0x0600417F RID: 16767 RVA: 0x00173C37 File Offset: 0x00171E37
	public float hitPoints
	{
		get
		{
			return this.amountInstance.value;
		}
		set
		{
			this.amountInstance.value = value;
		}
	}

	// Token: 0x170004D2 RID: 1234
	// (get) Token: 0x06004180 RID: 16768 RVA: 0x00173C45 File Offset: 0x00171E45
	public float maxHitPoints
	{
		get
		{
			return this.amountInstance.GetMax();
		}
	}

	// Token: 0x06004181 RID: 16769 RVA: 0x00173C52 File Offset: 0x00171E52
	public float percent()
	{
		return this.hitPoints / this.maxHitPoints;
	}

	// Token: 0x06004182 RID: 16770 RVA: 0x00173C64 File Offset: 0x00171E64
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.Health.Add(this);
		this.amountInstance = Db.Get().Amounts.HitPoints.Lookup(base.gameObject);
		this.amountInstance.value = this.amountInstance.GetMax();
		AmountInstance amountInstance = this.amountInstance;
		amountInstance.OnDelta = (Action<float>)Delegate.Combine(amountInstance.OnDelta, new Action<float>(this.OnHealthChanged));
	}

	// Token: 0x06004183 RID: 16771 RVA: 0x00173CE0 File Offset: 0x00171EE0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.State == Health.HealthState.Incapacitated || this.hitPoints == 0f)
		{
			if (this.CanBeIncapacitated)
			{
				this.Incapacitate(GameTags.HitPointsDepleted);
			}
			else
			{
				this.Kill();
			}
		}
		if (this.State != Health.HealthState.Incapacitated && this.State != Health.HealthState.Dead)
		{
			this.UpdateStatus();
		}
		this.effects = base.GetComponent<Effects>();
		this.UpdateHealthBar();
		this.UpdateWoundEffects();
	}

	// Token: 0x06004184 RID: 16772 RVA: 0x00173D54 File Offset: 0x00171F54
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.Health.Remove(this);
	}

	// Token: 0x06004185 RID: 16773 RVA: 0x00173D68 File Offset: 0x00171F68
	public void UpdateHealthBar()
	{
		if (NameDisplayScreen.Instance == null)
		{
			return;
		}
		bool flag = this.State == Health.HealthState.Dead || this.State == Health.HealthState.Incapacitated || this.hitPoints >= this.maxHitPoints || base.gameObject.HasTag("HideHealthBar");
		NameDisplayScreen.Instance.SetHealthDisplay(base.gameObject, new Func<float>(this.percent), !flag);
	}

	// Token: 0x06004186 RID: 16774 RVA: 0x00173DDC File Offset: 0x00171FDC
	private void Recover()
	{
		base.GetComponent<KPrefabID>().RemoveTag(GameTags.HitPointsDepleted);
	}

	// Token: 0x06004187 RID: 16775 RVA: 0x00173DF0 File Offset: 0x00171FF0
	public void OnHealthChanged(float delta)
	{
		base.Trigger(-1664904872, delta);
		if (this.State != Health.HealthState.Invincible)
		{
			if (this.hitPoints == 0f && !this.IsDefeated())
			{
				if (this.CanBeIncapacitated)
				{
					this.Incapacitate(GameTags.HitPointsDepleted);
				}
				else
				{
					this.Kill();
				}
			}
			else
			{
				base.GetComponent<KPrefabID>().RemoveTag(GameTags.HitPointsDepleted);
			}
		}
		this.UpdateStatus();
		this.UpdateWoundEffects();
		this.UpdateHealthBar();
	}

	// Token: 0x06004188 RID: 16776 RVA: 0x00173E6B File Offset: 0x0017206B
	[ContextMenu("DoDamage")]
	public void DoDamage()
	{
		this.Damage(1f);
	}

	// Token: 0x06004189 RID: 16777 RVA: 0x00173E78 File Offset: 0x00172078
	public void Damage(float amount)
	{
		if (this.State != Health.HealthState.Invincible)
		{
			this.hitPoints = Mathf.Max(0f, this.hitPoints - amount);
		}
		this.OnHealthChanged(-amount);
	}

	// Token: 0x0600418A RID: 16778 RVA: 0x00173EA4 File Offset: 0x001720A4
	private void UpdateWoundEffects()
	{
		if (!this.effects)
		{
			return;
		}
		if (this.isCritter != this.isCritterPrev)
		{
			if (this.isCritterPrev)
			{
				this.effects.Remove("LightWoundsCritter");
				this.effects.Remove("ModerateWoundsCritter");
				this.effects.Remove("SevereWoundsCritter");
			}
			else
			{
				this.effects.Remove("LightWounds");
				this.effects.Remove("ModerateWounds");
				this.effects.Remove("SevereWounds");
			}
			this.isCritterPrev = this.isCritter;
		}
		string effect_id;
		string effect_id2;
		string effect_id3;
		if (this.isCritter)
		{
			effect_id = "LightWoundsCritter";
			effect_id2 = "ModerateWoundsCritter";
			effect_id3 = "SevereWoundsCritter";
		}
		else
		{
			effect_id = "LightWounds";
			effect_id2 = "ModerateWounds";
			effect_id3 = "SevereWounds";
		}
		switch (this.State)
		{
		case Health.HealthState.Perfect:
		case Health.HealthState.Alright:
		case Health.HealthState.Incapacitated:
		case Health.HealthState.Dead:
			this.effects.Remove(effect_id);
			this.effects.Remove(effect_id2);
			this.effects.Remove(effect_id3);
			break;
		case Health.HealthState.Scuffed:
			if (!this.effects.HasEffect(effect_id))
			{
				this.effects.Add(effect_id, true);
			}
			this.effects.Remove(effect_id2);
			this.effects.Remove(effect_id3);
			return;
		case Health.HealthState.Injured:
			this.effects.Remove(effect_id);
			if (!this.effects.HasEffect(effect_id2))
			{
				this.effects.Add(effect_id2, true);
			}
			this.effects.Remove(effect_id3);
			return;
		case Health.HealthState.Critical:
			this.effects.Remove(effect_id);
			this.effects.Remove(effect_id2);
			if (!this.effects.HasEffect(effect_id3))
			{
				this.effects.Add(effect_id3, true);
				return;
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x0600418B RID: 16779 RVA: 0x00174060 File Offset: 0x00172260
	private void UpdateStatus()
	{
		float num = this.hitPoints / this.maxHitPoints;
		Health.HealthState healthState;
		if (this.State == Health.HealthState.Invincible)
		{
			healthState = Health.HealthState.Invincible;
		}
		else if (num >= 1f)
		{
			healthState = Health.HealthState.Perfect;
		}
		else if (num >= 0.85f)
		{
			healthState = Health.HealthState.Alright;
		}
		else if (num >= 0.66f)
		{
			healthState = Health.HealthState.Scuffed;
		}
		else if ((double)num >= 0.33)
		{
			healthState = Health.HealthState.Injured;
		}
		else if (num > 0f)
		{
			healthState = Health.HealthState.Critical;
		}
		else if (num == 0f)
		{
			healthState = Health.HealthState.Incapacitated;
		}
		else
		{
			healthState = Health.HealthState.Dead;
		}
		if (this.State != healthState)
		{
			if (this.State == Health.HealthState.Incapacitated && healthState != Health.HealthState.Dead)
			{
				this.Recover();
			}
			if (healthState == Health.HealthState.Perfect)
			{
				base.Trigger(-1491582671, this);
			}
			this.State = healthState;
			KSelectable component = base.GetComponent<KSelectable>();
			if (this.State != Health.HealthState.Dead && this.State != Health.HealthState.Perfect && this.State != Health.HealthState.Alright && !this.isCritter)
			{
				component.SetStatusItem(Db.Get().StatusItemCategories.Hitpoints, Db.Get().CreatureStatusItems.HealthStatus, this.State);
				return;
			}
			component.SetStatusItem(Db.Get().StatusItemCategories.Hitpoints, null, null);
		}
	}

	// Token: 0x0600418C RID: 16780 RVA: 0x0017417E File Offset: 0x0017237E
	public bool IsIncapacitated()
	{
		return this.State == Health.HealthState.Incapacitated;
	}

	// Token: 0x0600418D RID: 16781 RVA: 0x00174189 File Offset: 0x00172389
	public bool IsDefeated()
	{
		return this.State == Health.HealthState.Incapacitated || this.State == Health.HealthState.Dead;
	}

	// Token: 0x0600418E RID: 16782 RVA: 0x0017419F File Offset: 0x0017239F
	public void Incapacitate(Tag cause)
	{
		this.State = Health.HealthState.Incapacitated;
		base.GetComponent<KPrefabID>().AddTag(cause, false);
		this.Damage(this.hitPoints);
	}

	// Token: 0x0600418F RID: 16783 RVA: 0x001741C1 File Offset: 0x001723C1
	private void Kill()
	{
		if (base.gameObject.GetSMI<DeathMonitor.Instance>() != null)
		{
			base.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Slain);
		}
	}

	// Token: 0x04002B63 RID: 11107
	[Serialize]
	public bool CanBeIncapacitated;

	// Token: 0x04002B64 RID: 11108
	[Serialize]
	public Health.HealthState State;

	// Token: 0x04002B65 RID: 11109
	[Serialize]
	private Death source_of_death;

	// Token: 0x04002B66 RID: 11110
	public HealthBar healthBar;

	// Token: 0x04002B67 RID: 11111
	public bool isCritter;

	// Token: 0x04002B68 RID: 11112
	private bool isCritterPrev;

	// Token: 0x04002B69 RID: 11113
	private Effects effects;

	// Token: 0x04002B6A RID: 11114
	private AmountInstance amountInstance;

	// Token: 0x02001854 RID: 6228
	public enum HealthState
	{
		// Token: 0x04007592 RID: 30098
		Perfect,
		// Token: 0x04007593 RID: 30099
		Alright,
		// Token: 0x04007594 RID: 30100
		Scuffed,
		// Token: 0x04007595 RID: 30101
		Injured,
		// Token: 0x04007596 RID: 30102
		Critical,
		// Token: 0x04007597 RID: 30103
		Incapacitated,
		// Token: 0x04007598 RID: 30104
		Dead,
		// Token: 0x04007599 RID: 30105
		Invincible
	}
}
