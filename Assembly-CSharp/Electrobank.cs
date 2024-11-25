using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000870 RID: 2160
public class Electrobank : KMonoBehaviour, ISim1000ms, IConsumableUIItem, IGameObjectEffectDescriptor
{
	// Token: 0x17000443 RID: 1091
	// (get) Token: 0x06003C2D RID: 15405 RVA: 0x0014E019 File Offset: 0x0014C219
	// (set) Token: 0x06003C2C RID: 15404 RVA: 0x0014E010 File Offset: 0x0014C210
	public string ID { get; private set; }

	// Token: 0x17000444 RID: 1092
	// (get) Token: 0x06003C2E RID: 15406 RVA: 0x0014E021 File Offset: 0x0014C221
	public bool IsFullyCharged
	{
		get
		{
			return this.charge == Electrobank.capacity;
		}
	}

	// Token: 0x17000445 RID: 1093
	// (get) Token: 0x06003C2F RID: 15407 RVA: 0x0014E030 File Offset: 0x0014C230
	public float Charge
	{
		get
		{
			return this.charge;
		}
	}

	// Token: 0x06003C30 RID: 15408 RVA: 0x0014E038 File Offset: 0x0014C238
	protected override void OnPrefabInit()
	{
		this.ID = base.gameObject.PrefabID().ToString();
		base.Subscribe(748399584, new Action<object>(this.OnCraft));
		base.OnPrefabInit();
	}

	// Token: 0x06003C31 RID: 15409 RVA: 0x0014E082 File Offset: 0x0014C282
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06003C32 RID: 15410 RVA: 0x0014E08A File Offset: 0x0014C28A
	private void OnCraft(object data)
	{
		WorldResourceAmountTracker<ElectrobankTracker>.Get().RegisterAmountProduced(this.Charge);
	}

