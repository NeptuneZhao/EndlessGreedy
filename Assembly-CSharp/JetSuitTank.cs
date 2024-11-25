using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000931 RID: 2353
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/JetSuitTank")]
public class JetSuitTank : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06004457 RID: 17495 RVA: 0x001853F2 File Offset: 0x001835F2
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.amount = 25f;
		base.Subscribe<JetSuitTank>(-1617557748, JetSuitTank.OnEquippedDelegate);
		base.Subscribe<JetSuitTank>(-170173755, JetSuitTank.OnUnequippedDelegate);
	}

	// Token: 0x06004458 RID: 17496 RVA: 0x00185427 File Offset: 0x00183627
	public float PercentFull()
	{
		return this.amount / 25f;
	}

	// Token: 0x06004459 RID: 17497 RVA: 0x00185435 File Offset: 0x00183635
	public bool IsEmpty()
	{
		return this.amount <= 0f;
	}

	// Token: 0x0600445A RID: 17498 RVA: 0x00185447 File Offset: 0x00183647
	public bool IsFull()
	{
		return this.PercentFull() >= 1f;
	}

	// Token: 0x0600445B RID: 17499 RVA: 0x00185459 File Offset: 0x00183659
	public bool NeedsRecharging()
	{
		return this.PercentFull() < 0.25f;
	}

	// Token: 0x0600445C RID: 17500 RVA: 0x00185468 File Offset: 0x00183668
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		string text = string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.EFFECTS.JETSUIT_TANK, GameUtil.GetFormattedMass(this.amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
		list.Add(new Descriptor(text, text, Descriptor.DescriptorType.Effect, false));
		return list;
	}

	// Token: 0x0600445D RID: 17501 RVA: 0x001854AC File Offset: 0x001836AC
	private void OnEquipped(object data)
	{
		Equipment equipment = (Equipment)data;
		NameDisplayScreen.Instance.SetSuitFuelDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), new Func<float>(this.PercentFull), true);
		this.jetSuitMonitor = new JetSuitMonitor.Instance(this, equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject());
		this.jetSuitMonitor.StartSM();
		if (this.IsEmpty())
		{
			equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().AddTag(GameTags.JetSuitOutOfFuel);
		}
	}

	// Token: 0x0600445E RID: 17502 RVA: 0x00185524 File Offset: 0x00183724
	private void OnUnequipped(object data)
	{
		Equipment equipment = (Equipment)data;
		if (!equipment.destroyed)
		{
			equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().RemoveTag(GameTags.JetSuitOutOfFuel);
			NameDisplayScreen.Instance.SetSuitFuelDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), null, false);
			Navigator component = equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().GetComponent<Navigator>();
			if (component && component.CurrentNavType == NavType.Hover)
			{
				component.SetCurrentNavType(NavType.Floor);
			}
		}
		if (this.jetSuitMonitor != null)
		{
			this.jetSuitMonitor.StopSM("Removed jetsuit tank");
			this.jetSuitMonitor = null;
		}
	}

	// Token: 0x04002CB7 RID: 11447
	[MyCmpGet]
	private ElementEmitter elementConverter;

	// Token: 0x04002CB8 RID: 11448
	[Serialize]
	public float amount;

	// Token: 0x04002CB9 RID: 11449
	public const float FUEL_CAPACITY = 25f;

	// Token: 0x04002CBA RID: 11450
	public const float FUEL_BURN_RATE = 0.1f;

	// Token: 0x04002CBB RID: 11451
	public const float CO2_EMITTED_PER_FUEL_BURNED = 3f;

	// Token: 0x04002CBC RID: 11452
	public const float EMIT_TEMPERATURE = 473.15f;

	// Token: 0x04002CBD RID: 11453
	public const float REFILL_PERCENT = 0.25f;

	// Token: 0x04002CBE RID: 11454
	private JetSuitMonitor.Instance jetSuitMonitor;

	// Token: 0x04002CBF RID: 11455
	private static readonly EventSystem.IntraObjectHandler<JetSuitTank> OnEquippedDelegate = new EventSystem.IntraObjectHandler<JetSuitTank>(delegate(JetSuitTank component, object data)
	{
		component.OnEquipped(data);
	});

	// Token: 0x04002CC0 RID: 11456
	private static readonly EventSystem.IntraObjectHandler<JetSuitTank> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<JetSuitTank>(delegate(JetSuitTank component, object data)
	{
		component.OnUnequipped(data);
	});
}
