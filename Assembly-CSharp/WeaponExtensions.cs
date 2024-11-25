using System;
using UnityEngine;

// Token: 0x020007D1 RID: 2001
public static class WeaponExtensions
{
	// Token: 0x06003729 RID: 14121 RVA: 0x0012BF14 File Offset: 0x0012A114
	public static Weapon AddWeapon(this GameObject prefab, float base_damage_min, float base_damage_max, AttackProperties.DamageType attackType = AttackProperties.DamageType.Standard, AttackProperties.TargetType targetType = AttackProperties.TargetType.Single, int maxHits = 1, float aoeRadius = 0f)
	{
		Weapon weapon = prefab.AddOrGet<Weapon>();
		weapon.Configure(base_damage_min, base_damage_max, attackType, targetType, maxHits, aoeRadius);
		return weapon;
	}
}
