using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

// Token: 0x0200059B RID: 1435
public class OldNoteEntriesV5
{
	// Token: 0x060021C8 RID: 8648 RVA: 0x000BC388 File Offset: 0x000BA588
	public void Deserialize(BinaryReader reader)
	{
		int num = reader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			OldNoteEntriesV5.NoteStorageBlock item = default(OldNoteEntriesV5.NoteStorageBlock);
			item.Deserialize(reader);
			this.storageBlocks.Add(item);
		}
	}

	// Token: 0x04001304 RID: 4868
	public List<OldNoteEntriesV5.NoteStorageBlock> storageBlocks = new List<OldNoteEntriesV5.NoteStorageBlock>();

	// Token: 0x02001388 RID: 5000
	[StructLayout(LayoutKind.Explicit)]
	public struct NoteEntry
	{
		// Token: 0x040066EF RID: 26351
		[FieldOffset(0)]
		public int reportEntryId;

		// Token: 0x040066F0 RID: 26352
		[FieldOffset(4)]
		public int noteHash;

		// Token: 0x040066F1 RID: 26353
		[FieldOffset(8)]
		public float value;
	}

	// Token: 0x02001389 RID: 5001
	[StructLayout(LayoutKind.Explicit)]
	public struct NoteEntryArray
	{
		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x06008778 RID: 34680 RVA: 0x0032BC53 File Offset: 0x00329E53
		public int StructSizeInBytes
		{
			get
			{
				return Marshal.SizeOf(typeof(OldNoteEntriesV5.NoteEntry));
			}
		}

		// Token: 0x040066F2 RID: 26354
		[FieldOffset(0)]
		public byte[] bytes;

		// Token: 0x040066F3 RID: 26355
		[FieldOffset(0)]
		public OldNoteEntriesV5.NoteEntry[] structs;
	}

	// Token: 0x0200138A RID: 5002
	public struct NoteStorageBlock
	{
		// Token: 0x06008779 RID: 34681 RVA: 0x0032BC64 File Offset: 0x00329E64
		public void Deserialize(BinaryReader reader)
		{
			this.entryCount = reader.ReadInt32();
			this.entries.bytes = reader.ReadBytes(this.entries.StructSizeInBytes * this.entryCount);
		}

		// Token: 0x040066F4 RID: 26356
		public int entryCount;

		// Token: 0x040066F5 RID: 26357
		public OldNoteEntriesV5.NoteEntryArray entries;
	}
}
