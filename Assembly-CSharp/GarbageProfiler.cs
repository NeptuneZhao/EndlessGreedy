using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using KSerialization;
using UnityEngine;

// Token: 0x0200000D RID: 13
public static class GarbageProfiler
{
	// Token: 0x06000031 RID: 49 RVA: 0x00002D48 File Offset: 0x00000F48
	private static void UnloadUnusedAssets()
	{
		Resources.UnloadUnusedAssets();
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00002D50 File Offset: 0x00000F50
	private static void ClearFileName()
	{
		GarbageProfiler.filename_suffix = null;
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00002D58 File Offset: 0x00000F58
	public static string GetFileName(string name)
	{
		string fullPath = Path.GetFullPath(GarbageProfiler.ROOT_MEMORY_DUMP_PATH);
		if (GarbageProfiler.filename_suffix == null)
		{
			if (!Directory.Exists(fullPath))
			{
				Directory.CreateDirectory(fullPath);
			}
			System.DateTime now = System.DateTime.Now;
			GarbageProfiler.filename_suffix = string.Concat(new string[]
			{
				"_",
				now.Year.ToString(),
				"-",
				now.Month.ToString(),
				"-",
				now.Day.ToString(),
				"_",
				now.Hour.ToString(),
				"-",
				now.Minute.ToString(),
				"-",
				now.Second.ToString(),
				".csv"
			});
		}
		return Path.Combine(fullPath, name + GarbageProfiler.filename_suffix);
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00002E5C File Offset: 0x0000105C
	private static void Dump()
	{
		global::Debug.Log("Writing snapshot...");
		MemorySnapshot memorySnapshot = new MemorySnapshot();
		GarbageProfiler.ClearFileName();
		MemorySnapshot.TypeData[] array = new MemorySnapshot.TypeData[memorySnapshot.types.Count];
		memorySnapshot.types.Values.CopyTo(array, 0);
		Array.Sort<MemorySnapshot.TypeData>(array, 0, array.Length, new GarbageProfiler.InstanceCountComparer());
		using (StreamWriter streamWriter = new StreamWriter(GarbageProfiler.GetFileName("memory_instances")))
		{
			streamWriter.WriteLine("Delta,Instances,NumArrayEntries,Type Name");
			foreach (MemorySnapshot.TypeData typeData in array)
			{
				if (typeData.instanceCount != 0)
				{
					int num = typeData.instanceCount;
					if (GarbageProfiler.previousSnapshot != null)
					{
						MemorySnapshot.TypeData typeData2 = MemorySnapshot.GetTypeData(typeData.type, GarbageProfiler.previousSnapshot.types);
						num = typeData.instanceCount - typeData2.instanceCount;
					}
					streamWriter.WriteLine(string.Concat(new string[]
					{
						num.ToString(),
						",",
						typeData.instanceCount.ToString(),
						",",
						typeData.numArrayEntries.ToString(),
						",\"",
						typeData.type.ToString(),
						"\""
					}));
				}
			}
		}
		using (StreamWriter streamWriter2 = new StreamWriter(GarbageProfiler.GetFileName("memory_hierarchies")))
		{
			streamWriter2.WriteLine("Delta,Count,Type Hierarchy");
			foreach (MemorySnapshot.TypeData typeData3 in array)
			{
				if (typeData3.instanceCount != 0)
				{
					foreach (KeyValuePair<MemorySnapshot.HierarchyNode, int> keyValuePair in typeData3.hierarchies)
					{
						int num2 = keyValuePair.Value;
						if (GarbageProfiler.previousSnapshot != null)
						{
							MemorySnapshot.TypeData typeData4 = MemorySnapshot.GetTypeData(typeData3.type, GarbageProfiler.previousSnapshot.types);
							int num3 = 0;
							if (typeData4.hierarchies.TryGetValue(keyValuePair.Key, out num3))
							{
								num2 = keyValuePair.Value - num3;
							}
						}
						streamWriter2.WriteLine(string.Concat(new string[]
						{
							num2.ToString(),
							",",
							keyValuePair.Value.ToString(),
							", \"",
							typeData3.type.ToString(),
							": ",
							keyValuePair.Key.ToString(),
							"\""
						}));
					}
				}
			}
		}
		GarbageProfiler.previousSnapshot = memorySnapshot;
		global::Debug.Log("Done writing snapshot!");
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00003150 File Offset: 0x00001350
	public static void DebugDumpGarbageStats()
	{
		global::Debug.Log("Writing reference stats...");
		MemorySnapshot memorySnapshot = new MemorySnapshot();
		GarbageProfiler.ClearFileName();
		MemorySnapshot.TypeData[] array = new MemorySnapshot.TypeData[memorySnapshot.types.Count];
		memorySnapshot.types.Values.CopyTo(array, 0);
		Array.Sort<MemorySnapshot.TypeData>(array, 0, array.Length, new GarbageProfiler.InstanceCountComparer());
		using (StreamWriter streamWriter = new StreamWriter(GarbageProfiler.GetFileName("garbage_instances")))
		{
			foreach (MemorySnapshot.TypeData typeData in array)
			{
				if (typeData.instanceCount != 0)
				{
					int num = typeData.instanceCount;
					if (GarbageProfiler.previousSnapshot != null)
					{
						MemorySnapshot.TypeData typeData2 = MemorySnapshot.GetTypeData(typeData.type, GarbageProfiler.previousSnapshot.types);
						num = typeData.instanceCount - typeData2.instanceCount;
					}
					streamWriter.WriteLine(string.Concat(new string[]
					{
						num.ToString(),
						", ",
						typeData.instanceCount.ToString(),
						", \"",
						typeData.type.ToString(),
						"\""
					}));
				}
			}
		}
		Array.Sort<MemorySnapshot.TypeData>(array, 0, array.Length, new GarbageProfiler.RefCountComparer());
		using (StreamWriter streamWriter2 = new StreamWriter(GarbageProfiler.GetFileName("garbage_refs")))
		{
			foreach (MemorySnapshot.TypeData typeData3 in array)
			{
				if (typeData3.refCount != 0)
				{
					int num2 = typeData3.refCount;
					if (GarbageProfiler.previousSnapshot != null)
					{
						MemorySnapshot.TypeData typeData4 = MemorySnapshot.GetTypeData(typeData3.type, GarbageProfiler.previousSnapshot.types);
						num2 = typeData3.refCount - typeData4.refCount;
					}
					streamWriter2.WriteLine(string.Concat(new string[]
					{
						num2.ToString(),
						", ",
						typeData3.refCount.ToString(),
						", \"",
						typeData3.type.ToString(),
						"\""
					}));
				}
			}
		}
		MemorySnapshot.FieldCount[] array3 = new MemorySnapshot.FieldCount[memorySnapshot.fieldCounts.Count];
		memorySnapshot.fieldCounts.Values.CopyTo(array3, 0);
		Array.Sort<MemorySnapshot.FieldCount>(array3, 0, array3.Length, new GarbageProfiler.FieldCountComparer());
		using (StreamWriter streamWriter3 = new StreamWriter(GarbageProfiler.GetFileName("garbage_fields")))
		{
			foreach (MemorySnapshot.FieldCount fieldCount in array3)
			{
				int num3 = fieldCount.count;
				if (GarbageProfiler.previousSnapshot != null)
				{
					foreach (KeyValuePair<int, MemorySnapshot.FieldCount> keyValuePair in GarbageProfiler.previousSnapshot.fieldCounts)
					{
						if (keyValuePair.Value.name == fieldCount.name)
						{
							num3 = fieldCount.count - keyValuePair.Value.count;
							break;
						}
					}
				}
				streamWriter3.WriteLine(string.Concat(new string[]
				{
					num3.ToString(),
					", ",
					fieldCount.count.ToString(),
					", \"",
					fieldCount.name,
					"\""
				}));
			}
		}
		memorySnapshot.WriteTypeDetails(GarbageProfiler.previousSnapshot);
		GarbageProfiler.previousSnapshot = memorySnapshot;
		global::Debug.Log("Done writing reference stats!");
	}

	// Token: 0x06000036 RID: 54 RVA: 0x000034E8 File Offset: 0x000016E8
	public static void DebugDumpRootItems()
	{
		global::Debug.Log("Writing root items...");
		Type[] array = new Type[]
		{
			typeof(string),
			typeof(HashedString),
			typeof(KAnimHashedString),
			typeof(Tag),
			typeof(bool),
			typeof(CellOffset),
			typeof(Color),
			typeof(Color32),
			typeof(Vector2),
			typeof(Vector3),
			typeof(Vector2I)
		};
		Type[] array2 = new Type[]
		{
			typeof(List<>),
			typeof(HashSet<>),
			typeof(Dictionary<, >)
		};
		string fileName = GarbageProfiler.GetFileName("statics");
		GarbageProfiler.ClearFileName();
		using (StreamWriter streamWriter = new StreamWriter(fileName))
		{
			streamWriter.WriteLine("FieldName,Type,ListLength");
			Assembly[] array3 = new Assembly[]
			{
				Assembly.GetAssembly(typeof(Game)),
				Assembly.GetAssembly(typeof(App))
			};
			for (int i = 0; i < array3.Length; i++)
			{
				foreach (Type type in array3[i].GetTypes())
				{
					if (type == GarbageProfiler.DEBUG_STATIC_TYPE)
					{
						Debugger.Break();
					}
					if (!type.IsAbstract && !type.IsGenericType && !type.ToString().StartsWith("STRINGS."))
					{
						foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
						{
							if (fieldInfo.IsStatic && !fieldInfo.IsInitOnly && !fieldInfo.IsLiteral && !fieldInfo.Name.Contains("$cache"))
							{
								Type fieldType = fieldInfo.FieldType;
								if (!fieldType.IsPointer && !Helper.IsPOD(fieldType) && Array.IndexOf<Type>(array, fieldType) < 0)
								{
									if (typeof(Array).IsAssignableFrom(fieldType))
									{
										Type elementType = fieldType.GetElementType();
										if (elementType.IsPointer || Helper.IsPOD(elementType) || Array.IndexOf<Type>(array, elementType) >= 0)
										{
											goto IL_35F;
										}
									}
									if (fieldType.IsGenericType)
									{
										Type genericTypeDefinition = fieldType.GetGenericTypeDefinition();
										Type[] genericArguments = fieldType.GetGenericArguments();
										bool flag = false;
										foreach (Type right in array2)
										{
											if (genericTypeDefinition == right)
											{
												bool flag2 = true;
												foreach (Type type2 in genericArguments)
												{
													if (!Helper.IsPOD(type2) && Array.IndexOf<Type>(array, type2) < 0)
													{
														flag2 = false;
														break;
													}
												}
												if (flag2)
												{
													flag = true;
													break;
												}
											}
										}
										if (flag)
										{
											goto IL_35F;
										}
									}
									object value = fieldInfo.GetValue(null);
									if (value != null)
									{
										string value2;
										if (typeof(ICollection).IsAssignableFrom(fieldType))
										{
											int count = (value as ICollection).Count;
											value2 = string.Format("\"{0}.{1}\",\"{2}\",{3}", new object[]
											{
												type,
												fieldInfo.Name,
												fieldType,
												count
											});
										}
										else
										{
											value2 = string.Format("\"{0}.{1}\",\"{2}\"", type, fieldInfo.Name, fieldType);
										}
										streamWriter.WriteLine(value2);
									}
								}
							}
							IL_35F:;
						}
					}
				}
			}
		}
		global::Debug.Log("Done writing reference stats!");
	}

	// Token: 0x04000035 RID: 53
	private static MemorySnapshot previousSnapshot;

	// Token: 0x04000036 RID: 54
	private static string ROOT_MEMORY_DUMP_PATH = "./memory/";

	// Token: 0x04000037 RID: 55
	private static string filename_suffix = null;

	// Token: 0x04000038 RID: 56
	private static Type DEBUG_STATIC_TYPE = null;

	// Token: 0x02000F91 RID: 3985
	private class InstanceCountComparer : IComparer<MemorySnapshot.TypeData>
	{
		// Token: 0x060079F1 RID: 31217 RVA: 0x003017FA File Offset: 0x002FF9FA
		public int Compare(MemorySnapshot.TypeData a, MemorySnapshot.TypeData b)
		{
			return b.instanceCount - a.instanceCount;
		}
	}

	// Token: 0x02000F92 RID: 3986
	private class RefCountComparer : IComparer<MemorySnapshot.TypeData>
	{
		// Token: 0x060079F3 RID: 31219 RVA: 0x00301811 File Offset: 0x002FFA11
		public int Compare(MemorySnapshot.TypeData a, MemorySnapshot.TypeData b)
		{
			return b.refCount - a.refCount;
		}
	}

	// Token: 0x02000F93 RID: 3987
	private class FieldCountComparer : IComparer<MemorySnapshot.FieldCount>
	{
		// Token: 0x060079F5 RID: 31221 RVA: 0x00301828 File Offset: 0x002FFA28
		public int Compare(MemorySnapshot.FieldCount a, MemorySnapshot.FieldCount b)
		{
			return b.count - a.count;
		}
	}
}
