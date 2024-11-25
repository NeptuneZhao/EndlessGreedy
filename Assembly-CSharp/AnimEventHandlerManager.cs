using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02000526 RID: 1318
public class AnimEventHandlerManager : KMonoBehaviour
{
	// Token: 0x17000118 RID: 280
	// (get) Token: 0x06001DA1 RID: 7585 RVA: 0x000A486D File Offset: 0x000A2A6D
	// (set) Token: 0x06001DA2 RID: 7586 RVA: 0x000A4874 File Offset: 0x000A2A74
	public static AnimEventHandlerManager Instance { get; private set; }

	// Token: 0x06001DA3 RID: 7587 RVA: 0x000A487C File Offset: 0x000A2A7C
	public static void DestroyInstance()
	{
		AnimEventHandlerManager.Instance = null;
	}

	// Token: 0x06001DA4 RID: 7588 RVA: 0x000A4884 File Offset: 0x000A2A84
	protected override void OnPrefabInit()
	{
		AnimEventHandlerManager.Instance = this;
		this.handlers = new List<AnimEventHandler>();
	}

	// Token: 0x06001DA5 RID: 7589 RVA: 0x000A4897 File Offset: 0x000A2A97
	public void Add(AnimEventHandler handler)
	{
		this.handlers.Add(handler);
	}

	// Token: 0x06001DA6 RID: 7590 RVA: 0x000A48A5 File Offset: 0x000A2AA5
	public void Remove(AnimEventHandler handler)
	{
		this.handlers.Remove(handler);
	}

	// Token: 0x06001DA7 RID: 7591 RVA: 0x000A48B4 File Offset: 0x000A2AB4
	private bool IsVisibleToZoom()
	{
		return !(Game.MainCamera == null) && Game.MainCamera.orthographicSize < 40f;
	}

	// Token: 0x06001DA8 RID: 7592 RVA: 0x000A48D8 File Offset: 0x000A2AD8
	public void LateUpdate()
	{
		if (!this.IsVisibleToZoom())
		{
			return;
		}
		AnimEventHandlerManager.<>c__DisplayClass11_0 CS$<>8__locals1;
		Grid.GetVisibleCellRangeInActiveWorld(out CS$<>8__locals1.min, out CS$<>8__locals1.max, 4, 1.5f);
		foreach (AnimEventHandler animEventHandler in this.handlers)
		{
			if (AnimEventHandlerManager.<LateUpdate>g__IsVisible|11_0(animEventHandler, ref CS$<>8__locals1))
			{
				animEventHandler.UpdateOffset();
			}
		}
	}

	// Token: 0x06001DAA RID: 7594 RVA: 0x000A4960 File Offset: 0x000A2B60
	[CompilerGenerated]
	internal static bool <LateUpdate>g__IsVisible|11_0(AnimEventHandler handler, ref AnimEventHandlerManager.<>c__DisplayClass11_0 A_1)
	{
		int num;
		int num2;
		Grid.CellToXY(handler.GetCachedCell(), out num, out num2);
		return num >= A_1.min.x && num2 >= A_1.min.y && num < A_1.max.x && num2 < A_1.max.y;
	}

	// Token: 0x040010A8 RID: 4264
	private const float HIDE_DISTANCE = 40f;

	// Token: 0x040010AA RID: 4266
	private List<AnimEventHandler> handlers;
}
