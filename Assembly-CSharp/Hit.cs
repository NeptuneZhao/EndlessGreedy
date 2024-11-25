using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020007CF RID: 1999
public class Hit
{
	// Token: 0x0600371F RID: 14111 RVA: 0x0012BBB4 File Offset: 0x00129DB4
	public Hit(AttackProperties properties, GameObject target)
	{
		this.properties = properties;
		this.target = target;
		this.DeliverHit();
	}

	// Token: 0x06003720 RID: 14112 RVA: 0x0012BBD0 File Offset: 0x00129DD0
	private float rollDamage()
	{
		return (float)Mathf.RoundToInt(UnityEngine.Random.Range(this.properties.base_damage_min, this.properties.base_damage_max));
	}

	// Token: 0x06003721 RID: 14113 RVA: 0x0012BBF4 File Offset: 0x00129DF4
	private void DeliverHit()
	{
		Health component = this.target.GetComponent<Health>();
		if (!component)
		{
			return;
		}
		this.target.Trigger(-787691065, this.properties.attacker.GetComponent<FactionAlignment>());
		float num = this.rollDamage();
		AttackableBase component2 = this.target.GetComponent<AttackableBase>();
		num *= 1f + component2.GetDamageMultiplier();
		component.Damage(num);
		if (this.properties.effects == null)
		{
			return;
		}
		Effects component3 = this.target.GetComponent<Effects>();
		if (component3)
		{
			foreach (AttackEffect attackEffect in this.properties.effects)
			{
				if (UnityEngine.Random.Range(0f, 100f) < attackEffect.effectProbability * 100f)
				{
					component3.Add(attackEffect.effectID, true);
				}
			}
		}
	}

	// Token: 0x040020A5 RID: 8357
	private AttackProperties properties;

	// Token: 0x040020A6 RID: 8358
	private GameObject target;
}
