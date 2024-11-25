using System;
using System.Collections.Generic;

// Token: 0x02000606 RID: 1542
public class DevPanelList
{
	// Token: 0x06002606 RID: 9734 RVA: 0x000D2AA8 File Offset: 0x000D0CA8
	public DevPanel AddPanelFor<T>() where T : DevTool, new()
	{
		return this.AddPanelFor(Activator.CreateInstance<T>());
	}

	// Token: 0x06002607 RID: 9735 RVA: 0x000D2ABC File Offset: 0x000D0CBC
	public DevPanel AddPanelFor(DevTool devTool)
	{
		DevPanel devPanel = new DevPanel(devTool, this);
		this.activePanels.Add(devPanel);
		return devPanel;
	}

	// Token: 0x06002608 RID: 9736 RVA: 0x000D2AE0 File Offset: 0x000D0CE0
	public Option<T> GetDevTool<T>() where T : DevTool
	{
		foreach (DevPanel devPanel in this.activePanels)
		{
			T t = devPanel.GetCurrentDevTool() as T;
			if (t != null)
			{
				return t;
			}
		}
		return Option.None;
	}

	// Token: 0x06002609 RID: 9737 RVA: 0x000D2B58 File Offset: 0x000D0D58
	public T AddOrGetDevTool<T>() where T : DevTool, new()
	{
		bool flag;
		T t;
		this.GetDevTool<T>().Deconstruct(out flag, out t);
		bool flag2 = flag;
		T t2 = t;
		if (!flag2)
		{
			t2 = Activator.CreateInstance<T>();
			this.AddPanelFor(t2);
		}
		return t2;
	}

	// Token: 0x0600260A RID: 9738 RVA: 0x000D2B90 File Offset: 0x000D0D90
	public void ClosePanel(DevPanel panel)
	{
		if (this.activePanels.Remove(panel))
		{
			panel.Internal_Uninit();
		}
	}

	// Token: 0x0600260B RID: 9739 RVA: 0x000D2BA8 File Offset: 0x000D0DA8
	public void Render()
	{
		if (this.activePanels.Count == 0)
		{
			return;
		}
		using (ListPool<DevPanel, DevPanelList>.PooledList pooledList = ListPool<DevPanel, DevPanelList>.Allocate())
		{
			for (int i = 0; i < this.activePanels.Count; i++)
			{
				DevPanel devPanel = this.activePanels[i];
				devPanel.RenderPanel();
				if (devPanel.isRequestingToClose)
				{
					pooledList.Add(devPanel);
				}
			}
			foreach (DevPanel panel in pooledList)
			{
				this.ClosePanel(panel);
			}
		}
	}

	// Token: 0x0600260C RID: 9740 RVA: 0x000D2C5C File Offset: 0x000D0E5C
	public void Internal_InitPanelId(Type initialDevToolType, out string panelId, out uint idPostfixNumber)
	{
		idPostfixNumber = this.Internal_GetUniqueIdPostfix(initialDevToolType);
		panelId = initialDevToolType.Name + idPostfixNumber.ToString();
	}

	// Token: 0x0600260D RID: 9741 RVA: 0x000D2C7C File Offset: 0x000D0E7C
	public uint Internal_GetUniqueIdPostfix(Type initialDevToolType)
	{
		uint result;
		using (HashSetPool<uint, DevPanelList>.PooledHashSet pooledHashSet = HashSetPool<uint, DevPanelList>.Allocate())
		{
			foreach (DevPanel devPanel in this.activePanels)
			{
				if (!(devPanel.initialDevToolType != initialDevToolType))
				{
					pooledHashSet.Add(devPanel.idPostfixNumber);
				}
			}
			for (uint num = 0U; num < 100U; num += 1U)
			{
				if (!pooledHashSet.Contains(num))
				{
					return num;
				}
			}
			Debug.Assert(false, "Something went wrong, this should only assert if there's over 100 of the same type of debug window");
			uint num2 = this.fallbackUniqueIdPostfixNumber;
			this.fallbackUniqueIdPostfixNumber = num2 + 1U;
			result = num2;
		}
		return result;
	}

	// Token: 0x040015AF RID: 5551
	private List<DevPanel> activePanels = new List<DevPanel>();

	// Token: 0x040015B0 RID: 5552
	private uint fallbackUniqueIdPostfixNumber = 300U;
}
