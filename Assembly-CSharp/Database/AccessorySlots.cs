using System;

namespace Database
{
	// Token: 0x02000E41 RID: 3649
	public class AccessorySlots : ResourceSet<AccessorySlot>
	{
		// Token: 0x06007419 RID: 29721 RVA: 0x002C56F0 File Offset: 0x002C38F0
		public AccessorySlots(ResourceSet parent) : base("AccessorySlots", parent)
		{
			parent = Db.Get().Accessories;
			KAnimFile anim = Assets.GetAnim("head_swap_kanim");
			KAnimFile anim2 = Assets.GetAnim("body_comp_default_kanim");
			KAnimFile anim3 = Assets.GetAnim("body_swap_kanim");
			KAnimFile anim4 = Assets.GetAnim("hair_swap_kanim");
			KAnimFile anim5 = Assets.GetAnim("hat_swap_kanim");
			this.Eyes = new AccessorySlot("Eyes", this, anim, 0);
			this.Hair = new AccessorySlot("Hair", this, anim4, 0);
			this.HeadShape = new AccessorySlot("HeadShape", this, anim, 0);
			this.Mouth = new AccessorySlot("Mouth", this, anim, 0);
			this.Hat = new AccessorySlot("Hat", this, anim5, 4);
			this.HatHair = new AccessorySlot("Hat_Hair", this, anim4, 0);
			this.HeadEffects = new AccessorySlot("HeadFX", this, anim, 0);
			this.Body = new AccessorySlot("Torso", this, new KAnimHashedString("torso"), anim3, null, 0);
			this.Arm = new AccessorySlot("Arm_Sleeve", this, new KAnimHashedString("arm_sleeve"), anim3, null, 0);
			this.ArmLower = new AccessorySlot("Arm_Lower_Sleeve", this, new KAnimHashedString("arm_lower_sleeve"), anim3, null, 0);
			this.Belt = new AccessorySlot("Belt", this, new KAnimHashedString("belt"), anim2, null, 0);
			this.Neck = new AccessorySlot("Neck", this, new KAnimHashedString("neck"), anim2, null, 0);
			this.Pelvis = new AccessorySlot("Pelvis", this, new KAnimHashedString("pelvis"), anim2, null, 0);
			this.Foot = new AccessorySlot("Foot", this, new KAnimHashedString("foot"), anim2, Assets.GetAnim("shoes_basic_black_kanim"), 0);
			this.Leg = new AccessorySlot("Leg", this, new KAnimHashedString("leg"), anim2, null, 0);
			this.Necklace = new AccessorySlot("Necklace", this, new KAnimHashedString("necklace"), anim2, null, 0);
			this.Cuff = new AccessorySlot("Cuff", this, new KAnimHashedString("cuff"), anim2, null, 0);
			this.Hand = new AccessorySlot("Hand", this, new KAnimHashedString("hand_paint"), anim2, null, 0);
			this.Skirt = new AccessorySlot("Skirt", this, new KAnimHashedString("skirt"), anim3, null, 0);
			this.ArmLowerSkin = new AccessorySlot("Arm_Lower", this, new KAnimHashedString("arm_lower"), anim3, null, 0);
			this.ArmUpperSkin = new AccessorySlot("Arm_Upper", this, new KAnimHashedString("arm_upper"), anim3, null, 0);
			this.LegSkin = new AccessorySlot("Leg_Skin", this, new KAnimHashedString("leg_skin"), anim3, null, 0);
			foreach (AccessorySlot accessorySlot in this.resources)
			{
				accessorySlot.AddAccessories(accessorySlot.AnimFile, parent);
			}
			Db.Get().Accessories.AddCustomAccessories(Assets.GetAnim("body_lonelyminion_kanim"), parent, this);
		}

		// Token: 0x0600741A RID: 29722 RVA: 0x002C5A28 File Offset: 0x002C3C28
		public AccessorySlot Find(KAnimHashedString symbol_name)
		{
			foreach (AccessorySlot accessorySlot in Db.Get().AccessorySlots.resources)
			{
				if (symbol_name == accessorySlot.targetSymbolId)
				{
					return accessorySlot;
				}
			}
			return null;
		}

		// Token: 0x04004FB9 RID: 20409
		public AccessorySlot Eyes;

		// Token: 0x04004FBA RID: 20410
		public AccessorySlot Hair;

		// Token: 0x04004FBB RID: 20411
		public AccessorySlot HeadShape;

		// Token: 0x04004FBC RID: 20412
		public AccessorySlot Mouth;

		// Token: 0x04004FBD RID: 20413
		public AccessorySlot Body;

		// Token: 0x04004FBE RID: 20414
		public AccessorySlot Arm;

		// Token: 0x04004FBF RID: 20415
		public AccessorySlot ArmLower;

		// Token: 0x04004FC0 RID: 20416
		public AccessorySlot Hat;

		// Token: 0x04004FC1 RID: 20417
		public AccessorySlot HatHair;

		// Token: 0x04004FC2 RID: 20418
		public AccessorySlot HeadEffects;

		// Token: 0x04004FC3 RID: 20419
		public AccessorySlot Belt;

		// Token: 0x04004FC4 RID: 20420
		public AccessorySlot Neck;

		// Token: 0x04004FC5 RID: 20421
		public AccessorySlot Pelvis;

		// Token: 0x04004FC6 RID: 20422
		public AccessorySlot Leg;

		// Token: 0x04004FC7 RID: 20423
		public AccessorySlot Foot;

		// Token: 0x04004FC8 RID: 20424
		public AccessorySlot Skirt;

		// Token: 0x04004FC9 RID: 20425
		public AccessorySlot Necklace;

		// Token: 0x04004FCA RID: 20426
		public AccessorySlot Cuff;

		// Token: 0x04004FCB RID: 20427
		public AccessorySlot Hand;

		// Token: 0x04004FCC RID: 20428
		public AccessorySlot ArmLowerSkin;

		// Token: 0x04004FCD RID: 20429
		public AccessorySlot ArmUpperSkin;

		// Token: 0x04004FCE RID: 20430
		public AccessorySlot LegSkin;
	}
}
