﻿using System;
using System.Collections.Generic;
using System.IO;
using KSerialization;

// Token: 0x020004D3 RID: 1235
public class StateMachineSerializer
{
	// Token: 0x06001AA6 RID: 6822 RVA: 0x0008C2C8 File Offset: 0x0008A4C8
	public void Serialize(List<StateMachine.Instance> state_machines, BinaryWriter writer)
	{
		writer.Write(StateMachineSerializer.SERIALIZER_VERSION);
		long position = writer.BaseStream.Position;
		writer.Write(0);
		long position2 = writer.BaseStream.Position;
		try
		{
			int num = (int)writer.BaseStream.Position;
			int num2 = 0;
			writer.Write(num2);
			using (List<StateMachine.Instance>.Enumerator enumerator = state_machines.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (StateMachineSerializer.Entry.TrySerialize(enumerator.Current, writer))
					{
						num2++;
					}
				}
			}
			int num3 = (int)writer.BaseStream.Position;
			writer.BaseStream.Position = (long)num;
			writer.Write(num2);
			writer.BaseStream.Position = (long)num3;
		}
		catch (Exception obj)
		{
			Debug.Log("StateMachines: ");
			foreach (StateMachine.Instance instance in state_machines)
			{
				Debug.Log(instance.ToString());
			}
			Debug.LogError(obj);
		}
		long position3 = writer.BaseStream.Position;
		long num4 = position3 - position2;
		writer.BaseStream.Position = position;
		writer.Write((int)num4);
		writer.BaseStream.Position = position3;
	}

	// Token: 0x06001AA7 RID: 6823 RVA: 0x0008C428 File Offset: 0x0008A628
	public void Deserialize(IReader reader)
	{
		int num = reader.ReadInt32();
		int length = reader.ReadInt32();
		if (num < 10)
		{
			Debug.LogWarning(string.Concat(new string[]
			{
				"State machine serializer version mismatch: ",
				num.ToString(),
				"!=",
				StateMachineSerializer.SERIALIZER_VERSION.ToString(),
				"\nDiscarding data."
			}));
			reader.SkipBytes(length);
			return;
		}
		if (num < 12)
		{
			this.entries = StateMachineSerializer.OldEntryV11.DeserializeOldEntries(reader, num);
			return;
		}
		int num2 = reader.ReadInt32();
		this.entries = new List<StateMachineSerializer.Entry>(num2);
		for (int i = 0; i < num2; i++)
		{
			StateMachineSerializer.Entry entry = StateMachineSerializer.Entry.Deserialize(reader, num);
			if (entry != null)
			{
				this.entries.Add(entry);
			}
		}
	}

	// Token: 0x06001AA8 RID: 6824 RVA: 0x0008C4DC File Offset: 0x0008A6DC
	private static string TrimAssemblyInfo(string type_name)
	{
		int num = type_name.IndexOf("[[");
		if (num != -1)
		{
			return type_name.Substring(0, num);
		}
		return type_name;
	}

	// Token: 0x06001AA9 RID: 6825 RVA: 0x0008C504 File Offset: 0x0008A704
	public bool Restore(StateMachine.Instance instance)
	{
		Type type = instance.GetType();
		for (int i = 0; i < this.entries.Count; i++)
		{
			StateMachineSerializer.Entry entry = this.entries[i];
			if (entry.type == type && instance.serializationSuffix == entry.typeSuffix)
			{
				this.entries.RemoveAt(i);
				return entry.Restore(instance);
			}
		}
		return false;
	}

	// Token: 0x06001AAA RID: 6826 RVA: 0x0008C571 File Offset: 0x0008A771
	private static bool DoesVersionHaveTypeSuffix(int version)
	{
		return version >= 20 || version == 11;
	}

	// Token: 0x04000F19 RID: 3865
	public const int SERIALIZER_PRE_DLC1 = 10;

	// Token: 0x04000F1A RID: 3866
	public const int SERIALIZER_TYPE_SUFFIX = 11;

	// Token: 0x04000F1B RID: 3867
	public const int SERIALIZER_OPTIMIZE_BUFFERS = 12;

	// Token: 0x04000F1C RID: 3868
	public const int SERIALIZER_EXPANSION1 = 20;

	// Token: 0x04000F1D RID: 3869
	private static int SERIALIZER_VERSION = 20;

	// Token: 0x04000F1E RID: 3870
	private const string TargetParameterName = "TargetParameter";

	// Token: 0x04000F1F RID: 3871
	private List<StateMachineSerializer.Entry> entries = new List<StateMachineSerializer.Entry>();

	// Token: 0x020012AC RID: 4780
	private class Entry
	{
		// Token: 0x060084AB RID: 33963 RVA: 0x00323DEC File Offset: 0x00321FEC
		public static bool TrySerialize(StateMachine.Instance smi, BinaryWriter writer)
		{
			if (!smi.IsRunning())
			{
				return false;
			}
			int num = (int)writer.BaseStream.Position;
			writer.Write(0);
			writer.WriteKleiString(smi.GetType().FullName);
			writer.WriteKleiString(smi.serializationSuffix);
			writer.WriteKleiString(smi.GetCurrentState().name);
			int num2 = (int)writer.BaseStream.Position;
			writer.Write(0);
			int num3 = (int)writer.BaseStream.Position;
			Serializer.SerializeTypeless(smi, writer);
			if (smi.GetStateMachine().serializable == StateMachine.SerializeType.ParamsOnly || smi.GetStateMachine().serializable == StateMachine.SerializeType.Both_DEPRECATED)
			{
				StateMachine.Parameter.Context[] parameterContexts = smi.GetParameterContexts();
				writer.Write(parameterContexts.Length);
				foreach (StateMachine.Parameter.Context context in parameterContexts)
				{
					long position = (long)((int)writer.BaseStream.Position);
					writer.Write(0);
					long num4 = (long)((int)writer.BaseStream.Position);
					writer.WriteKleiString(context.GetType().FullName);
					writer.WriteKleiString(context.parameter.name);
					context.Serialize(writer);
					long num5 = (long)((int)writer.BaseStream.Position);
					writer.BaseStream.Position = position;
					long num6 = num5 - num4;
					writer.Write((int)num6);
					writer.BaseStream.Position = num5;
				}
			}
			int num7 = (int)writer.BaseStream.Position - num3;
			if (num7 > 0)
			{
				int num8 = (int)writer.BaseStream.Position;
				writer.BaseStream.Position = (long)num2;
				writer.Write(num7);
				writer.BaseStream.Position = (long)num8;
				return true;
			}
			writer.BaseStream.Position = (long)num;
			writer.BaseStream.SetLength((long)num);
			return false;
		}

		// Token: 0x060084AC RID: 33964 RVA: 0x00323FAC File Offset: 0x003221AC
		public static StateMachineSerializer.Entry Deserialize(IReader reader, int serializerVersion)
		{
			StateMachineSerializer.Entry entry = new StateMachineSerializer.Entry();
			reader.ReadInt32();
			entry.version = serializerVersion;
			string typeName = reader.ReadKleiString();
			entry.type = Type.GetType(typeName);
			entry.typeSuffix = (StateMachineSerializer.DoesVersionHaveTypeSuffix(serializerVersion) ? reader.ReadKleiString() : null);
			entry.currentState = reader.ReadKleiString();
			int length = reader.ReadInt32();
			entry.entryData = new FastReader(reader.ReadBytes(length));
			if (entry.type == null)
			{
				return null;
			}
			return entry;
		}

		// Token: 0x060084AD RID: 33965 RVA: 0x00324030 File Offset: 0x00322230
		public bool Restore(StateMachine.Instance smi)
		{
			if (Manager.HasDeserializationMapping(smi.GetType()))
			{
				Deserializer.DeserializeTypeless(smi, this.entryData);
			}
			StateMachine.SerializeType serializable = smi.GetStateMachine().serializable;
			if (serializable == StateMachine.SerializeType.Never)
			{
				return false;
			}
			if ((serializable == StateMachine.SerializeType.Both_DEPRECATED || serializable == StateMachine.SerializeType.ParamsOnly) && !this.entryData.IsFinished)
			{
				StateMachine.Parameter.Context[] parameterContexts = smi.GetParameterContexts();
				int num = this.entryData.ReadInt32();
				for (int i = 0; i < num; i++)
				{
					int num2 = this.entryData.ReadInt32();
					int position = this.entryData.Position;
					string text = this.entryData.ReadKleiString();
					text = text.Replace("Version=2.0.0.0", "Version=4.0.0.0");
					string b = this.entryData.ReadKleiString();
					foreach (StateMachine.Parameter.Context context in parameterContexts)
					{
						if (context.parameter.name == b && (this.version > 10 || !(context.parameter.GetType().Name == "TargetParameter")) && context.GetType().FullName == text)
						{
							context.Deserialize(this.entryData, smi);
							break;
						}
					}
					this.entryData.SkipBytes(num2 - (this.entryData.Position - position));
				}
			}
			if (serializable == StateMachine.SerializeType.Both_DEPRECATED || serializable == StateMachine.SerializeType.CurrentStateOnly_DEPRECATED)
			{
				StateMachine.BaseState state = smi.GetStateMachine().GetState(this.currentState);
				if (state != null)
				{
					smi.GoTo(state);
					return true;
				}
			}
			return false;
		}

		// Token: 0x040063F9 RID: 25593
		public int version;

		// Token: 0x040063FA RID: 25594
		public Type type;

		// Token: 0x040063FB RID: 25595
		public string typeSuffix;

		// Token: 0x040063FC RID: 25596
		public string currentState;

		// Token: 0x040063FD RID: 25597
		public FastReader entryData;
	}

	// Token: 0x020012AD RID: 4781
	private class OldEntryV11
	{
		// Token: 0x060084AF RID: 33967 RVA: 0x003241BB File Offset: 0x003223BB
		public OldEntryV11(int version, int dataPos, Type type, string typeSuffix, string currentState)
		{
			this.version = version;
			this.dataPos = dataPos;
			this.type = type;
			this.typeSuffix = typeSuffix;
			this.currentState = currentState;
		}

		// Token: 0x060084B0 RID: 33968 RVA: 0x003241E8 File Offset: 0x003223E8
		public static List<StateMachineSerializer.Entry> DeserializeOldEntries(IReader reader, int serializerVersion)
		{
			Debug.Assert(serializerVersion < 12);
			List<StateMachineSerializer.OldEntryV11> list = StateMachineSerializer.OldEntryV11.ReadEntries(reader, serializerVersion);
			byte[] bytes = StateMachineSerializer.OldEntryV11.ReadEntryData(reader);
			List<StateMachineSerializer.Entry> list2 = new List<StateMachineSerializer.Entry>(list.Count);
			foreach (StateMachineSerializer.OldEntryV11 oldEntryV in list)
			{
				StateMachineSerializer.Entry entry = new StateMachineSerializer.Entry();
				entry.version = serializerVersion;
				entry.type = oldEntryV.type;
				entry.typeSuffix = oldEntryV.typeSuffix;
				entry.currentState = oldEntryV.currentState;
				entry.entryData = new FastReader(bytes);
				entry.entryData.SkipBytes(oldEntryV.dataPos);
				list2.Add(entry);
			}
			return list2;
		}

		// Token: 0x060084B1 RID: 33969 RVA: 0x003242B0 File Offset: 0x003224B0
		private static StateMachineSerializer.OldEntryV11 Deserialize(IReader reader, int serializerVersion)
		{
			int num = reader.ReadInt32();
			int num2 = reader.ReadInt32();
			string typeName = reader.ReadKleiString();
			string text = StateMachineSerializer.DoesVersionHaveTypeSuffix(serializerVersion) ? reader.ReadKleiString() : null;
			string text2 = reader.ReadKleiString();
			Type left = Type.GetType(typeName);
			if (left == null)
			{
				return null;
			}
			return new StateMachineSerializer.OldEntryV11(num, num2, left, text, text2);
		}

		// Token: 0x060084B2 RID: 33970 RVA: 0x00324308 File Offset: 0x00322508
		private static List<StateMachineSerializer.OldEntryV11> ReadEntries(IReader reader, int serializerVersion)
		{
			List<StateMachineSerializer.OldEntryV11> list = new List<StateMachineSerializer.OldEntryV11>();
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				StateMachineSerializer.OldEntryV11 oldEntryV = StateMachineSerializer.OldEntryV11.Deserialize(reader, serializerVersion);
				if (oldEntryV != null)
				{
					list.Add(oldEntryV);
				}
			}
			return list;
		}

		// Token: 0x060084B3 RID: 33971 RVA: 0x00324344 File Offset: 0x00322544
		private static byte[] ReadEntryData(IReader reader)
		{
			int length = reader.ReadInt32();
			return reader.ReadBytes(length);
		}

		// Token: 0x040063FE RID: 25598
		public int version;

		// Token: 0x040063FF RID: 25599
		public int dataPos;

		// Token: 0x04006400 RID: 25600
		public Type type;

		// Token: 0x04006401 RID: 25601
		public string typeSuffix;

		// Token: 0x04006402 RID: 25602
		public string currentState;
	}
}
