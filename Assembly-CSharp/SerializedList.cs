using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using KSerialization;

// Token: 0x02000CD1 RID: 3281
[SerializationConfig(MemberSerialization.OptIn)]
public class SerializedList<ItemType>
{
	// Token: 0x17000756 RID: 1878
	// (get) Token: 0x0600657D RID: 25981 RVA: 0x0025E34A File Offset: 0x0025C54A
	public int Count
	{
		get
		{
			return this.items.Count;
		}
	}

	// Token: 0x0600657E RID: 25982 RVA: 0x0025E357 File Offset: 0x0025C557
	public IEnumerator<ItemType> GetEnumerator()
	{
		return this.items.GetEnumerator();
	}

	// Token: 0x17000757 RID: 1879
	public ItemType this[int idx]
	{
		get
		{
			return this.items[idx];
		}
	}

	// Token: 0x06006580 RID: 25984 RVA: 0x0025E377 File Offset: 0x0025C577
	public void Add(ItemType item)
	{
		this.items.Add(item);
	}

	// Token: 0x06006581 RID: 25985 RVA: 0x0025E385 File Offset: 0x0025C585
	public void Remove(ItemType item)
	{
		this.items.Remove(item);
	}

	// Token: 0x06006582 RID: 25986 RVA: 0x0025E394 File Offset: 0x0025C594
	public void RemoveAt(int idx)
	{
		this.items.RemoveAt(idx);
	}

	// Token: 0x06006583 RID: 25987 RVA: 0x0025E3A2 File Offset: 0x0025C5A2
	public bool Contains(ItemType item)
	{
		return this.items.Contains(item);
	}

	// Token: 0x06006584 RID: 25988 RVA: 0x0025E3B0 File Offset: 0x0025C5B0
	public void Clear()
	{
		this.items.Clear();
	}

	// Token: 0x06006585 RID: 25989 RVA: 0x0025E3C0 File Offset: 0x0025C5C0
	[OnSerializing]
	private void OnSerializing()
	{
		MemoryStream memoryStream = new MemoryStream();
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(this.items.Count);
		foreach (ItemType itemType in this.items)
		{
			binaryWriter.WriteKleiString(itemType.GetType().FullName);
			long position = binaryWriter.BaseStream.Position;
			binaryWriter.Write(0);
			long position2 = binaryWriter.BaseStream.Position;
			Serializer.SerializeTypeless(itemType, binaryWriter);
			long position3 = binaryWriter.BaseStream.Position;
			long num = position3 - position2;
			binaryWriter.BaseStream.Position = position;
			binaryWriter.Write((int)num);
			binaryWriter.BaseStream.Position = position3;
		}
		memoryStream.Flush();
		this.serializationBuffer = memoryStream.ToArray();
	}

	// Token: 0x06006586 RID: 25990 RVA: 0x0025E4C0 File Offset: 0x0025C6C0
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.serializationBuffer == null)
		{
			return;
		}
		FastReader fastReader = new FastReader(this.serializationBuffer);
		int num = fastReader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			string text = fastReader.ReadKleiString();
			int num2 = fastReader.ReadInt32();
			int position = fastReader.Position;
			Type type = Type.GetType(text);
			if (type == null)
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Type no longer exists: " + text
				});
				fastReader.SkipBytes(num2);
			}
			else
			{
				ItemType itemType;
				if (typeof(ItemType) != type)
				{
					itemType = (ItemType)((object)Activator.CreateInstance(type));
				}
				else
				{
					itemType = default(ItemType);
				}
				Deserializer.DeserializeTypeless(itemType, fastReader);
				if (fastReader.Position != position + num2)
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						"Expected to be at offset",
						position + num2,
						"but was only at offset",
						fastReader.Position,
						". Skipping to catch up."
					});
					fastReader.SkipBytes(position + num2 - fastReader.Position);
				}
				this.items.Add(itemType);
			}
		}
	}

	// Token: 0x04004495 RID: 17557
	[Serialize]
	private byte[] serializationBuffer;

	// Token: 0x04004496 RID: 17558
	private List<ItemType> items = new List<ItemType>();
}
