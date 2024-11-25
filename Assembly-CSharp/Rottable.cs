using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020005B2 RID: 1458
public class Rottable : GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>
{
	// Token: 0x060022C5 RID: 8901 RVA: 0x000C19E4 File Offset: 0x000BFBE4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Fresh;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.TagTransition(GameTags.Preserved, this.Preserved, false).TagTransition(GameTags.Entombed, this.Preserved, false);
		this.Fresh.ToggleStatusItem(Db.Get().CreatureStatusItems.Fresh, (Rottable.Instance smi) => smi).ParamTransition<float>(this.rotParameter, this.Stale_Pre, (Rottable.Instance smi, float p) => p <= smi.def.spoilTime - (smi.def.spoilTime - smi.def.staleTime)).Update(delegate(Rottable.Instance smi, float dt)
		{
			smi.sm.rotParameter.Set(smi.RotValue, smi, false);
		}, UpdateRate.SIM_1000ms, true).FastUpdate("Rot", Rottable.rotCB, UpdateRate.SIM_1000ms, true);
		this.Preserved.TagTransition(Rottable.PRESERVED_TAGS, this.Fresh, true).Enter("RefreshModifiers", delegate(Rottable.Instance smi)
		{
			smi.RefreshModifiers(0f);
		});
		this.Stale_Pre.Enter(delegate(Rottable.Instance smi)
		{
			smi.GoTo(this.Stale);
		});
		this.Stale.ToggleStatusItem(Db.Get().CreatureStatusItems.Stale, (Rottable.Instance smi) => smi).ParamTransition<float>(this.rotParameter, this.Fresh, (Rottable.Instance smi, float p) => p > smi.def.spoilTime - (smi.def.spoilTime - smi.def.staleTime)).ParamTransition<float>(this.rotParameter, this.Spoiled, GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.IsLTEZero).Update(delegate(Rottable.Instance smi, float dt)
		{
			smi.sm.rotParameter.Set(smi.RotValue, smi, false);
		}, UpdateRate.SIM_1000ms, false).FastUpdate("Rot", Rottable.rotCB, UpdateRate.SIM_1000ms, false);
		this.Spoiled.Enter(delegate(Rottable.Instance smi)
		{
			GameObject gameObject = Scenario.SpawnPrefab(Grid.PosToCell(smi.master.gameObject), 0, 0, "RotPile", Grid.SceneLayer.Ore);
			gameObject.gameObject.GetComponent<KSelectable>().SetName(UI.GAMEOBJECTEFFECTS.ROTTEN + " " + smi.master.gameObject.GetProperName());
			gameObject.transform.SetPosition(smi.master.transform.GetPosition());
			gameObject.GetComponent<PrimaryElement>().Mass = smi.master.GetComponent<PrimaryElement>().Mass;
			gameObject.GetComponent<PrimaryElement>().Temperature = smi.master.GetComponent<PrimaryElement>().Temperature;
			gameObject.SetActive(true);
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, ITEMS.FOOD.ROTPILE.NAME, gameObject.transform, 1.5f, false);
			Edible component = smi.GetComponent<Edible>();
			if (component != null)
			{
				if (component.worker != null)
				{
					ChoreDriver component2 = component.worker.GetComponent<ChoreDriver>();
					if (component2 != null && component2.GetCurrentChore() != null)
					{
						component2.GetCurrentChore().Fail("food rotted");
					}
				}
				ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, -component.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.ROTTED, "{0}", smi.gameObject.GetProperName()), UI.ENDOFDAYREPORT.NOTES.ROTTED_CONTEXT);
			}
			Util.KDestroyGameObject(smi.gameObject);
		});
	}

	// Token: 0x060022C6 RID: 8902 RVA: 0x000C1C08 File Offset: 0x000BFE08
	private static string OnStaleTooltip(List<Notification> notifications, object data)
	{
		string text = "\n";
		foreach (Notification notification in notifications)
		{
			if (notification.tooltipData != null)
			{
				GameObject gameObject = (GameObject)notification.tooltipData;
				if (gameObject != null)
				{
					text = text + "\n" + gameObject.GetProperName();
				}
			}
		}
		return string.Format(MISC.NOTIFICATIONS.FOODSTALE.TOOLTIP, text);
	}

	// Token: 0x060022C7 RID: 8903 RVA: 0x000C1C94 File Offset: 0x000BFE94
	public static void SetStatusItems(IRottable rottable)
	{
		Grid.PosToCell(rottable.gameObject);
		KSelectable component = rottable.gameObject.GetComponent<KSelectable>();
		Rottable.RotRefrigerationLevel rotRefrigerationLevel = Rottable.RefrigerationLevel(rottable);
		if (rotRefrigerationLevel != Rottable.RotRefrigerationLevel.Refrigerated)
		{
			if (rotRefrigerationLevel == Rottable.RotRefrigerationLevel.Frozen)
			{
				component.SetStatusItem(Db.Get().StatusItemCategories.PreservationTemperature, Db.Get().CreatureStatusItems.RefrigeratedFrozen, rottable);
			}
			else
			{
				component.SetStatusItem(Db.Get().StatusItemCategories.PreservationTemperature, Db.Get().CreatureStatusItems.Unrefrigerated, rottable);
			}
		}
		else
		{
			component.SetStatusItem(Db.Get().StatusItemCategories.PreservationTemperature, Db.Get().CreatureStatusItems.Refrigerated, rottable);
		}
		Rottable.RotAtmosphereQuality rotAtmosphereQuality = Rottable.AtmosphereQuality(rottable);
		if (rotAtmosphereQuality == Rottable.RotAtmosphereQuality.Sterilizing)
		{
			component.SetStatusItem(Db.Get().StatusItemCategories.PreservationAtmosphere, Db.Get().CreatureStatusItems.SterilizingAtmosphere, null);
			return;
		}
		if (rotAtmosphereQuality == Rottable.RotAtmosphereQuality.Contaminating)
		{
			component.SetStatusItem(Db.Get().StatusItemCategories.PreservationAtmosphere, Db.Get().CreatureStatusItems.ContaminatedAtmosphere, null);
			return;
		}
		component.SetStatusItem(Db.Get().StatusItemCategories.PreservationAtmosphere, null, null);
	}

	// Token: 0x060022C8 RID: 8904 RVA: 0x000C1DB4 File Offset: 0x000BFFB4
	public static bool IsInActiveFridge(IRottable rottable)
	{
		Pickupable component = rottable.gameObject.GetComponent<Pickupable>();
		if (component != null && component.storage != null)
		{
			Refrigerator component2 = component.storage.GetComponent<Refrigerator>();
			return component2 != null && component2.IsActive();
		}
		return false;
	}

	// Token: 0x060022C9 RID: 8905 RVA: 0x000C1E04 File Offset: 0x000C0004
	public static Rottable.RotRefrigerationLevel RefrigerationLevel(IRottable rottable)
	{
		int num = Grid.PosToCell(rottable.gameObject);
		Rottable.Instance smi = rottable.gameObject.GetSMI<Rottable.Instance>();
		PrimaryElement component = rottable.gameObject.GetComponent<PrimaryElement>();
		float num2 = component.Temperature;
		bool flag = false;
		if (!Grid.IsValidCell(num))
		{
			if (!smi.IsRottableInSpace())
			{
				return Rottable.RotRefrigerationLevel.Normal;
			}
			flag = true;
		}
		if (!flag && Grid.Element[num].id != SimHashes.Vacuum)
		{
			num2 = Mathf.Min(Grid.Temperature[num], component.Temperature);
		}
		if (num2 < rottable.PreserveTemperature)
		{
			return Rottable.RotRefrigerationLevel.Frozen;
		}
		if (num2 < rottable.RotTemperature || Rottable.IsInActiveFridge(rottable))
		{
			return Rottable.RotRefrigerationLevel.Refrigerated;
		}
		return Rottable.RotRefrigerationLevel.Normal;
	}

	// Token: 0x060022CA RID: 8906 RVA: 0x000C1EA4 File Offset: 0x000C00A4
	public static Rottable.RotAtmosphereQuality AtmosphereQuality(IRottable rottable)
	{
		int num = Grid.PosToCell(rottable.gameObject);
		int num2 = Grid.CellAbove(num);
		if (!Grid.IsValidCell(num))
		{
			if (rottable.gameObject.GetSMI<Rottable.Instance>().IsRottableInSpace())
			{
				return Rottable.RotAtmosphereQuality.Sterilizing;
			}
			return Rottable.RotAtmosphereQuality.Normal;
		}
		else
		{
			SimHashes id = Grid.Element[num].id;
			Rottable.RotAtmosphereQuality rotAtmosphereQuality = Rottable.RotAtmosphereQuality.Normal;
			Rottable.AtmosphereModifier.TryGetValue((int)id, out rotAtmosphereQuality);
			Rottable.RotAtmosphereQuality rotAtmosphereQuality2 = Rottable.RotAtmosphereQuality.Normal;
			if (Grid.IsValidCell(num2))
			{
				SimHashes id2 = Grid.Element[num2].id;
				if (!Rottable.AtmosphereModifier.TryGetValue((int)id2, out rotAtmosphereQuality2))
				{
					rotAtmosphereQuality2 = rotAtmosphereQuality;
				}
			}
			else
			{
				rotAtmosphereQuality2 = rotAtmosphereQuality;
			}
			if (rotAtmosphereQuality == rotAtmosphereQuality2)
			{
				return rotAtmosphereQuality;
			}
			if (rotAtmosphereQuality == Rottable.RotAtmosphereQuality.Contaminating || rotAtmosphereQuality2 == Rottable.RotAtmosphereQuality.Contaminating)
			{
				return Rottable.RotAtmosphereQuality.Contaminating;
			}
			if (rotAtmosphereQuality == Rottable.RotAtmosphereQuality.Normal || rotAtmosphereQuality2 == Rottable.RotAtmosphereQuality.Normal)
			{
				return Rottable.RotAtmosphereQuality.Normal;
			}
			return Rottable.RotAtmosphereQuality.Sterilizing;
		}
	}

	// Token: 0x040013A1 RID: 5025
	public StateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.FloatParameter rotParameter;

	// Token: 0x040013A2 RID: 5026
	public GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.State Preserved;

	// Token: 0x040013A3 RID: 5027
	public GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.State Fresh;

	// Token: 0x040013A4 RID: 5028
	public GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.State Stale_Pre;

	// Token: 0x040013A5 RID: 5029
	public GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.State Stale;

	// Token: 0x040013A6 RID: 5030
	public GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.State Spoiled;

	// Token: 0x040013A7 RID: 5031
	private static readonly Tag[] PRESERVED_TAGS = new Tag[]
	{
		GameTags.Preserved,
		GameTags.Dehydrated,
		GameTags.Entombed
	};

	// Token: 0x040013A8 RID: 5032
	private static readonly Rottable.RotCB rotCB = new Rottable.RotCB();

	// Token: 0x040013A9 RID: 5033
	public static Dictionary<int, Rottable.RotAtmosphereQuality> AtmosphereModifier = new Dictionary<int, Rottable.RotAtmosphereQuality>
	{
		{
			721531317,
			Rottable.RotAtmosphereQuality.Contaminating
		},
		{
			1887387588,
			Rottable.RotAtmosphereQuality.Contaminating
		},
		{
			-1528777920,
			Rottable.RotAtmosphereQuality.Normal
		},
		{
			1836671383,
			Rottable.RotAtmosphereQuality.Normal
		},
		{
			1960575215,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-899515856,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-1554872654,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-1858722091,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			758759285,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-1046145888,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-1324664829,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-1406916018,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-432557516,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-805366663,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			1966552544,
			Rottable.RotAtmosphereQuality.Sterilizing
		}
	};

	// Token: 0x020013AA RID: 5034
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006772 RID: 26482
		public float spoilTime;

		// Token: 0x04006773 RID: 26483
		public float staleTime;

		// Token: 0x04006774 RID: 26484
		public float preserveTemperature = 255.15f;

		// Token: 0x04006775 RID: 26485
		public float rotTemperature = 277.15f;
	}

	// Token: 0x020013AB RID: 5035
	private class RotCB : UpdateBucketWithUpdater<Rottable.Instance>.IUpdater
	{
		// Token: 0x060087ED RID: 34797 RVA: 0x0032CDE0 File Offset: 0x0032AFE0
		public void Update(Rottable.Instance smi, float dt)
		{
			smi.Rot(smi, dt);
		}
	}

	// Token: 0x020013AC RID: 5036
	public new class Instance : GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.GameInstance, IRottable
	{
		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x060087EF RID: 34799 RVA: 0x0032CDF2 File Offset: 0x0032AFF2
		// (set) Token: 0x060087F0 RID: 34800 RVA: 0x0032CDFF File Offset: 0x0032AFFF
		public float RotValue
		{
			get
			{
				return this.rotAmountInstance.value;
			}
			set
			{
				base.sm.rotParameter.Set(value, this, false);
				this.rotAmountInstance.SetValue(value);
			}
		}

		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x060087F1 RID: 34801 RVA: 0x0032CE22 File Offset: 0x0032B022
		public float RotConstitutionPercentage
		{
			get
			{
				return this.RotValue / base.def.spoilTime;
			}
		}

		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x060087F2 RID: 34802 RVA: 0x0032CE36 File Offset: 0x0032B036
		public float RotTemperature
		{
			get
			{
				return base.def.rotTemperature;
			}
		}

		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x060087F3 RID: 34803 RVA: 0x0032CE43 File Offset: 0x0032B043
		public float PreserveTemperature
		{
			get
			{
				return base.def.preserveTemperature;
			}
		}

		// Token: 0x060087F4 RID: 34804 RVA: 0x0032CE50 File Offset: 0x0032B050
		public Instance(IStateMachineTarget master, Rottable.Def def) : base(master, def)
		{
			this.pickupable = base.gameObject.RequireComponent<Pickupable>();
			base.master.Subscribe(-2064133523, new Action<object>(this.OnAbsorb));
			base.master.Subscribe(1335436905, new Action<object>(this.OnSplitFromChunk));
			this.primaryElement = base.gameObject.GetComponent<PrimaryElement>();
			Amounts amounts = master.gameObject.GetAmounts();
			this.rotAmountInstance = amounts.Add(new AmountInstance(Db.Get().Amounts.Rot, master.gameObject));
			this.rotAmountInstance.maxAttribute.Add(new AttributeModifier("Rot", def.spoilTime, null, false, false, true));
			this.rotAmountInstance.SetValue(def.spoilTime);
			base.sm.rotParameter.Set(this.rotAmountInstance.value, base.smi, false);
			if (Rottable.Instance.unrefrigeratedModifier == null)
			{
				Rottable.Instance.unrefrigeratedModifier = new AttributeModifier(this.rotAmountInstance.amount.Id, -0.7f, DUPLICANTS.MODIFIERS.ROTTEMPERATURE.UNREFRIGERATED, false, false, true);
				Rottable.Instance.refrigeratedModifier = new AttributeModifier(this.rotAmountInstance.amount.Id, -0.2f, DUPLICANTS.MODIFIERS.ROTTEMPERATURE.REFRIGERATED, false, false, true);
				Rottable.Instance.frozenModifier = new AttributeModifier(this.rotAmountInstance.amount.Id, --0f, DUPLICANTS.MODIFIERS.ROTTEMPERATURE.FROZEN, false, false, true);
				Rottable.Instance.contaminatedAtmosphereModifier = new AttributeModifier(this.rotAmountInstance.amount.Id, -1f, DUPLICANTS.MODIFIERS.ROTATMOSPHERE.CONTAMINATED, false, false, true);
				Rottable.Instance.normalAtmosphereModifier = new AttributeModifier(this.rotAmountInstance.amount.Id, -0.3f, DUPLICANTS.MODIFIERS.ROTATMOSPHERE.NORMAL, false, false, true);
				Rottable.Instance.sterileAtmosphereModifier = new AttributeModifier(this.rotAmountInstance.amount.Id, --0f, DUPLICANTS.MODIFIERS.ROTATMOSPHERE.STERILE, false, false, true);
			}
			this.RefreshModifiers(0f);
		}

		// Token: 0x060087F5 RID: 34805 RVA: 0x0032D06C File Offset: 0x0032B26C
		[OnDeserialized]
		private void OnDeserialized()
		{
			if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 23))
			{
				this.rotAmountInstance.SetValue(this.rotAmountInstance.value * 2f);
			}
		}

		// Token: 0x060087F6 RID: 34806 RVA: 0x0032D0B0 File Offset: 0x0032B2B0
		public string StateString()
		{
			string result = "";
			if (base.smi.GetCurrentState() == base.sm.Fresh)
			{
				result = Db.Get().CreatureStatusItems.Fresh.resolveStringCallback(CREATURES.STATUSITEMS.FRESH.NAME, this);
			}
			if (base.smi.GetCurrentState() == base.sm.Stale)
			{
				result = Db.Get().CreatureStatusItems.Fresh.resolveStringCallback(CREATURES.STATUSITEMS.STALE.NAME, this);
			}
			return result;
		}

		// Token: 0x060087F7 RID: 34807 RVA: 0x0032D13E File Offset: 0x0032B33E
		public void Rot(Rottable.Instance smi, float deltaTime)
		{
			this.RefreshModifiers(deltaTime);
			if (smi.pickupable.storage != null)
			{
				smi.pickupable.storage.Trigger(-1197125120, null);
			}
		}

		// Token: 0x060087F8 RID: 34808 RVA: 0x0032D170 File Offset: 0x0032B370
		public bool IsRottableInSpace()
		{
			if (base.gameObject.GetMyWorld() == null)
			{
				Pickupable component = base.GetComponent<Pickupable>();
				if (component != null && component.storage && (component.storage.GetComponent<RocketModuleCluster>() || component.storage.GetComponent<ClusterTraveler>()))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060087F9 RID: 34809 RVA: 0x0032D1D4 File Offset: 0x0032B3D4
		public void RefreshModifiers(float dt)
		{
			if (this.GetMaster().isNull)
			{
				return;
			}
			if (!Grid.IsValidCell(Grid.PosToCell(base.gameObject)) && !this.IsRottableInSpace())
			{
				return;
			}
			this.rotAmountInstance.deltaAttribute.ClearModifiers();
			KPrefabID component = base.GetComponent<KPrefabID>();
			if (!component.HasAnyTags(Rottable.PRESERVED_TAGS))
			{
				Rottable.RotRefrigerationLevel rotRefrigerationLevel = Rottable.RefrigerationLevel(this);
				if (rotRefrigerationLevel != Rottable.RotRefrigerationLevel.Refrigerated)
				{
					if (rotRefrigerationLevel == Rottable.RotRefrigerationLevel.Frozen)
					{
						this.rotAmountInstance.deltaAttribute.Add(Rottable.Instance.frozenModifier);
					}
					else
					{
						this.rotAmountInstance.deltaAttribute.Add(Rottable.Instance.unrefrigeratedModifier);
					}
				}
				else
				{
					this.rotAmountInstance.deltaAttribute.Add(Rottable.Instance.refrigeratedModifier);
				}
				Rottable.RotAtmosphereQuality rotAtmosphereQuality = Rottable.AtmosphereQuality(this);
				if (rotAtmosphereQuality != Rottable.RotAtmosphereQuality.Sterilizing)
				{
					if (rotAtmosphereQuality == Rottable.RotAtmosphereQuality.Contaminating)
					{
						this.rotAmountInstance.deltaAttribute.Add(Rottable.Instance.contaminatedAtmosphereModifier);
					}
					else
					{
						this.rotAmountInstance.deltaAttribute.Add(Rottable.Instance.normalAtmosphereModifier);
					}
				}
				else
				{
					this.rotAmountInstance.deltaAttribute.Add(Rottable.Instance.sterileAtmosphereModifier);
				}
			}
			if (component.HasTag(Db.Get().Spices.PreservingSpice.Id))
			{
				this.rotAmountInstance.deltaAttribute.Add(Db.Get().Spices.PreservingSpice.FoodModifier);
			}
			Rottable.SetStatusItems(this);
		}

		// Token: 0x060087FA RID: 34810 RVA: 0x0032D320 File Offset: 0x0032B520
		private void OnAbsorb(object data)
		{
			Pickupable pickupable = (Pickupable)data;
			if (pickupable != null)
			{
				PrimaryElement component = base.gameObject.GetComponent<PrimaryElement>();
				PrimaryElement primaryElement = pickupable.PrimaryElement;
				Rottable.Instance smi = pickupable.gameObject.GetSMI<Rottable.Instance>();
				if (component != null && primaryElement != null && smi != null)
				{
					float num = component.Units * base.sm.rotParameter.Get(base.smi);
					float num2 = primaryElement.Units * base.sm.rotParameter.Get(smi);
					float value = (num + num2) / (component.Units + primaryElement.Units);
					base.sm.rotParameter.Set(value, base.smi, false);
				}
			}
		}

		// Token: 0x060087FB RID: 34811 RVA: 0x0032D3D8 File Offset: 0x0032B5D8
		public bool IsRotLevelStackable(Rottable.Instance other)
		{
			return Mathf.Abs(this.RotConstitutionPercentage - other.RotConstitutionPercentage) < 0.1f;
		}

		// Token: 0x060087FC RID: 34812 RVA: 0x0032D3F3 File Offset: 0x0032B5F3
		public string GetToolTip()
		{
			return this.rotAmountInstance.GetTooltip();
		}

		// Token: 0x060087FD RID: 34813 RVA: 0x0032D400 File Offset: 0x0032B600
		private void OnSplitFromChunk(object data)
		{
			Pickupable pickupable = (Pickupable)data;
			if (pickupable != null)
			{
				Rottable.Instance smi = pickupable.GetSMI<Rottable.Instance>();
				if (smi != null)
				{
					this.RotValue = smi.RotValue;
				}
			}
		}

		// Token: 0x060087FE RID: 34814 RVA: 0x0032D433 File Offset: 0x0032B633
		public void OnPreserved(object data)
		{
			if ((bool)data)
			{
				base.smi.GoTo(base.sm.Preserved);
				return;
			}
			base.smi.GoTo(base.sm.Fresh);
		}

		// Token: 0x04006776 RID: 26486
		private AmountInstance rotAmountInstance;

		// Token: 0x04006777 RID: 26487
		private static AttributeModifier unrefrigeratedModifier;

		// Token: 0x04006778 RID: 26488
		private static AttributeModifier refrigeratedModifier;

		// Token: 0x04006779 RID: 26489
		private static AttributeModifier frozenModifier;

		// Token: 0x0400677A RID: 26490
		private static AttributeModifier contaminatedAtmosphereModifier;

		// Token: 0x0400677B RID: 26491
		private static AttributeModifier normalAtmosphereModifier;

		// Token: 0x0400677C RID: 26492
		private static AttributeModifier sterileAtmosphereModifier;

		// Token: 0x0400677D RID: 26493
		public PrimaryElement primaryElement;

		// Token: 0x0400677E RID: 26494
		public Pickupable pickupable;
	}

	// Token: 0x020013AD RID: 5037
	public enum RotAtmosphereQuality
	{
		// Token: 0x04006780 RID: 26496
		Normal,
		// Token: 0x04006781 RID: 26497
		Sterilizing,
		// Token: 0x04006782 RID: 26498
		Contaminating
	}

	// Token: 0x020013AE RID: 5038
	public enum RotRefrigerationLevel
	{
		// Token: 0x04006784 RID: 26500
		Normal,
		// Token: 0x04006785 RID: 26501
		Refrigerated,
		// Token: 0x04006786 RID: 26502
		Frozen
	}
}
