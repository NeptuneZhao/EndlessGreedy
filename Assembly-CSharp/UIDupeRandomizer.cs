using System;
using System.Collections.Generic;
using Database;
using UnityEngine;

// Token: 0x02000DDF RID: 3551
public class UIDupeRandomizer : MonoBehaviour
{
	// Token: 0x060070D0 RID: 28880 RVA: 0x002AB2A8 File Offset: 0x002A94A8
	protected virtual void Start()
	{
		this.slots = Db.Get().AccessorySlots;
		for (int i = 0; i < this.anims.Length; i++)
		{
			this.anims[i].curBody = null;
			this.GetNewBody(i);
		}
	}

	// Token: 0x060070D1 RID: 28881 RVA: 0x002AB2F4 File Offset: 0x002A94F4
	protected void GetNewBody(int minion_idx)
	{
		Personality random = Db.Get().Personalities.GetRandom(true, false);
		foreach (KBatchedAnimController dupe in this.anims[minion_idx].minions)
		{
			this.Apply(dupe, random);
		}
	}

	// Token: 0x060070D2 RID: 28882 RVA: 0x002AB368 File Offset: 0x002A9568
	private void Apply(KBatchedAnimController dupe, Personality personality)
	{
		KCompBuilder.BodyData bodyData = MinionStartingStats.CreateBodyData(personality);
		SymbolOverrideController component = dupe.GetComponent<SymbolOverrideController>();
		component.RemoveAllSymbolOverrides(0);
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Hair.Lookup(bodyData.hair));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.HatHair.Lookup("hat_" + HashCache.Get().Get(bodyData.hair)));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Eyes.Lookup(bodyData.eyes));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.HeadShape.Lookup(bodyData.headShape));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Mouth.Lookup(bodyData.mouth));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Body.Lookup(bodyData.body));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Arm.Lookup(bodyData.arms));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.ArmLower.Lookup(bodyData.armslower));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Belt.Lookup(bodyData.belt));
		if (this.applySuit && UnityEngine.Random.value < 0.15f)
		{
			component.AddBuildOverride(Assets.GetAnim("body_oxygen_kanim").GetData(), 6);
			dupe.SetSymbolVisiblity("snapto_neck", true);
			dupe.SetSymbolVisiblity("belt", false);
		}
		else
		{
			dupe.SetSymbolVisiblity("snapto_neck", false);
		}
		if (this.applyHat && UnityEngine.Random.value < 0.5f)
		{
			List<string> list = new List<string>();
			foreach (Skill skill in Db.Get().Skills.resources)
			{
				list.Add(skill.hat);
			}
			string id = list[UnityEngine.Random.Range(0, list.Count)];
			UIDupeRandomizer.AddAccessory(dupe, this.slots.Hat.Lookup(id));
			dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, false);
			dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, true);
		}
		else
		{
			dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, true);
			dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, false);
			dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.Hat.targetSymbolId, false);
		}
		dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.Skirt.targetSymbolId, false);
		dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.Necklace.targetSymbolId, false);
	}

	// Token: 0x060070D3 RID: 28883 RVA: 0x002AB668 File Offset: 0x002A9868
	public static KAnimHashedString AddAccessory(KBatchedAnimController minion, Accessory accessory)
	{
		if (accessory != null)
		{
			SymbolOverrideController component = minion.GetComponent<SymbolOverrideController>();
			DebugUtil.Assert(component != null, minion.name + " is missing symbol override controller");
			component.TryRemoveSymbolOverride(accessory.slot.targetSymbolId, 0);
			component.AddSymbolOverride(accessory.slot.targetSymbolId, accessory.symbol, 0);
			minion.SetSymbolVisiblity(accessory.slot.targetSymbolId, true);
			return accessory.slot.targetSymbolId;
		}
		return HashedString.Invalid;
	}

	// Token: 0x060070D4 RID: 28884 RVA: 0x002AB6F8 File Offset: 0x002A98F8
	public KAnimHashedString AddRandomAccessory(KBatchedAnimController minion, List<Accessory> choices)
	{
		Accessory accessory = choices[UnityEngine.Random.Range(1, choices.Count)];
		return UIDupeRandomizer.AddAccessory(minion, accessory);
	}

	// Token: 0x060070D5 RID: 28885 RVA: 0x002AB720 File Offset: 0x002A9920
	public void Randomize()
	{
		if (this.slots == null)
		{
			return;
		}
		for (int i = 0; i < this.anims.Length; i++)
		{
			this.GetNewBody(i);
		}
	}

	// Token: 0x060070D6 RID: 28886 RVA: 0x002AB750 File Offset: 0x002A9950
	protected virtual void Update()
	{
	}

	// Token: 0x04004D95 RID: 19861
	[Tooltip("Enable this to allow for a chance for skill hats to appear")]
	public bool applyHat = true;

	// Token: 0x04004D96 RID: 19862
	[Tooltip("Enable this to allow for a chance for suit helmets to appear (ie. atmosuit and leadsuit)")]
	public bool applySuit = true;

	// Token: 0x04004D97 RID: 19863
	public UIDupeRandomizer.AnimChoice[] anims;

	// Token: 0x04004D98 RID: 19864
	private AccessorySlots slots;

	// Token: 0x02001EEB RID: 7915
	[Serializable]
	public struct AnimChoice
	{
		// Token: 0x04008C00 RID: 35840
		public string anim_name;

		// Token: 0x04008C01 RID: 35841
		public List<KBatchedAnimController> minions;

		// Token: 0x04008C02 RID: 35842
		public float minSecondsBetweenAction;

		// Token: 0x04008C03 RID: 35843
		public float maxSecondsBetweenAction;

		// Token: 0x04008C04 RID: 35844
		public float lastWaitTime;

		// Token: 0x04008C05 RID: 35845
		public KAnimFile curBody;
	}
}
