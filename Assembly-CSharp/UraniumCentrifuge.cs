using System;
using UnityEngine;

// Token: 0x02000791 RID: 1937
public class UraniumCentrifuge : ComplexFabricator
{
	// Token: 0x060034F2 RID: 13554 RVA: 0x00120ABA File Offset: 0x0011ECBA
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<UraniumCentrifuge>(-1697596308, UraniumCentrifuge.DropEnrichedProductDelegate);
		base.Subscribe<UraniumCentrifuge>(-2094018600, UraniumCentrifuge.CheckPipesDelegate);
	}

	// Token: 0x060034F3 RID: 13555 RVA: 0x00120AE4 File Offset: 0x0011ECE4
	private void DropEnrichedProducts(object data)
	{
		Storage[] components = base.GetComponents<Storage>();
		for (int i = 0; i < components.Length; i++)
		{
			components[i].Drop(ElementLoader.FindElementByHash(SimHashes.EnrichedUranium).tag);
		}
	}

	// Token: 0x060034F4 RID: 13556 RVA: 0x00120B20 File Offset: 0x0011ED20
	private void CheckPipes(object data)
	{
		KSelectable component = base.GetComponent<KSelectable>();
		int cell = Grid.OffsetCell(Grid.PosToCell(this), UraniumCentrifugeConfig.outPipeOffset);
		GameObject gameObject = Grid.Objects[cell, 16];
		if (!(gameObject != null))
		{
			component.RemoveStatusItem(this.statusHandle, false);
			return;
		}
		if (gameObject.GetComponent<PrimaryElement>().Element.highTemp > ElementLoader.FindElementByHash(SimHashes.MoltenUranium).lowTemp)
		{
			component.RemoveStatusItem(this.statusHandle, false);
			return;
		}
		this.statusHandle = component.AddStatusItem(Db.Get().BuildingStatusItems.PipeMayMelt, null);
	}

	// Token: 0x04001F6B RID: 8043
	private Guid statusHandle;

	// Token: 0x04001F6C RID: 8044
	private static readonly EventSystem.IntraObjectHandler<UraniumCentrifuge> CheckPipesDelegate = new EventSystem.IntraObjectHandler<UraniumCentrifuge>(delegate(UraniumCentrifuge component, object data)
	{
		component.CheckPipes(data);
	});

	// Token: 0x04001F6D RID: 8045
	private static readonly EventSystem.IntraObjectHandler<UraniumCentrifuge> DropEnrichedProductDelegate = new EventSystem.IntraObjectHandler<UraniumCentrifuge>(delegate(UraniumCentrifuge component, object data)
	{
		component.DropEnrichedProducts(data);
	});
}
