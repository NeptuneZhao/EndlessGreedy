﻿using System;
using Klei;
using UnityEngine;

// Token: 0x02000563 RID: 1379
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/EntitySplitter")]
public class EntitySplitter : KMonoBehaviour
{
	// Token: 0x06001FF3 RID: 8179 RVA: 0x000B3CD3 File Offset: 0x000B1ED3
	protected static Pickupable OnTakeBehavior(Pickupable p, float a)
	{
		return EntitySplitter.Split(p, a, null);
	}

	// Token: 0x06001FF4 RID: 8180 RVA: 0x000B3CE0 File Offset: 0x000B1EE0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Pickupable pickupable = base.GetComponent<Pickupable>();
		if (pickupable == null)
		{
			global::Debug.LogError(base.name + " does not have a pickupable component!");
		}
		Pickupable pickupable2 = pickupable;
		pickupable2.OnTake = (Func<Pickupable, float, Pickupable>)Delegate.Combine(pickupable2.OnTake, new Func<Pickupable, float, Pickupable>(EntitySplitter.OnTakeBehavior));
		Rottable.Instance rottable = base.gameObject.GetSMI<Rottable.Instance>();
		pickupable.absorbable = true;
		pickupable.CanAbsorb = ((Pickupable other) => EntitySplitter.CanFirstAbsorbSecond(pickupable, rottable, other, this.maxStackSize));
		base.Subscribe<EntitySplitter>(-2064133523, EntitySplitter.OnAbsorbDelegate);
	}

	// Token: 0x06001FF5 RID: 8181 RVA: 0x000B3D9C File Offset: 0x000B1F9C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Pickupable component = base.GetComponent<Pickupable>();
		if (component != null)
		{
			Pickupable pickupable = component;
			pickupable.OnTake = (Func<Pickupable, float, Pickupable>)Delegate.Remove(pickupable.OnTake, new Func<Pickupable, float, Pickupable>(EntitySplitter.OnTakeBehavior));
		}
	}

	// Token: 0x06001FF6 RID: 8182 RVA: 0x000B3DE4 File Offset: 0x000B1FE4
	public static bool CanFirstAbsorbSecond(Pickupable pickupable, Rottable.Instance rottable, Pickupable other, float maxStackSize)
	{
		if (other == null)
		{
			return false;
		}
		KPrefabID kprefabID = pickupable.KPrefabID;
		KPrefabID kprefabID2 = other.KPrefabID;
		if (kprefabID == null)
		{
			return false;
		}
		if (kprefabID2 == null)
		{
			return false;
		}
		if (kprefabID.PrefabTag != kprefabID2.PrefabTag)
		{
			return false;
		}
		if (pickupable.TotalAmount + other.TotalAmount > maxStackSize)
		{
			return false;
		}
		if (kprefabID.HasTag(GameTags.MarkedForMove) || kprefabID2.HasTag(GameTags.MarkedForMove))
		{
			return false;
		}
		if (pickupable.PrimaryElement.Mass + other.PrimaryElement.Mass > maxStackSize)
		{
			return false;
		}
		if (rottable != null)
		{
			Rottable.Instance smi = other.GetSMI<Rottable.Instance>();
			if (smi == null)
			{
				return false;
			}
			if (!rottable.IsRotLevelStackable(smi))
			{
				return false;
			}
		}
		bool flag = kprefabID.HasTag(GameTags.SpicedFood);
		if (flag != kprefabID2.HasTag(GameTags.SpicedFood))
		{
			return false;
		}
		Edible component = kprefabID.GetComponent<Edible>();
		Edible component2 = kprefabID2.GetComponent<Edible>();
		if (flag && !component.CanAbsorb(component2))
		{
			return false;
		}
		if (kprefabID.HasTag(GameTags.Seed) || kprefabID.HasTag(GameTags.CropSeed) || kprefabID.HasTag(GameTags.Compostable))
		{
			MutantPlant component3 = pickupable.GetComponent<MutantPlant>();
			MutantPlant component4 = other.GetComponent<MutantPlant>();
			if (component3 != null || component4 != null)
			{
				if (component3 == null != (component4 == null))
				{
					return false;
				}
				if (kprefabID.HasTag(GameTags.UnidentifiedSeed) != kprefabID2.HasTag(GameTags.UnidentifiedSeed))
				{
					return false;
				}
				if (component3.SubSpeciesID != component4.SubSpeciesID)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06001FF7 RID: 8183 RVA: 0x000B3F68 File Offset: 0x000B2168
	public static Pickupable Split(Pickupable pickupable, float amount, GameObject prefab = null)
	{
		if (amount >= pickupable.TotalAmount && prefab == null)
		{
			return pickupable;
		}
		Storage storage = pickupable.storage;
		if (prefab == null)
		{
			prefab = Assets.GetPrefab(pickupable.KPrefabID.PrefabID());
		}
		GameObject parent = null;
		if (pickupable.transform.parent != null)
		{
			parent = pickupable.transform.parent.gameObject;
		}
		GameObject gameObject = GameUtil.KInstantiate(prefab, pickupable.transform.GetPosition(), Grid.SceneLayer.Ore, parent, null, 0);
		global::Debug.Assert(gameObject != null, "WTH, the GO is null, shouldn't happen on instantiate");
		Pickupable component = gameObject.GetComponent<Pickupable>();
		if (component == null)
		{
			global::Debug.LogError("Edible::OnTake() No Pickupable component for " + gameObject.name, gameObject);
		}
		gameObject.SetActive(true);
		component.TotalAmount = Mathf.Min(amount, pickupable.TotalAmount);
		component.PrimaryElement.Temperature = pickupable.PrimaryElement.Temperature;
		bool keepZeroMassObject = pickupable.PrimaryElement.KeepZeroMassObject;
		pickupable.PrimaryElement.KeepZeroMassObject = true;
		pickupable.TotalAmount -= amount;
		component.Trigger(1335436905, pickupable);
		pickupable.PrimaryElement.KeepZeroMassObject = keepZeroMassObject;
		pickupable.TotalAmount += 0f;
		if (storage != null)
		{
			storage.Trigger(-1697596308, pickupable.gameObject);
			storage.Trigger(-778359855, storage);
		}
		IExtendSplitting[] components = pickupable.GetComponents<IExtendSplitting>();
		if (components != null)
		{
			for (int i = 0; i < components.Length; i++)
			{
				components[i].OnSplitTick(component);
			}
		}
		return component;
	}

	// Token: 0x06001FF8 RID: 8184 RVA: 0x000B40F4 File Offset: 0x000B22F4
	private void OnAbsorb(object data)
	{
		Pickupable pickupable = (Pickupable)data;
		if (pickupable != null)
		{
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			PrimaryElement primaryElement = pickupable.PrimaryElement;
			if (primaryElement != null)
			{
				float temperature = 0f;
				float mass = component.Mass;
				float mass2 = primaryElement.Mass;
				if (mass > 0f && mass2 > 0f)
				{
					temperature = SimUtil.CalculateFinalTemperature(mass, component.Temperature, mass2, primaryElement.Temperature);
				}
				else if (primaryElement.Mass > 0f)
				{
					temperature = primaryElement.Temperature;
				}
				component.SetMassTemperature(mass + mass2, temperature);
				if (CameraController.Instance != null)
				{
					string sound = GlobalAssets.GetSound("Ore_absorb", false);
					Vector3 position = pickupable.transform.GetPosition();
					position.z = 0f;
					if (sound != null && CameraController.Instance.IsAudibleSound(position, sound))
					{
						KFMOD.PlayOneShot(sound, position, 1f);
					}
				}
			}
		}
	}

	// Token: 0x04001209 RID: 4617
	public float maxStackSize = PrimaryElement.MAX_MASS;

	// Token: 0x0400120A RID: 4618
	private static readonly EventSystem.IntraObjectHandler<EntitySplitter> OnAbsorbDelegate = new EventSystem.IntraObjectHandler<EntitySplitter>(delegate(EntitySplitter component, object data)
	{
		component.OnAbsorb(data);
	});
}
