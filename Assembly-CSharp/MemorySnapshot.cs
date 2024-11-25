using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

// Token: 0x0200095C RID: 2396
public class MemorySnapshot
{
	// Token: 0x060045FB RID: 17915 RVA: 0x0018E1D4 File Offset: 0x0018C3D4
	public static MemorySnapshot.TypeData GetTypeData(Type type, Dictionary<int, MemorySnapshot.TypeData> types)
	{
		int hashCode = type.GetHashCode();
		MemorySnapshot.TypeData typeData = null;
		if (!types.TryGetValue(hashCode, out typeData))
		{
			typeData = new MemorySnapshot.TypeData(type);
			types[hashCode] = typeData;
		}
		return typeData;
	}

	// Token: 0x060045FC RID: 17916 RVA: 0x0018E208 File Offset: 0x0018C408
	public static void IncrementFieldCount(Dictionary<int, MemorySnapshot.FieldCount> field_counts, string name)
	{
		int hashCode = name.GetHashCode();
		MemorySnapshot.FieldCount fieldCount = null;
		if (!field_counts.TryGetValue(hashCode, out fieldCount))
		{
			fieldCount = new MemorySnapshot.FieldCount();
			fieldCount.name = name;
			field_counts[hashCode] = fieldCount;
		}
		fieldCount.count++;
	}

	// Token: 0x060045FD RID: 17917 RVA: 0x0018E24C File Offset: 0x0018C44C
	private void CountReference(MemorySnapshot.ReferenceArgs refArgs)
	{
		if (MemorySnapshot.ShouldExclude(refArgs.reference_type))
		{
			return;
		}
		if (refArgs.reference_type == MemorySnapshot.detailType)
		{
			string text;
			if (refArgs.lineage.obj as UnityEngine.Object != null)
			{
				text = "\"" + ((UnityEngine.Object)refArgs.lineage.obj).name;
			}
			else
			{
				text = "\"" + MemorySnapshot.detailTypeStr;
			}
			if (refArgs.lineage.parent0 != null)
			{
				text += "\",\"";
				text += refArgs.lineage.parent0.ToString();
			}
			if (refArgs.lineage.parent1 != null)
			{
				text = text + "\",\"" + refArgs.lineage.parent1.ToString();
			}
			if (refArgs.lineage.parent2 != null)
			{
				text = text + "\",\"" + refArgs.lineage.parent2.ToString();
			}
			if (refArgs.lineage.parent3 != null)
			{
				text = text + "\",\"" + refArgs.lineage.parent3.ToString();
			}
			if (refArgs.lineage.parent4 != null)
			{
				text = text + "\",\"" + refArgs.lineage.parent4.ToString();
			}
			text += "\"\n";
			MemorySnapshot.DetailInfo value;
			this.detailTypeCount.TryGetValue(text, out value);
			value.count++;
			if (typeof(Array).IsAssignableFrom(refArgs.reference_type) && refArgs.lineage.obj != null)
			{
				Array array = refArgs.lineage.obj as Array;
				value.numArrayEntries += ((array != null) ? array.Length : 0);
			}
			this.detailTypeCount[text] = value;
		}
		if (refArgs.reference_type.IsClass)
		{
			MemorySnapshot.GetTypeData(refArgs.reference_type, this.types).refCount++;
			MemorySnapshot.IncrementFieldCount(this.fieldCounts, refArgs.field_name);
		}
		if (refArgs.lineage.obj == null)
		{
			return;
		}
		try
		{
			if (refArgs.lineage.obj.GetType().IsClass && !this.walked.Add(refArgs.lineage.obj))
			{
				return;
			}
		}
		catch
		{
			return;
		}
		MemorySnapshot.TypeData typeData = MemorySnapshot.GetTypeData(refArgs.lineage.obj.GetType(), this.types);
		if (typeData.type.IsClass)
		{
			typeData.instanceCount++;
			if (typeof(Array).IsAssignableFrom(typeData.type))
			{
				Array array2 = refArgs.lineage.obj as Array;
				typeData.numArrayEntries += ((array2 != null) ? array2.Length : 0);
			}
			MemorySnapshot.HierarchyNode key = new MemorySnapshot.HierarchyNode(refArgs.lineage.parent0, refArgs.lineage.parent1, refArgs.lineage.parent2, refArgs.lineage.parent3, refArgs.lineage.parent4);
			int num = 0;
			typeData.hierarchies.TryGetValue(key, out num);
			typeData.hierarchies[key] = num + 1;
		}
		foreach (FieldInfo fieldInfo in typeData.fields)
		{
			this.fieldsToProcess.Add(new MemorySnapshot.FieldArgs(fieldInfo, new MemorySnapshot.Lineage(refArgs.lineage.obj, refArgs.lineage.parent3, refArgs.lineage.parent2, refArgs.lineage.parent1, refArgs.lineage.parent0, fieldInfo.DeclaringType)));
		}
		ICollection collection = refArgs.lineage.obj as ICollection;
		if (collection != null)
		{
			Type type = typeof(object);
			if (collection.GetType().GetElementType() != null)
			{
				type = collection.GetType().GetElementType();
			}
			else if (collection.GetType().GetGenericArguments().Length != 0)
			{
				type = collection.GetType().GetGenericArguments()[0];
			}
			if (!MemorySnapshot.ShouldExclude(type))
			{
				foreach (object obj in collection)
				{
					this.refsToProcess.Add(new MemorySnapshot.ReferenceArgs(type, refArgs.field_name + ".Item", new MemorySnapshot.Lineage(obj, refArgs.lineage.parent3, refArgs.lineage.parent2, refArgs.lineage.parent1, refArgs.lineage.parent0, collection.GetType())));
				}
			}
		}
	}

