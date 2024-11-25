using System;
using UnityEngine;

namespace Rendering.World
{
	// Token: 0x02000E1A RID: 3610
	public struct Mask
	{
		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x06007310 RID: 29456 RVA: 0x002C1E4F File Offset: 0x002C004F
		// (set) Token: 0x06007311 RID: 29457 RVA: 0x002C1E57 File Offset: 0x002C0057
		public Vector2 UV0 { readonly get; private set; }

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x06007312 RID: 29458 RVA: 0x002C1E60 File Offset: 0x002C0060
		// (set) Token: 0x06007313 RID: 29459 RVA: 0x002C1E68 File Offset: 0x002C0068
		public Vector2 UV1 { readonly get; private set; }

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x06007314 RID: 29460 RVA: 0x002C1E71 File Offset: 0x002C0071
		// (set) Token: 0x06007315 RID: 29461 RVA: 0x002C1E79 File Offset: 0x002C0079
		public Vector2 UV2 { readonly get; private set; }

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x06007316 RID: 29462 RVA: 0x002C1E82 File Offset: 0x002C0082
		// (set) Token: 0x06007317 RID: 29463 RVA: 0x002C1E8A File Offset: 0x002C008A
		public Vector2 UV3 { readonly get; private set; }

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x06007318 RID: 29464 RVA: 0x002C1E93 File Offset: 0x002C0093
		// (set) Token: 0x06007319 RID: 29465 RVA: 0x002C1E9B File Offset: 0x002C009B
		public bool IsOpaque { readonly get; private set; }

		// Token: 0x0600731A RID: 29466 RVA: 0x002C1EA4 File Offset: 0x002C00A4
		public Mask(TextureAtlas atlas, int texture_idx, bool transpose, bool flip_x, bool flip_y, bool is_opaque)
		{
			this = default(Mask);
			this.atlas = atlas;
			this.texture_idx = texture_idx;
			this.transpose = transpose;
			this.flip_x = flip_x;
			this.flip_y = flip_y;
			this.atlas_offset = 0;
			this.IsOpaque = is_opaque;
			this.Refresh();
		}

		// Token: 0x0600731B RID: 29467 RVA: 0x002C1EF2 File Offset: 0x002C00F2
		public void SetOffset(int offset)
		{
			this.atlas_offset = offset;
			this.Refresh();
		}

		// Token: 0x0600731C RID: 29468 RVA: 0x002C1F04 File Offset: 0x002C0104
		public void Refresh()
		{
			int num = this.atlas_offset * 4 + this.atlas_offset;
			if (num + this.texture_idx >= this.atlas.items.Length)
			{
				num = 0;
			}
			Vector4 uvBox = this.atlas.items[num + this.texture_idx].uvBox;
			Vector2 zero = Vector2.zero;
			Vector2 zero2 = Vector2.zero;
			Vector2 zero3 = Vector2.zero;
			Vector2 zero4 = Vector2.zero;
			if (this.transpose)
			{
				float x = uvBox.x;
				float x2 = uvBox.z;
				if (this.flip_x)
				{
					x = uvBox.z;
					x2 = uvBox.x;
				}
				zero.x = x;
				zero2.x = x;
				zero3.x = x2;
				zero4.x = x2;
				float y = uvBox.y;
				float y2 = uvBox.w;
				if (this.flip_y)
				{
					y = uvBox.w;
					y2 = uvBox.y;
				}
				zero.y = y;
				zero2.y = y2;
				zero3.y = y;
				zero4.y = y2;
			}
			else
			{
				float x3 = uvBox.x;
				float x4 = uvBox.z;
				if (this.flip_x)
				{
					x3 = uvBox.z;
					x4 = uvBox.x;
				}
				zero.x = x3;
				zero2.x = x4;
				zero3.x = x3;
				zero4.x = x4;
				float y3 = uvBox.y;
				float y4 = uvBox.w;
				if (this.flip_y)
				{
					y3 = uvBox.w;
					y4 = uvBox.y;
				}
				zero.y = y4;
				zero2.y = y4;
				zero3.y = y3;
				zero4.y = y3;
			}
			this.UV0 = zero;
			this.UV1 = zero2;
			this.UV2 = zero3;
			this.UV3 = zero4;
		}

		// Token: 0x04004F49 RID: 20297
		private TextureAtlas atlas;

		// Token: 0x04004F4A RID: 20298
		private int texture_idx;

		// Token: 0x04004F4B RID: 20299
		private bool transpose;

		// Token: 0x04004F4C RID: 20300
		private bool flip_x;

		// Token: 0x04004F4D RID: 20301
		private bool flip_y;

		// Token: 0x04004F4E RID: 20302
		private int atlas_offset;

		// Token: 0x04004F4F RID: 20303
		private const int TILES_PER_SET = 4;
	}
}
