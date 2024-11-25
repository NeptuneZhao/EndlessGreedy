using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A9D RID: 2717
[AddComponentMenu("KMonoBehaviour/scripts/SituationalAnim")]
public class SituationalAnim : KMonoBehaviour
{
	// Token: 0x06004FF2 RID: 20466 RVA: 0x001CC250 File Offset: 0x001CA450
	protected override void OnSpawn()
	{
		base.OnSpawn();
		SituationalAnim.Situation situation = this.GetSituation();
		DebugUtil.LogArgs(new object[]
		{
			"Situation is",
			situation
		});
		this.SetAnimForSituation(situation);
	}

	// Token: 0x06004FF3 RID: 20467 RVA: 0x001CC290 File Offset: 0x001CA490
	private void SetAnimForSituation(SituationalAnim.Situation situation)
	{
		foreach (global::Tuple<SituationalAnim.Situation, string> tuple in this.anims)
		{
			if ((tuple.first & situation) == tuple.first)
			{
				DebugUtil.LogArgs(new object[]
				{
					"Chose Anim",
					tuple.first,
					tuple.second
				});
				this.SetAnim(tuple.second);
				break;
			}
		}
	}

	// Token: 0x06004FF4 RID: 20468 RVA: 0x001CC324 File Offset: 0x001CA524
	private void SetAnim(string animName)
	{
		base.GetComponent<KBatchedAnimController>().Play(animName, KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06004FF5 RID: 20469 RVA: 0x001CC344 File Offset: 0x001CA544
	private SituationalAnim.Situation GetSituation()
	{
		SituationalAnim.Situation situation = (SituationalAnim.Situation)0;
		Extents extents = base.GetComponent<Building>().GetExtents();
		int x = extents.x;
		int num = extents.x + extents.width - 1;
		int y = extents.y;
		int num2 = extents.y + extents.height - 1;
		if (this.DoesSatisfy(this.GetSatisfactionForEdge(x, num, y - 1, y - 1), this.mustSatisfy))
		{
			situation |= SituationalAnim.Situation.Bottom;
		}
		if (this.DoesSatisfy(this.GetSatisfactionForEdge(x - 1, x - 1, y, num2), this.mustSatisfy))
		{
			situation |= SituationalAnim.Situation.Left;
		}
		if (this.DoesSatisfy(this.GetSatisfactionForEdge(x, num, num2 + 1, num2 + 1), this.mustSatisfy))
		{
			situation |= SituationalAnim.Situation.Top;
		}
		if (this.DoesSatisfy(this.GetSatisfactionForEdge(num + 1, num + 1, y, num2), this.mustSatisfy))
		{
			situation |= SituationalAnim.Situation.Right;
		}
		return situation;
	}

	// Token: 0x06004FF6 RID: 20470 RVA: 0x001CC418 File Offset: 0x001CA618
	private bool DoesSatisfy(SituationalAnim.MustSatisfy result, SituationalAnim.MustSatisfy requirement)
	{
		if (requirement == SituationalAnim.MustSatisfy.All)
		{
			return result == SituationalAnim.MustSatisfy.All;
		}
		if (requirement == SituationalAnim.MustSatisfy.Any)
		{
			return result > SituationalAnim.MustSatisfy.None;
		}
		return result == SituationalAnim.MustSatisfy.None;
	}

	// Token: 0x06004FF7 RID: 20471 RVA: 0x001CC430 File Offset: 0x001CA630
	private SituationalAnim.MustSatisfy GetSatisfactionForEdge(int minx, int maxx, int miny, int maxy)
	{
		bool flag = false;
		bool flag2 = true;
		for (int i = minx; i <= maxx; i++)
		{
			for (int j = miny; j <= maxy; j++)
			{
				int arg = Grid.XYToCell(i, j);
				if (this.test(arg))
				{
					flag = true;
				}
				else
				{
					flag2 = false;
				}
			}
		}
		if (flag2)
		{
			return SituationalAnim.MustSatisfy.All;
		}
		if (flag)
		{
			return SituationalAnim.MustSatisfy.Any;
		}
		return SituationalAnim.MustSatisfy.None;
	}

	// Token: 0x0400351B RID: 13595
	public List<global::Tuple<SituationalAnim.Situation, string>> anims;

	// Token: 0x0400351C RID: 13596
	public Func<int, bool> test;

	// Token: 0x0400351D RID: 13597
	public SituationalAnim.MustSatisfy mustSatisfy;

	// Token: 0x02001ACF RID: 6863
	[Flags]
	public enum Situation
	{
		// Token: 0x04007DD0 RID: 32208
		Left = 1,
		// Token: 0x04007DD1 RID: 32209
		Right = 2,
		// Token: 0x04007DD2 RID: 32210
		Top = 4,
		// Token: 0x04007DD3 RID: 32211
		Bottom = 8
	}

	// Token: 0x02001AD0 RID: 6864
	public enum MustSatisfy
	{
		// Token: 0x04007DD5 RID: 32213
		None,
		// Token: 0x04007DD6 RID: 32214
		Any,
		// Token: 0x04007DD7 RID: 32215
		All
	}
}
