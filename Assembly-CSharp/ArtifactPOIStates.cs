using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000AB3 RID: 2739
[AddComponentMenu("KMonoBehaviour/scripts/ArtifactPOIStates")]
public class ArtifactPOIStates : GameStateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>
{
	// Token: 0x060050B7 RID: 20663 RVA: 0x001CFE28 File Offset: 0x001CE028
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.root.Enter(delegate(ArtifactPOIStates.Instance smi)
		{
			if (smi.configuration == null || smi.configuration.typeId == HashedString.Invalid)
			{
				smi.configuration = smi.GetComponent<ArtifactPOIConfigurator>().MakeConfiguration();
				smi.PickNewArtifactToHarvest();
				smi.poiCharge = 1f;
			}
		});
		this.idle.ParamTransition<float>(this.poiCharge, this.recharging, (ArtifactPOIStates.Instance smi, float f) => f < 1f);
		this.recharging.ParamTransition<float>(this.poiCharge, this.idle, (ArtifactPOIStates.Instance smi, float f) => f >= 1f).EventHandler(GameHashes.NewDay, (ArtifactPOIStates.Instance smi) => GameClock.Instance, delegate(ArtifactPOIStates.Instance smi)
		{
			smi.RechargePOI(600f);
		});
	}

	// Token: 0x040035AB RID: 13739
	public GameStateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.State idle;

	// Token: 0x040035AC RID: 13740
	public GameStateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.State recharging;

	// Token: 0x040035AD RID: 13741
	public StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.FloatParameter poiCharge = new StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.FloatParameter(1f);

	// Token: 0x02001AF0 RID: 6896
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001AF1 RID: 6897
	public new class Instance : GameStateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.GameInstance, IGameObjectEffectDescriptor
	{
		// Token: 0x17000B28 RID: 2856
		// (get) Token: 0x0600A1A0 RID: 41376 RVA: 0x00383ACF File Offset: 0x00381CCF
		// (set) Token: 0x0600A1A1 RID: 41377 RVA: 0x00383AD7 File Offset: 0x00381CD7
		public float poiCharge
		{
			get
			{
				return this._poiCharge;
			}
			set
			{
				this._poiCharge = value;
				base.smi.sm.poiCharge.Set(value, base.smi, false);
			}
		}

		// Token: 0x0600A1A2 RID: 41378 RVA: 0x00383AFE File Offset: 0x00381CFE
		public Instance(IStateMachineTarget target, ArtifactPOIStates.Def def) : base(target, def)
		{
		}

		// Token: 0x0600A1A3 RID: 41379 RVA: 0x00383B08 File Offset: 0x00381D08
		public void PickNewArtifactToHarvest()
		{
			if (this.numHarvests <= 0 && !string.IsNullOrEmpty(this.configuration.GetArtifactID()))
			{
				this.artifactToHarvest = this.configuration.GetArtifactID();
				ArtifactSelector.Instance.ReserveArtifactID(this.artifactToHarvest, ArtifactType.Any);
				return;
			}
			this.artifactToHarvest = ArtifactSelector.Instance.GetUniqueArtifactID(ArtifactType.Space);
		}

		// Token: 0x0600A1A4 RID: 41380 RVA: 0x00383B64 File Offset: 0x00381D64
		public string GetArtifactToHarvest()
		{
			if (this.CanHarvestArtifact())
			{
				if (string.IsNullOrEmpty(this.artifactToHarvest))
				{
					this.PickNewArtifactToHarvest();
				}
				return this.artifactToHarvest;
			}
			return null;
		}

		// Token: 0x0600A1A5 RID: 41381 RVA: 0x00383B89 File Offset: 0x00381D89
		public void HarvestArtifact()
		{
			if (this.CanHarvestArtifact())
			{
				this.numHarvests++;
				this.poiCharge = 0f;
				this.artifactToHarvest = null;
				this.PickNewArtifactToHarvest();
			}
		}

		// Token: 0x0600A1A6 RID: 41382 RVA: 0x00383BBC File Offset: 0x00381DBC
		public void RechargePOI(float dt)
		{
			float delta = dt / this.configuration.GetRechargeTime();
			this.DeltaPOICharge(delta);
		}

		// Token: 0x0600A1A7 RID: 41383 RVA: 0x00383BDE File Offset: 0x00381DDE
		public float RechargeTimeRemaining()
		{
			return (float)Mathf.CeilToInt((this.configuration.GetRechargeTime() - this.configuration.GetRechargeTime() * this.poiCharge) / 600f) * 600f;
		}

		// Token: 0x0600A1A8 RID: 41384 RVA: 0x00383C10 File Offset: 0x00381E10
		public void DeltaPOICharge(float delta)
		{
			this.poiCharge += delta;
			this.poiCharge = Mathf.Min(1f, this.poiCharge);
		}

		// Token: 0x0600A1A9 RID: 41385 RVA: 0x00383C36 File Offset: 0x00381E36
		public bool CanHarvestArtifact()
		{
			return this.poiCharge >= 1f;
		}

		// Token: 0x0600A1AA RID: 41386 RVA: 0x00383C48 File Offset: 0x00381E48
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			return new List<Descriptor>();
		}

		// Token: 0x04007E2D RID: 32301
		[Serialize]
		public ArtifactPOIConfigurator.ArtifactPOIInstanceConfiguration configuration;

		// Token: 0x04007E2E RID: 32302
		[Serialize]
		private float _poiCharge;

		// Token: 0x04007E2F RID: 32303
		[Serialize]
		public string artifactToHarvest;

		// Token: 0x04007E30 RID: 32304
		[Serialize]
		private int numHarvests;
	}
}
