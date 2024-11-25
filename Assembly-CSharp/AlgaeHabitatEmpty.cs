using System;
using TUNING;
using UnityEngine;

// Token: 0x0200067E RID: 1662
[AddComponentMenu("KMonoBehaviour/Workable/AlgaeHabitatEmpty")]
public class AlgaeHabitatEmpty : Workable
{
	// Token: 0x06002923 RID: 10531 RVA: 0x000E8BF0 File Offset: 0x000E6DF0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Cleaning;
		this.workingStatusItem = Db.Get().MiscStatusItems.Cleaning;
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.workAnims = AlgaeHabitatEmpty.CLEAN_ANIMS;
		this.workingPstComplete = new HashedString[]
		{
			AlgaeHabitatEmpty.PST_ANIM
		};
		this.workingPstFailed = new HashedString[]
		{
			AlgaeHabitatEmpty.PST_ANIM
		};
		this.synchronizeAnims = false;
	}

	// Token: 0x040017B1 RID: 6065
	private static readonly HashedString[] CLEAN_ANIMS = new HashedString[]
	{
		"sponge_pre",
		"sponge_loop"
	};

	// Token: 0x040017B2 RID: 6066
	private static readonly HashedString PST_ANIM = new HashedString("sponge_pst");
}
