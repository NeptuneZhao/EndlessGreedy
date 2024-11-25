using System;
using KSerialization;
using UnityEngine;

// Token: 0x020005F3 RID: 1523
[AddComponentMenu("KMonoBehaviour/Workable/Unsealable")]
public class Unsealable : Workable
{
	// Token: 0x060024F9 RID: 9465 RVA: 0x000CEE50 File Offset: 0x000CD050
	private Unsealable()
	{
	}

	// Token: 0x060024FA RID: 9466 RVA: 0x000CEE58 File Offset: 0x000CD058
	public override CellOffset[] GetOffsets(int cell)
	{
		if (this.facingRight)
		{
			return OffsetGroups.RightOnly;
		}
		return OffsetGroups.LeftOnly;
	}

	// Token: 0x060024FB RID: 9467 RVA: 0x000CEE6D File Offset: 0x000CD06D
	protected override void OnPrefabInit()
	{
		this.faceTargetWhenWorking = true;
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_door_poi_kanim")
		};
	}

	// Token: 0x060024FC RID: 9468 RVA: 0x000CEE9C File Offset: 0x000CD09C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(3f);
		if (this.unsealed)
		{
			Deconstructable component = base.GetComponent<Deconstructable>();
			if (component != null)
			{
				component.allowDeconstruction = true;
			}
		}
	}

	// Token: 0x060024FD RID: 9469 RVA: 0x000CEED9 File Offset: 0x000CD0D9
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
	}

	// Token: 0x060024FE RID: 9470 RVA: 0x000CEEE4 File Offset: 0x000CD0E4
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.unsealed = true;
		base.OnCompleteWork(worker);
		Deconstructable component = base.GetComponent<Deconstructable>();
		if (component != null)
		{
			component.allowDeconstruction = true;
			Game.Instance.Trigger(1980521255, base.gameObject);
		}
	}

	// Token: 0x040014EE RID: 5358
	[Serialize]
	public bool facingRight;

	// Token: 0x040014EF RID: 5359
	[Serialize]
	public bool unsealed;
}
