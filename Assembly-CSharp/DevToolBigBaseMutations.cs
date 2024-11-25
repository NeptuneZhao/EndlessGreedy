using System;
using ImGuiNET;

// Token: 0x0200060B RID: 1547
public class DevToolBigBaseMutations : DevTool
{
	// Token: 0x06002625 RID: 9765 RVA: 0x000D3A2E File Offset: 0x000D1C2E
	protected override void RenderTo(DevPanel panel)
	{
		if (Game.Instance != null)
		{
			this.ShowButtons();
			return;
		}
		ImGui.Text("Game not available");
	}

	// Token: 0x06002626 RID: 9766 RVA: 0x000D3A50 File Offset: 0x000D1C50
	private void ShowButtons()
	{
		if (ImGui.Button("Destroy Ladders"))
		{
			this.DestroyGameObjects<Ladder>(Components.Ladders, Tag.Invalid);
		}
		if (ImGui.Button("Destroy Tiles"))
		{
			this.DestroyGameObjects<BuildingComplete>(Components.BuildingCompletes, GameTags.FloorTiles);
		}
		if (ImGui.Button("Destroy Wires"))
		{
			this.DestroyGameObjects<BuildingComplete>(Components.BuildingCompletes, GameTags.Wires);
		}
		if (ImGui.Button("Destroy Pipes"))
		{
			this.DestroyGameObjects<BuildingComplete>(Components.BuildingCompletes, GameTags.Pipes);
		}
	}

	// Token: 0x06002627 RID: 9767 RVA: 0x000D3AD0 File Offset: 0x000D1CD0
	private void DestroyGameObjects<T>(Components.Cmps<T> componentsList, Tag filterForTag)
	{
		for (int i = componentsList.Count - 1; i >= 0; i--)
		{
			if (!componentsList[i].IsNullOrDestroyed() && (!(filterForTag != Tag.Invalid) || (componentsList[i] as KMonoBehaviour).gameObject.HasTag(filterForTag)))
			{
				Util.KDestroyGameObject(componentsList[i] as KMonoBehaviour);
			}
		}
	}
}