	// Token: 0x060045FE RID: 17918 RVA: 0x0018E754 File Offset: 0x0018C954
	private void CountField(MemorySnapshot.FieldArgs fieldArgs)
	{
		if (MemorySnapshot.ShouldExclude(fieldArgs.field.FieldType))
		{
			return;
		}
		object obj = null;
		try
		{
			if (!fieldArgs.field.FieldType.Name.Contains("*"))
			{
				obj = fieldArgs.field.GetValue(fieldArgs.lineage.obj);
			}
		}
		catch
		{
			obj = null;
		}
		string field_name = fieldArgs.field.DeclaringType.ToString() + "." + fieldArgs.field.Name;
		this.refsToProcess.Add(new MemorySnapshot.ReferenceArgs(fieldArgs.field.FieldType, field_name, new MemorySnapshot.Lineage(obj, fieldArgs.lineage.parent3, fieldArgs.lineage.parent2, fieldArgs.lineage.parent1, fieldArgs.lineage.parent0, fieldArgs.field.DeclaringType)));
	}

	// Token: 0x060045FF RID: 17919 RVA: 0x0018E840 File Offset: 0x0018CA40
	private static bool ShouldExclude(Type type)
	{
		return type.IsPrimitive || type.IsEnum || type == typeof(MemorySnapshot);
	}

	// Token: 0x06004600 RID: 17920 RVA: 0x0018E864 File Offset: 0x0018CA64
	private void CountAll()
	{
		while (this.refsToProcess.Count > 0 || this.fieldsToProcess.Count > 0)
		{
			while (this.fieldsToProcess.Count > 0)
			{
				MemorySnapshot.FieldArgs fieldArgs = this.fieldsToProcess[this.fieldsToProcess.Count - 1];
				this.fieldsToProcess.RemoveAt(this.fieldsToProcess.Count - 1);
				this.CountField(fieldArgs);
			}
			while (this.refsToProcess.Count > 0)
			{
				MemorySnapshot.ReferenceArgs refArgs = this.refsToProcess[this.refsToProcess.Count - 1];
				this.refsToProcess.RemoveAt(this.refsToProcess.Count - 1);
				this.CountReference(refArgs);
			}
		}
	}

	// Token: 0x06004601 RID: 17921 RVA: 0x0018E920 File Offset: 0x0018CB20
	public MemorySnapshot()
	{
		MemorySnapshot.Lineage lineage = new MemorySnapshot.Lineage(null, null, null, null, null, null);
		foreach (Type type in App.GetCurrentDomainTypes())
		{
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
			{
				if (fieldInfo.IsStatic)
				{
					this.statics.Add(fieldInfo);
					lineage.parent0 = fieldInfo.DeclaringType;
					this.fieldsToProcess.Add(new MemorySnapshot.FieldArgs(fieldInfo, lineage));
				}
			}
		}
		this.CountAll();
		foreach (UnityEngine.Object @object in Resources.FindObjectsOfTypeAll(typeof(UnityEngine.Object)))
		{
			lineage.obj = @object;
			lineage.parent0 = @object.GetType();
			this.refsToProcess.Add(new MemorySnapshot.ReferenceArgs(@object.GetType(), "Object." + @object.name, lineage));
		}
		this.CountAll();
	}

