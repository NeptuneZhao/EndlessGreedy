using System;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020007B7 RID: 1975
[AddComponentMenu("KMonoBehaviour/scripts/ClothingWearer")]
public class ClothingWearer : KMonoBehaviour
{
	// Token: 0x0600364F RID: 13903 RVA: 0x00127828 File Offset: 0x00125A28
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.decorProvider = base.GetComponent<DecorProvider>();
		if (this.decorModifier == null)
		{
			this.decorModifier = new AttributeModifier("Decor", 0f, DUPLICANTS.MODIFIERS.CLOTHING.NAME, false, false, false);
		}
		if (this.conductivityModifier == null)
		{
			AttributeInstance attributeInstance = base.gameObject.GetAttributes().Get("ThermalConductivityBarrier");
			this.conductivityModifier = new AttributeModifier("ThermalConductivityBarrier", ClothingWearer.ClothingInfo.BASIC_CLOTHING.conductivityMod, DUPLICANTS.MODIFIERS.CLOTHING.NAME, false, false, false);
			attributeInstance.Add(this.conductivityModifier);
		}
	}

	// Token: 0x06003650 RID: 13904 RVA: 0x001278C0 File Offset: 0x00125AC0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.decorProvider.decor.Add(this.decorModifier);
		this.decorProvider.decorRadius.Add(new AttributeModifier(Db.Get().BuildingAttributes.DecorRadius.Id, 3f, null, false, false, true));
		Traits component = base.GetComponent<Traits>();
		string format = UI.OVERLAYS.DECOR.CLOTHING;
		if (component != null)
		{
			if (component.HasTrait("DecorUp"))
			{
				format = UI.OVERLAYS.DECOR.CLOTHING_TRAIT_DECORUP;
			}
			else if (component.HasTrait("DecorDown"))
			{
				format = UI.OVERLAYS.DECOR.CLOTHING_TRAIT_DECORDOWN;
			}
		}
		this.decorProvider.overrideName = string.Format(format, base.gameObject.GetProperName());
		if (this.currentClothing == null)
		{
			this.ChangeToDefaultClothes();
		}
		else
		{
			this.ChangeClothes(this.currentClothing);
		}
		this.spawnApplyClothesHandle = GameScheduler.Instance.Schedule("ApplySpawnClothes", 2f, delegate(object obj)
		{
			base.GetComponent<CreatureSimTemperatureTransfer>().RefreshRegistration();
		}, null, null);
	}

	// Token: 0x06003651 RID: 13905 RVA: 0x001279C8 File Offset: 0x00125BC8
	protected override void OnCleanUp()
	{
		this.spawnApplyClothesHandle.ClearScheduler();
		base.OnCleanUp();
	}

	// Token: 0x06003652 RID: 13906 RVA: 0x001279DC File Offset: 0x00125BDC
	public void ChangeClothes(ClothingWearer.ClothingInfo clothingInfo)
	{
		this.decorProvider.baseRadius = 3f;
		this.currentClothing = clothingInfo;
		this.conductivityModifier.Description = clothingInfo.name;
		this.conductivityModifier.SetValue(this.currentClothing.conductivityMod);
		this.decorModifier.SetValue((float)this.currentClothing.decorMod);
	}

	// Token: 0x06003653 RID: 13907 RVA: 0x00127A3E File Offset: 0x00125C3E
	public void ChangeToDefaultClothes()
	{
		this.ChangeClothes(new ClothingWearer.ClothingInfo(ClothingWearer.ClothingInfo.BASIC_CLOTHING.name, ClothingWearer.ClothingInfo.BASIC_CLOTHING.decorMod, ClothingWearer.ClothingInfo.BASIC_CLOTHING.conductivityMod, ClothingWearer.ClothingInfo.BASIC_CLOTHING.homeostasisEfficiencyMultiplier));
	}

	// Token: 0x04002036 RID: 8246
	private DecorProvider decorProvider;

	// Token: 0x04002037 RID: 8247
	private SchedulerHandle spawnApplyClothesHandle;

	// Token: 0x04002038 RID: 8248
	private AttributeModifier decorModifier;

	// Token: 0x04002039 RID: 8249
	private AttributeModifier conductivityModifier;

	// Token: 0x0400203A RID: 8250
	[Serialize]
	public ClothingWearer.ClothingInfo currentClothing;

	// Token: 0x02001677 RID: 5751
	public class ClothingInfo
	{
		// Token: 0x06009269 RID: 37481 RVA: 0x00353C7F File Offset: 0x00351E7F
		public ClothingInfo(string _name, int _decor, float _temperature, float _homeostasisEfficiencyMultiplier)
		{
			this.name = _name;
			this.decorMod = _decor;
			this.conductivityMod = _temperature;
			this.homeostasisEfficiencyMultiplier = _homeostasisEfficiencyMultiplier;
		}

		// Token: 0x0600926A RID: 37482 RVA: 0x00353CB0 File Offset: 0x00351EB0
		public static void OnEquipVest(Equippable eq, ClothingWearer.ClothingInfo clothingInfo)
		{
			if (eq == null || eq.assignee == null)
			{
				return;
			}
			Ownables soleOwner = eq.assignee.GetSoleOwner();
			if (soleOwner == null)
			{
				return;
			}
			ClothingWearer component = (soleOwner.GetComponent<MinionAssignablesProxy>().target as KMonoBehaviour).GetComponent<ClothingWearer>();
			if (component != null)
			{
				component.ChangeClothes(clothingInfo);
				return;
			}
			global::Debug.LogWarning("Clothing item cannot be equipped to assignee because they lack ClothingWearer component");
		}

		// Token: 0x0600926B RID: 37483 RVA: 0x00353D18 File Offset: 0x00351F18
		public static void OnUnequipVest(Equippable eq)
		{
			if (eq != null && eq.assignee != null)
			{
				Ownables soleOwner = eq.assignee.GetSoleOwner();
				if (soleOwner == null)
				{
					return;
				}
				MinionAssignablesProxy component = soleOwner.GetComponent<MinionAssignablesProxy>();
				if (component == null)
				{
					return;
				}
				GameObject targetGameObject = component.GetTargetGameObject();
				if (targetGameObject == null)
				{
					return;
				}
				ClothingWearer component2 = targetGameObject.GetComponent<ClothingWearer>();
				if (component2 == null)
				{
					return;
				}
				component2.ChangeToDefaultClothes();
			}
		}

		// Token: 0x0600926C RID: 37484 RVA: 0x00353D88 File Offset: 0x00351F88
		public static void SetupVest(GameObject go)
		{
			go.GetComponent<KPrefabID>().AddTag(GameTags.Clothes, false);
			Equippable equippable = go.GetComponent<Equippable>();
			if (equippable == null)
			{
				equippable = go.AddComponent<Equippable>();
			}
			equippable.SetQuality(global::QualityLevel.Poor);
			go.GetComponent<KBatchedAnimController>().sceneLayer = Grid.SceneLayer.BuildingBack;
		}

		// Token: 0x04006FBF RID: 28607
		[Serialize]
		public string name = "";

		// Token: 0x04006FC0 RID: 28608
		[Serialize]
		public int decorMod;

		// Token: 0x04006FC1 RID: 28609
		[Serialize]
		public float conductivityMod;

		// Token: 0x04006FC2 RID: 28610
		[Serialize]
		public float homeostasisEfficiencyMultiplier;

		// Token: 0x04006FC3 RID: 28611
		public static readonly ClothingWearer.ClothingInfo BASIC_CLOTHING = new ClothingWearer.ClothingInfo(EQUIPMENT.PREFABS.COOL_VEST.GENERICNAME, -5, 0.0025f, -1.25f);

		// Token: 0x04006FC4 RID: 28612
		public static readonly ClothingWearer.ClothingInfo WARM_CLOTHING = new ClothingWearer.ClothingInfo(EQUIPMENT.PREFABS.WARM_VEST.NAME, 0, 0.008f, -1.25f);

		// Token: 0x04006FC5 RID: 28613
		public static readonly ClothingWearer.ClothingInfo COOL_CLOTHING = new ClothingWearer.ClothingInfo(EQUIPMENT.PREFABS.COOL_VEST.NAME, -10, 0.0005f, 0f);

		// Token: 0x04006FC6 RID: 28614
		public static readonly ClothingWearer.ClothingInfo FANCY_CLOTHING = new ClothingWearer.ClothingInfo(EQUIPMENT.PREFABS.FUNKY_VEST.NAME, 30, 0.0025f, -1.25f);

		// Token: 0x04006FC7 RID: 28615
		public static readonly ClothingWearer.ClothingInfo CUSTOM_CLOTHING = new ClothingWearer.ClothingInfo(EQUIPMENT.PREFABS.CUSTOMCLOTHING.NAME, 40, 0.0025f, -1.25f);

		// Token: 0x04006FC8 RID: 28616
		public static readonly ClothingWearer.ClothingInfo SLEEP_CLINIC_PAJAMAS = new ClothingWearer.ClothingInfo(EQUIPMENT.PREFABS.CUSTOMCLOTHING.NAME, 40, 0.0025f, -1.25f);
	}
}
