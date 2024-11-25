using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000810 RID: 2064
[SkipSaveFileSerialization]
public class IlluminationVulnerable : StateMachineComponent<IlluminationVulnerable.StatesInstance>, IGameObjectEffectDescriptor, IWiltCause, IIlluminationTracker
{
	// Token: 0x170003FE RID: 1022
	// (get) Token: 0x06003916 RID: 14614 RVA: 0x001374DF File Offset: 0x001356DF
	public int LightIntensityThreshold
	{
		get
		{
			if (this.minLuxAttributeInstance != null)
			{
				return Mathf.RoundToInt(this.minLuxAttributeInstance.GetTotalValue());
			}
			return Mathf.RoundToInt(base.GetComponent<Modifiers>().GetPreModifiedAttributeValue(Db.Get().PlantAttributes.MinLightLux));
		}
	}

	// Token: 0x06003917 RID: 14615 RVA: 0x00137519 File Offset: 0x00135719
	public string GetIlluminationUITooltip()
	{
		if ((this.prefersDarkness && this.IsComfortable()) || (!this.prefersDarkness && !this.IsComfortable()))
		{
			return UI.TOOLTIPS.VITALS_CHECKBOX_ILLUMINATION_DARK;
		}
		return UI.TOOLTIPS.VITALS_CHECKBOX_ILLUMINATION_LIGHT;
	}

	// Token: 0x06003918 RID: 14616 RVA: 0x00137550 File Offset: 0x00135750
	public string GetIlluminationUILabel()
	{
		return Db.Get().Amounts.Illumination.Name + "\n    • " + (this.prefersDarkness ? UI.GAMEOBJECTEFFECTS.DARKNESS.ToString() : GameUtil.GetFormattedLux(this.LightIntensityThreshold));
	}

	// Token: 0x06003919 RID: 14617 RVA: 0x0013758F File Offset: 0x0013578F
	public bool ShouldIlluminationUICheckboxBeChecked()
	{
		return this.IsComfortable();
	}

	// Token: 0x170003FF RID: 1023
	// (get) Token: 0x0600391A RID: 14618 RVA: 0x00137597 File Offset: 0x00135797
	private OccupyArea occupyArea
	{
		get
		{
			if (this._occupyArea == null)
			{
				this._occupyArea = base.GetComponent<OccupyArea>();
			}
			return this._occupyArea;
		}
	}

