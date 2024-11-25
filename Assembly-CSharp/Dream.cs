using System;
using UnityEngine;

// Token: 0x02000860 RID: 2144
public class Dream : Resource
{
	// Token: 0x06003BBE RID: 15294 RVA: 0x00149018 File Offset: 0x00147218
	public Dream(string id, ResourceSet parent, string background, string[] icons_sprite_names) : base(id, parent, null)
	{
		this.Icons = new Sprite[icons_sprite_names.Length];
		this.BackgroundAnim = background;
		for (int i = 0; i < icons_sprite_names.Length; i++)
		{
			this.Icons[i] = Assets.GetSprite(icons_sprite_names[i]);
		}
	}

	// Token: 0x06003BBF RID: 15295 RVA: 0x00149074 File Offset: 0x00147274
	public Dream(string id, ResourceSet parent, string background, string[] icons_sprite_names, float durationPerImage) : this(id, parent, background, icons_sprite_names)
	{
		this.secondPerImage = durationPerImage;
	}

	// Token: 0x04002415 RID: 9237
	public string BackgroundAnim;

	// Token: 0x04002416 RID: 9238
	public Sprite[] Icons;

	// Token: 0x04002417 RID: 9239
	public float secondPerImage = 2.4f;
}
