using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000AC8 RID: 2760
[AddComponentMenu("KMonoBehaviour/scripts/HarvestablePOIStates")]
public class HarvestablePOIStates : GameStateMachine<HarvestablePOIStates, HarvestablePOIStates.Instance, IStateMachineTarget, HarvestablePOIStates.Def>
{
	// Token: 0x06005202 RID: 20994 RVA: 0x001D6AC4 File Offset: 0x001D4CC4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.root.Enter(delegate(HarvestablePOIStates.Instance smi)
		{
			if (smi.configuration == null || smi.configuration.typeId == HashedString.Invalid)
			{
				smi.configuration = smi.GetComponent<HarvestablePOIConfigurator>().MakeConfiguration();
				smi.poiCapacity = UnityEngine.Random.Range(0f, smi.configuration.GetMaxCapacity());
			}
		});
		this.idle.ParamTransition<float>(this.poiCapacity, this.recharging, (HarvestablePOIStates.Instance smi, float f) => f < smi.configuration.GetMaxCapacity());
		this.recharging.EventHandler(GameHashes.NewDay, (HarvestablePOIStates.Instance smi) => GameClock.Instance, delegate(HarvestablePOIStates.Instance smi)
		{
			smi.RechargePOI(600f);
		}).ParamTransition<float>(this.poiCapacity, this.idle, (HarvestablePOIStates.Instance smi, float f) => f >= smi.configuration.GetMaxCapacity());
	}

	// Token: 0x04003623 RID: 13859
	public GameStateMachine<HarvestablePOIStates, HarvestablePOIStates.Instance, IStateMachineTarget, HarvestablePOIStates.Def>.State idle;

	// Token: 0x04003624 RID: 13860
	public GameStateMachine<HarvestablePOIStates, HarvestablePOIStates.Instance, IStateMachineTarget, HarvestablePOIStates.Def>.State recharging;

	// Token: 0x04003625 RID: 13861
	public StateMachine<HarvestablePOIStates, HarvestablePOIStates.Instance, IStateMachineTarget, HarvestablePOIStates.Def>.FloatParameter poiCapacity = new StateMachine<HarvestablePOIStates, HarvestablePOIStates.Instance, IStateMachineTarget, HarvestablePOIStates.Def>.FloatParameter(1f);

	// Token: 0x02001B0E RID: 6926
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001B0F RID: 6927
	public new class Instance : GameStateMachine<HarvestablePOIStates, HarvestablePOIStates.Instance, IStateMachineTarget, HarvestablePOIStates.Def>.GameInstance, IGameObjectEffectDescriptor
	{
		// Token: 0x17000B31 RID: 2865
		// (get) Token: 0x0600A224 RID: 41508 RVA: 0x00384D3E File Offset: 0x00382F3E
		// (set) Token: 0x0600A225 RID: 41509 RVA: 0x00384D46 File Offset: 0x00382F46
		public float poiCapacity
		{
			get
			{
				return this._poiCapacity;
			}
			set
			{
				this._poiCapacity = value;
				base.smi.sm.poiCapacity.Set(value, base.smi, false);
			}
		}

		// Token: 0x0600A226 RID: 41510 RVA: 0x00384D6D File Offset: 0x00382F6D
		public Instance(IStateMachineTarget target, HarvestablePOIStates.Def def) : base(target, def)
		{
		}

		// Token: 0x0600A227 RID: 41511 RVA: 0x00384D78 File Offset: 0x00382F78
		public void RechargePOI(float dt)
		{
			float num = dt / this.configuration.GetRechargeTime();
			float delta = this.configuration.GetMaxCapacity() * num;
			this.DeltaPOICapacity(delta);
		}

		// Token: 0x0600A228 RID: 41512 RVA: 0x00384DA8 File Offset: 0x00382FA8
		public void DeltaPOICapacity(float delta)
		{
			this.poiCapacity += delta;
			this.poiCapacity = Mathf.Min(this.configuration.GetMaxCapacity(), this.poiCapacity);
		}

		// Token: 0x0600A229 RID: 41513 RVA: 0x00384DD4 File Offset: 0x00382FD4
		public bool POICanBeHarvested()
		{
			return this.poiCapacity > 0f;
		}

		// Token: 0x0600A22A RID: 41514 RVA: 0x00384DE4 File Offset: 0x00382FE4
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> list = new List<Descriptor>();
			foreach (KeyValuePair<SimHashes, float> keyValuePair in this.configuration.GetElementsWithWeights())
			{
				SimHashes key = keyValuePair.Key;
				string arg = ElementLoader.FindElementByHash(key).tag.ProperName();
				list.Add(new Descriptor(string.Format(UI.SPACEDESTINATIONS.HARVESTABLE_POI.POI_PRODUCTION, arg), string.Format(UI.SPACEDESTINATIONS.HARVESTABLE_POI.POI_PRODUCTION_TOOLTIP, key.ToString()), Descriptor.DescriptorType.Effect, false));
			}
			list.Add(new Descriptor(string.Format("{0}/{1}", GameUtil.GetFormattedMass(this.poiCapacity, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedMass(this.configuration.GetMaxCapacity(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), "Capacity", Descriptor.DescriptorType.Effect, false));
			return list;
		}

		// Token: 0x04007EA0 RID: 32416
		[Serialize]
		public HarvestablePOIConfigurator.HarvestablePOIInstanceConfiguration configuration;

		// Token: 0x04007EA1 RID: 32417
		[Serialize]
		private float _poiCapacity;
	}
}
