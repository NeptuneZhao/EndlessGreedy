using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005BD RID: 1469
[AddComponentMenu("KMonoBehaviour/scripts/SnapOn")]
public class SnapOn : KMonoBehaviour
{
	// Token: 0x06002305 RID: 8965 RVA: 0x000C3468 File Offset: 0x000C1668
	protected override void OnPrefabInit()
	{
		this.kanimController = base.GetComponent<KAnimControllerBase>();
	}

	// Token: 0x06002306 RID: 8966 RVA: 0x000C3478 File Offset: 0x000C1678
	protected override void OnSpawn()
	{
		foreach (SnapOn.SnapPoint snapPoint in this.snapPoints)
		{
			if (snapPoint.automatic)
			{
				this.DoAttachSnapOn(snapPoint);
			}
		}
	}

	// Token: 0x06002307 RID: 8967 RVA: 0x000C34D4 File Offset: 0x000C16D4
	public void AttachSnapOnByName(string name)
	{
		foreach (SnapOn.SnapPoint snapPoint in this.snapPoints)
		{
			if (snapPoint.pointName == name)
			{
				HashedString context = base.GetComponent<AnimEventHandler>().GetContext();
				if (!context.IsValid || !snapPoint.context.IsValid || context == snapPoint.context)
				{
					this.DoAttachSnapOn(snapPoint);
				}
			}
		}
	}

	// Token: 0x06002308 RID: 8968 RVA: 0x000C3568 File Offset: 0x000C1768
	public void DetachSnapOnByName(string name)
	{
		foreach (SnapOn.SnapPoint snapPoint in this.snapPoints)
		{
			if (snapPoint.pointName == name)
			{
				HashedString context = base.GetComponent<AnimEventHandler>().GetContext();
				if (!context.IsValid || !snapPoint.context.IsValid || context == snapPoint.context)
				{
					base.GetComponent<SymbolOverrideController>().RemoveSymbolOverride(snapPoint.overrideSymbol, 5);
					this.kanimController.SetSymbolVisiblity(snapPoint.overrideSymbol, false);
					break;
				}
			}
		}
	}

	// Token: 0x06002309 RID: 8969 RVA: 0x000C3620 File Offset: 0x000C1820
	private void DoAttachSnapOn(SnapOn.SnapPoint point)
	{
		SnapOn.OverrideEntry overrideEntry = null;
		KAnimFile buildFile = point.buildFile;
		string symbol_name = "";
		if (this.overrideMap.TryGetValue(point.pointName, out overrideEntry))
		{
			buildFile = overrideEntry.buildFile;
			symbol_name = overrideEntry.symbolName;
		}
		KAnim.Build.Symbol symbol = SnapOn.GetSymbol(buildFile, symbol_name);
		base.GetComponent<SymbolOverrideController>().AddSymbolOverride(point.overrideSymbol, symbol, 5);
		this.kanimController.SetSymbolVisiblity(point.overrideSymbol, true);
	}

	// Token: 0x0600230A RID: 8970 RVA: 0x000C3694 File Offset: 0x000C1894
	private static KAnim.Build.Symbol GetSymbol(KAnimFile anim_file, string symbol_name)
	{
		KAnim.Build.Symbol result = anim_file.GetData().build.symbols[0];
		KAnimHashedString y = new KAnimHashedString(symbol_name);
		foreach (KAnim.Build.Symbol symbol in anim_file.GetData().build.symbols)
		{
			if (symbol.hash == y)
			{
				result = symbol;
				break;
			}
		}
		return result;
	}

	// Token: 0x0600230B RID: 8971 RVA: 0x000C36F5 File Offset: 0x000C18F5
	public void AddOverride(string point_name, KAnimFile build_override, string symbol_name)
	{
		this.overrideMap[point_name] = new SnapOn.OverrideEntry
		{
			buildFile = build_override,
			symbolName = symbol_name
		};
	}

	// Token: 0x0600230C RID: 8972 RVA: 0x000C3716 File Offset: 0x000C1916
	public void RemoveOverride(string point_name)
	{
		this.overrideMap.Remove(point_name);
	}

	// Token: 0x040013E7 RID: 5095
	private KAnimControllerBase kanimController;

	// Token: 0x040013E8 RID: 5096
	public List<SnapOn.SnapPoint> snapPoints = new List<SnapOn.SnapPoint>();

	// Token: 0x040013E9 RID: 5097
	private Dictionary<string, SnapOn.OverrideEntry> overrideMap = new Dictionary<string, SnapOn.OverrideEntry>();

	// Token: 0x020013B3 RID: 5043
	[Serializable]
	public class SnapPoint
	{
		// Token: 0x04006798 RID: 26520
		public string pointName;

		// Token: 0x04006799 RID: 26521
		public bool automatic = true;

		// Token: 0x0400679A RID: 26522
		public HashedString context;

		// Token: 0x0400679B RID: 26523
		public KAnimFile buildFile;

		// Token: 0x0400679C RID: 26524
		public HashedString overrideSymbol;
	}

	// Token: 0x020013B4 RID: 5044
	public class OverrideEntry
	{
		// Token: 0x0400679D RID: 26525
		public KAnimFile buildFile;

		// Token: 0x0400679E RID: 26526
		public string symbolName;
	}
}