	// Token: 0x06004602 RID: 17922 RVA: 0x0018EA90 File Offset: 0x0018CC90
	public void WriteTypeDetails(MemorySnapshot compare)
	{
		List<KeyValuePair<string, MemorySnapshot.DetailInfo>> list = null;
		if (compare != null)
		{
			list = compare.detailTypeCount.ToList<KeyValuePair<string, MemorySnapshot.DetailInfo>>();
		}
		List<KeyValuePair<string, MemorySnapshot.DetailInfo>> list2 = this.detailTypeCount.ToList<KeyValuePair<string, MemorySnapshot.DetailInfo>>();
		list2.Sort((KeyValuePair<string, MemorySnapshot.DetailInfo> x, KeyValuePair<string, MemorySnapshot.DetailInfo> y) => y.Value.count - x.Value.count);
		using (StreamWriter streamWriter = new StreamWriter(GarbageProfiler.GetFileName("type_details_" + MemorySnapshot.detailTypeStr)))
		{
			streamWriter.WriteLine("Delta,Count,NumArrayEntries,Type");
			foreach (KeyValuePair<string, MemorySnapshot.DetailInfo> keyValuePair in list2)
			{
				int num = keyValuePair.Value.count;
				if (list != null)
				{
					foreach (KeyValuePair<string, MemorySnapshot.DetailInfo> keyValuePair2 in list)
					{
						if (keyValuePair2.Key == keyValuePair.Key)
						{
							num -= keyValuePair2.Value.count;
							break;
						}
					}
				}
				TextWriter textWriter = streamWriter;
				string[] array = new string[7];
				array[0] = num.ToString();
				array[1] = ",";
				int num2 = 2;
				MemorySnapshot.DetailInfo value = keyValuePair.Value;
				array[num2] = value.count.ToString();
				array[3] = ",";
				int num3 = 4;
				value = keyValuePair.Value;
				array[num3] = value.numArrayEntries.ToString();
				array[5] = ",";
				array[6] = keyValuePair.Key;
				textWriter.Write(string.Concat(array));
			}
		}
	}

	// Token: 0x04002D81 RID: 11649
	public Dictionary<int, MemorySnapshot.TypeData> types = new Dictionary<int, MemorySnapshot.TypeData>();

	// Token: 0x04002D82 RID: 11650
	public Dictionary<int, MemorySnapshot.FieldCount> fieldCounts = new Dictionary<int, MemorySnapshot.FieldCount>();

	// Token: 0x04002D83 RID: 11651
	public HashSet<object> walked = new HashSet<object>();

	// Token: 0x04002D84 RID: 11652
	public List<FieldInfo> statics = new List<FieldInfo>();

	// Token: 0x04002D85 RID: 11653
	public Dictionary<string, MemorySnapshot.DetailInfo> detailTypeCount = new Dictionary<string, MemorySnapshot.DetailInfo>();

	// Token: 0x04002D86 RID: 11654
	private static readonly Type detailType = typeof(byte[]);

	// Token: 0x04002D87 RID: 11655
	private static readonly string detailTypeStr = MemorySnapshot.detailType.ToString();

	// Token: 0x04002D88 RID: 11656
	private List<MemorySnapshot.FieldArgs> fieldsToProcess = new List<MemorySnapshot.FieldArgs>();

	// Token: 0x04002D89 RID: 11657
	private List<MemorySnapshot.ReferenceArgs> refsToProcess = new List<MemorySnapshot.ReferenceArgs>();

	// Token: 0x020018C1 RID: 6337
	public struct HierarchyNode
	{
		// Token: 0x060099C3 RID: 39363 RVA: 0x0036B1F4 File Offset: 0x003693F4
		public HierarchyNode(Type parent_0, Type parent_1, Type parent_2, Type parent_3, Type parent_4)
		{
			this.parent0 = parent_0;
			this.parent1 = parent_1;
			this.parent2 = parent_2;
			this.parent3 = parent_3;
			this.parent4 = parent_4;
		}

		// Token: 0x060099C4 RID: 39364 RVA: 0x0036B21C File Offset: 0x0036941C
		public bool Equals(MemorySnapshot.HierarchyNode a, MemorySnapshot.HierarchyNode b)
		{
			return a.parent0 == b.parent0 && a.parent1 == b.parent1 && a.parent2 == b.parent2 && a.parent3 == b.parent3 && a.parent4 == b.parent4;
		}

		// Token: 0x060099C5 RID: 39365 RVA: 0x0036B288 File Offset: 0x00369488
		public override int GetHashCode()
		{
			int num = 0;
			if (this.parent0 != null)
			{
				num += this.parent0.GetHashCode();
			}
			if (this.parent1 != null)
			{
				num += this.parent1.GetHashCode();
			}
			if (this.parent2 != null)
			{
				num += this.parent2.GetHashCode();
			}
			if (this.parent3 != null)
			{
				num += this.parent3.GetHashCode();
			}
			if (this.parent4 != null)
			{
				num += this.parent4.GetHashCode();
			}
			return num;
		}

