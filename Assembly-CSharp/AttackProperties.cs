using System;
using System.Collections.Generic;

// Token: 0x020007CA RID: 1994
[Serializable]
public class AttackProperties
{
	// Token: 0x04002081 RID: 8321
	public Weapon attacker;

	// Token: 0x04002082 RID: 8322
	public AttackProperties.DamageType damageType;

	// Token: 0x04002083 RID: 8323
	public AttackProperties.TargetType targetType;

	// Token: 0x04002084 RID: 8324
	public float base_damage_min;

	// Token: 0x04002085 RID: 8325
	public float base_damage_max;

	// Token: 0x04002086 RID: 8326
	public int maxHits;

	// Token: 0x04002087 RID: 8327
	public float aoe_radius = 2f;

	// Token: 0x04002088 RID: 8328
	public List<AttackEffect> effects;

	// Token: 0x02001692 RID: 5778
	public enum DamageType
	{
		// Token: 0x04007011 RID: 28689
		Standard
	}

	// Token: 0x02001693 RID: 5779
	public enum TargetType
	{
		// Token: 0x04007013 RID: 28691
		Single,
		// Token: 0x04007014 RID: 28692
		AreaOfEffect
	}
}
