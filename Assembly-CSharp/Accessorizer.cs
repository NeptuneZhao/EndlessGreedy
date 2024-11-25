using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Database;
using KSerialization;
using UnityEngine;

// Token: 0x02000523 RID: 1315
[AddComponentMenu("KMonoBehaviour/scripts/Accessorizer")]
public class Accessorizer : KMonoBehaviour
{
	// Token: 0x06001D71 RID: 7537 RVA: 0x000A318D File Offset: 0x000A138D
	public List<ResourceRef<Accessory>> GetAccessories()
	{
		return this.accessories;
	}

	// Token: 0x06001D72 RID: 7538 RVA: 0x000A3195 File Offset: 0x000A1395
	public void SetAccessories(List<ResourceRef<Accessory>> data)
	{
		this.accessories = data;
	}

	// Token: 0x17000114 RID: 276
	// (get) Token: 0x06001D73 RID: 7539 RVA: 0x000A319E File Offset: 0x000A139E
	// (set) Token: 0x06001D74 RID: 7540 RVA: 0x000A31A6 File Offset: 0x000A13A6
	public KCompBuilder.BodyData bodyData { get; set; }

	// Token: 0x06001D75 RID: 7541 RVA: 0x000A31B0 File Offset: 0x000A13B0
	[OnDeserialized]
	private void OnDeserialized()
	{
		MinionIdentity component = base.GetComponent<MinionIdentity>();
		if (this.clothingItems.Count > 0 || (component != null && component.nameStringKey == "JORGE") || SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 30))
		{
			if (component != null)
			{
				this.bodyData = Accessorizer.UpdateAccessorySlots(component.nameStringKey, ref this.accessories);
			}
			this.accessories.RemoveAll((ResourceRef<Accessory> x) => x.Get() == null);
		}
		if (this.clothingItems.Count > 0)
		{
			base.GetComponent<WearableAccessorizer>().ApplyClothingItems(ClothingOutfitUtility.OutfitType.Clothing, from i in this.clothingItems
			select i.Get());
			this.clothingItems.Clear();
		}
		this.ApplyAccessories();
	}

	// Token: 0x06001D76 RID: 7542 RVA: 0x000A32AC File Offset: 0x000A14AC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		MinionIdentity component = base.GetComponent<MinionIdentity>();
		if (component != null)
		{
			this.bodyData = MinionStartingStats.CreateBodyData(Db.Get().Personalities.Get(component.personalityResourceId));
		}
	}

	// Token: 0x06001D77 RID: 7543 RVA: 0x000A32F0 File Offset: 0x000A14F0
	public void AddAccessory(Accessory accessory)
	{
		if (accessory != null)
		{
			if (this.animController == null)
			{
				this.animController = base.GetComponent<KAnimControllerBase>();
			}
			this.animController.GetComponent<SymbolOverrideController>().AddSymbolOverride(accessory.slot.targetSymbolId, accessory.symbol, accessory.slot.overrideLayer);
			if (!this.HasAccessory(accessory))
			{
				ResourceRef<Accessory> resourceRef = new ResourceRef<Accessory>(accessory);
				if (resourceRef != null)
				{
					this.accessories.Add(resourceRef);
				}
			}
		}
	}

	// Token: 0x06001D78 RID: 7544 RVA: 0x000A336C File Offset: 0x000A156C
	public void RemoveAccessory(Accessory accessory)
	{
		this.accessories.RemoveAll((ResourceRef<Accessory> x) => x.Get() == accessory);
		this.animController.GetComponent<SymbolOverrideController>().TryRemoveSymbolOverride(accessory.slot.targetSymbolId, accessory.slot.overrideLayer);
	}

	// Token: 0x06001D79 RID: 7545 RVA: 0x000A33D4 File Offset: 0x000A15D4
	public void ApplyAccessories()
	{
		foreach (ResourceRef<Accessory> resourceRef in this.accessories)
		{
			Accessory accessory = resourceRef.Get();
			if (accessory != null)
			{
				this.AddAccessory(accessory);
			}
		}
	}

	// Token: 0x06001D7A RID: 7546 RVA: 0x000A3430 File Offset: 0x000A1630
	public static KCompBuilder.BodyData UpdateAccessorySlots(string nameString, ref List<ResourceRef<Accessory>> accessories)
	{
		accessories.RemoveAll((ResourceRef<Accessory> acc) => acc.Get() == null);
		Personality personalityFromNameStringKey = Db.Get().Personalities.GetPersonalityFromNameStringKey(nameString);
		if (personalityFromNameStringKey != null)
		{
			KCompBuilder.BodyData bodyData = MinionStartingStats.CreateBodyData(personalityFromNameStringKey);
			foreach (AccessorySlot accessorySlot in Db.Get().AccessorySlots.resources)
			{
				if (accessorySlot.accessories.Count != 0)
				{
					Accessory accessory = null;
					if (accessorySlot == Db.Get().AccessorySlots.Body)
					{
						accessory = accessorySlot.Lookup(bodyData.body);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.Arm)
					{
						accessory = accessorySlot.Lookup(bodyData.arms);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.ArmLower)
					{
						accessory = accessorySlot.Lookup(bodyData.armslower);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.ArmLowerSkin)
					{
						accessory = accessorySlot.Lookup(bodyData.armLowerSkin);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.ArmUpperSkin)
					{
						accessory = accessorySlot.Lookup(bodyData.armUpperSkin);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.LegSkin)
					{
						accessory = accessorySlot.Lookup(bodyData.legSkin);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.Leg)
					{
						accessory = accessorySlot.Lookup(bodyData.legs);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.Belt)
					{
						accessory = accessorySlot.Lookup(bodyData.belt);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.Neck)
					{
						accessory = accessorySlot.Lookup(bodyData.neck);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.Pelvis)
					{
						accessory = accessorySlot.Lookup(bodyData.pelvis);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.Foot)
					{
						accessory = accessorySlot.Lookup(bodyData.foot);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.Cuff)
					{
						accessory = accessorySlot.Lookup(bodyData.cuff);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.Hand)
					{
						accessory = accessorySlot.Lookup(bodyData.hand);
					}
					if (accessory != null)
					{
						ResourceRef<Accessory> item = new ResourceRef<Accessory>(accessory);
						accessories.RemoveAll((ResourceRef<Accessory> old_acc) => old_acc.Get().slot == accessory.slot);
						accessories.Add(item);
					}
				}
			}
			return bodyData;
		}
		return default(KCompBuilder.BodyData);
	}

	// Token: 0x06001D7B RID: 7547 RVA: 0x000A3750 File Offset: 0x000A1950
	public bool HasAccessory(Accessory accessory)
	{
		return this.accessories.Exists((ResourceRef<Accessory> x) => x.Get() == accessory);
	}

	// Token: 0x06001D7C RID: 7548 RVA: 0x000A3784 File Offset: 0x000A1984
	public Accessory GetAccessory(AccessorySlot slot)
	{
		for (int i = 0; i < this.accessories.Count; i++)
		{
			if (this.accessories[i].Get() != null && this.accessories[i].Get().slot == slot)
			{
				return this.accessories[i].Get();
			}
		}
		return null;
	}

	// Token: 0x06001D7D RID: 7549 RVA: 0x000A37E8 File Offset: 0x000A19E8
	public void ApplyMinionPersonality(Personality personality)
	{
		this.bodyData = MinionStartingStats.CreateBodyData(personality);
		this.accessories.Clear();
		if (this.animController == null)
		{
			this.animController = base.GetComponent<KAnimControllerBase>();
		}
		foreach (string text in new string[]
		{
			"snapTo_hat",
			"snapTo_hat_hair",
			"snapTo_goggles",
			"snapTo_headFX",
			"snapTo_neck",
			"snapTo_chest",
			"snapTo_pivot",
			"skirt",
			"necklace"
		})
		{
			this.animController.GetComponent<SymbolOverrideController>().RemoveSymbolOverride(text, 0);
			this.animController.SetSymbolVisiblity(text, false);
		}
		this.AddAccessory(Db.Get().AccessorySlots.Eyes.Lookup(this.bodyData.eyes));
		this.AddAccessory(Db.Get().AccessorySlots.Hair.Lookup(this.bodyData.hair));
		this.AddAccessory(Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(this.bodyData.hair)));
		this.AddAccessory(Db.Get().AccessorySlots.HeadShape.Lookup(this.bodyData.headShape));
		this.AddAccessory(Db.Get().AccessorySlots.Mouth.Lookup(this.bodyData.mouth));
		this.AddAccessory(Db.Get().AccessorySlots.Body.Lookup(this.bodyData.body));
		this.AddAccessory(Db.Get().AccessorySlots.Arm.Lookup(this.bodyData.arms));
		this.AddAccessory(Db.Get().AccessorySlots.ArmLower.Lookup(this.bodyData.armslower));
		this.AddAccessory(Db.Get().AccessorySlots.Neck.Lookup(this.bodyData.neck));
		this.AddAccessory(Db.Get().AccessorySlots.Pelvis.Lookup(this.bodyData.pelvis));
		this.AddAccessory(Db.Get().AccessorySlots.Leg.Lookup(this.bodyData.legs));
		this.AddAccessory(Db.Get().AccessorySlots.Foot.Lookup(this.bodyData.foot));
		this.AddAccessory(Db.Get().AccessorySlots.Hand.Lookup(this.bodyData.hand));
		this.AddAccessory(Db.Get().AccessorySlots.Cuff.Lookup(this.bodyData.cuff));
		this.AddAccessory(Db.Get().AccessorySlots.Belt.Lookup(this.bodyData.belt));
		this.AddAccessory(Db.Get().AccessorySlots.ArmLowerSkin.Lookup(this.bodyData.armLowerSkin));
		this.AddAccessory(Db.Get().AccessorySlots.ArmUpperSkin.Lookup(this.bodyData.armUpperSkin));
		this.AddAccessory(Db.Get().AccessorySlots.LegSkin.Lookup(this.bodyData.legSkin));
		this.UpdateHairBasedOnHat();
	}

	// Token: 0x06001D7E RID: 7550 RVA: 0x000A3B68 File Offset: 0x000A1D68
	public void ApplyBodyData(KCompBuilder.BodyData bodyData)
	{
		this.accessories.Clear();
		if (this.animController == null)
		{
			this.animController = base.GetComponent<KAnimControllerBase>();
		}
		foreach (string text in new string[]
		{
			"snapTo_hat",
			"snapTo_hat_hair",
			"snapTo_goggles",
			"snapTo_headFX",
			"snapTo_neck",
			"snapTo_chest",
			"snapTo_pivot",
			"skirt",
			"necklace"
		})
		{
			this.animController.GetComponent<SymbolOverrideController>().RemoveSymbolOverride(text, 0);
			this.animController.SetSymbolVisiblity(text, false);
		}
		this.AddAccessory(Db.Get().AccessorySlots.Eyes.Lookup(bodyData.eyes));
		this.AddAccessory(Db.Get().AccessorySlots.Hair.Lookup(bodyData.hair));
		this.AddAccessory(Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(bodyData.hair)));
		this.AddAccessory(Db.Get().AccessorySlots.HeadShape.Lookup(bodyData.headShape));
		this.AddAccessory(Db.Get().AccessorySlots.Mouth.Lookup(bodyData.mouth));
		this.AddAccessory(Db.Get().AccessorySlots.Body.Lookup(bodyData.body));
		this.AddAccessory(Db.Get().AccessorySlots.Arm.Lookup(bodyData.arms));
		this.AddAccessory(Db.Get().AccessorySlots.ArmLower.Lookup(bodyData.armslower));
		this.AddAccessory(Db.Get().AccessorySlots.Neck.Lookup(bodyData.neck));
		this.AddAccessory(Db.Get().AccessorySlots.Pelvis.Lookup(bodyData.pelvis));
		this.AddAccessory(Db.Get().AccessorySlots.Leg.Lookup(bodyData.legs));
		this.AddAccessory(Db.Get().AccessorySlots.Foot.Lookup(bodyData.foot));
		this.AddAccessory(Db.Get().AccessorySlots.Hand.Lookup(bodyData.hand));
		this.AddAccessory(Db.Get().AccessorySlots.Cuff.Lookup(bodyData.cuff));
		this.AddAccessory(Db.Get().AccessorySlots.Belt.Lookup(bodyData.belt));
		this.AddAccessory(Db.Get().AccessorySlots.ArmLowerSkin.Lookup(bodyData.armLowerSkin));
		this.AddAccessory(Db.Get().AccessorySlots.ArmUpperSkin.Lookup(bodyData.armUpperSkin));
		this.AddAccessory(Db.Get().AccessorySlots.LegSkin.Lookup(bodyData.legSkin));
		this.UpdateHairBasedOnHat();
	}

	// Token: 0x06001D7F RID: 7551 RVA: 0x000A3E80 File Offset: 0x000A2080
	public void UpdateHairBasedOnHat()
	{
		if (!this.GetAccessory(Db.Get().AccessorySlots.Hat).IsNullOrDestroyed())
		{
			this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, false);
			this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, true);
			return;
		}
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, true);
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, false);
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Hat.targetSymbolId, false);
	}

	// Token: 0x06001D80 RID: 7552 RVA: 0x000A3F50 File Offset: 0x000A2150
	public void GetBodySlots(ref KCompBuilder.BodyData fd)
	{
		fd.eyes = HashedString.Invalid;
		fd.hair = HashedString.Invalid;
		fd.headShape = HashedString.Invalid;
		fd.mouth = HashedString.Invalid;
		fd.neck = HashedString.Invalid;
		fd.body = HashedString.Invalid;
		fd.arms = HashedString.Invalid;
		fd.armslower = HashedString.Invalid;
		fd.hat = HashedString.Invalid;
		fd.faceFX = HashedString.Invalid;
		fd.armLowerSkin = HashedString.Invalid;
		fd.armUpperSkin = HashedString.Invalid;
		fd.legSkin = HashedString.Invalid;
		fd.belt = HashedString.Invalid;
		fd.pelvis = HashedString.Invalid;
		fd.foot = HashedString.Invalid;
		fd.skirt = HashedString.Invalid;
		fd.necklace = HashedString.Invalid;
		fd.cuff = HashedString.Invalid;
		fd.hand = HashedString.Invalid;
		for (int i = 0; i < this.accessories.Count; i++)
		{
			Accessory accessory = this.accessories[i].Get();
			if (accessory != null)
			{
				if (accessory.slot.Id == "Eyes")
				{
					fd.eyes = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Hair")
				{
					fd.hair = accessory.IdHash;
				}
				else if (accessory.slot.Id == "HeadShape")
				{
					fd.headShape = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Mouth")
				{
					fd.mouth = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Neck")
				{
					fd.neck = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Torso")
				{
					fd.body = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Arm_Sleeve")
				{
					fd.arms = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Arm_Lower_Sleeve")
				{
					fd.armslower = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Hat")
				{
					fd.hat = HashedString.Invalid;
				}
				else if (accessory.slot.Id == "FaceEffect")
				{
					fd.faceFX = HashedString.Invalid;
				}
				else if (accessory.slot.Id == "Arm_Lower")
				{
					fd.armLowerSkin = accessory.Id;
				}
				else if (accessory.slot.Id == "Arm_Upper")
				{
					fd.armUpperSkin = accessory.Id;
				}
				else if (accessory.slot.Id == "Leg_Skin")
				{
					fd.legSkin = accessory.Id;
				}
				else if (accessory.slot.Id == "Leg")
				{
					fd.legs = accessory.Id;
				}
				else if (accessory.slot.Id == "Belt")
				{
					fd.belt = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Pelvis")
				{
					fd.pelvis = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Foot")
				{
					fd.foot = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Cuff")
				{
					fd.cuff = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Skirt")
				{
					fd.skirt = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Hand")
				{
					fd.hand = accessory.IdHash;
				}
			}
		}
	}

	// Token: 0x04001092 RID: 4242
	[Serialize]
	private List<ResourceRef<Accessory>> accessories = new List<ResourceRef<Accessory>>();

	// Token: 0x04001093 RID: 4243
	[MyCmpReq]
	private KAnimControllerBase animController;

	// Token: 0x04001094 RID: 4244
	[Serialize]
	private List<ResourceRef<ClothingItemResource>> clothingItems = new List<ResourceRef<ClothingItemResource>>();
}
