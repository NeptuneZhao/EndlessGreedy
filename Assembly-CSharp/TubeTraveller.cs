using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020009AC RID: 2476
public class TubeTraveller : GameStateMachine<TubeTraveller, TubeTraveller.Instance>
{
	// Token: 0x06004800 RID: 18432 RVA: 0x0019C74C File Offset: 0x0019A94C
	public void InitModifiers()
	{
		this.modifiers.Add(new AttributeModifier(Db.Get().Attributes.Insulation.Id, (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_INSULATION, STRINGS.BUILDINGS.PREFABS.TRAVELTUBE.NAME, false, false, true));
		this.modifiers.Add(new AttributeModifier(Db.Get().Attributes.ThermalConductivityBarrier.Id, TUNING.EQUIPMENT.SUITS.ATMOSUIT_THERMAL_CONDUCTIVITY_BARRIER, STRINGS.BUILDINGS.PREFABS.TRAVELTUBE.NAME, false, false, true));
		this.modifiers.Add(new AttributeModifier(Db.Get().Amounts.Bladder.deltaAttribute.Id, TUNING.EQUIPMENT.SUITS.ATMOSUIT_BLADDER, STRINGS.BUILDINGS.PREFABS.TRAVELTUBE.NAME, false, false, true));
		this.modifiers.Add(new AttributeModifier(Db.Get().Attributes.ScaldingThreshold.Id, (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_SCALDING, STRINGS.BUILDINGS.PREFABS.TRAVELTUBE.NAME, false, false, true));
		this.modifiers.Add(new AttributeModifier(Db.Get().Attributes.ScoldingThreshold.Id, (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_SCOLDING, STRINGS.BUILDINGS.PREFABS.TRAVELTUBE.NAME, false, false, true));
		this.waxSpeedBoostModifier = new AttributeModifier(Db.Get().Attributes.TransitTubeTravelSpeed.Id, DUPLICANTSTATS.STANDARD.BaseStats.TRANSIT_TUBE_TRAVEL_SPEED * 0.25f, STRINGS.BUILDINGS.PREFABS.TRAVELTUBE.NAME, false, false, true);
		this.immunities.Add(Db.Get().effects.Get("SoakingWet"));
		this.immunities.Add(Db.Get().effects.Get("WetFeet"));
		this.immunities.Add(Db.Get().effects.Get("PoppedEarDrums"));
		this.immunities.Add(Db.Get().effects.Get("MinorIrritation"));
		this.immunities.Add(Db.Get().effects.Get("MajorIrritation"));
	}

	// Token: 0x06004801 RID: 18433 RVA: 0x0019C94B File Offset: 0x0019AB4B
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		this.InitModifiers();
		default_state = this.root;
		this.root.DoNothing();
	}

	// Token: 0x06004802 RID: 18434 RVA: 0x0019C967 File Offset: 0x0019AB67
	public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
	{
	}

	// Token: 0x06004803 RID: 18435 RVA: 0x0019C969 File Offset: 0x0019AB69
	public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
	{
	}

	// Token: 0x06004804 RID: 18436 RVA: 0x0019C96B File Offset: 0x0019AB6B
	public bool ConsumeGas(OxygenBreather oxygen_breather, float amount)
	{
		return false;
	}

	// Token: 0x06004805 RID: 18437 RVA: 0x0019C96E File Offset: 0x0019AB6E
	public bool ShouldEmitCO2()
	{
		return false;
	}

	// Token: 0x06004806 RID: 18438 RVA: 0x0019C971 File Offset: 0x0019AB71
	public bool ShouldStoreCO2()
	{
		return false;
	}

	// Token: 0x04002F23 RID: 12067
	private List<Effect> immunities = new List<Effect>();

	// Token: 0x04002F24 RID: 12068
	private List<AttributeModifier> modifiers = new List<AttributeModifier>();

	// Token: 0x04002F25 RID: 12069
	private AttributeModifier waxSpeedBoostModifier;

	// Token: 0x04002F26 RID: 12070
	private const float WaxSpeedBoost = 0.25f;

	// Token: 0x020019A4 RID: 6564
	public new class Instance : GameStateMachine<TubeTraveller, TubeTraveller.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x06009D80 RID: 40320 RVA: 0x003752EA File Offset: 0x003734EA
		public int prefabInstanceID
		{
			get
			{
				return base.GetComponent<Navigator>().gameObject.GetComponent<KPrefabID>().InstanceID;
			}
		}

		// Token: 0x06009D81 RID: 40321 RVA: 0x00375301 File Offset: 0x00373501
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06009D82 RID: 40322 RVA: 0x00375315 File Offset: 0x00373515
		public void OnPathAdvanced(object data)
		{
			this.UnreserveEntrances();
			this.ReserveEntrances();
		}

		// Token: 0x06009D83 RID: 40323 RVA: 0x00375324 File Offset: 0x00373524
		public void ReserveEntrances()
		{
			PathFinder.Path path = base.GetComponent<Navigator>().path;
			if (path.nodes == null)
			{
				return;
			}
			for (int i = 0; i < path.nodes.Count - 1; i++)
			{
				if (path.nodes[i].navType == NavType.Floor && path.nodes[i + 1].navType == NavType.Tube)
				{
					int cell = path.nodes[i].cell;
					if (Grid.HasUsableTubeEntrance(cell, this.prefabInstanceID))
					{
						GameObject gameObject = Grid.Objects[cell, 1];
						if (gameObject)
						{
							TravelTubeEntrance component = gameObject.GetComponent<TravelTubeEntrance>();
							if (component)
							{
								component.Reserve(this, this.prefabInstanceID);
								this.reservations.Add(component);
							}
						}
					}
				}
			}
		}

		// Token: 0x06009D84 RID: 40324 RVA: 0x003753F0 File Offset: 0x003735F0
		public void UnreserveEntrances()
		{
			foreach (TravelTubeEntrance travelTubeEntrance in this.reservations)
			{
				if (!(travelTubeEntrance == null))
				{
					travelTubeEntrance.Unreserve(this, this.prefabInstanceID);
				}
			}
			this.reservations.Clear();
		}

		// Token: 0x06009D85 RID: 40325 RVA: 0x00375460 File Offset: 0x00373660
		public void ApplyEnteringTubeEffects()
		{
			Effects component = base.GetComponent<Effects>();
			Attributes attributes = base.gameObject.GetAttributes();
			base.gameObject.AddTag(GameTags.InTransitTube);
			string name = GameTags.InTransitTube.Name;
			foreach (Effect effect in base.sm.immunities)
			{
				component.AddImmunity(effect, name, true);
			}
			foreach (AttributeModifier modifier in base.sm.modifiers)
			{
				attributes.Add(modifier);
			}
			if (this.isWaxed)
			{
				attributes.Add(base.sm.waxSpeedBoostModifier);
			}
			CreatureSimTemperatureTransfer component2 = base.gameObject.GetComponent<CreatureSimTemperatureTransfer>();
			if (component2 != null)
			{
				component2.RefreshRegistration();
			}
		}

		// Token: 0x06009D86 RID: 40326 RVA: 0x00375570 File Offset: 0x00373770
		public void ClearAllEffects()
		{
			Effects component = base.GetComponent<Effects>();
			Attributes attributes = base.gameObject.GetAttributes();
			base.gameObject.RemoveTag(GameTags.InTransitTube);
			string name = GameTags.InTransitTube.Name;
			foreach (Effect effect in base.sm.immunities)
			{
				component.RemoveImmunity(effect, name);
			}
			foreach (AttributeModifier modifier in base.sm.modifiers)
			{
				attributes.Remove(modifier);
			}
			this.SetWaxState(false);
			attributes.Remove(base.sm.waxSpeedBoostModifier);
			CreatureSimTemperatureTransfer component2 = base.gameObject.GetComponent<CreatureSimTemperatureTransfer>();
			if (component2 != null)
			{
				component2.RefreshRegistration();
			}
		}

		// Token: 0x06009D87 RID: 40327 RVA: 0x0037567C File Offset: 0x0037387C
		public void SetWaxState(bool isWaxed)
		{
			this.isWaxed = isWaxed;
			KSelectable component = base.GetComponent<KSelectable>();
			if (component != null)
			{
				if (isWaxed)
				{
					component.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.WaxedForTransitTube, 0.25f);
					return;
				}
				component.RemoveStatusItem(Db.Get().DuplicantStatusItems.WaxedForTransitTube, false);
			}
		}

		// Token: 0x06009D88 RID: 40328 RVA: 0x003756EA File Offset: 0x003738EA
		public void OnTubeTransition(bool nowInTube)
		{
			if (nowInTube != this.inTube)
			{
				this.inTube = nowInTube;
				base.GetComponent<Effects>();
				base.gameObject.GetAttributes();
				if (nowInTube)
				{
					this.ApplyEnteringTubeEffects();
					return;
				}
				this.ClearAllEffects();
			}
		}

		// Token: 0x04007A2B RID: 31275
		private List<TravelTubeEntrance> reservations = new List<TravelTubeEntrance>();

		// Token: 0x04007A2C RID: 31276
		public bool inTube;

		// Token: 0x04007A2D RID: 31277
		public bool isWaxed;
	}
}
