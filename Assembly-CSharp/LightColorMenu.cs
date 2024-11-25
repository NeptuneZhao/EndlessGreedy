using System;
using UnityEngine;

// Token: 0x0200057D RID: 1405
[AddComponentMenu("KMonoBehaviour/scripts/LightColorMenu")]
public class LightColorMenu : KMonoBehaviour
{
	// Token: 0x060020A4 RID: 8356 RVA: 0x000B65D2 File Offset: 0x000B47D2
	protected override void OnPrefabInit()
	{
		base.Subscribe<LightColorMenu>(493375141, LightColorMenu.OnRefreshUserMenuDelegate);
		this.SetColor(0);
	}

	// Token: 0x060020A5 RID: 8357 RVA: 0x000B65EC File Offset: 0x000B47EC
	private void OnRefreshUserMenu(object data)
	{
		if (this.lightColors.Length != 0)
		{
			int num = this.lightColors.Length;
			for (int i = 0; i < num; i++)
			{
				if (i != this.currentColor)
				{
					int new_color = i;
					Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo(this.lightColors[i].name, this.lightColors[i].name, delegate()
					{
						this.SetColor(new_color);
					}, global::Action.NumActions, null, null, null, "", true), 1f);
				}
			}
		}
	}

	// Token: 0x060020A6 RID: 8358 RVA: 0x000B6694 File Offset: 0x000B4894
	private void SetColor(int color_index)
	{
		if (this.lightColors.Length != 0 && color_index < this.lightColors.Length)
		{
			Light2D[] componentsInChildren = base.GetComponentsInChildren<Light2D>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Color = this.lightColors[color_index].color;
			}
			MeshRenderer[] componentsInChildren2 = base.GetComponentsInChildren<MeshRenderer>(true);
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				foreach (Material material in componentsInChildren2[i].materials)
				{
					if (material.name.StartsWith("matScriptedGlow01"))
					{
						material.color = this.lightColors[color_index].color;
					}
				}
			}
		}
		this.currentColor = color_index;
	}

	// Token: 0x04001256 RID: 4694
	public LightColorMenu.LightColor[] lightColors;

	// Token: 0x04001257 RID: 4695
	private int currentColor;

	// Token: 0x04001258 RID: 4696
	private static readonly EventSystem.IntraObjectHandler<LightColorMenu> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<LightColorMenu>(delegate(LightColorMenu component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x02001372 RID: 4978
	[Serializable]
	public struct LightColor
	{
		// Token: 0x0600872D RID: 34605 RVA: 0x0032AE23 File Offset: 0x00329023
		public LightColor(string name, Color color)
		{
			this.name = name;
			this.color = color;
		}

		// Token: 0x040066A5 RID: 26277
		public string name;

		// Token: 0x040066A6 RID: 26278
		public Color color;
	}
}
