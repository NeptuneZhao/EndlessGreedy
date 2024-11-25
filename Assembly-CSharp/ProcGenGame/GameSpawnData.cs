using System;
using System.Collections.Generic;
using Klei.CustomSettings;
using KSerialization;
using TemplateClasses;

namespace ProcGenGame
{
	// Token: 0x02000E07 RID: 3591
	[SerializationConfig(MemberSerialization.OptOut)]
	public class GameSpawnData
	{
		// Token: 0x06007209 RID: 29193 RVA: 0x002B4E54 File Offset: 0x002B3054
		public void AddRange(IEnumerable<KeyValuePair<int, string>> newItems)
		{
			foreach (KeyValuePair<int, string> keyValuePair in newItems)
			{
				Vector2I vector2I = Grid.CellToXY(keyValuePair.Key);
				Prefab item = new Prefab(keyValuePair.Value, Prefab.Type.Other, vector2I.x, vector2I.y, (SimHashes)0, -1f, 1f, null, 0, Orientation.Neutral, null, null, 0, null);
				this.otherEntities.Add(item);
			}
		}

		// Token: 0x0600720A RID: 29194 RVA: 0x002B4EDC File Offset: 0x002B30DC
		public void AddTemplate(TemplateContainer template, Vector2I position, ref Dictionary<int, int> claimedCells)
		{
			int cell = Grid.XYToCell(position.x, position.y);
			bool flag = true;
			if (DlcManager.IsExpansion1Active() && CustomGameSettings.Instance != null)
			{
				flag = (CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.Teleporters).id == "Enabled");
			}
			if (template.buildings != null)
			{
				foreach (Prefab prefab in template.buildings)
				{
					if (!claimedCells.ContainsKey(Grid.OffsetCell(cell, prefab.location_x, prefab.location_y)) && (flag || !this.IsWarpTeleporter(prefab)))
					{
						this.buildings.Add(prefab.Clone(position));
					}
				}
			}
			if (template.pickupables != null)
			{
				foreach (Prefab prefab2 in template.pickupables)
				{
					if (!claimedCells.ContainsKey(Grid.OffsetCell(cell, prefab2.location_x, prefab2.location_y)))
					{
						this.pickupables.Add(prefab2.Clone(position));
					}
				}
			}
			if (template.elementalOres != null)
			{
				foreach (Prefab prefab3 in template.elementalOres)
				{
					if (!claimedCells.ContainsKey(Grid.OffsetCell(cell, prefab3.location_x, prefab3.location_y)))
					{
						this.elementalOres.Add(prefab3.Clone(position));
					}
				}
			}
			if (template.otherEntities != null)
			{
				foreach (Prefab prefab4 in template.otherEntities)
				{
					if (!claimedCells.ContainsKey(Grid.OffsetCell(cell, prefab4.location_x, prefab4.location_y)) && (flag || !this.IsWarpTeleporter(prefab4)))
					{
						this.otherEntities.Add(prefab4.Clone(position));
					}
				}
			}
			if (template.cells != null)
			{
				for (int i = 0; i < template.cells.Count; i++)
				{
					int num = Grid.XYToCell(position.x + template.cells[i].location_x, position.y + template.cells[i].location_y);
					if (!claimedCells.ContainsKey(num))
					{
						claimedCells[num] = 1;
						this.preventFoWReveal.Add(new KeyValuePair<Vector2I, bool>(new Vector2I(position.x + template.cells[i].location_x, position.y + template.cells[i].location_y), template.cells[i].preventFoWReveal));
					}
					else
					{
						Dictionary<int, int> dictionary = claimedCells;
						int j = num;
						dictionary[j]++;
					}
				}
			}
			if (template.info != null && template.info.discover_tags != null)
			{
				foreach (Tag item in template.info.discover_tags)
				{
					this.discoveredResources.Add(item);
				}
			}
		}

		// Token: 0x0600720B RID: 29195 RVA: 0x002B5258 File Offset: 0x002B3458
		private bool IsWarpTeleporter(Prefab prefab)
		{
			return prefab.id == "WarpPortal" || prefab.id == WarpReceiverConfig.ID || prefab.id == "WarpConduitSender" || prefab.id == "WarpConduitReceiver";
		}

		// Token: 0x04004EA9 RID: 20137
		public Vector2I baseStartPos;

		// Token: 0x04004EAA RID: 20138
		public List<Prefab> buildings = new List<Prefab>();

		// Token: 0x04004EAB RID: 20139
		public List<Prefab> pickupables = new List<Prefab>();

		// Token: 0x04004EAC RID: 20140
		public List<Prefab> elementalOres = new List<Prefab>();

		// Token: 0x04004EAD RID: 20141
		public List<Prefab> otherEntities = new List<Prefab>();

		// Token: 0x04004EAE RID: 20142
		public List<Tag> discoveredResources = new List<Tag>();

		// Token: 0x04004EAF RID: 20143
		public List<KeyValuePair<Vector2I, bool>> preventFoWReveal = new List<KeyValuePair<Vector2I, bool>>();
	}
}
