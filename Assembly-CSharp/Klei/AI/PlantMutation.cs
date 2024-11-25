using System;
using System.Collections.Generic;
using System.Text;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F7A RID: 3962
	public class PlantMutation : Modifier
	{
		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x0600798E RID: 31118 RVA: 0x00300282 File Offset: 0x002FE482
		public List<string> AdditionalSoundEvents
		{
			get
			{
				return this.additionalSoundEvents;
			}
		}

		// Token: 0x0600798F RID: 31119 RVA: 0x0030028C File Offset: 0x002FE48C
		public PlantMutation(string id, string name, string desc) : base(id, name, desc)
		{
		}

		// Token: 0x06007990 RID: 31120 RVA: 0x00300310 File Offset: 0x002FE510
		public void ApplyTo(MutantPlant target)
		{
			this.ApplyFunctionalTo(target);
			if (!target.HasTag(GameTags.Seed) && !target.HasTag(GameTags.CropSeed) && !target.HasTag(GameTags.Compostable))
			{
				this.ApplyVisualTo(target);
			}
		}

		// Token: 0x06007991 RID: 31121 RVA: 0x00300348 File Offset: 0x002FE548
		private void ApplyFunctionalTo(MutantPlant target)
		{
			SeedProducer component = target.GetComponent<SeedProducer>();
			if (component != null && component.seedInfo.productionType == SeedProducer.ProductionType.Harvest)
			{
				component.Configure(component.seedInfo.seedId, SeedProducer.ProductionType.Sterile, 1);
			}
			if (this.bonusCropID.IsValid)
			{
				target.Subscribe(-1072826864, new Action<object>(this.OnHarvestBonusCrop));
			}
			if (!this.forcePrefersDarkness)
			{
				if (this.SelfModifiers.Find((AttributeModifier m) => m.AttributeId == Db.Get().PlantAttributes.MinLightLux.Id) == null)
				{
					goto IL_F0;
				}
			}
			IlluminationVulnerable illuminationVulnerable = target.GetComponent<IlluminationVulnerable>();
			if (illuminationVulnerable == null)
			{
				illuminationVulnerable = target.gameObject.AddComponent<IlluminationVulnerable>();
			}
			if (this.forcePrefersDarkness)
			{
				if (illuminationVulnerable != null)
				{
					illuminationVulnerable.SetPrefersDarkness(true);
				}
			}
			else
			{
				if (illuminationVulnerable != null)
				{
					illuminationVulnerable.SetPrefersDarkness(false);
				}
				target.GetComponent<Modifiers>().attributes.Add(Db.Get().PlantAttributes.MinLightLux);
			}
			IL_F0:
			byte b = this.droppedDiseaseID;
			if (this.harvestDiseaseID != 255)
			{
				target.Subscribe(35625290, new Action<object>(this.OnCropSpawnedAddDisease));
			}
			bool isValid = this.ensureIrrigationInfo.tag.IsValid;
			Attributes attributes = target.GetAttributes();
			this.AddTo(attributes);
		}

		// Token: 0x06007992 RID: 31122 RVA: 0x00300498 File Offset: 0x002FE698
		private void ApplyVisualTo(MutantPlant target)
		{
			KBatchedAnimController component = target.GetComponent<KBatchedAnimController>();
			if (this.symbolOverrideInfo != null && this.symbolOverrideInfo.Count > 0)
			{
				SymbolOverrideController component2 = target.GetComponent<SymbolOverrideController>();
				if (component2 != null)
				{
					foreach (PlantMutation.SymbolOverrideInfo symbolOverrideInfo in this.symbolOverrideInfo)
					{
						KAnim.Build.Symbol symbol = Assets.GetAnim(symbolOverrideInfo.sourceAnim).GetData().build.GetSymbol(symbolOverrideInfo.sourceSymbol);
						component2.AddSymbolOverride(symbolOverrideInfo.targetSymbolName, symbol, 0);
					}
				}
			}
			if (this.bGFXAnim != null)
			{
				PlantMutation.CreateFXObject(target, this.bGFXAnim, "_BGFX", 0.1f);
			}
			if (this.fGFXAnim != null)
			{
				PlantMutation.CreateFXObject(target, this.fGFXAnim, "_FGFX", -0.1f);
			}
			if (this.plantTint != Color.white)
			{
				component.TintColour = this.plantTint;
			}
			if (this.symbolTints.Count > 0)
			{
				for (int i = 0; i < this.symbolTints.Count; i++)
				{
					component.SetSymbolTint(this.symbolTintTargets[i], this.symbolTints[i]);
				}
			}
			if (this.symbolScales.Count > 0)
			{
				for (int j = 0; j < this.symbolScales.Count; j++)
				{
					component.SetSymbolScale(this.symbolScaleTargets[j], this.symbolScales[j]);
				}
			}
			if (this.additionalSoundEvents.Count > 0)
			{
				for (int k = 0; k < this.additionalSoundEvents.Count; k++)
				{
				}
			}
		}

		// Token: 0x06007993 RID: 31123 RVA: 0x0030067C File Offset: 0x002FE87C
		private static void CreateFXObject(MutantPlant target, string anim, string nameSuffix, float offset)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Assets.GetPrefab(SimpleFXConfig.ID));
			gameObject.name = target.name + nameSuffix;
			gameObject.transform.parent = target.transform;
			gameObject.AddComponent<LoopingSounds>();
			gameObject.GetComponent<KPrefabID>().PrefabTag = new Tag(gameObject.name);
			Extents extents = target.GetComponent<OccupyArea>().GetExtents();
			Vector3 position = target.transform.GetPosition();
			position.x = (float)extents.x + (float)extents.width / 2f;
			position.y = (float)extents.y + (float)extents.height / 2f;
			position.z += offset;
			gameObject.transform.SetPosition(position);
			KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
			component.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim(anim)
			};
			component.initialAnim = "idle";
			component.initialMode = KAnim.PlayMode.Loop;
			component.randomiseLoopedOffset = true;
			component.fgLayer = Grid.SceneLayer.NoLayer;
			if (target.HasTag(GameTags.Hanging))
			{
				component.Rotation = 180f;
			}
			gameObject.SetActive(true);
		}

		// Token: 0x06007994 RID: 31124 RVA: 0x003007AB File Offset: 0x002FE9AB
		private void OnHarvestBonusCrop(object data)
		{
			((Crop)data).SpawnSomeFruit(this.bonusCropID, this.bonusCropAmount);
		}

		// Token: 0x06007995 RID: 31125 RVA: 0x003007C4 File Offset: 0x002FE9C4
		private void OnCropSpawnedAddDisease(object data)
		{
			((GameObject)data).GetComponent<PrimaryElement>().AddDisease(this.harvestDiseaseID, this.harvestDiseaseAmount, this.Name);
		}

		// Token: 0x06007996 RID: 31126 RVA: 0x003007E8 File Offset: 0x002FE9E8
		public string GetTooltip()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.desc);
			foreach (AttributeModifier attributeModifier in this.SelfModifiers)
			{
				Attribute attribute = Db.Get().Attributes.TryGet(attributeModifier.AttributeId);
				if (attribute == null)
				{
					attribute = Db.Get().PlantAttributes.Get(attributeModifier.AttributeId);
				}
				if (attribute.ShowInUI != Attribute.Display.Never)
				{
					stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
					stringBuilder.Append(string.Format(DUPLICANTS.TRAITS.ATTRIBUTE_MODIFIERS, attribute.Name, attributeModifier.GetFormattedString()));
				}
			}
			if (this.bonusCropID != null)
			{
				string newValue;
				if (GameTags.DisplayAsCalories.Contains(this.bonusCropID))
				{
					EdiblesManager.FoodInfo foodInfo = EdiblesManager.GetFoodInfo(this.bonusCropID.Name);
					DebugUtil.Assert(foodInfo != null, "Eeh? Trying to spawn a bonus crop that is caloric but isn't a food??", this.bonusCropID.ToString());
					newValue = GameUtil.GetFormattedCalories(this.bonusCropAmount * foodInfo.CaloriesPerUnit, GameUtil.TimeSlice.None, true);
				}
				else if (GameTags.DisplayAsUnits.Contains(this.bonusCropID))
				{
					newValue = GameUtil.GetFormattedUnits(this.bonusCropAmount, GameUtil.TimeSlice.None, false, "");
				}
				else
				{
					newValue = GameUtil.GetFormattedMass(this.bonusCropAmount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
				}
				stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
				stringBuilder.Append(CREATURES.PLANT_MUTATIONS.BONUS_CROP_FMT.Replace("{Crop}", this.bonusCropID.ProperName()).Replace("{Amount}", newValue));
			}
			if (this.droppedDiseaseID != 255)
			{
				if (this.droppedDiseaseOnGrowAmount > 0)
				{
					stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
					stringBuilder.Append(UI.UISIDESCREENS.PLANTERSIDESCREEN.DISEASE_DROPPER_BURST.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.droppedDiseaseID, false)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.droppedDiseaseOnGrowAmount, GameUtil.TimeSlice.None)));
				}
				if (this.droppedDiseaseContinuousAmount > 0)
				{
					stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
					stringBuilder.Append(UI.UISIDESCREENS.PLANTERSIDESCREEN.DISEASE_DROPPER_CONSTANT.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.droppedDiseaseID, false)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.droppedDiseaseContinuousAmount, GameUtil.TimeSlice.PerSecond)));
				}
			}
			if (this.harvestDiseaseID != 255)
			{
				stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
				stringBuilder.Append(UI.UISIDESCREENS.PLANTERSIDESCREEN.DISEASE_ON_HARVEST.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.harvestDiseaseID, false)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.harvestDiseaseAmount, GameUtil.TimeSlice.None)));
			}
			if (this.forcePrefersDarkness)
			{
				stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
				stringBuilder.Append(UI.GAMEOBJECTEFFECTS.REQUIRES_DARKNESS);
			}
			if (this.forceSelfHarvestOnGrown)
			{
				stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
				stringBuilder.Append(UI.UISIDESCREENS.PLANTERSIDESCREEN.AUTO_SELF_HARVEST);
			}
			if (this.ensureIrrigationInfo.tag.IsValid)
			{
				stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
				stringBuilder.Append(string.Format(UI.GAMEOBJECTEFFECTS.IDEAL_FERTILIZER, this.ensureIrrigationInfo.tag.ProperName(), GameUtil.GetFormattedMass(-this.ensureIrrigationInfo.massConsumptionRate, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), true));
			}
			if (!this.originalMutation)
			{
				stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
				stringBuilder.Append(UI.GAMEOBJECTEFFECTS.MUTANT_STERILE);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06007997 RID: 31127 RVA: 0x00300B98 File Offset: 0x002FED98
		public void GetDescriptors(ref List<Descriptor> descriptors, GameObject go)
		{
			if (this.harvestDiseaseID != 255)
			{
				descriptors.Add(new Descriptor(UI.UISIDESCREENS.PLANTERSIDESCREEN.DISEASE_ON_HARVEST.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.harvestDiseaseID, false)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.harvestDiseaseAmount, GameUtil.TimeSlice.None)), UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.DISEASE_ON_HARVEST.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.harvestDiseaseID, false)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.harvestDiseaseAmount, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect, false));
			}
			if (this.forceSelfHarvestOnGrown)
			{
				descriptors.Add(new Descriptor(UI.UISIDESCREENS.PLANTERSIDESCREEN.AUTO_SELF_HARVEST, UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.AUTO_SELF_HARVEST, Descriptor.DescriptorType.Effect, false));
			}
		}

		// Token: 0x06007998 RID: 31128 RVA: 0x00300C4C File Offset: 0x002FEE4C
		public PlantMutation Original()
		{
			this.originalMutation = true;
			return this;
		}

		// Token: 0x06007999 RID: 31129 RVA: 0x00300C56 File Offset: 0x002FEE56
		public PlantMutation RequiredPrefabID(string requiredID)
		{
			this.requiredPrefabIDs.Add(requiredID);
			return this;
		}

		// Token: 0x0600799A RID: 31130 RVA: 0x00300C65 File Offset: 0x002FEE65
		public PlantMutation RestrictPrefabID(string restrictedID)
		{
			this.restrictedPrefabIDs.Add(restrictedID);
			return this;
		}

		// Token: 0x0600799B RID: 31131 RVA: 0x00300C74 File Offset: 0x002FEE74
		public PlantMutation AttributeModifier(Attribute attribute, float amount, bool multiplier = false)
		{
			DebugUtil.Assert(!this.forcePrefersDarkness || attribute != Db.Get().PlantAttributes.MinLightLux, "A plant mutation has both darkness and light defined!", this.Id);
			base.Add(new AttributeModifier(attribute.Id, amount, this.Name, multiplier, false, true));
			return this;
		}

		// Token: 0x0600799C RID: 31132 RVA: 0x00300CCD File Offset: 0x002FEECD
		public PlantMutation BonusCrop(Tag cropPrefabID, float bonucCropAmount)
		{
			this.bonusCropID = cropPrefabID;
			this.bonusCropAmount = bonucCropAmount;
			return this;
		}

		// Token: 0x0600799D RID: 31133 RVA: 0x00300CDE File Offset: 0x002FEEDE
		public PlantMutation DiseaseDropper(byte diseaseID, int onGrowAmount, int continuousAmount)
		{
			this.droppedDiseaseID = diseaseID;
			this.droppedDiseaseOnGrowAmount = onGrowAmount;
			this.droppedDiseaseContinuousAmount = continuousAmount;
			return this;
		}

		// Token: 0x0600799E RID: 31134 RVA: 0x00300CF6 File Offset: 0x002FEEF6
		public PlantMutation AddDiseaseToHarvest(byte diseaseID, int amount)
		{
			this.harvestDiseaseID = diseaseID;
			this.harvestDiseaseAmount = amount;
			return this;
		}

		// Token: 0x0600799F RID: 31135 RVA: 0x00300D08 File Offset: 0x002FEF08
		public PlantMutation ForcePrefersDarkness()
		{
			DebugUtil.Assert(this.SelfModifiers.Find((AttributeModifier m) => m.AttributeId == Db.Get().PlantAttributes.MinLightLux.Id) == null, "A plant mutation has both darkness and light defined!", this.Id);
			this.forcePrefersDarkness = true;
			return this;
		}

		// Token: 0x060079A0 RID: 31136 RVA: 0x00300D5A File Offset: 0x002FEF5A
		public PlantMutation ForceSelfHarvestOnGrown()
		{
			this.forceSelfHarvestOnGrown = true;
			this.AttributeModifier(Db.Get().Amounts.OldAge.maxAttribute, -0.999999f, true);
			return this;
		}

		// Token: 0x060079A1 RID: 31137 RVA: 0x00300D85 File Offset: 0x002FEF85
		public PlantMutation EnsureIrrigated(PlantElementAbsorber.ConsumeInfo consumeInfo)
		{
			this.ensureIrrigationInfo = consumeInfo;
			return this;
		}

		// Token: 0x060079A2 RID: 31138 RVA: 0x00300D90 File Offset: 0x002FEF90
		public PlantMutation VisualTint(float r, float g, float b)
		{
			global::Debug.Assert(Mathf.Sign(r) == Mathf.Sign(g) && Mathf.Sign(r) == Mathf.Sign(b), "Vales for tints must be all positive or all negative for the shader to work correctly!");
			if (r < 0f)
			{
				this.plantTint = Color.white + new Color(r, g, b, 0f);
			}
			else
			{
				this.plantTint = new Color(r, g, b, 0f);
			}
			return this;
		}

		// Token: 0x060079A3 RID: 31139 RVA: 0x00300E04 File Offset: 0x002FF004
		public PlantMutation VisualSymbolTint(string targetSymbolName, float r, float g, float b)
		{
			global::Debug.Assert(Mathf.Sign(r) == Mathf.Sign(g) && Mathf.Sign(r) == Mathf.Sign(b), "Vales for tints must be all positive or all negative for the shader to work correctly!");
			this.symbolTintTargets.Add(targetSymbolName);
			this.symbolTints.Add(Color.white + new Color(r, g, b, 0f));
			return this;
		}

		// Token: 0x060079A4 RID: 31140 RVA: 0x00300E6B File Offset: 0x002FF06B
		public PlantMutation VisualSymbolOverride(string targetSymbolName, string sourceAnim, string sourceSymbol)
		{
			if (this.symbolOverrideInfo == null)
			{
				this.symbolOverrideInfo = new List<PlantMutation.SymbolOverrideInfo>();
			}
			this.symbolOverrideInfo.Add(new PlantMutation.SymbolOverrideInfo
			{
				targetSymbolName = targetSymbolName,
				sourceAnim = sourceAnim,
				sourceSymbol = sourceSymbol
			});
			return this;
		}

		// Token: 0x060079A5 RID: 31141 RVA: 0x00300EA6 File Offset: 0x002FF0A6
		public PlantMutation VisualSymbolScale(string targetSymbolName, float scale)
		{
			this.symbolScaleTargets.Add(targetSymbolName);
			this.symbolScales.Add(scale);
			return this;
		}

		// Token: 0x060079A6 RID: 31142 RVA: 0x00300EC1 File Offset: 0x002FF0C1
		public PlantMutation VisualBGFX(string animName)
		{
			this.bGFXAnim = animName;
			return this;
		}

		// Token: 0x060079A7 RID: 31143 RVA: 0x00300ECB File Offset: 0x002FF0CB
		public PlantMutation VisualFGFX(string animName)
		{
			this.fGFXAnim = animName;
			return this;
		}

		// Token: 0x060079A8 RID: 31144 RVA: 0x00300ED5 File Offset: 0x002FF0D5
		public PlantMutation AddSoundEvent(string soundEventName)
		{
			this.additionalSoundEvents.Add(soundEventName);
			return this;
		}

		// Token: 0x04005AAD RID: 23213
		public string desc;

		// Token: 0x04005AAE RID: 23214
		public string animationSoundEvent;

		// Token: 0x04005AAF RID: 23215
		public bool originalMutation;

		// Token: 0x04005AB0 RID: 23216
		public List<string> requiredPrefabIDs = new List<string>();

		// Token: 0x04005AB1 RID: 23217
		public List<string> restrictedPrefabIDs = new List<string>();

		// Token: 0x04005AB2 RID: 23218
		private Tag bonusCropID;

		// Token: 0x04005AB3 RID: 23219
		private float bonusCropAmount;

		// Token: 0x04005AB4 RID: 23220
		private byte droppedDiseaseID = byte.MaxValue;

		// Token: 0x04005AB5 RID: 23221
		private int droppedDiseaseOnGrowAmount;

		// Token: 0x04005AB6 RID: 23222
		private int droppedDiseaseContinuousAmount;

		// Token: 0x04005AB7 RID: 23223
		private byte harvestDiseaseID = byte.MaxValue;

		// Token: 0x04005AB8 RID: 23224
		private int harvestDiseaseAmount;

		// Token: 0x04005AB9 RID: 23225
		private bool forcePrefersDarkness;

		// Token: 0x04005ABA RID: 23226
		private bool forceSelfHarvestOnGrown;

		// Token: 0x04005ABB RID: 23227
		private PlantElementAbsorber.ConsumeInfo ensureIrrigationInfo;

		// Token: 0x04005ABC RID: 23228
		private Color plantTint = Color.white;

		// Token: 0x04005ABD RID: 23229
		private List<string> symbolTintTargets = new List<string>();

		// Token: 0x04005ABE RID: 23230
		private List<Color> symbolTints = new List<Color>();

		// Token: 0x04005ABF RID: 23231
		private List<PlantMutation.SymbolOverrideInfo> symbolOverrideInfo;

		// Token: 0x04005AC0 RID: 23232
		private List<string> symbolScaleTargets = new List<string>();

		// Token: 0x04005AC1 RID: 23233
		private List<float> symbolScales = new List<float>();

		// Token: 0x04005AC2 RID: 23234
		private string bGFXAnim;

		// Token: 0x04005AC3 RID: 23235
		private string fGFXAnim;

		// Token: 0x04005AC4 RID: 23236
		private List<string> additionalSoundEvents = new List<string>();

		// Token: 0x0200236F RID: 9071
		private class SymbolOverrideInfo
		{
			// Token: 0x04009ED4 RID: 40660
			public string targetSymbolName;

			// Token: 0x04009ED5 RID: 40661
			public string sourceAnim;

			// Token: 0x04009ED6 RID: 40662
			public string sourceSymbol;
		}
	}
}