		// Token: 0x060099C6 RID: 39366 RVA: 0x0036B324 File Offset: 0x00369524
		public override string ToString()
		{
			if (this.parent4 != null)
			{
				return string.Concat(new string[]
				{
					this.parent4.ToString(),
					"--",
					this.parent3.ToString(),
					"--",
					this.parent2.ToString(),
					"--",
					this.parent1.ToString(),
					"--",
					this.parent0.ToString()
				});
			}
			if (this.parent3 != null)
			{
				return string.Concat(new string[]
				{
					this.parent3.ToString(),
					"--",
					this.parent2.ToString(),
					"--",
					this.parent1.ToString(),
					"--",
					this.parent0.ToString()
				});
			}
			if (this.parent2 != null)
			{
				return string.Concat(new string[]
				{
					this.parent2.ToString(),
					"--",
					this.parent1.ToString(),
					"--",
					this.parent0.ToString()
				});
			}
			if (this.parent1 != null)
			{
				return this.parent1.ToString() + "--" + this.parent0.ToString();
			}
			return this.parent0.ToString();
		}

		// Token: 0x0400774B RID: 30539
		public Type parent0;

		// Token: 0x0400774C RID: 30540
		public Type parent1;

		// Token: 0x0400774D RID: 30541
		public Type parent2;

		// Token: 0x0400774E RID: 30542
		public Type parent3;

		// Token: 0x0400774F RID: 30543
		public Type parent4;
	}

	// Token: 0x020018C2 RID: 6338
	public class FieldCount
	{
		// Token: 0x04007750 RID: 30544
		public string name;

		// Token: 0x04007751 RID: 30545
		public int count;
	}

	// Token: 0x020018C3 RID: 6339
	public class TypeData
	{
		// Token: 0x060099C8 RID: 39368 RVA: 0x0036B4B4 File Offset: 0x003696B4
		public TypeData(Type type)
		{
			this.type = type;
			this.fields = new List<FieldInfo>();
			this.instanceCount = 0;
			this.refCount = 0;
			this.numArrayEntries = 0;
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
			{
				if (!fieldInfo.IsStatic && !MemorySnapshot.ShouldExclude(fieldInfo.FieldType))
				{
					this.fields.Add(fieldInfo);
				}
			}
		}

		// Token: 0x04007752 RID: 30546
		public Dictionary<MemorySnapshot.HierarchyNode, int> hierarchies = new Dictionary<MemorySnapshot.HierarchyNode, int>();

		// Token: 0x04007753 RID: 30547
		public Type type;

		// Token: 0x04007754 RID: 30548
		public List<FieldInfo> fields;

		// Token: 0x04007755 RID: 30549
		public int instanceCount;

		// Token: 0x04007756 RID: 30550
		public int refCount;

		// Token: 0x04007757 RID: 30551
		public int numArrayEntries;
	}

	// Token: 0x020018C4 RID: 6340
	public struct DetailInfo
	{
		// Token: 0x04007758 RID: 30552
		public int count;

		// Token: 0x04007759 RID: 30553
		public int numArrayEntries;
	}

	// Token: 0x020018C5 RID: 6341
	private struct Lineage
	{
		// Token: 0x060099C9 RID: 39369 RVA: 0x0036B535 File Offset: 0x00369735
		public Lineage(object obj, Type parent4, Type parent3, Type parent2, Type parent1, Type parent0)
		{
			this.obj = obj;
			this.parent0 = parent0;
			this.parent1 = parent1;
			this.parent2 = parent2;
			this.parent3 = parent3;
			this.parent4 = parent4;
		}

		// Token: 0x0400775A RID: 30554
		public object obj;

		// Token: 0x0400775B RID: 30555
		public Type parent0;

		// Token: 0x0400775C RID: 30556
		public Type parent1;

		// Token: 0x0400775D RID: 30557
		public Type parent2;

		// Token: 0x0400775E RID: 30558
		public Type parent3;

		// Token: 0x0400775F RID: 30559
		public Type parent4;
	}

	// Token: 0x020018C6 RID: 6342
	private struct ReferenceArgs
	{
		// Token: 0x060099CA RID: 39370 RVA: 0x0036B564 File Offset: 0x00369764
		public ReferenceArgs(Type reference_type, string field_name, MemorySnapshot.Lineage lineage)
		{
			this.reference_type = reference_type;
			this.lineage = lineage;
			this.field_name = field_name;
		}

		// Token: 0x04007760 RID: 30560
		public Type reference_type;

		// Token: 0x04007761 RID: 30561
		public string field_name;

		// Token: 0x04007762 RID: 30562
		public MemorySnapshot.Lineage lineage;
	}

	// Token: 0x020018C7 RID: 6343
	private struct FieldArgs
	{
		// Token: 0x060099CB RID: 39371 RVA: 0x0036B57B File Offset: 0x0036977B
		public FieldArgs(FieldInfo field, MemorySnapshot.Lineage lineage)
		{
			this.field = field;
			this.lineage = lineage;
		}

		// Token: 0x04007763 RID: 30563
		public FieldInfo field;

		// Token: 0x04007764 RID: 30564
		public MemorySnapshot.Lineage lineage;
	}
}
