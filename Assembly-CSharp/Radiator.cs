using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000A42 RID: 2626
[AddComponentMenu("KMonoBehaviour/scripts/Radiator")]
public class Radiator : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06004C0B RID: 19467 RVA: 0x001B213C File Offset: 0x001B033C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.emitter = new RadiationGridEmitter(Grid.PosToCell(base.gameObject), this.intensity);
		this.emitter.projectionCount = this.projectionCount;
		this.emitter.direction = this.direction;
		this.emitter.angle = this.angle;
		if (base.GetComponent<Operational>() == null)
		{
			this.emitter.enabled = true;
		}
		else
		{
			base.Subscribe(824508782, new Action<object>(this.OnOperationalChanged));
		}
		RadiationGridManager.emitters.Add(this.emitter);
	}

	// Token: 0x06004C0C RID: 19468 RVA: 0x001B21E2 File Offset: 0x001B03E2
	protected override void OnCleanUp()
	{
		RadiationGridManager.emitters.Remove(this.emitter);
		base.OnCleanUp();
	}

	// Token: 0x06004C0D RID: 19469 RVA: 0x001B21FC File Offset: 0x001B03FC
	private void OnOperationalChanged(object data)
	{
		bool isActive = base.GetComponent<Operational>().IsActive;
		this.emitter.enabled = isActive;
	}

	// Token: 0x06004C0E RID: 19470 RVA: 0x001B2221 File Offset: 0x001B0421
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.EMITS_LIGHT, this.intensity), UI.GAMEOBJECTEFFECTS.TOOLTIPS.EMITS_LIGHT, Descriptor.DescriptorType.Effect, false)
		};
	}

	// Token: 0x06004C0F RID: 19471 RVA: 0x001B2259 File Offset: 0x001B0459
	private void Update()
	{
		this.emitter.originCell = Grid.PosToCell(base.gameObject);
	}

	// Token: 0x04003284 RID: 12932
	public RadiationGridEmitter emitter;

	// Token: 0x04003285 RID: 12933
	public int intensity;

	// Token: 0x04003286 RID: 12934
	public int projectionCount;

	// Token: 0x04003287 RID: 12935
	public int direction;

	// Token: 0x04003288 RID: 12936
	public int angle = 360;
}
