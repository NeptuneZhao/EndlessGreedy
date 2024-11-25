using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A70 RID: 2672
public class RoomProber : ISim1000ms
{
	// Token: 0x06004DBE RID: 19902 RVA: 0x001BE694 File Offset: 0x001BC894
	public RoomProber()
	{
		this.CellCavityID = new HandleVector<int>.Handle[Grid.CellCount];
		this.floodFiller = new RoomProber.CavityFloodFiller(this.CellCavityID);
		for (int i = 0; i < this.CellCavityID.Length; i++)
		{
			this.solidChanges.Add(i);
		}
		this.ProcessSolidChanges();
		this.RefreshRooms();
		Game instance = Game.Instance;
		instance.OnSpawnComplete = (System.Action)Delegate.Combine(instance.OnSpawnComplete, new System.Action(this.Refresh));
		World instance2 = World.Instance;
		instance2.OnSolidChanged = (Action<int>)Delegate.Combine(instance2.OnSolidChanged, new Action<int>(this.SolidChangedEvent));
		GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], new Action<int, object>(this.OnBuildingsChanged));
		GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[2], new Action<int, object>(this.OnBuildingsChanged));
	}

	// Token: 0x06004DBF RID: 19903 RVA: 0x001BE7DD File Offset: 0x001BC9DD
	public void Refresh()
	{
		this.ProcessSolidChanges();
		this.RefreshRooms();
	}

	// Token: 0x06004DC0 RID: 19904 RVA: 0x001BE7EB File Offset: 0x001BC9EB
	private void SolidChangedEvent(int cell)
	{
		this.SolidChangedEvent(cell, true);
	}

	// Token: 0x06004DC1 RID: 19905 RVA: 0x001BE7F5 File Offset: 0x001BC9F5
	private void OnBuildingsChanged(int cell, object building)
	{
		if (this.GetCavityForCell(cell) != null)
		{
			this.solidChanges.Add(cell);
			this.dirty = true;
		}
	}

	// Token: 0x06004DC2 RID: 19906 RVA: 0x001BE814 File Offset: 0x001BCA14
	public void SolidChangedEvent(int cell, bool ignoreDoors)
	{
		if (ignoreDoors && Grid.HasDoor[cell])
		{
			return;
		}
		this.solidChanges.Add(cell);
		this.dirty = true;
	}

	// Token: 0x06004DC3 RID: 19907 RVA: 0x001BE83C File Offset: 0x001BCA3C
	private CavityInfo CreateNewCavity()
	{
		CavityInfo cavityInfo = new CavityInfo();
		cavityInfo.handle = this.cavityInfos.Allocate(cavityInfo);
		return cavityInfo;
	}

	// Token: 0x06004DC4 RID: 19908 RVA: 0x001BE864 File Offset: 0x001BCA64
	private unsafe void ProcessSolidChanges()
	{
		int* ptr = stackalloc int[(UIntPtr)20];
		*ptr = 0;
		ptr[1] = -Grid.WidthInCells;
		ptr[2] = -1;
		ptr[3] = 1;
		ptr[4] = Grid.WidthInCells;
		foreach (int num in this.solidChanges)
		{
			for (int i = 0; i < 5; i++)
			{
				int num2 = num + ptr[i];
				if (Grid.IsValidCell(num2))
				{
					this.floodFillSet.Add(num2);
					HandleVector<int>.Handle item = this.CellCavityID[num2];
					if (item.IsValid())
					{
						this.CellCavityID[num2] = HandleVector<int>.InvalidHandle;
						this.releasedIDs.Add(item);
					}
				}
			}
		}
		CavityInfo cavityInfo = this.CreateNewCavity();
		foreach (int num3 in this.floodFillSet)
		{
			if (!this.visitedCells.Contains(num3))
			{
				HandleVector<int>.Handle handle = this.CellCavityID[num3];
				if (!handle.IsValid())
				{
					CavityInfo cavityInfo2 = cavityInfo;
					this.floodFiller.Reset(cavityInfo2.handle);
					GameUtil.FloodFillConditional(num3, new Func<int, bool>(this.floodFiller.ShouldContinue), this.visitedCells, null);
					if (this.floodFiller.NumCells > 0)
					{
						cavityInfo2.numCells = this.floodFiller.NumCells;
						cavityInfo2.minX = this.floodFiller.MinX;
						cavityInfo2.minY = this.floodFiller.MinY;
						cavityInfo2.maxX = this.floodFiller.MaxX;
						cavityInfo2.maxY = this.floodFiller.MaxY;
						cavityInfo = this.CreateNewCavity();
					}
				}
			}
		}
		if (cavityInfo.numCells == 0)
		{
			this.releasedIDs.Add(cavityInfo.handle);
		}
		foreach (HandleVector<int>.Handle handle2 in this.releasedIDs)
		{
			CavityInfo data = this.cavityInfos.GetData(handle2);
			this.releasedCritters.AddRange(data.creatures);
			if (data.room != null)
			{
				this.ClearRoom(data.room);
			}
			this.cavityInfos.Free(handle2);
		}
		this.RebuildDirtyCavities(this.visitedCells);
		this.releasedIDs.Clear();
		this.visitedCells.Clear();
		this.solidChanges.Clear();
		this.floodFillSet.Clear();
	}

	// Token: 0x06004DC5 RID: 19909 RVA: 0x001BEB38 File Offset: 0x001BCD38
	private void RebuildDirtyCavities(ICollection<int> visited_cells)
	{
		int maxRoomSize = TuningData<RoomProber.Tuning>.Get().maxRoomSize;
		foreach (int num in visited_cells)
		{
			HandleVector<int>.Handle handle = this.CellCavityID[num];
			if (handle.IsValid())
			{
				CavityInfo data = this.cavityInfos.GetData(handle);
				if (0 < data.numCells && data.numCells <= maxRoomSize)
				{
					GameObject gameObject = Grid.Objects[num, 1];
					if (gameObject != null)
					{
						KPrefabID component = gameObject.GetComponent<KPrefabID>();
						bool flag = false;
						foreach (KPrefabID kprefabID in data.buildings)
						{
							if (component.InstanceID == kprefabID.InstanceID)
							{
								flag = true;
								break;
							}
						}
						foreach (KPrefabID kprefabID2 in data.plants)
						{
							if (component.InstanceID == kprefabID2.InstanceID)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							if (component.HasTag(GameTags.RoomProberBuilding))
							{
								data.AddBuilding(component);
							}
							else if (component.HasTag(GameTags.Plant) && !component.HasTag(GameTags.PlantBranch))
							{
								data.AddPlants(component);
							}
						}
					}
				}
			}
		}
		visited_cells.Clear();
	}

	// Token: 0x06004DC6 RID: 19910 RVA: 0x001BED04 File Offset: 0x001BCF04
	public void Sim1000ms(float dt)
	{
		if (this.dirty)
		{
			this.ProcessSolidChanges();
			this.RefreshRooms();
		}
	}

	// Token: 0x06004DC7 RID: 19911 RVA: 0x001BED1C File Offset: 0x001BCF1C
	private void CreateRoom(CavityInfo cavity)
	{
		global::Debug.Assert(cavity.room == null);
		Room room = new Room();
		room.cavity = cavity;
		cavity.room = room;
		this.rooms.Add(room);
		room.roomType = Db.Get().RoomTypes.GetRoomType(room);
		this.AssignBuildingsToRoom(room);
	}

	// Token: 0x06004DC8 RID: 19912 RVA: 0x001BED74 File Offset: 0x001BCF74
	private void ClearRoom(Room room)
	{
		this.UnassignBuildingsToRoom(room);
		room.CleanUp();
		this.rooms.Remove(room);
	}

	// Token: 0x06004DC9 RID: 19913 RVA: 0x001BED90 File Offset: 0x001BCF90
	private void RefreshRooms()
	{
		int maxRoomSize = TuningData<RoomProber.Tuning>.Get().maxRoomSize;
		foreach (CavityInfo cavityInfo in this.cavityInfos.GetDataList())
		{
			if (cavityInfo.dirty)
			{
				global::Debug.Assert(cavityInfo.room == null, "I expected info.room to always be null by this point");
				if (cavityInfo.numCells > 0)
				{
					if (cavityInfo.numCells <= maxRoomSize)
					{
						this.CreateRoom(cavityInfo);
					}
					foreach (KPrefabID kprefabID in cavityInfo.buildings)
					{
						kprefabID.Trigger(144050788, cavityInfo.room);
					}
					foreach (KPrefabID kprefabID2 in cavityInfo.plants)
					{
						kprefabID2.Trigger(144050788, cavityInfo.room);
					}
				}
				cavityInfo.dirty = false;
			}
		}
		foreach (KPrefabID kprefabID3 in this.releasedCritters)
		{
			if (kprefabID3 != null)
			{
				OvercrowdingMonitor.Instance smi = kprefabID3.GetSMI<OvercrowdingMonitor.Instance>();
				if (smi != null)
				{
					smi.RoomRefreshUpdateCavity();
				}
			}
		}
		this.releasedCritters.Clear();
		this.dirty = false;
	}

	// Token: 0x06004DCA RID: 19914 RVA: 0x001BEF34 File Offset: 0x001BD134
	private void AssignBuildingsToRoom(Room room)
	{
		global::Debug.Assert(room != null);
		RoomType roomType = room.roomType;
		if (roomType == Db.Get().RoomTypes.Neutral)
		{
			return;
		}
		foreach (KPrefabID kprefabID in room.buildings)
		{
			if (!(kprefabID == null) && !kprefabID.HasTag(GameTags.NotRoomAssignable))
			{
				Assignable component = kprefabID.GetComponent<Assignable>();
				if (component != null && (roomType.primary_constraint == null || !roomType.primary_constraint.building_criteria(kprefabID.GetComponent<KPrefabID>())))
				{
					component.Assign(room);
				}
			}
		}
	}

	// Token: 0x06004DCB RID: 19915 RVA: 0x001BEFF0 File Offset: 0x001BD1F0
	private void UnassignKPrefabIDs(Room room, List<KPrefabID> list)
	{
		foreach (KPrefabID kprefabID in list)
		{
			if (!(kprefabID == null))
			{
				kprefabID.Trigger(144050788, null);
				Assignable component = kprefabID.GetComponent<Assignable>();
				if (component != null && component.assignee == room)
				{
					component.Unassign();
				}
			}
		}
	}

	// Token: 0x06004DCC RID: 19916 RVA: 0x001BF06C File Offset: 0x001BD26C
	private void UnassignBuildingsToRoom(Room room)
	{
		global::Debug.Assert(room != null);
		this.UnassignKPrefabIDs(room, room.buildings);
		this.UnassignKPrefabIDs(room, room.plants);
	}

	// Token: 0x06004DCD RID: 19917 RVA: 0x001BF094 File Offset: 0x001BD294
	public void UpdateRoom(CavityInfo cavity)
	{
		if (cavity == null)
		{
			return;
		}
		if (cavity.room != null)
		{
			this.ClearRoom(cavity.room);
			cavity.room = null;
		}
		this.CreateRoom(cavity);
		foreach (KPrefabID kprefabID in cavity.buildings)
		{
			if (kprefabID != null)
			{
				kprefabID.Trigger(144050788, cavity.room);
			}
		}
		foreach (KPrefabID kprefabID2 in cavity.plants)
		{
			if (kprefabID2 != null)
			{
				kprefabID2.Trigger(144050788, cavity.room);
			}
		}
	}

	// Token: 0x06004DCE RID: 19918 RVA: 0x001BF178 File Offset: 0x001BD378
	public Room GetRoomOfGameObject(GameObject go)
	{
		if (go == null)
		{
			return null;
		}
		int cell = Grid.PosToCell(go);
		if (!Grid.IsValidCell(cell))
		{
			return null;
		}
		CavityInfo cavityForCell = this.GetCavityForCell(cell);
		if (cavityForCell == null)
		{
			return null;
		}
		return cavityForCell.room;
	}

	// Token: 0x06004DCF RID: 19919 RVA: 0x001BF1B4 File Offset: 0x001BD3B4
	public bool IsInRoomType(GameObject go, RoomType checkType)
	{
		Room roomOfGameObject = this.GetRoomOfGameObject(go);
		if (roomOfGameObject != null)
		{
			RoomType roomType = roomOfGameObject.roomType;
			return checkType == roomType;
		}
		return false;
	}

	// Token: 0x06004DD0 RID: 19920 RVA: 0x001BF1DC File Offset: 0x001BD3DC
	private CavityInfo GetCavityInfo(HandleVector<int>.Handle id)
	{
		CavityInfo result = null;
		if (id.IsValid())
		{
			result = this.cavityInfos.GetData(id);
		}
		return result;
	}

	// Token: 0x06004DD1 RID: 19921 RVA: 0x001BF204 File Offset: 0x001BD404
	public CavityInfo GetCavityForCell(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return null;
		}
		HandleVector<int>.Handle id = this.CellCavityID[cell];
		return this.GetCavityInfo(id);
	}

	// Token: 0x040033BE RID: 13246
	public List<Room> rooms = new List<Room>();

	// Token: 0x040033BF RID: 13247
	private KCompactedVector<CavityInfo> cavityInfos = new KCompactedVector<CavityInfo>(1024);

	// Token: 0x040033C0 RID: 13248
	private HandleVector<int>.Handle[] CellCavityID;

	// Token: 0x040033C1 RID: 13249
	private bool dirty = true;

	// Token: 0x040033C2 RID: 13250
	private HashSet<int> solidChanges = new HashSet<int>();

	// Token: 0x040033C3 RID: 13251
	private HashSet<int> visitedCells = new HashSet<int>();

	// Token: 0x040033C4 RID: 13252
	private HashSet<int> floodFillSet = new HashSet<int>();

	// Token: 0x040033C5 RID: 13253
	private HashSet<HandleVector<int>.Handle> releasedIDs = new HashSet<HandleVector<int>.Handle>();

	// Token: 0x040033C6 RID: 13254
	private RoomProber.CavityFloodFiller floodFiller;

	// Token: 0x040033C7 RID: 13255
	private List<KPrefabID> releasedCritters = new List<KPrefabID>();

	// Token: 0x02001A89 RID: 6793
	public class Tuning : TuningData<RoomProber.Tuning>
	{
		// Token: 0x04007CDE RID: 31966
		public int maxRoomSize;
	}

	// Token: 0x02001A8A RID: 6794
	private class CavityFloodFiller
	{
		// Token: 0x0600A073 RID: 41075 RVA: 0x0037FBA5 File Offset: 0x0037DDA5
		public CavityFloodFiller(HandleVector<int>.Handle[] grid)
		{
			this.grid = grid;
		}

		// Token: 0x0600A074 RID: 41076 RVA: 0x0037FBB4 File Offset: 0x0037DDB4
		public void Reset(HandleVector<int>.Handle search_id)
		{
			this.cavityID = search_id;
			this.numCells = 0;
			this.minX = int.MaxValue;
			this.minY = int.MaxValue;
			this.maxX = 0;
			this.maxY = 0;
		}

		// Token: 0x0600A075 RID: 41077 RVA: 0x0037FBE8 File Offset: 0x0037DDE8
		private static bool IsWall(int cell)
		{
			return (Grid.BuildMasks[cell] & (Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation)) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor) || Grid.HasDoor[cell];
		}

		// Token: 0x0600A076 RID: 41078 RVA: 0x0037FC08 File Offset: 0x0037DE08
		public bool ShouldContinue(int flood_cell)
		{
			if (RoomProber.CavityFloodFiller.IsWall(flood_cell))
			{
				this.grid[flood_cell] = HandleVector<int>.InvalidHandle;
				return false;
			}
			this.grid[flood_cell] = this.cavityID;
			int val;
			int val2;
			Grid.CellToXY(flood_cell, out val, out val2);
			this.minX = Math.Min(val, this.minX);
			this.minY = Math.Min(val2, this.minY);
			this.maxX = Math.Max(val, this.maxX);
			this.maxY = Math.Max(val2, this.maxY);
			this.numCells++;
			return true;
		}

		// Token: 0x17000B0F RID: 2831
		// (get) Token: 0x0600A077 RID: 41079 RVA: 0x0037FCA3 File Offset: 0x0037DEA3
		public int NumCells
		{
			get
			{
				return this.numCells;
			}
		}

		// Token: 0x17000B10 RID: 2832
		// (get) Token: 0x0600A078 RID: 41080 RVA: 0x0037FCAB File Offset: 0x0037DEAB
		public int MinX
		{
			get
			{
				return this.minX;
			}
		}

		// Token: 0x17000B11 RID: 2833
		// (get) Token: 0x0600A079 RID: 41081 RVA: 0x0037FCB3 File Offset: 0x0037DEB3
		public int MinY
		{
			get
			{
				return this.minY;
			}
		}

		// Token: 0x17000B12 RID: 2834
		// (get) Token: 0x0600A07A RID: 41082 RVA: 0x0037FCBB File Offset: 0x0037DEBB
		public int MaxX
		{
			get
			{
				return this.maxX;
			}
		}

		// Token: 0x17000B13 RID: 2835
		// (get) Token: 0x0600A07B RID: 41083 RVA: 0x0037FCC3 File Offset: 0x0037DEC3
		public int MaxY
		{
			get
			{
				return this.maxY;
			}
		}

		// Token: 0x04007CDF RID: 31967
		private HandleVector<int>.Handle[] grid;

		// Token: 0x04007CE0 RID: 31968
		private HandleVector<int>.Handle cavityID;

		// Token: 0x04007CE1 RID: 31969
		private int numCells;

		// Token: 0x04007CE2 RID: 31970
		private int minX;

		// Token: 0x04007CE3 RID: 31971
		private int minY;

		// Token: 0x04007CE4 RID: 31972
		private int maxX;

		// Token: 0x04007CE5 RID: 31973
		private int maxY;
	}
}
