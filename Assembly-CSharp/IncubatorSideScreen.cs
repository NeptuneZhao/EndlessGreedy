using System;
using UnityEngine;

// Token: 0x02000D73 RID: 3443
public class IncubatorSideScreen : ReceptacleSideScreen
{
	// Token: 0x06006C54 RID: 27732 RVA: 0x0028C05F File Offset: 0x0028A25F
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<EggIncubator>() != null;
	}

	// Token: 0x06006C55 RID: 27733 RVA: 0x0028C070 File Offset: 0x0028A270
	protected override void SetResultDescriptions(GameObject go)
	{
		string text = "";
		InfoDescription component = go.GetComponent<InfoDescription>();
		if (component)
		{
			text += component.description;
		}
		this.descriptionLabel.SetText(text);
	}

	// Token: 0x06006C56 RID: 27734 RVA: 0x0028C0AB File Offset: 0x0028A2AB
	protected override bool RequiresAvailableAmountToDeposit()
	{
		return false;
	}

	// Token: 0x06006C57 RID: 27735 RVA: 0x0028C0AE File Offset: 0x0028A2AE
	protected override Sprite GetEntityIcon(Tag prefabTag)
	{
		return Def.GetUISprite(Assets.GetPrefab(prefabTag), "ui", false).first;
	}

	// Token: 0x06006C58 RID: 27736 RVA: 0x0028C0C8 File Offset: 0x0028A2C8
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		EggIncubator incubator = target.GetComponent<EggIncubator>();
		this.continuousToggle.ChangeState(incubator.autoReplaceEntity ? 0 : 1);
		this.continuousToggle.onClick = delegate()
		{
			incubator.autoReplaceEntity = !incubator.autoReplaceEntity;
			this.continuousToggle.ChangeState(incubator.autoReplaceEntity ? 0 : 1);
		};
	}

	// Token: 0x040049DE RID: 18910
	public DescriptorPanel RequirementsDescriptorPanel;

	// Token: 0x040049DF RID: 18911
	public DescriptorPanel HarvestDescriptorPanel;

	// Token: 0x040049E0 RID: 18912
	public DescriptorPanel EffectsDescriptorPanel;

	// Token: 0x040049E1 RID: 18913
	public MultiToggle continuousToggle;
}
