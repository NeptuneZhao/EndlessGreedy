using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000669 RID: 1641
[AddComponentMenu("KMonoBehaviour/scripts/BubbleManager")]
public class BubbleManager : KMonoBehaviour, ISim33ms, IRenderEveryTick
{
	// Token: 0x0600287B RID: 10363 RVA: 0x000E540E File Offset: 0x000E360E
	public static void DestroyInstance()
	{
		BubbleManager.instance = null;
	}

	// Token: 0x0600287C RID: 10364 RVA: 0x000E5416 File Offset: 0x000E3616
	protected override void OnPrefabInit()
	{
		BubbleManager.instance = this;
	}

	// Token: 0x0600287D RID: 10365 RVA: 0x000E5420 File Offset: 0x000E3620
	public void SpawnBubble(Vector2 position, Vector2 velocity, SimHashes element, float mass, float temperature)
	{
		BubbleManager.Bubble item = new BubbleManager.Bubble
		{
			position = position,
			velocity = velocity,
			element = element,
			temperature = temperature,
			mass = mass
		};
		this.bubbles.Add(item);
	}

	// Token: 0x0600287E RID: 10366 RVA: 0x000E5470 File Offset: 0x000E3670
	public void Sim33ms(float dt)
	{
		ListPool<BubbleManager.Bubble, BubbleManager>.PooledList pooledList = ListPool<BubbleManager.Bubble, BubbleManager>.Allocate();
		ListPool<BubbleManager.Bubble, BubbleManager>.PooledList pooledList2 = ListPool<BubbleManager.Bubble, BubbleManager>.Allocate();
		foreach (BubbleManager.Bubble bubble in this.bubbles)
		{
			bubble.position += bubble.velocity * dt;
			bubble.elapsedTime += dt;
			int num = Grid.PosToCell(bubble.position);
			if (!Grid.IsVisiblyInLiquid(bubble.position) || Grid.Element[num].id == bubble.element)
			{
				pooledList2.Add(bubble);
			}
			else
			{
				pooledList.Add(bubble);
			}
		}
		foreach (BubbleManager.Bubble bubble2 in pooledList2)
		{
			SimMessages.AddRemoveSubstance(Grid.PosToCell(bubble2.position), bubble2.element, CellEventLogger.Instance.FallingWaterAddToSim, bubble2.mass, bubble2.temperature, byte.MaxValue, 0, true, -1);
		}
		this.bubbles.Clear();
		this.bubbles.AddRange(pooledList);
		pooledList2.Recycle();
		pooledList.Recycle();
	}

	// Token: 0x0600287F RID: 10367 RVA: 0x000E55C8 File Offset: 0x000E37C8
	public void RenderEveryTick(float dt)
	{
		ListPool<SpriteSheetAnimator.AnimInfo, BubbleManager>.PooledList pooledList = ListPool<SpriteSheetAnimator.AnimInfo, BubbleManager>.Allocate();
		SpriteSheetAnimator spriteSheetAnimator = SpriteSheetAnimManager.instance.GetSpriteSheetAnimator("liquid_splash1");
		foreach (BubbleManager.Bubble bubble in this.bubbles)
		{
			SpriteSheetAnimator.AnimInfo item = new SpriteSheetAnimator.AnimInfo
			{
				frame = spriteSheetAnimator.GetFrameFromElapsedTimeLooping(bubble.elapsedTime),
				elapsedTime = bubble.elapsedTime,
				pos = new Vector3(bubble.position.x, bubble.position.y, 0f),
				rotation = Quaternion.identity,
				size = Vector2.one,
				colour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
			};
			pooledList.Add(item);
		}
		pooledList.Recycle();
	}

	// Token: 0x04001745 RID: 5957
	public static BubbleManager instance;

	// Token: 0x04001746 RID: 5958
	private List<BubbleManager.Bubble> bubbles = new List<BubbleManager.Bubble>();

	// Token: 0x0200144F RID: 5199
	private struct Bubble
	{
		// Token: 0x0400695B RID: 26971
		public Vector2 position;

		// Token: 0x0400695C RID: 26972
		public Vector2 velocity;

		// Token: 0x0400695D RID: 26973
		public float elapsedTime;

		// Token: 0x0400695E RID: 26974
		public int frame;

		// Token: 0x0400695F RID: 26975
		public SimHashes element;

		// Token: 0x04006960 RID: 26976
		public float temperature;

		// Token: 0x04006961 RID: 26977
		public float mass;
	}
}
