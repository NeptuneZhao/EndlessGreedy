using System;
using Klei.CustomSettings;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x02000866 RID: 2150
[AddComponentMenu("KMonoBehaviour/scripts/Durability")]
public class Durability : KMonoBehaviour
{
	// Token: 0x17000441 RID: 1089
	// (get) Token: 0x06003BF0 RID: 15344 RVA: 0x0014A218 File Offset: 0x00148418
	// (set) Token: 0x06003BF1 RID: 15345 RVA: 0x0014A220 File Offset: 0x00148420
	public float TimeEquipped
	{
		get
		{
			return this.timeEquipped;
		}
		set
		{
			this.timeEquipped = value;
		}
	}

	// Token: 0x06003BF2 RID: 15346 RVA: 0x0014A229 File Offset: 0x00148429
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Durability>(-1617557748, Durability.OnEquippedDelegate);
		base.Subscribe<Durability>(-170173755, Durability.OnUnequippedDelegate);
	}

	// Token: 0x06003BF3 RID: 15347 RVA: 0x0014A254 File Offset: 0x00148454
	protected override void OnSpawn()
	{
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.Durability, base.gameObject);
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.Durability);
		if (currentQualitySetting != null)
		{
			string id = currentQualitySetting.id;
			if (id == "Indestructible")
			{
				this.difficultySettingMod = EQUIPMENT.SUITS.INDESTRUCTIBLE_DURABILITY_MOD;
				return;
			}
			if (id == "Reinforced")
			{
				this.difficultySettingMod = EQUIPMENT.SUITS.REINFORCED_DURABILITY_MOD;
				return;
			}
			if (id == "Flimsy")
			{
				this.difficultySettingMod = EQUIPMENT.SUITS.FLIMSY_DURABILITY_MOD;
				return;
			}
			if (!(id == "Threadbare"))
			{
				return;
			}
			this.difficultySettingMod = EQUIPMENT.SUITS.THREADBARE_DURABILITY_MOD;
		}
	}

	// Token: 0x06003BF4 RID: 15348 RVA: 0x0014A300 File Offset: 0x00148500
	private void OnEquipped()
	{
		if (!this.isEquipped)
		{
			this.isEquipped = true;
			this.timeEquipped = GameClock.Instance.GetTimeInCycles();
		}
	}

	// Token: 0x06003BF5 RID: 15349 RVA: 0x0014A324 File Offset: 0x00148524
	private void OnUnequipped()
	{
		if (this.isEquipped)
		{
			this.isEquipped = false;
			float num = GameClock.Instance.GetTimeInCycles() - this.timeEquipped;
			this.DeltaDurability(num * this.durabilityLossPerCycle);
		}
	}

	// Token: 0x06003BF6 RID: 15350 RVA: 0x0014A360 File Offset: 0x00148560
	private void DeltaDurability(float delta)
	{
		delta *= this.difficultySettingMod;
		this.durability = Mathf.Clamp01(this.durability + delta);
	}

	// Token: 0x06003BF7 RID: 15351 RVA: 0x0014A380 File Offset: 0x00148580
	public void ConvertToWornObject()
	{
		GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(this.wornEquipmentPrefabID), Grid.SceneLayer.Ore, null, 0);
		gameObject.transform.SetPosition(base.transform.GetPosition());
		gameObject.GetComponent<PrimaryElement>().SetElement(base.GetComponent<PrimaryElement>().ElementID, false);
		gameObject.SetActive(true);
		EquippableFacade component = base.GetComponent<EquippableFacade>();
		if (component != null)
		{
			gameObject.GetComponent<RepairableEquipment>().facadeID = component.FacadeID;
		}
		Storage component2 = base.gameObject.GetComponent<Storage>();
		if (component2)
		{
			JetSuitTank component3 = base.gameObject.GetComponent<JetSuitTank>();
			if (component3)
			{
				component2.AddLiquid(SimHashes.Petroleum, component3.amount, base.GetComponent<PrimaryElement>().Temperature, byte.MaxValue, 0, false, true);
			}
			component2.DropAll(false, false, default(Vector3), true, null);
		}
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x06003BF8 RID: 15352 RVA: 0x0014A46C File Offset: 0x0014866C
	public float GetDurability()
	{
		if (this.isEquipped)
		{
			float num = GameClock.Instance.GetTimeInCycles() - this.timeEquipped;
			return this.durability - num * this.durabilityLossPerCycle;
		}
		return this.durability;
	}

	// Token: 0x06003BF9 RID: 15353 RVA: 0x0014A4A9 File Offset: 0x001486A9
	public bool IsWornOut()
	{
		return this.GetDurability() <= 0f;
	}

	// Token: 0x0400243F RID: 9279
	private static readonly EventSystem.IntraObjectHandler<Durability> OnEquippedDelegate = new EventSystem.IntraObjectHandler<Durability>(delegate(Durability component, object data)
	{
		component.OnEquipped();
	});

	// Token: 0x04002440 RID: 9280
	private static readonly EventSystem.IntraObjectHandler<Durability> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<Durability>(delegate(Durability component, object data)
	{
		component.OnUnequipped();
	});

	// Token: 0x04002441 RID: 9281
	[Serialize]
	private bool isEquipped;

	// Token: 0x04002442 RID: 9282
	[Serialize]
	private float timeEquipped;

	// Token: 0x04002443 RID: 9283
	[Serialize]
	private float durability = 1f;

	// Token: 0x04002444 RID: 9284
	public float durabilityLossPerCycle = -0.1f;

	// Token: 0x04002445 RID: 9285
	public string wornEquipmentPrefabID;

	// Token: 0x04002446 RID: 9286
	private float difficultySettingMod = 1f;
}
