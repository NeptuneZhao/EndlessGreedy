using System;
using System.Collections.Generic;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000B2B RID: 2859
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/SuitTank")]
public class SuitTank : KMonoBehaviour, IGameObjectEffectDescriptor, OxygenBreather.IGasProvider
{
	// Token: 0x06005543 RID: 21827 RVA: 0x001E7560 File Offset: 0x001E5760
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<SuitTank>(-1617557748, SuitTank.OnEquippedDelegate);
		base.Subscribe<SuitTank>(-170173755, SuitTank.OnUnequippedDelegate);
	}

	// Token: 0x06005544 RID: 21828 RVA: 0x001E758C File Offset: 0x001E578C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.amount != 0f)
		{
			this.storage.AddGasChunk(SimHashes.Oxygen, this.amount, base.GetComponent<PrimaryElement>().Temperature, byte.MaxValue, 0, false, true);
			this.amount = 0f;
		}
	}

	// Token: 0x06005545 RID: 21829 RVA: 0x001E75E1 File Offset: 0x001E57E1
	public float GetTankAmount()
	{
		if (this.storage == null)
		{
			this.storage = base.GetComponent<Storage>();
		}
		return this.storage.GetMassAvailable(this.elementTag);
	}

	// Token: 0x06005546 RID: 21830 RVA: 0x001E760E File Offset: 0x001E580E
	public float PercentFull()
	{
		return this.GetTankAmount() / this.capacity;
	}

	// Token: 0x06005547 RID: 21831 RVA: 0x001E761D File Offset: 0x001E581D
	public bool IsEmpty()
	{
		return this.GetTankAmount() <= 0f;
	}

	// Token: 0x06005548 RID: 21832 RVA: 0x001E762F File Offset: 0x001E582F
	public bool IsFull()
	{
		return this.PercentFull() >= 1f;
	}

	// Token: 0x06005549 RID: 21833 RVA: 0x001E7641 File Offset: 0x001E5841
	public bool NeedsRecharging()
	{
		return this.PercentFull() < 0.25f;
	}

	// Token: 0x0600554A RID: 21834 RVA: 0x001E7650 File Offset: 0x001E5850
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.elementTag == GameTags.Breathable)
		{
			string text = this.underwaterSupport ? string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.EFFECTS.OXYGEN_TANK_UNDERWATER, GameUtil.GetFormattedMass(this.GetTankAmount(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")) : string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.EFFECTS.OXYGEN_TANK, GameUtil.GetFormattedMass(this.GetTankAmount(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			list.Add(new Descriptor(text, text, Descriptor.DescriptorType.Effect, false));
		}
		return list;
	}

	// Token: 0x0600554B RID: 21835 RVA: 0x001E76D4 File Offset: 0x001E58D4
	private void OnEquipped(object data)
	{
		Equipment equipment = (Equipment)data;
		NameDisplayScreen.Instance.SetSuitTankDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), new Func<float>(this.PercentFull), true);
		GameObject targetGameObject = equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
		OxygenBreather component = targetGameObject.GetComponent<OxygenBreather>();
		if (component != null)
		{
			component.SetGasProvider(this);
		}
		targetGameObject.AddTag(GameTags.HasSuitTank);
	}

	// Token: 0x0600554C RID: 21836 RVA: 0x001E7738 File Offset: 0x001E5938
	private void OnUnequipped(object data)
	{
		Equipment equipment = (Equipment)data;
		if (!equipment.destroyed)
		{
			NameDisplayScreen.Instance.SetSuitTankDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), new Func<float>(this.PercentFull), false);
			GameObject targetGameObject = equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
			OxygenBreather component = targetGameObject.GetComponent<OxygenBreather>();
			if (component != null)
			{
				component.SetGasProvider(new GasBreatherFromWorldProvider());
			}
			targetGameObject.RemoveTag(GameTags.HasSuitTank);
		}
	}

	// Token: 0x0600554D RID: 21837 RVA: 0x001E77A6 File Offset: 0x001E59A6
	public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
	{
		this.suitSuffocationMonitor = new SuitSuffocationMonitor.Instance(oxygen_breather, this);
		this.suitSuffocationMonitor.StartSM();
	}

	// Token: 0x0600554E RID: 21838 RVA: 0x001E77C0 File Offset: 0x001E59C0
	public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
	{
		this.suitSuffocationMonitor.StopSM("Removed suit tank");
		this.suitSuffocationMonitor = null;
	}

	// Token: 0x0600554F RID: 21839 RVA: 0x001E77DC File Offset: 0x001E59DC
	public bool ConsumeGas(OxygenBreather oxygen_breather, float gas_consumed)
	{
		if (this.IsEmpty())
		{
			return false;
		}
		float num;
		SimUtil.DiseaseInfo diseaseInfo;
		float num2;
		this.storage.ConsumeAndGetDisease(this.elementTag, gas_consumed, out num, out diseaseInfo, out num2);
		Game.Instance.accumulators.Accumulate(oxygen_breather.O2Accumulator, num);
		ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, -num, oxygen_breather.GetProperName(), null);
		base.Trigger(608245985, base.gameObject);
		return true;
	}

	// Token: 0x06005550 RID: 21840 RVA: 0x001E7848 File Offset: 0x001E5A48
	public bool ShouldEmitCO2()
	{
		return !base.GetComponent<KPrefabID>().HasTag(GameTags.AirtightSuit);
	}

	// Token: 0x06005551 RID: 21841 RVA: 0x001E785D File Offset: 0x001E5A5D
	public bool ShouldStoreCO2()
	{
		return base.GetComponent<KPrefabID>().HasTag(GameTags.AirtightSuit);
	}

	// Token: 0x06005552 RID: 21842 RVA: 0x001E786F File Offset: 0x001E5A6F
	public bool IsLowOxygen()
	{
		return this.IsEmpty();
	}

	// Token: 0x06005553 RID: 21843 RVA: 0x001E7878 File Offset: 0x001E5A78
	[ContextMenu("SetToRefillAmount")]
	public void SetToRefillAmount()
	{
		float tankAmount = this.GetTankAmount();
		float num = 0.25f * this.capacity;
		if (tankAmount > num)
		{
			this.storage.ConsumeIgnoringDisease(this.elementTag, tankAmount - num);
		}
	}

	// Token: 0x06005554 RID: 21844 RVA: 0x001E78B1 File Offset: 0x001E5AB1
	[ContextMenu("Empty")]
	public void Empty()
	{
		this.storage.ConsumeIgnoringDisease(this.elementTag, this.GetTankAmount());
	}

	// Token: 0x06005555 RID: 21845 RVA: 0x001E78CA File Offset: 0x001E5ACA
	[ContextMenu("Fill Tank")]
	public void FillTank()
	{
		this.Empty();
		this.storage.AddGasChunk(SimHashes.Oxygen, this.capacity, 15f, 0, 0, false, false);
	}

	// Token: 0x040037D4 RID: 14292
	[Serialize]
	public string element;

	// Token: 0x040037D5 RID: 14293
	[Serialize]
	public float amount;

	// Token: 0x040037D6 RID: 14294
	public Tag elementTag;

	// Token: 0x040037D7 RID: 14295
	[MyCmpReq]
	public Storage storage;

	// Token: 0x040037D8 RID: 14296
	public float capacity;

	// Token: 0x040037D9 RID: 14297
	public const float REFILL_PERCENT = 0.25f;

	// Token: 0x040037DA RID: 14298
	public bool underwaterSupport;

	// Token: 0x040037DB RID: 14299
	private SuitSuffocationMonitor.Instance suitSuffocationMonitor;

	// Token: 0x040037DC RID: 14300
	private static readonly EventSystem.IntraObjectHandler<SuitTank> OnEquippedDelegate = new EventSystem.IntraObjectHandler<SuitTank>(delegate(SuitTank component, object data)
	{
		component.OnEquipped(data);
	});

	// Token: 0x040037DD RID: 14301
	private static readonly EventSystem.IntraObjectHandler<SuitTank> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<SuitTank>(delegate(SuitTank component, object data)
	{
		component.OnUnequipped(data);
	});
}
