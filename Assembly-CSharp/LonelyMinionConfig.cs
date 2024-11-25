using System;
using STRINGS;
using UnityEngine;

// Token: 0x020002DA RID: 730
public class LonelyMinionConfig : IEntityConfig
{
	// Token: 0x06000F44 RID: 3908 RVA: 0x0005854E File Offset: 0x0005674E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F45 RID: 3909 RVA: 0x00058558 File Offset: 0x00056758
	public GameObject CreatePrefab()
	{
		string name = DUPLICANTS.MODEL.STANDARD.NAME;
		GameObject gameObject = EntityTemplates.CreateEntity(LonelyMinionConfig.ID, name, true);
		gameObject.AddComponent<Accessorizer>();
		gameObject.AddOrGet<WearableAccessorizer>();
		gameObject.AddComponent<Storage>().doDiseaseTransfer = false;
		gameObject.AddComponent<StateMachineController>();
		LonelyMinion.Def def = gameObject.AddOrGetDef<LonelyMinion.Def>();
		def.Personality = Db.Get().Personalities.Get("JORGE");
		def.Personality.Disabled = true;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.defaultAnim = "idle_default";
		kbatchedAnimController.initialAnim = "idle_default";
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("body_comp_default_kanim"),
			Assets.GetAnim("anim_idles_default_kanim"),
			Assets.GetAnim("anim_interacts_lonely_dupe_kanim")
		};
		this.ConfigurePackageOverride(gameObject);
		SymbolOverrideController symbolOverrideController = SymbolOverrideControllerUtil.AddToPrefab(gameObject);
		symbolOverrideController.applySymbolOverridesEveryFrame = true;
		symbolOverrideController.AddSymbolOverride("snapto_cheek", Assets.GetAnim("head_swap_kanim").GetData().build.GetSymbol(string.Format("cheek_00{0}", def.Personality.headShape)), 1);
		BaseMinionConfig.ConfigureSymbols(gameObject, true);
		return gameObject;
	}

	// Token: 0x06000F46 RID: 3910 RVA: 0x00058699 File Offset: 0x00056899
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000F47 RID: 3911 RVA: 0x0005869B File Offset: 0x0005689B
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x06000F48 RID: 3912 RVA: 0x000586A0 File Offset: 0x000568A0
	private void ConfigurePackageOverride(GameObject go)
	{
		GameObject gameObject = new GameObject("PackageSnapPoint");
		gameObject.transform.SetParent(go.transform);
		KBatchedAnimController component = go.GetComponent<KBatchedAnimController>();
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.transform.position = Vector3.forward * -0.1f;
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("mushbar_kanim")
		};
		kbatchedAnimController.initialAnim = "object";
		component.SetSymbolVisiblity(LonelyMinionConfig.PARCEL_SNAPTO, false);
		KBatchedAnimTracker kbatchedAnimTracker = gameObject.AddOrGet<KBatchedAnimTracker>();
		kbatchedAnimTracker.controller = component;
		kbatchedAnimTracker.symbol = LonelyMinionConfig.PARCEL_SNAPTO;
	}

	// Token: 0x04000959 RID: 2393
	public static string ID = "LonelyMinion";

	// Token: 0x0400095A RID: 2394
	public const int VOICE_IDX = -2;

	// Token: 0x0400095B RID: 2395
	public const int STARTING_SKILL_POINTS = 3;

	// Token: 0x0400095C RID: 2396
	public const int BASE_ATTRIBUTE_LEVEL = 7;

	// Token: 0x0400095D RID: 2397
	public const int AGE_MIN = 2190;

	// Token: 0x0400095E RID: 2398
	public const int AGE_MAX = 3102;

	// Token: 0x0400095F RID: 2399
	public const float MIN_IDLE_DELAY = 20f;

	// Token: 0x04000960 RID: 2400
	public const float MAX_IDLE_DELAY = 40f;

	// Token: 0x04000961 RID: 2401
	public const string IDLE_PREFIX = "idle_blinds";

	// Token: 0x04000962 RID: 2402
	public static readonly HashedString GreetingCriteraId = "Neighbor";

	// Token: 0x04000963 RID: 2403
	public static readonly HashedString FoodCriteriaId = "FoodQuality";

	// Token: 0x04000964 RID: 2404
	public static readonly HashedString DecorCriteriaId = "Decor";

	// Token: 0x04000965 RID: 2405
	public static readonly HashedString PowerCriteriaId = "SuppliedPower";

	// Token: 0x04000966 RID: 2406
	public static readonly HashedString CHECK_MAIL = "mail_pre";

	// Token: 0x04000967 RID: 2407
	public static readonly HashedString CHECK_MAIL_SUCCESS = "mail_success_pst";

	// Token: 0x04000968 RID: 2408
	public static readonly HashedString CHECK_MAIL_FAILURE = "mail_failure_pst";

	// Token: 0x04000969 RID: 2409
	public static readonly HashedString CHECK_MAIL_DUPLICATE = "mail_duplicate_pst";

	// Token: 0x0400096A RID: 2410
	public static readonly HashedString FOOD_SUCCESS = "food_like_loop";

	// Token: 0x0400096B RID: 2411
	public static readonly HashedString FOOD_FAILURE = "food_dislike_loop";

	// Token: 0x0400096C RID: 2412
	public static readonly HashedString FOOD_DUPLICATE = "food_duplicate_loop";

	// Token: 0x0400096D RID: 2413
	public static readonly HashedString FOOD_IDLE = "idle_food_quest";

	// Token: 0x0400096E RID: 2414
	public static readonly HashedString DECOR_IDLE = "idle_decor_quest";

	// Token: 0x0400096F RID: 2415
	public static readonly HashedString POWER_IDLE = "idle_power_quest";

	// Token: 0x04000970 RID: 2416
	public static readonly HashedString BLINDS_IDLE_0 = "idle_blinds_0";

	// Token: 0x04000971 RID: 2417
	public static readonly HashedString PARCEL_SNAPTO = "parcel_snapTo";

	// Token: 0x04000972 RID: 2418
	public const string PERSONALITY_ID = "JORGE";

	// Token: 0x04000973 RID: 2419
	public const string BODY_ANIM_FILE = "body_lonelyminion_kanim";
}
