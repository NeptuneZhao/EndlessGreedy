using System;
using System.Collections.Generic;
using Database;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020003CD RID: 973
public class SpiceGrinder : GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>
{
	// Token: 0x0600144D RID: 5197 RVA: 0x0006F820 File Offset: 0x0006DA20
	public static void InitializeSpices()
	{
		Spices spices = Db.Get().Spices;
		SpiceGrinder.SettingOptions = new Dictionary<Tag, SpiceGrinder.Option>();
		for (int i = 0; i < spices.Count; i++)
		{
			Spice spice = spices[i];
			if (DlcManager.IsDlcListValidForCurrentContent(spice.DlcIds))
			{
				SpiceGrinder.SettingOptions.Add(spice.Id, new SpiceGrinder.Option(spice));
			}
		}
	}

	// Token: 0x0600144E RID: 5198 RVA: 0x0006F884 File Offset: 0x0006DA84
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.root.Enter(new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State.Callback(this.OnEnterRoot)).EventHandler(GameHashes.OnStorageChange, new GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.GameEvent.Callback(this.OnStorageChanged));
		this.inoperational.EventTransition(GameHashes.OperationalChanged, this.ready, new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.Transition.ConditionCallback(this.IsOperational)).EventHandler(GameHashes.UpdateRoom, new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State.Callback(this.UpdateInKitchen)).Enter(delegate(SpiceGrinder.StatesInstance smi)
		{
			smi.Play((smi.SelectedOption != null) ? "off" : "default", KAnim.PlayMode.Once);
			smi.CancelFetches("inoperational");
			if (smi.SelectedOption == null)
			{
				smi.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoSpiceSelected, null);
			}
		}).Exit(delegate(SpiceGrinder.StatesInstance smi)
		{
			smi.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoSpiceSelected, false);
		});
		this.operational.EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.Not(new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.Transition.ConditionCallback(this.IsOperational))).EventHandler(GameHashes.UpdateRoom, new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State.Callback(this.UpdateInKitchen)).ParamTransition<bool>(this.isReady, this.ready, GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.IsTrue).Update(delegate(SpiceGrinder.StatesInstance smi, float dt)
		{
			if (smi.CurrentFood != null && !smi.HasOpenFetches)
			{
				bool value = smi.CanSpice(smi.CurrentFood.Calories);
				this.isReady.Set(value, smi, false);
			}
		}, UpdateRate.SIM_1000ms, false).PlayAnim("on");
		this.ready.EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.Not(new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.Transition.ConditionCallback(this.IsOperational))).EventHandler(GameHashes.UpdateRoom, new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State.Callback(this.UpdateInKitchen)).ParamTransition<bool>(this.isReady, this.operational, GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.IsFalse).ToggleRecurringChore(new Func<SpiceGrinder.StatesInstance, Chore>(this.CreateChore), null);
	}

	// Token: 0x0600144F RID: 5199 RVA: 0x0006FA1F File Offset: 0x0006DC1F
	private void UpdateInKitchen(SpiceGrinder.StatesInstance smi)
	{
		smi.GetComponent<Operational>().SetFlag(SpiceGrinder.inKitchen, smi.roomTracker.IsInCorrectRoom());
	}

	// Token: 0x06001450 RID: 5200 RVA: 0x0006FA3C File Offset: 0x0006DC3C
	private void OnEnterRoot(SpiceGrinder.StatesInstance smi)
	{
		smi.Initialize();
	}

	// Token: 0x06001451 RID: 5201 RVA: 0x0006FA44 File Offset: 0x0006DC44
	private bool IsOperational(SpiceGrinder.StatesInstance smi)
	{
		return smi.IsOperational;
	}

	// Token: 0x06001452 RID: 5202 RVA: 0x0006FA4C File Offset: 0x0006DC4C
	private void OnStorageChanged(SpiceGrinder.StatesInstance smi, object data)
	{
		smi.UpdateMeter();
		smi.UpdateFoodSymbol();
		if (smi.SelectedOption == null)
		{
			return;
		}
		bool value = smi.AvailableFood > 0f && smi.CanSpice(smi.CurrentFood.Calories);
		smi.sm.isReady.Set(value, smi, false);
	}

	// Token: 0x06001453 RID: 5203 RVA: 0x0006FAA4 File Offset: 0x0006DCA4
	private Chore CreateChore(SpiceGrinder.StatesInstance smi)
	{
		return new WorkChore<SpiceGrinderWorkable>(Db.Get().ChoreTypes.Cook, smi.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
	}

	// Token: 0x04000B9D RID: 2973
	public static Dictionary<Tag, SpiceGrinder.Option> SettingOptions = null;

	// Token: 0x04000B9E RID: 2974
	public static readonly Operational.Flag spiceSet = new Operational.Flag("spiceSet", Operational.Flag.Type.Functional);

	// Token: 0x04000B9F RID: 2975
	public static Operational.Flag inKitchen = new Operational.Flag("inKitchen", Operational.Flag.Type.Functional);

	// Token: 0x04000BA0 RID: 2976
	public GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State inoperational;

	// Token: 0x04000BA1 RID: 2977
	public GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State operational;

	// Token: 0x04000BA2 RID: 2978
	public GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State ready;

	// Token: 0x04000BA3 RID: 2979
	public StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.BoolParameter isReady;

	// Token: 0x02001156 RID: 4438
	public class Option : IConfigurableConsumerOption
	{
		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x06007F32 RID: 32562 RVA: 0x0030C484 File Offset: 0x0030A684
		public Effect StatBonus
		{
			get
			{
				if (this.statBonus == null)
				{
					return null;
				}
				if (string.IsNullOrEmpty(this.spiceDescription))
				{
					this.CreateDescription();
					this.GetName();
				}
				this.statBonus.Name = this.name;
				this.statBonus.description = this.spiceDescription;
				return this.statBonus;
			}
		}

		// Token: 0x06007F33 RID: 32563 RVA: 0x0030C4E0 File Offset: 0x0030A6E0
		public Option(Spice spice)
		{
			this.Id = new Tag(spice.Id);
			this.Spice = spice;
			if (spice.StatBonus != null)
			{
				this.statBonus = new Effect(spice.Id, this.GetName(), this.spiceDescription, 600f, true, false, false, null, -1f, 0f, null, "");
				this.statBonus.Add(spice.StatBonus);
				Db.Get().effects.Add(this.statBonus);
			}
		}

		// Token: 0x06007F34 RID: 32564 RVA: 0x0030C570 File Offset: 0x0030A770
		public Tag GetID()
		{
			return this.Spice.Id;
		}

		// Token: 0x06007F35 RID: 32565 RVA: 0x0030C584 File Offset: 0x0030A784
		public string GetName()
		{
			if (string.IsNullOrEmpty(this.name))
			{
				string text = "STRINGS.ITEMS.SPICES." + this.Spice.Id.ToUpper() + ".NAME";
				StringEntry stringEntry;
				Strings.TryGet(text, out stringEntry);
				this.name = "MISSING " + text;
				if (stringEntry != null)
				{
					this.name = stringEntry;
				}
			}
			return this.name;
		}

		// Token: 0x06007F36 RID: 32566 RVA: 0x0030C5ED File Offset: 0x0030A7ED
		public string GetDetailedDescription()
		{
			if (string.IsNullOrEmpty(this.fullDescription))
			{
				this.CreateDescription();
			}
			return this.fullDescription;
		}

		// Token: 0x06007F37 RID: 32567 RVA: 0x0030C608 File Offset: 0x0030A808
		public string GetDescription()
		{
			if (!string.IsNullOrEmpty(this.spiceDescription))
			{
				return this.spiceDescription;
			}
			string text = "STRINGS.ITEMS.SPICES." + this.Spice.Id.ToUpper() + ".DESC";
			StringEntry stringEntry;
			Strings.TryGet(text, out stringEntry);
			this.spiceDescription = "MISSING " + text;
			if (stringEntry != null)
			{
				this.spiceDescription = stringEntry.String;
			}
			return this.spiceDescription;
		}

		// Token: 0x06007F38 RID: 32568 RVA: 0x0030C678 File Offset: 0x0030A878
		private void CreateDescription()
		{
			string text = "STRINGS.ITEMS.SPICES." + this.Spice.Id.ToUpper() + ".DESC";
			StringEntry stringEntry;
			Strings.TryGet(text, out stringEntry);
			this.spiceDescription = "MISSING " + text;
			if (stringEntry != null)
			{
				this.spiceDescription = stringEntry.String;
			}
			this.ingredientDescriptions = string.Format("\n\n<b>{0}</b>", BUILDINGS.PREFABS.SPICEGRINDER.INGREDIENTHEADER);
			for (int i = 0; i < this.Spice.Ingredients.Length; i++)
			{
				Spice.Ingredient ingredient = this.Spice.Ingredients[i];
				GameObject prefab = Assets.GetPrefab((ingredient.IngredientSet != null && ingredient.IngredientSet.Length != 0) ? ingredient.IngredientSet[0] : null);
				this.ingredientDescriptions += string.Format("\n{0}{1} {2}{3}", new object[]
				{
					"    • ",
					prefab.GetProperName(),
					ingredient.AmountKG,
					GameUtil.GetUnitTypeMassOrUnit(prefab)
				});
			}
			this.fullDescription = this.spiceDescription + this.ingredientDescriptions;
		}

		// Token: 0x06007F39 RID: 32569 RVA: 0x0030C79D File Offset: 0x0030A99D
		public Sprite GetIcon()
		{
			return Assets.GetSprite(this.Spice.Image);
		}

		// Token: 0x06007F3A RID: 32570 RVA: 0x0030C7B4 File Offset: 0x0030A9B4
		public IConfigurableConsumerIngredient[] GetIngredients()
		{
			return this.Spice.Ingredients;
		}

		// Token: 0x04005FCB RID: 24523
		public readonly Tag Id;

		// Token: 0x04005FCC RID: 24524
		public readonly Spice Spice;

		// Token: 0x04005FCD RID: 24525
		private string name;

		// Token: 0x04005FCE RID: 24526
		private string fullDescription;

		// Token: 0x04005FCF RID: 24527
		private string spiceDescription;

		// Token: 0x04005FD0 RID: 24528
		private string ingredientDescriptions;

		// Token: 0x04005FD1 RID: 24529
		private Effect statBonus;
	}

	// Token: 0x02001157 RID: 4439
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001158 RID: 4440
	public class StatesInstance : GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.GameInstance
	{
		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x06007F3C RID: 32572 RVA: 0x0030C7D6 File Offset: 0x0030A9D6
		public bool IsOperational
		{
			get
			{
				return this.operational != null && this.operational.IsOperational;
			}
		}

		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x06007F3D RID: 32573 RVA: 0x0030C7F3 File Offset: 0x0030A9F3
		public float AvailableFood
		{
			get
			{
				if (!(this.foodStorage == null))
				{
					return this.foodStorage.MassStored();
				}
				return 0f;
			}
		}

		// Token: 0x170008FA RID: 2298
		// (get) Token: 0x06007F3E RID: 32574 RVA: 0x0030C814 File Offset: 0x0030AA14
		public SpiceGrinder.Option SelectedOption
		{
			get
			{
				if (!(this.currentSpice.Id == Tag.Invalid))
				{
					return SpiceGrinder.SettingOptions[this.currentSpice.Id];
				}
				return null;
			}
		}

		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x06007F3F RID: 32575 RVA: 0x0030C844 File Offset: 0x0030AA44
		public Edible CurrentFood
		{
			get
			{
				GameObject gameObject = this.foodStorage.FindFirst(GameTags.Edible);
				this.currentFood = ((gameObject != null) ? gameObject.GetComponent<Edible>() : null);
				return this.currentFood;
			}
		}

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x06007F40 RID: 32576 RVA: 0x0030C880 File Offset: 0x0030AA80
		public bool HasOpenFetches
		{
			get
			{
				return Array.Exists<FetchChore>(this.SpiceFetches, (FetchChore fetch) => fetch != null);
			}
		}

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x06007F41 RID: 32577 RVA: 0x0030C8AC File Offset: 0x0030AAAC
		// (set) Token: 0x06007F42 RID: 32578 RVA: 0x0030C8B4 File Offset: 0x0030AAB4
		public bool AllowMutantSeeds
		{
			get
			{
				return this.allowMutantSeeds;
			}
			set
			{
				this.allowMutantSeeds = value;
				this.ToggleMutantSeedFetches(this.allowMutantSeeds);
			}
		}

		// Token: 0x06007F43 RID: 32579 RVA: 0x0030C8CC File Offset: 0x0030AACC
		public StatesInstance(IStateMachineTarget master, SpiceGrinder.Def def) : base(master, def)
		{
			this.workable.Grinder = this;
			Storage[] components = base.gameObject.GetComponents<Storage>();
			this.foodStorage = components[0];
			this.seedStorage = components[1];
			this.operational = base.GetComponent<Operational>();
			this.kbac = base.GetComponent<KBatchedAnimController>();
			this.foodStorageFilter = new FilteredStorage(base.GetComponent<KPrefabID>(), this.foodFilter, null, false, Db.Get().ChoreTypes.CookFetch);
			this.foodStorageFilter.SetHasMeter(false);
			this.meter = new MeterController(this.kbac, "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_frame",
				"meter_level"
			});
			this.SetupFoodSymbol();
			this.UpdateFoodSymbol();
			base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
			base.sm.UpdateInKitchen(this);
			Prioritizable.AddRef(base.gameObject);
			base.Subscribe(493375141, new Action<object>(this.OnRefreshUserMenu));
		}

		// Token: 0x06007F44 RID: 32580 RVA: 0x0030C9FA File Offset: 0x0030ABFA
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			Prioritizable.RemoveRef(base.gameObject);
		}

		// Token: 0x06007F45 RID: 32581 RVA: 0x0030CA10 File Offset: 0x0030AC10
		public void Initialize()
		{
			if (DlcManager.IsExpansion1Active())
			{
				this.mutantSeedStatusItem = new StatusItem("SPICEGRINDERACCEPTSMUTANTSEEDS", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
				if (this.AllowMutantSeeds)
				{
					KSelectable component = base.GetComponent<KSelectable>();
					if (component != null)
					{
						component.AddStatusItem(this.mutantSeedStatusItem, null);
					}
				}
			}
			SpiceGrinder.Option spiceOption;
			SpiceGrinder.SettingOptions.TryGetValue(new Tag(this.spiceHash), out spiceOption);
			this.OnOptionSelected(spiceOption);
			base.sm.OnStorageChanged(this, null);
			this.UpdateMeter();
		}

		// Token: 0x06007F46 RID: 32582 RVA: 0x0030CAA8 File Offset: 0x0030ACA8
		private void OnRefreshUserMenu(object data)
		{
			if (DlcManager.FeatureRadiationEnabled())
			{
				Game.Instance.userMenu.AddButton(base.smi.gameObject, new KIconButtonMenu.ButtonInfo("action_switch_toggle", base.smi.AllowMutantSeeds ? UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.REJECT : UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.ACCEPT, delegate()
				{
					base.smi.AllowMutantSeeds = !base.smi.AllowMutantSeeds;
					this.OnRefreshUserMenu(base.smi);
				}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.TOOLTIP, true), 1f);
			}
		}

		// Token: 0x06007F47 RID: 32583 RVA: 0x0030CB24 File Offset: 0x0030AD24
		public void ToggleMutantSeedFetches(bool allow)
		{
			if (DlcManager.IsExpansion1Active())
			{
				this.UpdateMutantSeedFetches();
				if (allow)
				{
					this.seedStorage.storageFilters.Add(GameTags.MutatedSeed);
					KSelectable component = base.GetComponent<KSelectable>();
					if (component != null)
					{
						component.AddStatusItem(this.mutantSeedStatusItem, null);
						return;
					}
				}
				else
				{
					if (this.seedStorage.GetMassAvailable(GameTags.MutatedSeed) > 0f)
					{
						this.seedStorage.Drop(GameTags.MutatedSeed);
					}
					this.seedStorage.storageFilters.Remove(GameTags.MutatedSeed);
					KSelectable component2 = base.GetComponent<KSelectable>();
					if (component2 != null)
					{
						component2.RemoveStatusItem(this.mutantSeedStatusItem, false);
					}
				}
			}
		}

		// Token: 0x06007F48 RID: 32584 RVA: 0x0030CBD4 File Offset: 0x0030ADD4
		private void UpdateMutantSeedFetches()
		{
			if (this.SpiceFetches != null)
			{
				Tag[] tags = new Tag[]
				{
					GameTags.Seed,
					GameTags.CropSeed
				};
				for (int i = this.SpiceFetches.Length - 1; i >= 0; i--)
				{
					FetchChore fetchChore = this.SpiceFetches[i];
					if (fetchChore != null)
					{
						using (HashSet<Tag>.Enumerator enumerator = this.SpiceFetches[i].tags.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (Assets.GetPrefab(enumerator.Current).HasAnyTags(tags))
								{
									fetchChore.Cancel("MutantSeedChanges");
									this.SpiceFetches[i] = this.CreateFetchChore(fetchChore.tags, fetchChore.amount);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06007F49 RID: 32585 RVA: 0x0030CCA4 File Offset: 0x0030AEA4
		private void OnCopySettings(object data)
		{
			SpiceGrinderWorkable component = ((GameObject)data).GetComponent<SpiceGrinderWorkable>();
			if (component != null)
			{
				this.currentSpice = component.Grinder.currentSpice;
				SpiceGrinder.Option spiceOption;
				SpiceGrinder.SettingOptions.TryGetValue(new Tag(component.Grinder.spiceHash), out spiceOption);
				this.OnOptionSelected(spiceOption);
				this.allowMutantSeeds = component.Grinder.AllowMutantSeeds;
			}
		}

		// Token: 0x06007F4A RID: 32586 RVA: 0x0030CD0C File Offset: 0x0030AF0C
		public void SetupFoodSymbol()
		{
			GameObject gameObject = Util.NewGameObject(base.gameObject, "foodSymbol");
			gameObject.SetActive(false);
			bool flag;
			Vector3 position = this.kbac.GetSymbolTransform(SpiceGrinder.StatesInstance.HASH_FOOD, out flag).GetColumn(3);
			position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
			gameObject.transform.SetPosition(position);
			this.foodKBAC = gameObject.AddComponent<KBatchedAnimController>();
			this.foodKBAC.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim("mushbar_kanim")
			};
			this.foodKBAC.initialAnim = "object";
			this.kbac.SetSymbolVisiblity(SpiceGrinder.StatesInstance.HASH_FOOD, false);
		}

		// Token: 0x06007F4B RID: 32587 RVA: 0x0030CDC8 File Offset: 0x0030AFC8
		public void UpdateFoodSymbol()
		{
			bool flag = this.AvailableFood > 0f && this.CurrentFood != null;
			this.foodKBAC.gameObject.SetActive(flag);
			if (flag)
			{
				this.foodKBAC.SwapAnims(this.CurrentFood.GetComponent<KBatchedAnimController>().AnimFiles);
				this.foodKBAC.Play("object", KAnim.PlayMode.Loop, 1f, 0f);
			}
		}

		// Token: 0x06007F4C RID: 32588 RVA: 0x0030CE41 File Offset: 0x0030B041
		public void UpdateMeter()
		{
			this.meter.SetPositionPercent(this.seedStorage.MassStored() / this.seedStorage.capacityKg);
		}

		// Token: 0x06007F4D RID: 32589 RVA: 0x0030CE68 File Offset: 0x0030B068
		public void SpiceFood()
		{
			float num = this.CurrentFood.Calories / 1000f;
			this.CurrentFood.SpiceEdible(this.currentSpice, SpiceGrinderConfig.SpicedStatus);
			this.foodStorage.Drop(this.CurrentFood.gameObject, true);
			this.currentFood = null;
			this.UpdateFoodSymbol();
			foreach (Spice.Ingredient ingredient in SpiceGrinder.SettingOptions[this.currentSpice.Id].Spice.Ingredients)
			{
				float num2 = num * ingredient.AmountKG / 1000f;
				int num3 = ingredient.IngredientSet.Length - 1;
				while (num2 > 0f && num3 >= 0)
				{
					Tag tag = ingredient.IngredientSet[num3];
					float num4;
					SimUtil.DiseaseInfo diseaseInfo;
					float num5;
					this.seedStorage.ConsumeAndGetDisease(tag, num2, out num4, out diseaseInfo, out num5);
					num2 -= num4;
					num3--;
				}
			}
			base.sm.isReady.Set(false, this, false);
		}

		// Token: 0x06007F4E RID: 32590 RVA: 0x0030CF68 File Offset: 0x0030B168
		public bool CanSpice(float kcalToSpice)
		{
			bool flag = true;
			float num = kcalToSpice / 1000f;
			Spice.Ingredient[] ingredients = SpiceGrinder.SettingOptions[this.currentSpice.Id].Spice.Ingredients;
			Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
			for (int i = 0; i < ingredients.Length; i++)
			{
				Spice.Ingredient ingredient = ingredients[i];
				float num2 = 0f;
				int num3 = 0;
				while (ingredient.IngredientSet != null && num3 < ingredient.IngredientSet.Length)
				{
					num2 += this.seedStorage.GetMassAvailable(ingredient.IngredientSet[num3]);
					num3++;
				}
				float num4 = num * ingredient.AmountKG / 1000f;
				flag &= (num4 <= num2);
				if (num4 > num2)
				{
					dictionary.Add(ingredient.IngredientSet[0], num4 - num2);
					if (this.SpiceFetches != null && this.SpiceFetches[i] == null)
					{
						this.SpiceFetches[i] = this.CreateFetchChore(ingredient.IngredientSet, ingredient.AmountKG * 10f);
					}
				}
			}
			this.UpdateSpiceIngredientStatus(flag, dictionary);
			return flag;
		}

		// Token: 0x06007F4F RID: 32591 RVA: 0x0030D083 File Offset: 0x0030B283
		private FetchChore CreateFetchChore(Tag[] ingredientIngredientSet, float amount)
		{
			return this.CreateFetchChore(new HashSet<Tag>(ingredientIngredientSet), amount);
		}

		// Token: 0x06007F50 RID: 32592 RVA: 0x0030D094 File Offset: 0x0030B294
		private FetchChore CreateFetchChore(HashSet<Tag> ingredients, float amount)
		{
			float num = Mathf.Max(amount, 1f);
			ChoreType cookFetch = Db.Get().ChoreTypes.CookFetch;
			Storage destination = this.seedStorage;
			float amount2 = num;
			FetchChore.MatchCriteria criteria = FetchChore.MatchCriteria.MatchID;
			Tag invalid = Tag.Invalid;
			Action<Chore> on_complete = new Action<Chore>(this.ClearFetchChore);
			Tag[] forbidden_tags;
			if (!this.AllowMutantSeeds)
			{
				(forbidden_tags = new Tag[1])[0] = GameTags.MutatedSeed;
			}
			else
			{
				forbidden_tags = null;
			}
			return new FetchChore(cookFetch, destination, amount2, ingredients, criteria, invalid, forbidden_tags, null, true, on_complete, null, null, Operational.State.Operational, 0);
		}

		// Token: 0x06007F51 RID: 32593 RVA: 0x0030D100 File Offset: 0x0030B300
		private void ClearFetchChore(Chore obj)
		{
			FetchChore fetchChore = obj as FetchChore;
			if (fetchChore == null || !fetchChore.isComplete || this.SpiceFetches == null)
			{
				return;
			}
			int i = this.SpiceFetches.Length - 1;
			while (i >= 0)
			{
				if (this.SpiceFetches[i] == fetchChore)
				{
					float num = fetchChore.originalAmount - fetchChore.amount;
					if (num > 0f)
					{
						this.SpiceFetches[i] = this.CreateFetchChore(fetchChore.tags, num);
						return;
					}
					this.SpiceFetches[i] = null;
					return;
				}
				else
				{
					i--;
				}
			}
		}

		// Token: 0x06007F52 RID: 32594 RVA: 0x0030D180 File Offset: 0x0030B380
		private void UpdateSpiceIngredientStatus(bool can_spice, Dictionary<Tag, float> missing_spices)
		{
			KSelectable component = base.GetComponent<KSelectable>();
			if (can_spice)
			{
				this.missingResourceStatusItem = component.RemoveStatusItem(this.missingResourceStatusItem, false);
				return;
			}
			if (this.missingResourceStatusItem != Guid.Empty)
			{
				this.missingResourceStatusItem = component.ReplaceStatusItem(this.missingResourceStatusItem, Db.Get().BuildingStatusItems.MaterialsUnavailable, missing_spices);
				return;
			}
			this.missingResourceStatusItem = component.AddStatusItem(Db.Get().BuildingStatusItems.MaterialsUnavailable, missing_spices);
		}

		// Token: 0x06007F53 RID: 32595 RVA: 0x0030D1FC File Offset: 0x0030B3FC
		public void OnOptionSelected(SpiceGrinder.Option spiceOption)
		{
			base.smi.GetComponent<Operational>().SetFlag(SpiceGrinder.spiceSet, spiceOption != null);
			if (spiceOption == null)
			{
				this.kbac.Play("default", KAnim.PlayMode.Once, 1f, 0f);
				this.kbac.SetSymbolTint("stripe_anim2", Color.white);
			}
			else
			{
				this.kbac.Play(this.IsOperational ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
			}
			this.CancelFetches("SpiceChanged");
			if (this.currentSpice.Id != Tag.Invalid)
			{
				this.seedStorage.DropAll(false, false, default(Vector3), true, null);
				this.UpdateMeter();
				base.sm.isReady.Set(false, this, false);
			}
			if (this.missingResourceStatusItem != Guid.Empty)
			{
				this.missingResourceStatusItem = base.GetComponent<KSelectable>().RemoveStatusItem(this.missingResourceStatusItem, false);
			}
			if (spiceOption != null)
			{
				this.currentSpice = new SpiceInstance
				{
					Id = spiceOption.Id,
					TotalKG = spiceOption.Spice.TotalKG
				};
				this.SetSpiceSymbolColours(spiceOption.Spice);
				this.spiceHash = this.currentSpice.Id.GetHash();
				this.seedStorage.capacityKg = this.currentSpice.TotalKG * 10f;
				Spice.Ingredient[] ingredients = spiceOption.Spice.Ingredients;
				this.SpiceFetches = new FetchChore[ingredients.Length];
				Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
				for (int i = 0; i < ingredients.Length; i++)
				{
					Spice.Ingredient ingredient = ingredients[i];
					float num = (this.CurrentFood != null) ? (this.CurrentFood.Calories * ingredient.AmountKG / 1000000f) : 0f;
					if (this.seedStorage.GetMassAvailable(ingredient.IngredientSet[0]) < num)
					{
						this.SpiceFetches[i] = this.CreateFetchChore(ingredient.IngredientSet, ingredient.AmountKG * 10f);
					}
					if (this.CurrentFood != null)
					{
						dictionary.Add(ingredient.IngredientSet[0], num);
					}
				}
				if (this.CurrentFood != null)
				{
					this.UpdateSpiceIngredientStatus(false, dictionary);
				}
				this.foodFilter[0] = this.currentSpice.Id;
				this.foodStorageFilter.FilterChanged();
			}
		}

		// Token: 0x06007F54 RID: 32596 RVA: 0x0030D488 File Offset: 0x0030B688
		public void CancelFetches(string reason)
		{
			if (this.SpiceFetches != null)
			{
				for (int i = 0; i < this.SpiceFetches.Length; i++)
				{
					if (this.SpiceFetches[i] != null)
					{
						this.SpiceFetches[i].Cancel(reason);
						this.SpiceFetches[i] = null;
					}
				}
			}
		}

		// Token: 0x06007F55 RID: 32597 RVA: 0x0030D4D4 File Offset: 0x0030B6D4
		private void SetSpiceSymbolColours(Spice spice)
		{
			this.kbac.SetSymbolTint("stripe_anim2", spice.PrimaryColor);
			this.kbac.SetSymbolTint("stripe_anim1", spice.SecondaryColor);
			this.kbac.SetSymbolTint("grinder", spice.PrimaryColor);
		}

		// Token: 0x04005FD2 RID: 24530
		private static string HASH_FOOD = "food";

		// Token: 0x04005FD3 RID: 24531
		private KBatchedAnimController kbac;

		// Token: 0x04005FD4 RID: 24532
		private KBatchedAnimController foodKBAC;

		// Token: 0x04005FD5 RID: 24533
		[MyCmpReq]
		public RoomTracker roomTracker;

		// Token: 0x04005FD6 RID: 24534
		[MyCmpReq]
		public SpiceGrinderWorkable workable;

		// Token: 0x04005FD7 RID: 24535
		[Serialize]
		private int spiceHash;

		// Token: 0x04005FD8 RID: 24536
		private SpiceInstance currentSpice;

		// Token: 0x04005FD9 RID: 24537
		private Edible currentFood;

		// Token: 0x04005FDA RID: 24538
		private Storage seedStorage;

		// Token: 0x04005FDB RID: 24539
		private Storage foodStorage;

		// Token: 0x04005FDC RID: 24540
		private MeterController meter;

		// Token: 0x04005FDD RID: 24541
		private Tag[] foodFilter = new Tag[1];

		// Token: 0x04005FDE RID: 24542
		private FilteredStorage foodStorageFilter;

		// Token: 0x04005FDF RID: 24543
		private Operational operational;

		// Token: 0x04005FE0 RID: 24544
		private Guid missingResourceStatusItem = Guid.Empty;

		// Token: 0x04005FE1 RID: 24545
		private StatusItem mutantSeedStatusItem;

		// Token: 0x04005FE2 RID: 24546
		private FetchChore[] SpiceFetches;

		// Token: 0x04005FE3 RID: 24547
		[Serialize]
		private bool allowMutantSeeds = true;
	}
}
