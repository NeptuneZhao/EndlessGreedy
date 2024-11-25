using System;
using System.Collections.Generic;
using HUSL;
using UnityEngine;

// Token: 0x02000485 RID: 1157
public class NavGrid
{
	// Token: 0x17000098 RID: 152
	// (get) Token: 0x060018EA RID: 6378 RVA: 0x00085DDD File Offset: 0x00083FDD
	// (set) Token: 0x060018EB RID: 6379 RVA: 0x00085DE5 File Offset: 0x00083FE5
	public NavTable NavTable { get; private set; }

	// Token: 0x17000099 RID: 153
	// (get) Token: 0x060018EC RID: 6380 RVA: 0x00085DEE File Offset: 0x00083FEE
	// (set) Token: 0x060018ED RID: 6381 RVA: 0x00085DF6 File Offset: 0x00083FF6
	public NavGrid.Transition[] transitions { get; set; }

	// Token: 0x1700009A RID: 154
	// (get) Token: 0x060018EE RID: 6382 RVA: 0x00085DFF File Offset: 0x00083FFF
	// (set) Token: 0x060018EF RID: 6383 RVA: 0x00085E07 File Offset: 0x00084007
	public NavGrid.Transition[][] transitionsByNavType { get; private set; }

	// Token: 0x1700009B RID: 155
	// (get) Token: 0x060018F0 RID: 6384 RVA: 0x00085E10 File Offset: 0x00084010
	// (set) Token: 0x060018F1 RID: 6385 RVA: 0x00085E18 File Offset: 0x00084018
	public int updateRangeX { get; private set; }

	// Token: 0x1700009C RID: 156
	// (get) Token: 0x060018F2 RID: 6386 RVA: 0x00085E21 File Offset: 0x00084021
	// (set) Token: 0x060018F3 RID: 6387 RVA: 0x00085E29 File Offset: 0x00084029
	public int updateRangeY { get; private set; }

	// Token: 0x1700009D RID: 157
	// (get) Token: 0x060018F4 RID: 6388 RVA: 0x00085E32 File Offset: 0x00084032
	// (set) Token: 0x060018F5 RID: 6389 RVA: 0x00085E3A File Offset: 0x0008403A
	public int maxLinksPerCell { get; private set; }

	// Token: 0x060018F6 RID: 6390 RVA: 0x00085E43 File Offset: 0x00084043
	public static NavType MirrorNavType(NavType nav_type)
	{
		if (nav_type == NavType.LeftWall)
		{
			return NavType.RightWall;
		}
		if (nav_type == NavType.RightWall)
		{
			return NavType.LeftWall;
		}
		return nav_type;
	}

	// Token: 0x060018F7 RID: 6391 RVA: 0x00085E54 File Offset: 0x00084054
	public NavGrid(string id, NavGrid.Transition[] transitions, NavGrid.NavTypeData[] nav_type_data, CellOffset[] bounding_offsets, NavTableValidator[] validators, int update_range_x, int update_range_y, int max_links_per_cell)
	{
		this.DirtyBitFlags = new byte[(Grid.CellCount + 7) / 8];
		this.DirtyCells = new List<int>();
		this.id = id;
		this.Validators = validators;
		this.navTypeData = nav_type_data;
		this.transitions = transitions;
		this.boundingOffsets = bounding_offsets;
		List<NavType> list = new List<NavType>();
		this.updateRangeX = update_range_x;
		this.updateRangeY = update_range_y;
		this.maxLinksPerCell = max_links_per_cell + 1;
		for (int i = 0; i < transitions.Length; i++)
		{
			DebugUtil.Assert(i >= 0 && i <= 255);
			transitions[i].id = (byte)i;
			if (!list.Contains(transitions[i].start))
			{
				list.Add(transitions[i].start);
			}
			if (!list.Contains(transitions[i].end))
			{
				list.Add(transitions[i].end);
			}
		}
		this.ValidNavTypes = list.ToArray();
		this.DebugViewLinkType = new bool[this.ValidNavTypes.Length];
		this.DebugViewValidCellsType = new bool[this.ValidNavTypes.Length];
		foreach (NavType nav_type in this.ValidNavTypes)
		{
			this.GetNavTypeData(nav_type);
		}
		this.Links = new NavGrid.Link[this.maxLinksPerCell * Grid.CellCount];
		this.NavTable = new NavTable(Grid.CellCount);
		this.transitions = transitions;
		this.transitionsByNavType = new NavGrid.Transition[11][];
		for (int k = 0; k < 11; k++)
		{
			List<NavGrid.Transition> list2 = new List<NavGrid.Transition>();
			NavType navType = (NavType)k;
			foreach (NavGrid.Transition transition in transitions)
			{
				if (transition.start == navType)
				{
					list2.Add(transition);
				}
			}
			this.transitionsByNavType[k] = list2.ToArray();
		}
		foreach (NavTableValidator navTableValidator in validators)
		{
			navTableValidator.onDirty = (Action<int>)Delegate.Combine(navTableValidator.onDirty, new Action<int>(this.AddDirtyCell));
		}
		this.potentialScratchPad = new PathFinder.PotentialScratchPad(this.maxLinksPerCell);
		this.InitializeGraph();
	}

