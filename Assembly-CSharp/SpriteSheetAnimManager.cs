using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005C0 RID: 1472
[AddComponentMenu("KMonoBehaviour/scripts/SpriteSheetAnimManager")]
public class SpriteSheetAnimManager : KMonoBehaviour, IRenderEveryTick
{
	// Token: 0x06002317 RID: 8983 RVA: 0x000C3E24 File Offset: 0x000C2024
	public static void DestroyInstance()
	{
		SpriteSheetAnimManager.instance = null;
	}

	// Token: 0x06002318 RID: 8984 RVA: 0x000C3E2C File Offset: 0x000C202C
	protected override void OnPrefabInit()
	{
		SpriteSheetAnimManager.instance = this;
	}

	// Token: 0x06002319 RID: 8985 RVA: 0x000C3E34 File Offset: 0x000C2034
	protected override void OnSpawn()
	{
		for (int i = 0; i < this.sheets.Length; i++)
		{
			int key = Hash.SDBMLower(this.sheets[i].name);
			this.nameIndexMap[key] = new SpriteSheetAnimator(this.sheets[i]);
		}
	}

	// Token: 0x0600231A RID: 8986 RVA: 0x000C3E88 File Offset: 0x000C2088
	public void Play(string name, Vector3 pos, Vector2 size, Color32 colour)
	{
		int name_hash = Hash.SDBMLower(name);
		this.Play(name_hash, pos, Quaternion.identity, size, colour);
	}

	// Token: 0x0600231B RID: 8987 RVA: 0x000C3EAC File Offset: 0x000C20AC
	public void Play(string name, Vector3 pos, Quaternion rotation, Vector2 size, Color32 colour)
	{
		int name_hash = Hash.SDBMLower(name);
		this.Play(name_hash, pos, rotation, size, colour);
	}

	// Token: 0x0600231C RID: 8988 RVA: 0x000C3ECD File Offset: 0x000C20CD
	public void Play(int name_hash, Vector3 pos, Quaternion rotation, Vector2 size, Color32 colour)
	{
		this.nameIndexMap[name_hash].Play(pos, rotation, size, colour);
	}

	// Token: 0x0600231D RID: 8989 RVA: 0x000C3EEB File Offset: 0x000C20EB
	public void RenderEveryTick(float dt)
	{
		this.UpdateAnims(dt);
		this.Render();
	}

	// Token: 0x0600231E RID: 8990 RVA: 0x000C3EFC File Offset: 0x000C20FC
	public void UpdateAnims(float dt)
	{
		foreach (KeyValuePair<int, SpriteSheetAnimator> keyValuePair in this.nameIndexMap)
		{
			keyValuePair.Value.UpdateAnims(dt);
		}
	}

	// Token: 0x0600231F RID: 8991 RVA: 0x000C3F58 File Offset: 0x000C2158
	public void Render()
	{
		Vector3 zero = Vector3.zero;
		foreach (KeyValuePair<int, SpriteSheetAnimator> keyValuePair in this.nameIndexMap)
		{
			keyValuePair.Value.Render();
		}
	}

	// Token: 0x06002320 RID: 8992 RVA: 0x000C3FB8 File Offset: 0x000C21B8
	public SpriteSheetAnimator GetSpriteSheetAnimator(HashedString name)
	{
		return this.nameIndexMap[name.HashValue];
	}

	// Token: 0x040013F6 RID: 5110
	public const float SECONDS_PER_FRAME = 0.033333335f;

	// Token: 0x040013F7 RID: 5111
	[SerializeField]
	private SpriteSheet[] sheets;

	// Token: 0x040013F8 RID: 5112
	private Dictionary<int, SpriteSheetAnimator> nameIndexMap = new Dictionary<int, SpriteSheetAnimator>();

	// Token: 0x040013F9 RID: 5113
	public static SpriteSheetAnimManager instance;
}
