using System;
using System.Collections.Generic;
using Klei.AI;

// Token: 0x02000890 RID: 2192
public class EquipmentDef : Def
{
	// Token: 0x17000481 RID: 1153
	// (get) Token: 0x06003D74 RID: 15732 RVA: 0x00154090 File Offset: 0x00152290
	public override string Name
	{
		get
		{
			return Strings.Get("STRINGS.EQUIPMENT.PREFABS." + this.Id.ToUpper() + ".NAME");
		}
	}

	// Token: 0x17000482 RID: 1154
	// (get) Token: 0x06003D75 RID: 15733 RVA: 0x001540B6 File Offset: 0x001522B6
	public string Desc
	{
		get
		{
			return Strings.Get("STRINGS.EQUIPMENT.PREFABS." + this.Id.ToUpper() + ".DESC");
		}
	}

	// Token: 0x17000483 RID: 1155
	// (get) Token: 0x06003D76 RID: 15734 RVA: 0x001540DC File Offset: 0x001522DC
	public string Effect
	{
		get
		{
			return Strings.Get("STRINGS.EQUIPMENT.PREFABS." + this.Id.ToUpper() + ".EFFECT");
		}
	}

	// Token: 0x17000484 RID: 1156
	// (get) Token: 0x06003D77 RID: 15735 RVA: 0x00154102 File Offset: 0x00152302
	public string GenericName
	{
		get
		{
			return Strings.Get("STRINGS.EQUIPMENT.PREFABS." + this.Id.ToUpper() + ".GENERICNAME");
		}
	}

	// Token: 0x17000485 RID: 1157
	// (get) Token: 0x06003D78 RID: 15736 RVA: 0x00154128 File Offset: 0x00152328
	public string WornName
	{
		get
		{
			return Strings.Get("STRINGS.EQUIPMENT.PREFABS." + this.Id.ToUpper() + ".WORN_NAME");
		}
	}

	// Token: 0x17000486 RID: 1158
	// (get) Token: 0x06003D79 RID: 15737 RVA: 0x0015414E File Offset: 0x0015234E
	public string WornDesc
	{
		get
		{
			return Strings.Get("STRINGS.EQUIPMENT.PREFABS." + this.Id.ToUpper() + ".WORN_DESC");
		}
	}

	// Token: 0x04002573 RID: 9587
	public string Id;

	// Token: 0x04002574 RID: 9588
	public string Slot;

	// Token: 0x04002575 RID: 9589
	public string FabricatorId;

	// Token: 0x04002576 RID: 9590
	public float FabricationTime;

	// Token: 0x04002577 RID: 9591
	public string RecipeTechUnlock;

	// Token: 0x04002578 RID: 9592
	public SimHashes OutputElement;

	// Token: 0x04002579 RID: 9593
	public Dictionary<string, float> InputElementMassMap;

	// Token: 0x0400257A RID: 9594
	public float Mass;

	// Token: 0x0400257B RID: 9595
	public KAnimFile Anim;

	// Token: 0x0400257C RID: 9596
	public string SnapOn;

	// Token: 0x0400257D RID: 9597
	public string SnapOn1;

	// Token: 0x0400257E RID: 9598
	public KAnimFile BuildOverride;

	// Token: 0x0400257F RID: 9599
	public int BuildOverridePriority;

	// Token: 0x04002580 RID: 9600
	public bool IsBody;

	// Token: 0x04002581 RID: 9601
	public List<AttributeModifier> AttributeModifiers;

	// Token: 0x04002582 RID: 9602
	public string RecipeDescription;

	// Token: 0x04002583 RID: 9603
	public List<Effect> EffectImmunites = new List<Effect>();

	// Token: 0x04002584 RID: 9604
	public Action<Equippable> OnEquipCallBack;

	// Token: 0x04002585 RID: 9605
	public Action<Equippable> OnUnequipCallBack;

	// Token: 0x04002586 RID: 9606
	public EntityTemplates.CollisionShape CollisionShape;

	// Token: 0x04002587 RID: 9607
	public float width;

	// Token: 0x04002588 RID: 9608
	public float height = 0.325f;

	// Token: 0x04002589 RID: 9609
	public Tag[] AdditionalTags;

	// Token: 0x0400258A RID: 9610
	public string wornID;

	// Token: 0x0400258B RID: 9611
	public List<Descriptor> additionalDescriptors = new List<Descriptor>();
}
