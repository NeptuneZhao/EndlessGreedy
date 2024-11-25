using System;
using System.Collections.Generic;
using ProcGen;
using ProcGenGame;
using STRINGS;
using UnityEngine;

// Token: 0x02000C16 RID: 3094
public class ColonyDestinationAsteroidBeltData
{
	// Token: 0x17000718 RID: 1816
	// (get) Token: 0x06005EC8 RID: 24264 RVA: 0x0023363C File Offset: 0x0023183C
	// (set) Token: 0x06005EC9 RID: 24265 RVA: 0x00233644 File Offset: 0x00231844
	public float TargetScale { get; set; }

	// Token: 0x17000719 RID: 1817
	// (get) Token: 0x06005ECA RID: 24266 RVA: 0x0023364D File Offset: 0x0023184D
	// (set) Token: 0x06005ECB RID: 24267 RVA: 0x00233655 File Offset: 0x00231855
	public float Scale { get; set; }

	// Token: 0x1700071A RID: 1818
	// (get) Token: 0x06005ECC RID: 24268 RVA: 0x0023365E File Offset: 0x0023185E
	// (set) Token: 0x06005ECD RID: 24269 RVA: 0x00233666 File Offset: 0x00231866
	public int seed { get; private set; }

	// Token: 0x1700071B RID: 1819
	// (get) Token: 0x06005ECE RID: 24270 RVA: 0x0023366F File Offset: 0x0023186F
	public string startWorldPath
	{
		get
		{
			return this.startWorld.filePath;
		}
	}

	// Token: 0x1700071C RID: 1820
	// (get) Token: 0x06005ECF RID: 24271 RVA: 0x0023367C File Offset: 0x0023187C
	// (set) Token: 0x06005ED0 RID: 24272 RVA: 0x00233684 File Offset: 0x00231884
	public Sprite sprite { get; private set; }

	// Token: 0x1700071D RID: 1821
	// (get) Token: 0x06005ED1 RID: 24273 RVA: 0x0023368D File Offset: 0x0023188D
	// (set) Token: 0x06005ED2 RID: 24274 RVA: 0x00233695 File Offset: 0x00231895
	public int difficulty { get; private set; }

	// Token: 0x1700071E RID: 1822
	// (get) Token: 0x06005ED3 RID: 24275 RVA: 0x0023369E File Offset: 0x0023189E
	public string startWorldName
	{
		get
		{
			return Strings.Get(this.startWorld.name);
		}
	}

	// Token: 0x1700071F RID: 1823
	// (get) Token: 0x06005ED4 RID: 24276 RVA: 0x002336B5 File Offset: 0x002318B5
	public string properName
	{
		get
		{
			if (this.clusterLayout == null)
			{
				return "";
			}
			return this.clusterLayout.name;
		}
	}

	// Token: 0x17000720 RID: 1824
	// (get) Token: 0x06005ED5 RID: 24277 RVA: 0x002336D0 File Offset: 0x002318D0
	public string beltPath
	{
		get
		{
			if (this.clusterLayout == null)
			{
				return WorldGenSettings.ClusterDefaultName;
			}
			return this.clusterLayout.filePath;
		}
	}

	// Token: 0x17000721 RID: 1825
	// (get) Token: 0x06005ED6 RID: 24278 RVA: 0x002336EB File Offset: 0x002318EB
	// (set) Token: 0x06005ED7 RID: 24279 RVA: 0x002336F3 File Offset: 0x002318F3
	public List<ProcGen.World> worlds { get; private set; }

	// Token: 0x17000722 RID: 1826
	// (get) Token: 0x06005ED8 RID: 24280 RVA: 0x002336FC File Offset: 0x002318FC
	public ClusterLayout Layout
	{
		get
		{
			if (this.mutatedClusterLayout != null)
			{
				return this.mutatedClusterLayout.layout;
			}
			return this.clusterLayout;
		}
	}

	// Token: 0x17000723 RID: 1827
	// (get) Token: 0x06005ED9 RID: 24281 RVA: 0x00233718 File Offset: 0x00231918
	public ProcGen.World GetStartWorld
	{
		get
		{
			return this.startWorld;
		}
	}

	// Token: 0x06005EDA RID: 24282 RVA: 0x00233720 File Offset: 0x00231920
	public ColonyDestinationAsteroidBeltData(string staringWorldName, int seed, string clusterPath)
	{
		this.startWorld = SettingsCache.worlds.GetWorldData(staringWorldName);
		this.Scale = (this.TargetScale = this.startWorld.iconScale);
		this.worlds = new List<ProcGen.World>();
		if (clusterPath != null)
		{
			this.clusterLayout = SettingsCache.clusterLayouts.GetClusterData(clusterPath);
		}
		this.ReInitialize(seed);
	}

