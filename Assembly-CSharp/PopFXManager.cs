using System;
using System.Collections.Generic;
using Klei;
using UnityEngine;

// Token: 0x02000D12 RID: 3346
public class PopFXManager : KScreen
{
	// Token: 0x06006869 RID: 26729 RVA: 0x00271A91 File Offset: 0x0026FC91
	public static void DestroyInstance()
	{
		PopFXManager.Instance = null;
	}

	// Token: 0x0600686A RID: 26730 RVA: 0x00271A99 File Offset: 0x0026FC99
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		PopFXManager.Instance = this;
	}

	// Token: 0x0600686B RID: 26731 RVA: 0x00271AA8 File Offset: 0x0026FCA8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.ready = true;
		if (GenericGameSettings.instance.disablePopFx)
		{
			return;
		}
		for (int i = 0; i < 20; i++)
		{
			PopFX item = this.CreatePopFX();
			this.Pool.Add(item);
		}
	}

	// Token: 0x0600686C RID: 26732 RVA: 0x00271AEF File Offset: 0x0026FCEF
	public bool Ready()
	{
		return this.ready;
	}

	// Token: 0x0600686D RID: 26733 RVA: 0x00271AF8 File Offset: 0x0026FCF8
	public PopFX SpawnFX(Sprite icon, string text, Transform target_transform, Vector3 offset, float lifetime = 1.5f, bool track_target = false, bool force_spawn = false)
	{
		if (GenericGameSettings.instance.disablePopFx)
		{
			return null;
		}
		if (Game.IsQuitting())
		{
			return null;
		}
		Vector3 vector = offset;
		if (target_transform != null)
		{
			vector += target_transform.GetPosition();
		}
		if (!force_spawn)
		{
			int cell = Grid.PosToCell(vector);
			if (!Grid.IsValidCell(cell) || !Grid.IsVisible(cell))
			{
				return null;
			}
		}
		PopFX popFX;
		if (this.Pool.Count > 0)
		{
			popFX = this.Pool[0];
			this.Pool[0].gameObject.SetActive(true);
			this.Pool[0].Spawn(icon, text, target_transform, offset, lifetime, track_target);
			this.Pool.RemoveAt(0);
		}
		else
		{
			popFX = this.CreatePopFX();
			popFX.gameObject.SetActive(true);
			popFX.Spawn(icon, text, target_transform, offset, lifetime, track_target);
		}
		return popFX;
	}

	// Token: 0x0600686E RID: 26734 RVA: 0x00271BD1 File Offset: 0x0026FDD1
	public PopFX SpawnFX(Sprite icon, string text, Transform target_transform, float lifetime = 1.5f, bool track_target = false)
	{
		return this.SpawnFX(icon, text, target_transform, Vector3.zero, lifetime, track_target, false);
	}

	// Token: 0x0600686F RID: 26735 RVA: 0x00271BE6 File Offset: 0x0026FDE6
	private PopFX CreatePopFX()
	{
		GameObject gameObject = Util.KInstantiate(this.Prefab_PopFX, base.gameObject, "Pooled_PopFX");
		gameObject.transform.localScale = Vector3.one;
		return gameObject.GetComponent<PopFX>();
	}

	// Token: 0x06006870 RID: 26736 RVA: 0x00271C13 File Offset: 0x0026FE13
	public void RecycleFX(PopFX fx)
	{
		this.Pool.Add(fx);
	}

	// Token: 0x04004699 RID: 18073
	public static PopFXManager Instance;

	// Token: 0x0400469A RID: 18074
	public GameObject Prefab_PopFX;

	// Token: 0x0400469B RID: 18075
	public List<PopFX> Pool = new List<PopFX>();

	// Token: 0x0400469C RID: 18076
	public Sprite sprite_Plus;

	// Token: 0x0400469D RID: 18077
	public Sprite sprite_Negative;

	// Token: 0x0400469E RID: 18078
	public Sprite sprite_Resource;

	// Token: 0x0400469F RID: 18079
	public Sprite sprite_Building;

	// Token: 0x040046A0 RID: 18080
	public Sprite sprite_Research;

	// Token: 0x040046A1 RID: 18081
	private bool ready;
}
