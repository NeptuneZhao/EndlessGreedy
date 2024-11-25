using System;
using System.Collections.Generic;
using ImGuiNET;

// Token: 0x02000628 RID: 1576
public class DevToolSpaceScannerNetwork : DevTool
{
	// Token: 0x060026BD RID: 9917 RVA: 0x000DC2A4 File Offset: 0x000DA4A4
	public DevToolSpaceScannerNetwork()
	{
		this.tableDrawer = ImGuiObjectTableDrawer<DevToolSpaceScannerNetwork.Entry>.New().Column("WorldId", (DevToolSpaceScannerNetwork.Entry e) => e.worldId).Column("Network Quality (0->1)", (DevToolSpaceScannerNetwork.Entry e) => e.networkQuality).Column("Targets Detected", (DevToolSpaceScannerNetwork.Entry e) => e.targetsString).FixedHeight(300f).Build();
	}

	// Token: 0x060026BE RID: 9918 RVA: 0x000DC34C File Offset: 0x000DA54C
	protected override void RenderTo(DevPanel panel)
	{
		if (Game.Instance == null)
		{
			ImGui.Text("Game instance is null");
			return;
		}
		if (Game.Instance.spaceScannerNetworkManager == null)
		{
			ImGui.Text("SpaceScannerNetworkQualityManager instance is null");
			return;
		}
		if (ClusterManager.Instance == null)
		{
			ImGui.Text("ClusterManager instance is null");
			return;
		}
		if (ImGui.CollapsingHeader("Worlds Data"))
		{
			this.tableDrawer.Draw(DevToolSpaceScannerNetwork.GetData());
		}
		if (ImGui.CollapsingHeader("Full DevToolSpaceScannerNetwork Info"))
		{
			ImGuiEx.DrawObject(Game.Instance.spaceScannerNetworkManager, null);
		}
	}

	// Token: 0x060026BF RID: 9919 RVA: 0x000DC3E0 File Offset: 0x000DA5E0
	public static IEnumerable<DevToolSpaceScannerNetwork.Entry> GetData()
	{
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			yield return new DevToolSpaceScannerNetwork.Entry(worldContainer.id, Game.Instance.spaceScannerNetworkManager.GetQualityForWorld(worldContainer.id), DevToolSpaceScannerNetwork.GetTargetsString(worldContainer));
		}
		IEnumerator<WorldContainer> enumerator = null;
		yield break;
		yield break;
	}

	// Token: 0x060026C0 RID: 9920 RVA: 0x000DC3EC File Offset: 0x000DA5EC
	public static string GetTargetsString(WorldContainer world)
	{
		SpaceScannerWorldData spaceScannerWorldData;
		if (!Game.Instance.spaceScannerNetworkManager.DEBUG_GetWorldIdToDataMap().TryGetValue(world.id, out spaceScannerWorldData))
		{
			return "<none>";
		}
		if (spaceScannerWorldData.targetIdsDetected.Count == 0)
		{
			return "<none>";
		}
		return string.Join(",", spaceScannerWorldData.targetIdsDetected);
	}

	// Token: 0x04001643 RID: 5699
	private ImGuiObjectTableDrawer<DevToolSpaceScannerNetwork.Entry> tableDrawer;

	// Token: 0x02001404 RID: 5124
	public readonly struct Entry
	{
		// Token: 0x0600890A RID: 35082 RVA: 0x0032F8B3 File Offset: 0x0032DAB3
		public Entry(int worldId, float networkQuality, string targetsString)
		{
			this.worldId = worldId;
			this.networkQuality = networkQuality;
			this.targetsString = targetsString;
		}

		// Token: 0x0400689B RID: 26779
		public readonly int worldId;

		// Token: 0x0400689C RID: 26780
		public readonly float networkQuality;

		// Token: 0x0400689D RID: 26781
		public readonly string targetsString;
	}
}