	// Token: 0x06005EDB RID: 24283 RVA: 0x0023379C File Offset: 0x0023199C
	public static Sprite GetUISprite(string filename)
	{
		if (filename.IsNullOrWhiteSpace())
		{
			filename = (DlcManager.FeatureClusterSpaceEnabled() ? "asteroid_sandstone_start_kanim" : "Asteroid_sandstone");
		}
		KAnimFile kanimFile;
		Assets.TryGetAnim(filename, out kanimFile);
		if (kanimFile != null)
		{
			return Def.GetUISpriteFromMultiObjectAnim(kanimFile, "ui", false, "");
		}
		return Assets.GetSprite(filename);
	}

	// Token: 0x06005EDC RID: 24284 RVA: 0x002337FC File Offset: 0x002319FC
	public void ReInitialize(int seed)
	{
		this.seed = seed;
		this.paramDescriptors.Clear();
		this.traitDescriptors.Clear();
		this.sprite = ColonyDestinationAsteroidBeltData.GetUISprite(this.startWorld.asteroidIcon);
		this.difficulty = this.clusterLayout.difficulty;
		this.mutatedClusterLayout = WorldgenMixing.DoWorldMixing(this.clusterLayout, seed, true, true);
		this.RemixClusterLayout();
	}

	// Token: 0x06005EDD RID: 24285 RVA: 0x00233868 File Offset: 0x00231A68
	public void RemixClusterLayout()
	{
		if (!WorldgenMixing.RefreshWorldMixing(this.mutatedClusterLayout, this.seed, true, true))
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"World remix failed, using default cluster instead."
			});
			this.mutatedClusterLayout = new MutatedClusterLayout(this.clusterLayout);
		}
		this.worlds.Clear();
		for (int i = 0; i < this.Layout.worldPlacements.Count; i++)
		{
			if (i != this.Layout.startWorldIndex)
			{
				this.worlds.Add(SettingsCache.worlds.GetWorldData(this.Layout.worldPlacements[i].world));
			}
		}
	}

	// Token: 0x06005EDE RID: 24286 RVA: 0x0023390D File Offset: 0x00231B0D
	public List<AsteroidDescriptor> GetParamDescriptors()
	{
		if (this.paramDescriptors.Count == 0)
		{
			this.paramDescriptors = this.GenerateParamDescriptors();
		}
		return this.paramDescriptors;
	}

	// Token: 0x06005EDF RID: 24287 RVA: 0x0023392E File Offset: 0x00231B2E
	public List<AsteroidDescriptor> GetTraitDescriptors()
	{
		if (this.traitDescriptors.Count == 0)
		{
			this.traitDescriptors = this.GenerateTraitDescriptors();
		}
		return this.traitDescriptors;
	}

	// Token: 0x06005EE0 RID: 24288 RVA: 0x00233950 File Offset: 0x00231B50
	private List<AsteroidDescriptor> GenerateParamDescriptors()
	{
		List<AsteroidDescriptor> list = new List<AsteroidDescriptor>();
		if (this.clusterLayout != null && DlcManager.FeatureClusterSpaceEnabled())
		{
			list.Add(new AsteroidDescriptor(string.Format(WORLDS.SURVIVAL_CHANCE.CLUSTERNAME, Strings.Get(this.clusterLayout.name)), Strings.Get(this.clusterLayout.description), Color.white, null, null));
		}
		list.Add(new AsteroidDescriptor(string.Format(WORLDS.SURVIVAL_CHANCE.PLANETNAME, this.startWorldName), null, Color.white, null, null));
		list.Add(new AsteroidDescriptor(Strings.Get(this.startWorld.description), null, Color.white, null, null));
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			list.Add(new AsteroidDescriptor(string.Format(WORLDS.SURVIVAL_CHANCE.MOONNAMES, Array.Empty<object>()), null, Color.white, null, null));
			foreach (ProcGen.World world in this.worlds)
			{
				list.Add(new AsteroidDescriptor(string.Format("{0}", Strings.Get(world.name)), Strings.Get(world.description), Color.white, null, null));
			}
		}
		int index = Mathf.Clamp(this.difficulty, 0, ColonyDestinationAsteroidBeltData.survivalOptions.Count - 1);
		global::Tuple<string, string, string> tuple = ColonyDestinationAsteroidBeltData.survivalOptions[index];
		list.Add(new AsteroidDescriptor(string.Format(WORLDS.SURVIVAL_CHANCE.TITLE, tuple.first, tuple.third), null, Color.white, null, null));
		return list;
	}

	// Token: 0x06005EE1 RID: 24289 RVA: 0x00233B08 File Offset: 0x00231D08
	private List<AsteroidDescriptor> GenerateTraitDescriptors()
	{
		List<AsteroidDescriptor> list = new List<AsteroidDescriptor>();
		List<ProcGen.World> list2 = new List<ProcGen.World>();
		list2.Add(this.startWorld);
		list2.AddRange(this.worlds);
		for (int i = 0; i < list2.Count; i++)
		{
			ProcGen.World world = list2[i];
			if (DlcManager.IsExpansion1Active())
			{
				list.Add(new AsteroidDescriptor("", null, Color.white, null, null));
				list.Add(new AsteroidDescriptor(string.Format("<b>{0}</b>", Strings.Get(world.name)), null, Color.white, null, null));
			}
			List<WorldTrait> worldTraits = this.GetWorldTraits(world);
			foreach (WorldTrait worldTrait in worldTraits)
			{
				string associatedIcon = worldTrait.filePath.Substring(worldTrait.filePath.LastIndexOf("/") + 1);
				list.Add(new AsteroidDescriptor(string.Format("<color=#{1}>{0}</color>", Strings.Get(worldTrait.name), worldTrait.colorHex), Strings.Get(worldTrait.description), global::Util.ColorFromHex(worldTrait.colorHex), null, associatedIcon));
			}
			if (worldTraits.Count == 0)
			{
				list.Add(new AsteroidDescriptor(WORLD_TRAITS.NO_TRAITS.NAME, WORLD_TRAITS.NO_TRAITS.DESCRIPTION, Color.white, null, "NoTraits"));
			}
		}
		return list;
	}

	// Token: 0x06005EE2 RID: 24290 RVA: 0x00233C84 File Offset: 0x00231E84
	public List<AsteroidDescriptor> GenerateTraitDescriptors(ProcGen.World singleWorld, bool includeDefaultTrait = true)
	{
		List<AsteroidDescriptor> list = new List<AsteroidDescriptor>();
		List<ProcGen.World> list2 = new List<ProcGen.World>();
		list2.Add(this.startWorld);
		list2.AddRange(this.worlds);
		for (int i = 0; i < list2.Count; i++)
		{
			if (list2[i] == singleWorld)
			{
				ProcGen.World singleWorld2 = list2[i];
				List<WorldTrait> worldTraits = this.GetWorldTraits(singleWorld2);
				foreach (WorldTrait worldTrait in worldTraits)
				{
					string associatedIcon = worldTrait.filePath.Substring(worldTrait.filePath.LastIndexOf("/") + 1);
					list.Add(new AsteroidDescriptor(string.Format("<color=#{1}>{0}</color>", Strings.Get(worldTrait.name), worldTrait.colorHex), Strings.Get(worldTrait.description), global::Util.ColorFromHex(worldTrait.colorHex), null, associatedIcon));
				}
				if (worldTraits.Count == 0 && includeDefaultTrait)
				{
					list.Add(new AsteroidDescriptor(WORLD_TRAITS.NO_TRAITS.NAME, WORLD_TRAITS.NO_TRAITS.DESCRIPTION, Color.white, null, "NoTraits"));
				}
			}
		}
		return list;
	}

	// Token: 0x06005EE3 RID: 24291 RVA: 0x00233DCC File Offset: 0x00231FCC
	public List<WorldTrait> GetWorldTraits(ProcGen.World singleWorld)
	{
		List<WorldTrait> list = new List<WorldTrait>();
		List<ProcGen.World> list2 = new List<ProcGen.World>();
		list2.Add(this.startWorld);
		list2.AddRange(this.worlds);
		for (int i = 0; i < list2.Count; i++)
		{
			if (list2[i] == singleWorld)
			{
				ProcGen.World world = list2[i];
				int num = this.seed;
				if (num > 0)
				{
					num += this.clusterLayout.worldPlacements.FindIndex((WorldPlacement x) => x.world == world.filePath);
				}
				foreach (string name in SettingsCache.GetRandomTraits(num, world))
				{
					WorldTrait cachedWorldTrait = SettingsCache.GetCachedWorldTrait(name, true);
					list.Add(cachedWorldTrait);
				}
			}
		}
		return list;
	}

	// Token: 0x04003F60 RID: 16224
	private ProcGen.World startWorld;

	// Token: 0x04003F61 RID: 16225
	private ClusterLayout clusterLayout;

	// Token: 0x04003F62 RID: 16226
	private MutatedClusterLayout mutatedClusterLayout;

	// Token: 0x04003F63 RID: 16227
	private List<AsteroidDescriptor> paramDescriptors = new List<AsteroidDescriptor>();

	// Token: 0x04003F64 RID: 16228
	private List<AsteroidDescriptor> traitDescriptors = new List<AsteroidDescriptor>();

	// Token: 0x04003F65 RID: 16229
	public static List<global::Tuple<string, string, string>> survivalOptions = new List<global::Tuple<string, string, string>>
	{
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.MOSTHOSPITABLE, "", "D2F40C"),
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.VERYHIGH, "", "7DE419"),
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.HIGH, "", "36D246"),
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.NEUTRAL, "", "63C2B7"),
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.LOW, "", "6A8EB1"),
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.VERYLOW, "", "937890"),
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.LEASTHOSPITABLE, "", "9636DF")
	};
}