	// Token: 0x060018F8 RID: 6392 RVA: 0x000860A0 File Offset: 0x000842A0
	public NavGrid.NavTypeData GetNavTypeData(NavType nav_type)
	{
		foreach (NavGrid.NavTypeData navTypeData in this.navTypeData)
		{
			if (navTypeData.navType == nav_type)
			{
				return navTypeData;
			}
		}
		throw new Exception("Missing nav type data for nav type:" + nav_type.ToString());
	}

	// Token: 0x060018F9 RID: 6393 RVA: 0x000860F4 File Offset: 0x000842F4
	public bool HasNavTypeData(NavType nav_type)
	{
		NavGrid.NavTypeData[] array = this.navTypeData;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].navType == nav_type)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060018FA RID: 6394 RVA: 0x00086128 File Offset: 0x00084328
	public HashedString GetIdleAnim(NavType nav_type)
	{
		return this.GetNavTypeData(nav_type).idleAnim;
	}

	// Token: 0x060018FB RID: 6395 RVA: 0x00086136 File Offset: 0x00084336
	public void InitializeGraph()
	{
		NavGridUpdater.InitializeNavGrid(this.NavTable, this.Validators, this.boundingOffsets, this.maxLinksPerCell, this.Links, this.transitionsByNavType);
	}

	// Token: 0x060018FC RID: 6396 RVA: 0x00086164 File Offset: 0x00084364
	public void UpdateGraph()
	{
		int count = this.DirtyCells.Count;
		for (int i = 0; i < count; i++)
		{
			int num;
			int num2;
			Grid.CellToXY(this.DirtyCells[i], out num, out num2);
			int num3 = Grid.ClampX(num - this.updateRangeX);
			int num4 = Grid.ClampY(num2 - this.updateRangeY);
			int num5 = Grid.ClampX(num + this.updateRangeX);
			int num6 = Grid.ClampY(num2 + this.updateRangeY);
			for (int j = num4; j <= num6; j++)
			{
				for (int k = num3; k <= num5; k++)
				{
					this.AddDirtyCell(Grid.XYToCell(k, j));
				}
			}
		}
		this.UpdateGraph(this.DirtyCells);
		foreach (int num7 in this.DirtyCells)
		{
			this.DirtyBitFlags[num7 / 8] = 0;
		}
		this.DirtyCells.Clear();
	}

	// Token: 0x060018FD RID: 6397 RVA: 0x00086274 File Offset: 0x00084474
	public void UpdateGraph(IEnumerable<int> dirty_nav_cells)
	{
		NavGridUpdater.UpdateNavGrid(this.NavTable, this.Validators, this.boundingOffsets, this.maxLinksPerCell, this.Links, this.transitionsByNavType, this.teleportTransitions, dirty_nav_cells);
		if (this.OnNavGridUpdateComplete != null)
		{
			this.OnNavGridUpdateComplete(dirty_nav_cells);
		}
	}

	// Token: 0x060018FE RID: 6398 RVA: 0x000862C5 File Offset: 0x000844C5
	public static void DebugDrawPath(int start_cell, int end_cell)
	{
		Grid.CellToPosCCF(start_cell, Grid.SceneLayer.Move);
		Grid.CellToPosCCF(end_cell, Grid.SceneLayer.Move);
	}

	// Token: 0x060018FF RID: 6399 RVA: 0x000862DC File Offset: 0x000844DC
	public static void DebugDrawPath(PathFinder.Path path)
	{
		if (path.nodes != null)
		{
			for (int i = 0; i < path.nodes.Count - 1; i++)
			{
				NavGrid.DebugDrawPath(path.nodes[i].cell, path.nodes[i + 1].cell);
			}
		}
	}

	// Token: 0x06001900 RID: 6400 RVA: 0x00086334 File Offset: 0x00084534
	private void DebugDrawValidCells()
	{
		Color white = Color.white;
		int cellCount = Grid.CellCount;
		for (int i = 0; i < cellCount; i++)
		{
			for (int j = 0; j < 11; j++)
			{
				NavType nav_type = (NavType)j;
				if (this.NavTable.IsValid(i, nav_type) && this.DrawNavTypeCell(nav_type, ref white))
				{
					DebugExtension.DebugPoint(NavTypeHelper.GetNavPos(i, nav_type), white, 1f, 0f, false);
				}
			}
		}
	}

	// Token: 0x06001901 RID: 6401 RVA: 0x000863A0 File Offset: 0x000845A0
	private void DebugDrawLinks()
	{
		Color white = Color.white;
		for (int i = 0; i < Grid.CellCount; i++)
		{
			int num = i * this.maxLinksPerCell;
			for (int link = this.Links[num].link; link != NavGrid.InvalidCell; link = this.Links[num].link)
			{
				NavTypeHelper.GetNavPos(i, this.Links[num].startNavType);
				if (this.DrawNavTypeLink(this.Links[num].startNavType, ref white) || this.DrawNavTypeLink(this.Links[num].endNavType, ref white))
				{
					NavTypeHelper.GetNavPos(link, this.Links[num].endNavType);
				}
				num++;
			}
		}
	}

	// Token: 0x06001902 RID: 6402 RVA: 0x00086470 File Offset: 0x00084670
	private bool DrawNavTypeLink(NavType nav_type, ref Color color)
	{
		color = this.NavTypeColor(nav_type);
		if (this.DebugViewLinksAll)
		{
			return true;
		}
		for (int i = 0; i < this.ValidNavTypes.Length; i++)
		{
			if (this.ValidNavTypes[i] == nav_type)
			{
				return this.DebugViewLinkType[i];
			}
		}
		return false;
	}

	// Token: 0x06001903 RID: 6403 RVA: 0x000864BC File Offset: 0x000846BC
	private bool DrawNavTypeCell(NavType nav_type, ref Color color)
	{
		color = this.NavTypeColor(nav_type);
		if (this.DebugViewValidCellsAll)
		{
			return true;
		}
		for (int i = 0; i < this.ValidNavTypes.Length; i++)
		{
			if (this.ValidNavTypes[i] == nav_type)
			{
				return this.DebugViewValidCellsType[i];
			}
		}
		return false;
	}

	// Token: 0x06001904 RID: 6404 RVA: 0x00086508 File Offset: 0x00084708
	public void DebugUpdate()
	{
		if (this.DebugViewValidCells)
		{
			this.DebugDrawValidCells();
		}
		if (this.DebugViewLinks)
		{
			this.DebugDrawLinks();
		}
	}

	// Token: 0x06001905 RID: 6405 RVA: 0x00086528 File Offset: 0x00084728
	public void AddDirtyCell(int cell)
	{
		if (Grid.IsValidCell(cell) && ((int)this.DirtyBitFlags[cell / 8] & 1 << cell % 8) == 0)
		{
			this.DirtyCells.Add(cell);
			byte[] dirtyBitFlags = this.DirtyBitFlags;
			int num = cell / 8;
			dirtyBitFlags[num] |= (byte)(1 << cell % 8);
		}
	}

	// Token: 0x06001906 RID: 6406 RVA: 0x0008657C File Offset: 0x0008477C
	public void Clear()
	{
		NavTableValidator[] validators = this.Validators;
		for (int i = 0; i < validators.Length; i++)
		{
			validators[i].Clear();
		}
	}

	// Token: 0x06001907 RID: 6407 RVA: 0x000865A8 File Offset: 0x000847A8
	public Color NavTypeColor(NavType navType)
	{
		if (this.debugColorLookup == null)
		{
			this.debugColorLookup = new Color[11];
			for (int i = 0; i < 11; i++)
			{
				double num = (double)i / 11.0;
				IList<double> list = ColorConverter.HUSLToRGB(new double[]
				{
					num * 360.0,
					100.0,
					50.0
				});
				this.debugColorLookup[i] = new Color((float)list[0], (float)list[1], (float)list[2]);
			}
		}
		return this.debugColorLookup[(int)navType];
	}

	// Token: 0x04000DF0 RID: 3568
	public bool DebugViewAllPaths;

	// Token: 0x04000DF1 RID: 3569
	public bool DebugViewValidCells;

	// Token: 0x04000DF2 RID: 3570
	public bool[] DebugViewValidCellsType;

	// Token: 0x04000DF3 RID: 3571
	public bool DebugViewValidCellsAll;

	// Token: 0x04000DF4 RID: 3572
	public bool DebugViewLinks;

	// Token: 0x04000DF5 RID: 3573
	public bool[] DebugViewLinkType;

	// Token: 0x04000DF6 RID: 3574
	public bool DebugViewLinksAll;

	// Token: 0x04000DF7 RID: 3575
	public static int InvalidHandle = -1;

	// Token: 0x04000DF8 RID: 3576
	public static int InvalidIdx = -1;

	// Token: 0x04000DF9 RID: 3577
	public static int InvalidCell = -1;

	// Token: 0x04000DFA RID: 3578
	public Dictionary<int, int> teleportTransitions = new Dictionary<int, int>();

	// Token: 0x04000DFB RID: 3579
	public NavGrid.Link[] Links;

	// Token: 0x04000DFD RID: 3581
	private byte[] DirtyBitFlags;

	// Token: 0x04000DFE RID: 3582
	private List<int> DirtyCells;

	// Token: 0x04000DFF RID: 3583
	private NavTableValidator[] Validators = new NavTableValidator[0];

	// Token: 0x04000E00 RID: 3584
	private CellOffset[] boundingOffsets;

	// Token: 0x04000E01 RID: 3585
	public string id;

	// Token: 0x04000E02 RID: 3586
	public bool updateEveryFrame;

	// Token: 0x04000E03 RID: 3587
	public PathFinder.PotentialScratchPad potentialScratchPad;

	// Token: 0x04000E04 RID: 3588
	public Action<IEnumerable<int>> OnNavGridUpdateComplete;

	// Token: 0x04000E07 RID: 3591
	public NavType[] ValidNavTypes;

	// Token: 0x04000E08 RID: 3592
	public NavGrid.NavTypeData[] navTypeData;

	// Token: 0x04000E0C RID: 3596
	private Color[] debugColorLookup;

	// Token: 0x0200125F RID: 4703
	public struct Link
	{
		// Token: 0x060082F8 RID: 33528 RVA: 0x0031DAF0 File Offset: 0x0031BCF0
		public Link(int link, NavType start_nav_type, NavType end_nav_type, byte transition_id, byte cost)
		{
			this.link = link;
			this.startNavType = start_nav_type;
			this.endNavType = end_nav_type;
			this.transitionId = transition_id;
			this.cost = cost;
		}

		// Token: 0x04006320 RID: 25376
		public int link;

		// Token: 0x04006321 RID: 25377
		public NavType startNavType;

		// Token: 0x04006322 RID: 25378
		public NavType endNavType;

		// Token: 0x04006323 RID: 25379
		public byte transitionId;

		// Token: 0x04006324 RID: 25380
		public byte cost;
	}

	// Token: 0x02001260 RID: 4704
	public struct NavTypeData
	{
		// Token: 0x04006325 RID: 25381
		public NavType navType;

		// Token: 0x04006326 RID: 25382
		public Vector2 animControllerOffset;

		// Token: 0x04006327 RID: 25383
		public bool flipX;

		// Token: 0x04006328 RID: 25384
		public bool flipY;

		// Token: 0x04006329 RID: 25385
		public float rotation;

		// Token: 0x0400632A RID: 25386
		public HashedString idleAnim;
	}

	// Token: 0x02001261 RID: 4705
	public struct Transition
	{
		// Token: 0x060082F9 RID: 33529 RVA: 0x0031DB18 File Offset: 0x0031BD18
		public override string ToString()
		{
			return string.Format("{0}: {1}->{2} ({3}); offset {4},{5}", new object[]
			{
				this.id,
				this.start,
				this.end,
				this.startAxis,
				this.x,
				this.y
			});
		}

		// Token: 0x060082FA RID: 33530 RVA: 0x0031DB8C File Offset: 0x0031BD8C
		public Transition(NavType start, NavType end, int x, int y, NavAxis start_axis, bool is_looping, bool loop_has_pre, bool is_escape, int cost, string anim, CellOffset[] void_offsets, CellOffset[] solid_offsets, NavOffset[] valid_nav_offsets, NavOffset[] invalid_nav_offsets, bool critter = false, float animSpeed = 1f)
		{
			DebugUtil.Assert(cost <= 255 && cost >= 0);
			this.id = byte.MaxValue;
			this.start = start;
			this.end = end;
			this.x = x;
			this.y = y;
			this.startAxis = start_axis;
			this.isLooping = is_looping;
			this.isEscape = is_escape;
			this.anim = anim;
			this.preAnim = "";
			this.cost = (byte)cost;
			if (string.IsNullOrEmpty(this.anim))
			{
				this.anim = string.Concat(new string[]
				{
					start.ToString().ToLower(),
					"_",
					end.ToString().ToLower(),
					"_",
					x.ToString(),
					"_",
					y.ToString()
				});
			}
			if (this.isLooping)
			{
				if (loop_has_pre)
				{
					this.preAnim = this.anim + "_pre";
				}
				this.anim += "_loop";
			}
			if (this.startAxis != NavAxis.NA)
			{
				this.anim += ((this.startAxis == NavAxis.X) ? "_x" : "_y");
			}
			this.voidOffsets = void_offsets;
			this.solidOffsets = solid_offsets;
			this.validNavOffsets = valid_nav_offsets;
			this.invalidNavOffsets = invalid_nav_offsets;
			this.isCritter = critter;
			this.animSpeed = animSpeed;
		}

		// Token: 0x060082FB RID: 33531 RVA: 0x0031DD18 File Offset: 0x0031BF18
		public int IsValid(int cell, NavTable nav_table)
		{
			if (!Grid.IsCellOffsetValid(cell, this.x, this.y))
			{
				return Grid.InvalidCell;
			}
			int num = Grid.OffsetCell(cell, this.x, this.y);
			if (!nav_table.IsValid(num, this.end))
			{
				return Grid.InvalidCell;
			}
			Grid.BuildFlags buildFlags = Grid.BuildFlags.Solid | Grid.BuildFlags.DupeImpassable;
			if (this.isCritter)
			{
				buildFlags |= Grid.BuildFlags.CritterImpassable;
			}
			foreach (CellOffset cellOffset in this.voidOffsets)
			{
				int num2 = Grid.OffsetCell(cell, cellOffset.x, cellOffset.y);
				if (Grid.IsValidCell(num2) && (Grid.BuildMasks[num2] & buildFlags) != ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor))
				{
					if (this.isCritter)
					{
						return Grid.InvalidCell;
					}
					if ((Grid.BuildMasks[num2] & Grid.BuildFlags.DupePassable) == ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor))
					{
						return Grid.InvalidCell;
					}
				}
			}
			foreach (CellOffset cellOffset2 in this.solidOffsets)
			{
				int num3 = Grid.OffsetCell(cell, cellOffset2.x, cellOffset2.y);
				if (Grid.IsValidCell(num3) && !Grid.Solid[num3])
				{
					return Grid.InvalidCell;
				}
			}
			foreach (NavOffset navOffset in this.validNavOffsets)
			{
				int cell2 = Grid.OffsetCell(cell, navOffset.offset.x, navOffset.offset.y);
				if (!nav_table.IsValid(cell2, navOffset.navType))
				{
					return Grid.InvalidCell;
				}
			}
			foreach (NavOffset navOffset2 in this.invalidNavOffsets)
			{
				int cell3 = Grid.OffsetCell(cell, navOffset2.offset.x, navOffset2.offset.y);
				if (nav_table.IsValid(cell3, navOffset2.navType))
				{
					return Grid.InvalidCell;
				}
			}
			if (this.start == NavType.Tube)
			{
				if (this.end == NavType.Tube)
				{
					GameObject gameObject = Grid.Objects[cell, 9];
					GameObject gameObject2 = Grid.Objects[num, 9];
					TravelTubeUtilityNetworkLink travelTubeUtilityNetworkLink = gameObject ? gameObject.GetComponent<TravelTubeUtilityNetworkLink>() : null;
					TravelTubeUtilityNetworkLink travelTubeUtilityNetworkLink2 = gameObject2 ? gameObject2.GetComponent<TravelTubeUtilityNetworkLink>() : null;
					if (travelTubeUtilityNetworkLink)
					{
						int num4;
						int num5;
						travelTubeUtilityNetworkLink.GetCells(out num4, out num5);
						if (num != num4 && num != num5)
						{
							return Grid.InvalidCell;
						}
						UtilityConnections utilityConnections = UtilityConnectionsExtensions.DirectionFromToCell(cell, num);
						if (utilityConnections == (UtilityConnections)0)
						{
							return Grid.InvalidCell;
						}
						if (Game.Instance.travelTubeSystem.GetConnections(num, false) != utilityConnections)
						{
							return Grid.InvalidCell;
						}
					}
					else if (travelTubeUtilityNetworkLink2)
					{
						int num6;
						int num7;
						travelTubeUtilityNetworkLink2.GetCells(out num6, out num7);
						if (cell != num6 && cell != num7)
						{
							return Grid.InvalidCell;
						}
						UtilityConnections utilityConnections2 = UtilityConnectionsExtensions.DirectionFromToCell(num, cell);
						if (utilityConnections2 == (UtilityConnections)0)
						{
							return Grid.InvalidCell;
						}
						if (Game.Instance.travelTubeSystem.GetConnections(cell, false) != utilityConnections2)
						{
							return Grid.InvalidCell;
						}
					}
					else
					{
						bool flag = this.startAxis == NavAxis.X;
						int cell4 = cell;
						for (int j = 0; j < 2; j++)
						{
							if ((flag && j == 0) || (!flag && j == 1))
							{
								int num8 = (this.x > 0) ? 1 : -1;
								for (int k = 0; k < Mathf.Abs(this.x); k++)
								{
									UtilityConnections connections = Game.Instance.travelTubeSystem.GetConnections(cell4, false);
									if (num8 > 0 && (connections & UtilityConnections.Right) == (UtilityConnections)0)
									{
										return Grid.InvalidCell;
									}
									if (num8 < 0 && (connections & UtilityConnections.Left) == (UtilityConnections)0)
									{
										return Grid.InvalidCell;
									}
									cell4 = Grid.OffsetCell(cell4, num8, 0);
								}
							}
							else
							{
								int num9 = (this.y > 0) ? 1 : -1;
								for (int l = 0; l < Mathf.Abs(this.y); l++)
								{
									UtilityConnections connections2 = Game.Instance.travelTubeSystem.GetConnections(cell4, false);
									if (num9 > 0 && (connections2 & UtilityConnections.Up) == (UtilityConnections)0)
									{
										return Grid.InvalidCell;
									}
									if (num9 < 0 && (connections2 & UtilityConnections.Down) == (UtilityConnections)0)
									{
										return Grid.InvalidCell;
									}
									cell4 = Grid.OffsetCell(cell4, 0, num9);
								}
							}
						}
					}
				}
				else
				{
					UtilityConnections connections3 = Game.Instance.travelTubeSystem.GetConnections(cell, false);
					if (this.y > 0)
					{
						if (connections3 != UtilityConnections.Down)
						{
							return Grid.InvalidCell;
						}
					}
					else if (this.x > 0)
					{
						if (connections3 != UtilityConnections.Left)
						{
							return Grid.InvalidCell;
						}
					}
					else if (this.x < 0)
					{
						if (connections3 != UtilityConnections.Right)
						{
							return Grid.InvalidCell;
						}
					}
					else
					{
						if (this.y >= 0)
						{
							return Grid.InvalidCell;
						}
						if (connections3 != UtilityConnections.Up)
						{
							return Grid.InvalidCell;
						}
					}
				}
			}
			else if (this.start == NavType.Floor && this.end == NavType.Tube)
			{
				int cell5 = Grid.OffsetCell(cell, this.x, this.y);
				if (Game.Instance.travelTubeSystem.GetConnections(cell5, false) != UtilityConnections.Up)
				{
					return Grid.InvalidCell;
				}
			}
			return num;
		}

		// Token: 0x0400632B RID: 25387
		public NavType start;

		// Token: 0x0400632C RID: 25388
		public NavType end;

		// Token: 0x0400632D RID: 25389
		public NavAxis startAxis;

		// Token: 0x0400632E RID: 25390
		public int x;

		// Token: 0x0400632F RID: 25391
		public int y;

		// Token: 0x04006330 RID: 25392
		public byte id;

		// Token: 0x04006331 RID: 25393
		public byte cost;

		// Token: 0x04006332 RID: 25394
		public bool isLooping;

		// Token: 0x04006333 RID: 25395
		public bool isEscape;

		// Token: 0x04006334 RID: 25396
		public string preAnim;

		// Token: 0x04006335 RID: 25397
		public string anim;

		// Token: 0x04006336 RID: 25398
		public float animSpeed;

		// Token: 0x04006337 RID: 25399
		public CellOffset[] voidOffsets;

		// Token: 0x04006338 RID: 25400
		public CellOffset[] solidOffsets;

		// Token: 0x04006339 RID: 25401
		public NavOffset[] validNavOffsets;

		// Token: 0x0400633A RID: 25402
		public NavOffset[] invalidNavOffsets;

		// Token: 0x0400633B RID: 25403
		public bool isCritter;
	}
}