	// Token: 0x0600391B RID: 14619 RVA: 0x001375BC File Offset: 0x001357BC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.GetAmounts().Add(new AmountInstance(Db.Get().Amounts.Illumination, base.gameObject));
		this.minLuxAttributeInstance = base.gameObject.GetAttributes().Add(Db.Get().PlantAttributes.MinLightLux);
	}

	// Token: 0x0600391C RID: 14620 RVA: 0x0013761F File Offset: 0x0013581F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x0600391D RID: 14621 RVA: 0x00137632 File Offset: 0x00135832
	public void SetPrefersDarkness(bool prefersDarkness = false)
	{
		this.prefersDarkness = prefersDarkness;
	}

	// Token: 0x0600391E RID: 14622 RVA: 0x0013763B File Offset: 0x0013583B
	protected override void OnCleanUp()
	{
		this.handle.ClearScheduler();
		base.OnCleanUp();
	}

	// Token: 0x0600391F RID: 14623 RVA: 0x0013764E File Offset: 0x0013584E
	public bool IsCellSafe(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		if (this.prefersDarkness)
		{
			return Grid.LightIntensity[cell] == 0;
		}
		return Grid.LightIntensity[cell] >= this.LightIntensityThreshold;
	}

	// Token: 0x17000400 RID: 1024
	// (get) Token: 0x06003920 RID: 14624 RVA: 0x00137687 File Offset: 0x00135887
	WiltCondition.Condition[] IWiltCause.Conditions
	{
		get
		{
			return new WiltCondition.Condition[]
			{
				WiltCondition.Condition.Darkness,
				WiltCondition.Condition.IlluminationComfort
			};
		}
	}

	// Token: 0x17000401 RID: 1025
	// (get) Token: 0x06003921 RID: 14625 RVA: 0x00137698 File Offset: 0x00135898
	public string WiltStateString
	{
		get
		{
			if (base.smi.IsInsideState(base.smi.sm.too_bright))
			{
				return Db.Get().CreatureStatusItems.Crop_Too_Bright.GetName(this);
			}
			if (base.smi.IsInsideState(base.smi.sm.too_dark))
			{
				return Db.Get().CreatureStatusItems.Crop_Too_Dark.GetName(this);
			}
			return "";
		}
	}

	// Token: 0x06003922 RID: 14626 RVA: 0x00137710 File Offset: 0x00135910
	public bool IsComfortable()
	{
		return base.smi.IsInsideState(base.smi.sm.comfortable);
	}

	// Token: 0x06003923 RID: 14627 RVA: 0x00137730 File Offset: 0x00135930
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		if (this.prefersDarkness)
		{
			return new List<Descriptor>
			{
				new Descriptor(UI.GAMEOBJECTEFFECTS.REQUIRES_DARKNESS, UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_DARKNESS, Descriptor.DescriptorType.Requirement, false)
			};
		}
		return new List<Descriptor>
		{
			new Descriptor(UI.GAMEOBJECTEFFECTS.REQUIRES_LIGHT.Replace("{Lux}", GameUtil.GetFormattedLux(this.LightIntensityThreshold)), UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_LIGHT.Replace("{Lux}", GameUtil.GetFormattedLux(this.LightIntensityThreshold)), Descriptor.DescriptorType.Requirement, false)
		};
	}

	// Token: 0x04002259 RID: 8793
	private OccupyArea _occupyArea;

	// Token: 0x0400225A RID: 8794
	private SchedulerHandle handle;

	// Token: 0x0400225B RID: 8795
	public bool prefersDarkness;

	// Token: 0x0400225C RID: 8796
	private AttributeInstance minLuxAttributeInstance;

	// Token: 0x02001710 RID: 5904
	public class StatesInstance : GameStateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable, object>.GameInstance
	{
		// Token: 0x0600947C RID: 38012 RVA: 0x0035C93B File Offset: 0x0035AB3B
		public StatesInstance(IlluminationVulnerable master) : base(master)
		{
		}

		// Token: 0x04007198 RID: 29080
		public bool hasMaturity;
	}

	// Token: 0x02001711 RID: 5905
	public class States : GameStateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable>
	{
		// Token: 0x0600947D RID: 38013 RVA: 0x0035C944 File Offset: 0x0035AB44
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.comfortable;
			this.root.Update("Illumination", delegate(IlluminationVulnerable.StatesInstance smi, float dt)
			{
				int num = Grid.PosToCell(smi.master.gameObject);
				if (Grid.IsValidCell(num))
				{
					smi.master.GetAmounts().Get(Db.Get().Amounts.Illumination).SetValue((float)Grid.LightCount[num]);
					return;
				}
				smi.master.GetAmounts().Get(Db.Get().Amounts.Illumination).SetValue(0f);
			}, UpdateRate.SIM_1000ms, false);
			this.comfortable.Update("Illumination.Comfortable", delegate(IlluminationVulnerable.StatesInstance smi, float dt)
			{
				int cell = Grid.PosToCell(smi.master.gameObject);
				if (!smi.master.IsCellSafe(cell))
				{
					GameStateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable, object>.State state = smi.master.prefersDarkness ? this.too_bright : this.too_dark;
					smi.GoTo(state);
				}
			}, UpdateRate.SIM_1000ms, false).Enter(delegate(IlluminationVulnerable.StatesInstance smi)
			{
				smi.master.Trigger(1113102781, null);
			});
			this.too_dark.TriggerOnEnter(GameHashes.IlluminationDiscomfort, null).Update("Illumination.too_dark", delegate(IlluminationVulnerable.StatesInstance smi, float dt)
			{
				int cell = Grid.PosToCell(smi.master.gameObject);
				if (smi.master.IsCellSafe(cell))
				{
					smi.GoTo(this.comfortable);
				}
			}, UpdateRate.SIM_1000ms, false);
			this.too_bright.TriggerOnEnter(GameHashes.IlluminationDiscomfort, null).Update("Illumination.too_bright", delegate(IlluminationVulnerable.StatesInstance smi, float dt)
			{
				int cell = Grid.PosToCell(smi.master.gameObject);
				if (smi.master.IsCellSafe(cell))
				{
					smi.GoTo(this.comfortable);
				}
			}, UpdateRate.SIM_1000ms, false);
		}

		// Token: 0x04007199 RID: 29081
		public StateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable, object>.BoolParameter illuminated;

		// Token: 0x0400719A RID: 29082
		public GameStateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable, object>.State comfortable;

		// Token: 0x0400719B RID: 29083
		public GameStateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable, object>.State too_dark;

		// Token: 0x0400719C RID: 29084
		public GameStateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable, object>.State too_bright;
	}
}
