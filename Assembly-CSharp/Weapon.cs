using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007D0 RID: 2000
[AddComponentMenu("KMonoBehaviour/scripts/Weapon")]
public class Weapon : KMonoBehaviour
{
	// Token: 0x06003722 RID: 14114 RVA: 0x0012BCF4 File Offset: 0x00129EF4
	public void Configure(float base_damage_min, float base_damage_max, AttackProperties.DamageType attackType = AttackProperties.DamageType.Standard, AttackProperties.TargetType targetType = AttackProperties.TargetType.Single, int maxHits = 1, float aoeRadius = 0f)
	{
		this.properties = new AttackProperties();
		this.properties.base_damage_min = base_damage_min;
		this.properties.base_damage_max = base_damage_max;
		this.properties.maxHits = maxHits;
		this.properties.damageType = attackType;
		this.properties.aoe_radius = aoeRadius;
		this.properties.attacker = this;
	}

	// Token: 0x06003723 RID: 14115 RVA: 0x0012BD56 File Offset: 0x00129F56
	public void AddEffect(string effectID = "WasAttacked", float probability = 1f)
	{
		if (this.properties.effects == null)
		{
			this.properties.effects = new List<AttackEffect>();
		}
		this.properties.effects.Add(new AttackEffect(effectID, probability));
	}

	// Token: 0x06003724 RID: 14116 RVA: 0x0012BD8C File Offset: 0x00129F8C
	public int AttackArea(Vector3 centerPoint)
	{
		Vector3 b = Vector3.zero;
		this.alignment = base.GetComponent<FactionAlignment>();
		if (this.alignment == null)
		{
			return 0;
		}
		List<GameObject> list = new List<GameObject>();
		foreach (Health health in Components.Health.Items)
		{
			if (!(health.gameObject == base.gameObject) && !health.IsDefeated())
			{
				FactionAlignment component = health.GetComponent<FactionAlignment>();
				if (!(component == null) && component.IsAlignmentActive() && FactionManager.Instance.GetDisposition(this.alignment.Alignment, component.Alignment) == FactionManager.Disposition.Attack)
				{
					b = health.transform.GetPosition();
					b.z = centerPoint.z;
					if (Vector3.Distance(centerPoint, b) <= this.properties.aoe_radius)
					{
						list.Add(health.gameObject);
					}
				}
			}
		}
		this.AttackTargets(list.ToArray());
		return list.Count;
	}

	// Token: 0x06003725 RID: 14117 RVA: 0x0012BEB4 File Offset: 0x0012A0B4
	public void AttackTarget(GameObject target)
	{
		this.AttackTargets(new GameObject[]
		{
			target
		});
	}

	// Token: 0x06003726 RID: 14118 RVA: 0x0012BEC6 File Offset: 0x0012A0C6
	public void AttackTargets(GameObject[] targets)
	{
		if (this.properties == null)
		{
			global::Debug.LogWarning(string.Format("Attack properties not configured. {0} cannot attack with weapon.", base.gameObject.name));
			return;
		}
		new Attack(this.properties, targets);
	}

	// Token: 0x06003727 RID: 14119 RVA: 0x0012BEF8 File Offset: 0x0012A0F8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.properties.attacker = this;
	}

	// Token: 0x040020A7 RID: 8359
	[MyCmpReq]
	private FactionAlignment alignment;

	// Token: 0x040020A8 RID: 8360
	public AttackProperties properties;
}
