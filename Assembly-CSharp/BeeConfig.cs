using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020000B6 RID: 182
public class BeeConfig : IEntityConfig
{
	// Token: 0x06000335 RID: 821 RVA: 0x0001B3DC File Offset: 0x000195DC
	public static GameObject CreateBee(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseBeeConfig.BaseBee(id, name, desc, anim_file, "BeeBaseTrait", DECOR.BONUS.TIER4, is_baby, null);
		Trait trait = Db.Get().CreateTrait("BeeBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 5f, name, false, false, true));
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x06000336 RID: 822 RVA: 0x0001B479 File Offset: 0x00019679
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000337 RID: 823 RVA: 0x0001B480 File Offset: 0x00019680
	public GameObject CreatePrefab()
	{
		return BeeConfig.CreateBee("Bee", STRINGS.CREATURES.SPECIES.BEE.NAME, STRINGS.CREATURES.SPECIES.BEE.DESC, "bee_kanim", false);
	}

	// Token: 0x06000338 RID: 824 RVA: 0x0001B4A6 File Offset: 0x000196A6
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000339 RID: 825 RVA: 0x0001B4A8 File Offset: 0x000196A8
	public void OnSpawn(GameObject inst)
	{
		BaseBeeConfig.SetupLoopingSounds(inst);
	}

	// Token: 0x0400024C RID: 588
	public const string ID = "Bee";

	// Token: 0x0400024D RID: 589
	public const string BASE_TRAIT_ID = "BeeBaseTrait";
}
