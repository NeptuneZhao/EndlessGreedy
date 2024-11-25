using System;

// Token: 0x02000812 RID: 2066
public class InvalidPortReporter : KMonoBehaviour
{
	// Token: 0x06003930 RID: 14640 RVA: 0x00137D0D File Offset: 0x00135F0D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.OnTagsChanged(null);
		base.Subscribe<InvalidPortReporter>(-1582839653, InvalidPortReporter.OnTagsChangedDelegate);
	}

	// Token: 0x06003931 RID: 14641 RVA: 0x00137D2D File Offset: 0x00135F2D
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06003932 RID: 14642 RVA: 0x00137D38 File Offset: 0x00135F38
	private void OnTagsChanged(object data)
	{
		bool flag = base.gameObject.HasTag(GameTags.HasInvalidPorts);
		Operational component = base.GetComponent<Operational>();
		if (component != null)
		{
			component.SetFlag(InvalidPortReporter.portsNotOverlapping, !flag);
		}
		KSelectable component2 = base.GetComponent<KSelectable>();
		if (component2 != null)
		{
			component2.ToggleStatusItem(Db.Get().BuildingStatusItems.InvalidPortOverlap, flag, base.gameObject);
		}
	}

	// Token: 0x04002267 RID: 8807
	public static readonly Operational.Flag portsNotOverlapping = new Operational.Flag("ports_not_overlapping", Operational.Flag.Type.Functional);

	// Token: 0x04002268 RID: 8808
	private static readonly EventSystem.IntraObjectHandler<InvalidPortReporter> OnTagsChangedDelegate = new EventSystem.IntraObjectHandler<InvalidPortReporter>(delegate(InvalidPortReporter component, object data)
	{
		component.OnTagsChanged(data);
	});
}
