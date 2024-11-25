using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000B25 RID: 2853
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Sublimates")]
public class Sublimates : KMonoBehaviour, ISim200ms
{
	// Token: 0x17000662 RID: 1634
	// (get) Token: 0x06005511 RID: 21777 RVA: 0x001E6598 File Offset: 0x001E4798
	public float Temperature
	{
		get
		{
			return this.primaryElement.Temperature;
		}
	}

	// Token: 0x06005512 RID: 21778 RVA: 0x001E65A5 File Offset: 0x001E47A5
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Sublimates>(-2064133523, Sublimates.OnAbsorbDelegate);
		base.Subscribe<Sublimates>(1335436905, Sublimates.OnSplitFromChunkDelegate);
		this.simRenderLoadBalance = true;
	}

	// Token: 0x06005513 RID: 21779 RVA: 0x001E65D6 File Offset: 0x001E47D6
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.flowAccumulator = Game.Instance.accumulators.Add("EmittedMass", this);
		this.RefreshStatusItem(Sublimates.EmitState.Emitting);
	}

	// Token: 0x06005514 RID: 21780 RVA: 0x001E6600 File Offset: 0x001E4800
	protected override void OnCleanUp()
	{
		this.flowAccumulator = Game.Instance.accumulators.Remove(this.flowAccumulator);
		base.OnCleanUp();
	}

	// Token: 0x06005515 RID: 21781 RVA: 0x001E6624 File Offset: 0x001E4824
	private void OnAbsorb(object data)
	{
		Pickupable pickupable = (Pickupable)data;
		if (pickupable != null)
		{
			Sublimates component = pickupable.GetComponent<Sublimates>();
			if (component != null)
			{
				this.sublimatedMass += component.sublimatedMass;
			}
		}
	}

	// Token: 0x06005516 RID: 21782 RVA: 0x001E6664 File Offset: 0x001E4864
	private void OnSplitFromChunk(object data)
	{
		Pickupable pickupable = data as Pickupable;
		PrimaryElement primaryElement = pickupable.PrimaryElement;
		Sublimates component = pickupable.GetComponent<Sublimates>();
		if (component == null)
		{
			return;
		}
		float mass = this.primaryElement.Mass;
		float mass2 = primaryElement.Mass;
		float num = mass / (mass2 + mass);
		this.sublimatedMass = component.sublimatedMass * num;
		float num2 = 1f - num;
		component.sublimatedMass *= num2;
	}

	// Token: 0x06005517 RID: 21783 RVA: 0x001E66D0 File Offset: 0x001E48D0
	public void Sim200ms(float dt)
	{
		int num = Grid.PosToCell(base.transform.GetPosition());
		if (!Grid.IsValidCell(num))
		{
			return;
		}
		bool flag = this.HasTag(GameTags.Sealed);
		Pickupable component = base.GetComponent<Pickupable>();
		Storage storage = (component != null) ? component.storage : null;
		if (flag && !this.decayStorage)
		{
			return;
		}
		if (flag && storage != null && storage.HasTag(GameTags.CorrosionProof))
		{
			return;
		}
		Element element = ElementLoader.FindElementByHash(this.info.sublimatedElement);
		if (this.primaryElement.Temperature <= element.lowTemp)
		{
			this.RefreshStatusItem(Sublimates.EmitState.BlockedOnTemperature);
			return;
		}
		float num2 = Grid.Mass[num];
		if (num2 < this.info.maxDestinationMass)
		{
			float num3 = this.primaryElement.Mass;
			if (num3 > 0f)
			{
				float num4 = Mathf.Pow(num3, this.info.massPower);
				float num5 = Mathf.Max(this.info.sublimationRate, this.info.sublimationRate * num4);
				num5 *= dt;
				num5 = Mathf.Min(num5, num3);
				this.sublimatedMass += num5;
				num3 -= num5;
				if (this.sublimatedMass > this.info.minSublimationAmount)
				{
					float num6 = this.sublimatedMass / this.primaryElement.Mass;
					byte diseaseIdx;
					int num7;
					if (this.info.diseaseIdx == 255)
					{
						diseaseIdx = this.primaryElement.DiseaseIdx;
						num7 = (int)((float)this.primaryElement.DiseaseCount * num6);
						this.primaryElement.ModifyDiseaseCount(-num7, "Sublimates.SimUpdate");
					}
					else
					{
						float num8 = this.sublimatedMass / this.info.sublimationRate;
						diseaseIdx = this.info.diseaseIdx;
						num7 = (int)((float)this.info.diseaseCount * num8);
					}
					float num9 = Mathf.Min(this.sublimatedMass, this.info.maxDestinationMass - num2);
					if (num9 <= 0f)
					{
						this.RefreshStatusItem(Sublimates.EmitState.BlockedOnPressure);
						return;
					}
					this.Emit(num, num9, this.primaryElement.Temperature, diseaseIdx, num7);
					this.sublimatedMass = Mathf.Max(0f, this.sublimatedMass - num9);
					this.primaryElement.Mass = Mathf.Max(0f, this.primaryElement.Mass - num9);
					this.UpdateStorage();
					this.RefreshStatusItem(Sublimates.EmitState.Emitting);
					if (flag && this.decayStorage && storage != null)
					{
						storage.Trigger(-794517298, new BuildingHP.DamageSourceInfo
						{
							damage = 1,
							source = BUILDINGS.DAMAGESOURCES.CORROSIVE_ELEMENT,
							popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.CORROSIVE_ELEMENT,
							fullDamageEffectName = "smoke_damage_kanim"
						});
						return;
					}
				}
			}
			else if (this.sublimatedMass > 0f)
			{
				float num10 = Mathf.Min(this.sublimatedMass, this.info.maxDestinationMass - num2);
				if (num10 > 0f)
				{
					this.Emit(num, num10, this.primaryElement.Temperature, this.primaryElement.DiseaseIdx, this.primaryElement.DiseaseCount);
					this.sublimatedMass = Mathf.Max(0f, this.sublimatedMass - num10);
					this.primaryElement.Mass = Mathf.Max(0f, this.primaryElement.Mass - num10);
					this.UpdateStorage();
					this.RefreshStatusItem(Sublimates.EmitState.Emitting);
					return;
				}
				this.RefreshStatusItem(Sublimates.EmitState.BlockedOnPressure);
				return;
			}
			else if (!this.primaryElement.KeepZeroMassObject)
			{
				Util.KDestroyGameObject(base.gameObject);
				return;
			}
		}
		else
		{
			this.RefreshStatusItem(Sublimates.EmitState.BlockedOnPressure);
		}
	}

	// Token: 0x06005518 RID: 21784 RVA: 0x001E6A80 File Offset: 0x001E4C80
	private void UpdateStorage()
	{
		Pickupable component = base.GetComponent<Pickupable>();
		if (component != null && component.storage != null)
		{
			component.storage.Trigger(-1697596308, base.gameObject);
		}
	}

	// Token: 0x06005519 RID: 21785 RVA: 0x001E6AC4 File Offset: 0x001E4CC4
	private void Emit(int cell, float mass, float temperature, byte disease_idx, int disease_count)
	{
		SimMessages.AddRemoveSubstance(cell, this.info.sublimatedElement, CellEventLogger.Instance.SublimatesEmit, mass, temperature, disease_idx, disease_count, true, -1);
		Game.Instance.accumulators.Accumulate(this.flowAccumulator, mass);
		if (this.spawnFXHash != SpawnFXHashes.None)
		{
			base.transform.GetPosition().z = Grid.GetLayerZ(Grid.SceneLayer.Front);
			Game.Instance.SpawnFX(this.spawnFXHash, base.transform.GetPosition(), 0f);
		}
	}

	// Token: 0x0600551A RID: 21786 RVA: 0x001E6B4C File Offset: 0x001E4D4C
	public float AvgFlowRate()
	{
		return Game.Instance.accumulators.GetAverageRate(this.flowAccumulator);
	}

	// Token: 0x0600551B RID: 21787 RVA: 0x001E6B64 File Offset: 0x001E4D64
	private void RefreshStatusItem(Sublimates.EmitState newEmitState)
	{
		if (newEmitState == this.lastEmitState)
		{
			return;
		}
		switch (newEmitState)
		{
		case Sublimates.EmitState.Emitting:
			if (this.info.sublimatedElement == SimHashes.Oxygen)
			{
				this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.EmittingOxygenAvg, this);
			}
			else
			{
				this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.EmittingGasAvg, this);
			}
			break;
		case Sublimates.EmitState.BlockedOnPressure:
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.EmittingBlockedHighPressure, this);
			break;
		case Sublimates.EmitState.BlockedOnTemperature:
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.EmittingBlockedLowTemperature, this);
			break;
		}
		this.lastEmitState = newEmitState;
	}

	// Token: 0x040037B3 RID: 14259
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x040037B4 RID: 14260
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x040037B5 RID: 14261
	[SerializeField]
	public SpawnFXHashes spawnFXHash;

	// Token: 0x040037B6 RID: 14262
	public bool decayStorage;

	// Token: 0x040037B7 RID: 14263
	[SerializeField]
	public Sublimates.Info info;

	// Token: 0x040037B8 RID: 14264
	[Serialize]
	private float sublimatedMass;

	// Token: 0x040037B9 RID: 14265
	private HandleVector<int>.Handle flowAccumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x040037BA RID: 14266
	private Sublimates.EmitState lastEmitState = (Sublimates.EmitState)(-1);

	// Token: 0x040037BB RID: 14267
	private static readonly EventSystem.IntraObjectHandler<Sublimates> OnAbsorbDelegate = new EventSystem.IntraObjectHandler<Sublimates>(delegate(Sublimates component, object data)
	{
		component.OnAbsorb(data);
	});

	// Token: 0x040037BC RID: 14268
	private static readonly EventSystem.IntraObjectHandler<Sublimates> OnSplitFromChunkDelegate = new EventSystem.IntraObjectHandler<Sublimates>(delegate(Sublimates component, object data)
	{
		component.OnSplitFromChunk(data);
	});

	// Token: 0x02001B79 RID: 7033
	[Serializable]
	public struct Info
	{
		// Token: 0x0600A379 RID: 41849 RVA: 0x00389E24 File Offset: 0x00388024
		public Info(float rate, float min_amount, float max_destination_mass, float mass_power, SimHashes element, byte disease_idx = 255, int disease_count = 0)
		{
			this.sublimationRate = rate;
			this.minSublimationAmount = min_amount;
			this.maxDestinationMass = max_destination_mass;
			this.massPower = mass_power;
			this.sublimatedElement = element;
			this.diseaseIdx = disease_idx;
			this.diseaseCount = disease_count;
		}

		// Token: 0x04007FD6 RID: 32726
		public float sublimationRate;

		// Token: 0x04007FD7 RID: 32727
		public float minSublimationAmount;

		// Token: 0x04007FD8 RID: 32728
		public float maxDestinationMass;

		// Token: 0x04007FD9 RID: 32729
		public float massPower;

		// Token: 0x04007FDA RID: 32730
		public byte diseaseIdx;

		// Token: 0x04007FDB RID: 32731
		public int diseaseCount;

		// Token: 0x04007FDC RID: 32732
		[HashedEnum]
		public SimHashes sublimatedElement;
	}

	// Token: 0x02001B7A RID: 7034
	private enum EmitState
	{
		// Token: 0x04007FDE RID: 32734
		Emitting,
		// Token: 0x04007FDF RID: 32735
		BlockedOnPressure,
		// Token: 0x04007FE0 RID: 32736
		BlockedOnTemperature
	}
}
