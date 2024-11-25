using System;

// Token: 0x0200051E RID: 1310
public class Blueprints
{
	// Token: 0x06001D55 RID: 7509 RVA: 0x000995D8 File Offset: 0x000977D8
	public static Blueprints Get()
	{
		if (Blueprints.instance == null)
		{
			Blueprints.instance = new Blueprints();
			Blueprints.instance.all.AddBlueprintsFrom<Blueprints_Default>(new Blueprints_Default());
			foreach (BlueprintProvider provider in Blueprints.instance.skinsReleaseProviders)
			{
				Blueprints.instance.skinsRelease.AddBlueprintsFrom<BlueprintProvider>(provider);
			}
			Blueprints.instance.all.AddBlueprintsFrom(Blueprints.instance.skinsRelease);
			Blueprints.instance.skinsRelease.PostProcess();
			Blueprints.instance.all.PostProcess();
		}
		return Blueprints.instance;
	}

	// Token: 0x0400108E RID: 4238
	public BlueprintCollection all = new BlueprintCollection();

	// Token: 0x0400108F RID: 4239
	public BlueprintCollection skinsRelease = new BlueprintCollection();

	// Token: 0x04001090 RID: 4240
	public BlueprintProvider[] skinsReleaseProviders = new BlueprintProvider[]
	{
		new Blueprints_U51AndBefore(),
		new Blueprints_DlcPack2()
	};

	// Token: 0x04001091 RID: 4241
	private static Blueprints instance;
}