	// Token: 0x06003C33 RID: 15411 RVA: 0x0014E09C File Offset: 0x0014C29C
	public static GameObject ReplaceEmptyWithCharged(GameObject EmptyElectrobank, bool dropFromStorage = false)
	{
		Vector3 position = EmptyElectrobank.transform.GetPosition();
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("Electrobank"), position);
		gameObject.GetComponent<PrimaryElement>().SetElement(EmptyElectrobank.GetComponent<PrimaryElement>().Element.id, true);
		gameObject.SetActive(true);
		Storage storage = EmptyElectrobank.GetComponent<Pickupable>().storage;
		EmptyElectrobank.DeleteObject();
		if (storage != null && !dropFromStorage)
		{
			storage.Store(gameObject, false, false, true, false);
		}
		return gameObject;
	}

	// Token: 0x06003C34 RID: 15412 RVA: 0x0014E11C File Offset: 0x0014C31C
	public static GameObject ReplaceChargedWithEmpty(GameObject ChargedElectrobank, bool dropFromStorage = false)
	{
		Vector3 position = ChargedElectrobank.transform.GetPosition();
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("EmptyElectrobank"), position);
		gameObject.GetComponent<PrimaryElement>().SetElement(ChargedElectrobank.GetComponent<PrimaryElement>().Element.id, true);
		gameObject.SetActive(true);
		Storage storage = ChargedElectrobank.GetComponent<Pickupable>().storage;
		ChargedElectrobank.DeleteObject();
		if (storage != null && !dropFromStorage)
		{
			storage.Store(gameObject, false, false, true, false);
		}
		return gameObject;
	}

	// Token: 0x06003C35 RID: 15413 RVA: 0x0014E19C File Offset: 0x0014C39C
	public static GameObject ReplaceEmptyWithGarbage(GameObject ChargedElectrobank, bool dropFromStorage = false)
	{
		Vector3 position = ChargedElectrobank.transform.GetPosition();
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("GarbageElectrobank"), position);
		gameObject.GetComponent<PrimaryElement>().SetElement(ChargedElectrobank.GetComponent<PrimaryElement>().Element.id, true);
		gameObject.SetActive(true);
		Storage storage = ChargedElectrobank.GetComponent<Pickupable>().storage;
		ChargedElectrobank.DeleteObject();
		if (storage != null && !dropFromStorage)
		{
			storage.Store(gameObject, false, false, true, false);
		}
		return gameObject;
	}

	// Token: 0x06003C36 RID: 15414 RVA: 0x0014E219 File Offset: 0x0014C419
	public void AddPower(float joules)
	{
		this.charge = Mathf.Clamp(this.charge + joules, 0f, Electrobank.capacity);
	}

	// Token: 0x06003C37 RID: 15415 RVA: 0x0014E238 File Offset: 0x0014C438
	public float RemovePower(float joules, bool dropWhenEmpty)
	{
		float num = Mathf.Min(this.charge, joules);
		this.charge -= num;
		if (this.charge <= 0f)
		{
			if (this.rechargeable)
			{
				Electrobank.ReplaceChargedWithEmpty(base.gameObject, dropWhenEmpty);
			}
			else
			{
				Util.KDestroyGameObject(base.gameObject);
			}
		}
		return num;
	}

	// Token: 0x06003C38 RID: 15416 RVA: 0x0014E290 File Offset: 0x0014C490
	public void FullyCharge()
	{
		this.charge = Electrobank.capacity;
	}

	// Token: 0x06003C39 RID: 15417 RVA: 0x0014E2A0 File Offset: 0x0014C4A0
	public void Explode()
	{
		int num = Grid.PosToCell(base.gameObject.transform.position);
		float num2 = Grid.Temperature[num];
		num2 += this.charge / (Grid.Mass[num] * Grid.Element[num].specificHeatCapacity);
		num2 = Mathf.Clamp(num2, 1f, 9999f);
		SimMessages.ReplaceElement(num, Grid.Element[num].id, CellEventLogger.Instance.SandBoxTool, Grid.Mass[num], num2, Grid.DiseaseIdx[num], Grid.DiseaseCount[num], -1);
		Game.Instance.SpawnFX(SpawnFXHashes.MeteorImpactMetal, base.gameObject.transform.position, 0f);
		KFMOD.PlayOneShot(GlobalAssets.GetSound("Battery_explode", false), base.gameObject.transform.position, 1f);
		Electrobank.ReplaceEmptyWithGarbage(base.gameObject, false);
	}

	// Token: 0x06003C3A RID: 15418 RVA: 0x0014E398 File Offset: 0x0014C598
	public void Sim1000ms(float dt)
	{
		if (this.pickupable.KPrefabID.HasTag(GameTags.Stored))
		{
			return;
		}
		if (Grid.IsValidCell(this.pickupable.cachedCell) && Grid.Element[this.pickupable.cachedCell].HasTag(GameTags.AnyWater))
		{
			this.Damage(dt);
		}
	}

	// Token: 0x06003C3B RID: 15419 RVA: 0x0014E3F4 File Offset: 0x0014C5F4
	private void Damage(float amount)
	{
		PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, DUPLICANTS.MODIFIERS.WATERDAMAGE.NAME, base.transform, 1.5f, false);
		Game.Instance.SpawnFX(SpawnFXHashes.BuildingSpark, Grid.PosToCell(base.gameObject), 0f);
		this.health -= amount;
		if (this.health <= 0f)
		{
			this.Explode();
		}
	}

	// Token: 0x06003C3C RID: 15420 RVA: 0x0014E46C File Offset: 0x0014C66C
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELECTROBANKS, GameUtil.GetFormattedJoules(this.Charge, "F1", GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELECTROBANKS, GameUtil.GetFormattedJoules(this.Charge, "F1", GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect);
		list.Add(item);
		return list;
	}

	// Token: 0x17000446 RID: 1094
	// (get) Token: 0x06003C3D RID: 15421 RVA: 0x0014E4D8 File Offset: 0x0014C6D8
	public string ConsumableId
	{
		get
		{
			return this.PrefabID().Name;
		}
	}

	// Token: 0x17000447 RID: 1095
	// (get) Token: 0x06003C3E RID: 15422 RVA: 0x0014E4F3 File Offset: 0x0014C6F3
	public string ConsumableName
	{
		get
		{
			return this.GetProperName();
		}
	}

	// Token: 0x17000448 RID: 1096
	// (get) Token: 0x06003C3F RID: 15423 RVA: 0x0014E4FB File Offset: 0x0014C6FB
	public int MajorOrder
	{
		get
		{
			return 500;
		}
	}

	// Token: 0x17000449 RID: 1097
	// (get) Token: 0x06003C40 RID: 15424 RVA: 0x0014E502 File Offset: 0x0014C702
	public int MinorOrder
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x1700044A RID: 1098
	// (get) Token: 0x06003C41 RID: 15425 RVA: 0x0014E505 File Offset: 0x0014C705
	public bool Display
	{
		get
		{
			return true;
		}
	}

	// Token: 0x04002484 RID: 9348
	private static float capacity = 120000f;

	// Token: 0x04002485 RID: 9349
	[Serialize]
	private float charge = Electrobank.capacity;

	// Token: 0x04002486 RID: 9350
	[Serialize]
	private float health = 10f;

	// Token: 0x04002487 RID: 9351
	public bool rechargeable;

	// Token: 0x04002488 RID: 9352
	[MyCmpGet]
	private Pickupable pickupable;
}
