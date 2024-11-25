using System;
using UnityEngine;

// Token: 0x02000915 RID: 2325
public class PrebuildTool : InterfaceTool
{
	// Token: 0x06004363 RID: 17251 RVA: 0x0017EBE5 File Offset: 0x0017CDE5
	public static void DestroyInstance()
	{
		PrebuildTool.Instance = null;
	}

	// Token: 0x06004364 RID: 17252 RVA: 0x0017EBED File Offset: 0x0017CDED
	protected override void OnPrefabInit()
	{
		PrebuildTool.Instance = this;
	}

	// Token: 0x06004365 RID: 17253 RVA: 0x0017EBF5 File Offset: 0x0017CDF5
	protected override void OnActivateTool()
	{
		this.viewMode = this.def.ViewMode;
		base.OnActivateTool();
	}

	// Token: 0x06004366 RID: 17254 RVA: 0x0017EC0E File Offset: 0x0017CE0E
	public void Activate(BuildingDef def, string errorMessage)
	{
		this.def = def;
		PlayerController.Instance.ActivateTool(this);
		PrebuildToolHoverTextCard component = base.GetComponent<PrebuildToolHoverTextCard>();
		component.errorMessage = errorMessage;
		component.currentDef = def;
	}

	// Token: 0x06004367 RID: 17255 RVA: 0x0017EC35 File Offset: 0x0017CE35
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		UISounds.PlaySound(UISounds.Sound.Negative);
		base.OnLeftClickDown(cursor_pos);
	}

	// Token: 0x04002C58 RID: 11352
	public static PrebuildTool Instance;

	// Token: 0x04002C59 RID: 11353
	private BuildingDef def;
}
