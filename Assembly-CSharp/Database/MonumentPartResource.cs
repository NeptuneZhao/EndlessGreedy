using System;
using UnityEngine;

namespace Database
{
	// Token: 0x02000E69 RID: 3689
	public class MonumentPartResource : PermitResource
	{
		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x0600749F RID: 29855 RVA: 0x002D7691 File Offset: 0x002D5891
		// (set) Token: 0x060074A0 RID: 29856 RVA: 0x002D7699 File Offset: 0x002D5899
		public KAnimFile AnimFile { get; private set; }

		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x060074A1 RID: 29857 RVA: 0x002D76A2 File Offset: 0x002D58A2
		// (set) Token: 0x060074A2 RID: 29858 RVA: 0x002D76AA File Offset: 0x002D58AA
		public string SymbolName { get; private set; }

		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x060074A3 RID: 29859 RVA: 0x002D76B3 File Offset: 0x002D58B3
		// (set) Token: 0x060074A4 RID: 29860 RVA: 0x002D76BB File Offset: 0x002D58BB
		public string State { get; private set; }

		// Token: 0x060074A5 RID: 29861 RVA: 0x002D76C4 File Offset: 0x002D58C4
		public MonumentPartResource(string id, string name, string desc, PermitRarity rarity, string animFilename, string state, string symbolName, MonumentPartResource.Part part, string[] dlcIds) : base(id, name, desc, PermitCategory.Artwork, rarity, dlcIds)
		{
			this.AnimFile = Assets.GetAnim(animFilename);
			this.SymbolName = symbolName;
			this.State = state;
			this.part = part;
		}

		// Token: 0x060074A6 RID: 29862 RVA: 0x002D76FF File Offset: 0x002D58FF
		public global::Tuple<Sprite, Color> GetUISprite()
		{
			Sprite sprite = Assets.GetSprite("unknown");
			return new global::Tuple<Sprite, Color>(sprite, (sprite != null) ? Color.white : Color.clear);
		}

		// Token: 0x060074A7 RID: 29863 RVA: 0x002D772C File Offset: 0x002D592C
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			PermitPresentationInfo result = default(PermitPresentationInfo);
			result.sprite = this.GetUISprite().first;
			result.SetFacadeForText("_monument part");
			return result;
		}

		// Token: 0x04005402 RID: 21506
		public MonumentPartResource.Part part;

		// Token: 0x02001F6C RID: 8044
		public enum Part
		{
			// Token: 0x04008E85 RID: 36485
			Bottom,
			// Token: 0x04008E86 RID: 36486
			Middle,
			// Token: 0x04008E87 RID: 36487
			Top
		}
	}
}
