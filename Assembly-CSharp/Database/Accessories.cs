using System;

namespace Database
{
	// Token: 0x02000E40 RID: 3648
	public class Accessories : ResourceSet<Accessory>
	{
		// Token: 0x06007416 RID: 29718 RVA: 0x002C5560 File Offset: 0x002C3760
		public Accessories(ResourceSet parent) : base("Accessories", parent)
		{
		}

		// Token: 0x06007417 RID: 29719 RVA: 0x002C5570 File Offset: 0x002C3770
		public void AddAccessories(string id, KAnimFile anim_file)
		{
			if (anim_file != null)
			{
				KAnim.Build build = anim_file.GetData().build;
				for (int i = 0; i < build.symbols.Length; i++)
				{
					string text = HashCache.Get().Get(build.symbols[i].hash);
					AccessorySlot accessorySlot = Db.Get().AccessorySlots.Find(text);
					if (accessorySlot != null)
					{
						Accessory accessory = new Accessory(id + text, this, accessorySlot, anim_file.batchTag, build.symbols[i], anim_file, null);
						accessorySlot.accessories.Add(accessory);
						HashCache.Get().Add(accessory.IdHash.HashValue, accessory.Id);
					}
				}
			}
		}

		// Token: 0x06007418 RID: 29720 RVA: 0x002C5628 File Offset: 0x002C3828
		public void AddCustomAccessories(KAnimFile anim_file, ResourceSet parent, AccessorySlots slots)
		{
			if (anim_file != null)
			{
				KAnim.Build build = anim_file.GetData().build;
				for (int i = 0; i < build.symbols.Length; i++)
				{
					string symbol_name = HashCache.Get().Get(build.symbols[i].hash);
					AccessorySlot accessorySlot = slots.resources.Find((AccessorySlot slot) => symbol_name.IndexOf(slot.Id, 0, StringComparison.OrdinalIgnoreCase) != -1);
					if (accessorySlot != null)
					{
						Accessory accessory = new Accessory(symbol_name, parent, accessorySlot, anim_file.batchTag, build.symbols[i], anim_file, null);
						accessorySlot.accessories.Add(accessory);
						HashCache.Get().Add(accessory.IdHash.HashValue, accessory.Id);
					}
				}
			}
		}
	}
}
