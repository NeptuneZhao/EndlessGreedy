using System;
using System.Collections.Generic;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

namespace FoodRehydrator
{
	// Token: 0x02000E2C RID: 3628
	public class DehydratedManager : KMonoBehaviour, FewOptionSideScreen.IFewOptionSideScreen
	{
		// Token: 0x060073C3 RID: 29635 RVA: 0x002C48AC File Offset: 0x002C2AAC
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			base.Subscribe<DehydratedManager>(-905833192, DehydratedManager.OnCopySettingsDelegate);
		}

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x060073C4 RID: 29636 RVA: 0x002C48C5 File Offset: 0x002C2AC5
		// (set) Token: 0x060073C5 RID: 29637 RVA: 0x002C48D0 File Offset: 0x002C2AD0
		public Tag ChosenContent
		{
			get
			{
				return this.chosenContent;
			}
			set
			{
				if (this.chosenContent != value)
				{
					base.GetComponent<ManualDeliveryKG>().RequestedItemTag = value;
					this.chosenContent = value;
					this.packages.DropUnlessHasTag(this.chosenContent);
					if (this.chosenContent != GameTags.Dehydrated)
					{
						AccessabilityManager component = base.GetComponent<AccessabilityManager>();
						if (component != null)
						{
							component.CancelActiveWorkable();
						}
					}
				}
			}
		}

		// Token: 0x060073C6 RID: 29638 RVA: 0x002C4938 File Offset: 0x002C2B38
		public void SetFabricatedFoodSymbol(Tag material)
		{
			this.foodKBAC.gameObject.SetActive(true);
			GameObject prefab = Assets.GetPrefab(material);
			this.foodKBAC.SwapAnims(prefab.GetComponent<KBatchedAnimController>().AnimFiles);
			this.foodKBAC.Play("object", KAnim.PlayMode.Loop, 1f, 0f);
		}

		// Token: 0x060073C7 RID: 29639 RVA: 0x002C4994 File Offset: 0x002C2B94
		protected override void OnSpawn()
		{
			base.OnSpawn();
			Storage[] components = base.GetComponents<Storage>();
			global::Debug.Assert(components.Length == 2);
			this.packages = components[0];
			this.water = components[1];
			this.packagesMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, new string[]
			{
				"meter_target"
			});
			base.Subscribe(-1697596308, new Action<object>(this.StorageChangeHandler));
			this.SetupFoodSymbol();
			this.packagesMeter.SetPositionPercent((float)this.packages.items.Count / 5f);
		}

		// Token: 0x060073C8 RID: 29640 RVA: 0x002C4A3C File Offset: 0x002C2C3C
		public void ConsumeResourcesForRehydration(GameObject package, GameObject food)
		{
			global::Debug.Assert(this.packages.items.Contains(package));
			this.packages.ConsumeIgnoringDisease(package);
			float num;
			SimUtil.DiseaseInfo diseaseInfo;
			float num2;
			this.water.ConsumeAndGetDisease(FoodRehydratorConfig.REHYDRATION_TAG, 1f, out num, out diseaseInfo, out num2);
			PrimaryElement component = food.GetComponent<PrimaryElement>();
			if (component != null)
			{
				component.AddDisease(diseaseInfo.idx, diseaseInfo.count, "rehydrating");
				component.SetMassTemperature(component.Mass, component.Temperature * 0.125f + num2 * 0.875f);
			}
		}

		// Token: 0x060073C9 RID: 29641 RVA: 0x002C4ACD File Offset: 0x002C2CCD
		private void StorageChangeHandler(object obj)
		{
			if (((GameObject)obj).GetComponent<DehydratedFoodPackage>() != null)
			{
				this.packagesMeter.SetPositionPercent((float)this.packages.items.Count / 5f);
			}
		}

		// Token: 0x060073CA RID: 29642 RVA: 0x002C4B04 File Offset: 0x002C2D04
		private void SetupFoodSymbol()
		{
			GameObject gameObject = Util.NewGameObject(base.gameObject, "food_symbol");
			gameObject.SetActive(false);
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			bool flag;
			Vector3 position = component.GetSymbolTransform(DehydratedManager.HASH_FOOD, out flag).GetColumn(3);
			position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
			gameObject.transform.SetPosition(position);
			this.foodKBAC = gameObject.AddComponent<KBatchedAnimController>();
			this.foodKBAC.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim("mushbar_kanim")
			};
			this.foodKBAC.initialAnim = "object";
			component.SetSymbolVisiblity(DehydratedManager.HASH_FOOD, false);
			this.foodKBAC.sceneLayer = Grid.SceneLayer.BuildingUse;
			KBatchedAnimTracker kbatchedAnimTracker = gameObject.AddComponent<KBatchedAnimTracker>();
			kbatchedAnimTracker.symbol = new HashedString("food");
			kbatchedAnimTracker.offset = Vector3.zero;
		}

		// Token: 0x060073CB RID: 29643 RVA: 0x002C4BEC File Offset: 0x002C2DEC
		public FewOptionSideScreen.IFewOptionSideScreen.Option[] GetOptions()
		{
			HashSet<Tag> discoveredResourcesFromTag = DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(GameTags.Dehydrated);
			FewOptionSideScreen.IFewOptionSideScreen.Option[] array = new FewOptionSideScreen.IFewOptionSideScreen.Option[1 + discoveredResourcesFromTag.Count];
			array[0] = new FewOptionSideScreen.IFewOptionSideScreen.Option(GameTags.Dehydrated, UI.UISIDESCREENS.FILTERSIDESCREEN.DRIEDFOOD, Def.GetUISprite("icon_category_food", "ui", false), "");
			int num = 1;
			foreach (Tag tag in discoveredResourcesFromTag)
			{
				array[num] = new FewOptionSideScreen.IFewOptionSideScreen.Option(tag, tag.ProperName(), Def.GetUISprite(tag, "ui", false), "");
				num++;
			}
			return array;
		}

		// Token: 0x060073CC RID: 29644 RVA: 0x002C4CB8 File Offset: 0x002C2EB8
		public void OnOptionSelected(FewOptionSideScreen.IFewOptionSideScreen.Option option)
		{
			this.ChosenContent = option.tag;
		}

		// Token: 0x060073CD RID: 29645 RVA: 0x002C4CC6 File Offset: 0x002C2EC6
		public Tag GetSelectedOption()
		{
			return this.chosenContent;
		}

		// Token: 0x060073CE RID: 29646 RVA: 0x002C4CD0 File Offset: 0x002C2ED0
		protected void OnCopySettings(object data)
		{
			GameObject gameObject = data as GameObject;
			if (gameObject != null)
			{
				DehydratedManager component = gameObject.GetComponent<DehydratedManager>();
				if (component != null)
				{
					this.ChosenContent = component.ChosenContent;
				}
			}
		}

		// Token: 0x04004FB1 RID: 20401
		[MyCmpAdd]
		private CopyBuildingSettings copyBuildingSettings;

		// Token: 0x04004FB2 RID: 20402
		private Storage packages;

		// Token: 0x04004FB3 RID: 20403
		private Storage water;

		// Token: 0x04004FB4 RID: 20404
		private MeterController packagesMeter;

		// Token: 0x04004FB5 RID: 20405
		private static string HASH_FOOD = "food";

		// Token: 0x04004FB6 RID: 20406
		private KBatchedAnimController foodKBAC;

		// Token: 0x04004FB7 RID: 20407
		private static readonly EventSystem.IntraObjectHandler<DehydratedManager> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<DehydratedManager>(delegate(DehydratedManager component, object data)
		{
			component.OnCopySettings(data);
		});

		// Token: 0x04004FB8 RID: 20408
		[Serialize]
		private Tag chosenContent = GameTags.Dehydrated;
	}
}
