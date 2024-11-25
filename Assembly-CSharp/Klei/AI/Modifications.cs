using System;
using System.Collections.Generic;
using System.IO;
using KSerialization;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F6C RID: 3948
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Modifications<ModifierType, InstanceType> : ISaveLoadableDetails where ModifierType : Resource where InstanceType : ModifierInstance<ModifierType>
	{
		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x0600791C RID: 31004 RVA: 0x002FE8AB File Offset: 0x002FCAAB
		public int Count
		{
			get
			{
				return this.ModifierList.Count;
			}
		}

		// Token: 0x0600791D RID: 31005 RVA: 0x002FE8B8 File Offset: 0x002FCAB8
		public IEnumerator<InstanceType> GetEnumerator()
		{
			return this.ModifierList.GetEnumerator();
		}

		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x0600791E RID: 31006 RVA: 0x002FE8CA File Offset: 0x002FCACA
		// (set) Token: 0x0600791F RID: 31007 RVA: 0x002FE8D2 File Offset: 0x002FCAD2
		public GameObject gameObject { get; private set; }

		// Token: 0x170008AF RID: 2223
		public InstanceType this[int idx]
		{
			get
			{
				return this.ModifierList[idx];
			}
		}

		// Token: 0x06007921 RID: 31009 RVA: 0x002FE8E9 File Offset: 0x002FCAE9
		public ComponentType GetComponent<ComponentType>()
		{
			return this.gameObject.GetComponent<ComponentType>();
		}

		// Token: 0x06007922 RID: 31010 RVA: 0x002FE8F6 File Offset: 0x002FCAF6
		public void Trigger(GameHashes hash, object data = null)
		{
			this.gameObject.GetComponent<KPrefabID>().Trigger((int)hash, data);
		}

		// Token: 0x06007923 RID: 31011 RVA: 0x002FE90C File Offset: 0x002FCB0C
		public virtual InstanceType CreateInstance(ModifierType modifier)
		{
			return default(InstanceType);
		}

		// Token: 0x06007924 RID: 31012 RVA: 0x002FE922 File Offset: 0x002FCB22
		public Modifications(GameObject go, ResourceSet<ModifierType> resources = null)
		{
			this.resources = resources;
			this.gameObject = go;
		}

		// Token: 0x06007925 RID: 31013 RVA: 0x002FE943 File Offset: 0x002FCB43
		public virtual InstanceType Add(InstanceType instance)
		{
			this.ModifierList.Add(instance);
			return instance;
		}

		// Token: 0x06007926 RID: 31014 RVA: 0x002FE954 File Offset: 0x002FCB54
		public virtual void Remove(InstanceType instance)
		{
			for (int i = 0; i < this.ModifierList.Count; i++)
			{
				if (this.ModifierList[i] == instance)
				{
					this.ModifierList.RemoveAt(i);
					instance.OnCleanUp();
					return;
				}
			}
		}

		// Token: 0x06007927 RID: 31015 RVA: 0x002FE9A8 File Offset: 0x002FCBA8
		public bool Has(ModifierType modifier)
		{
			return this.Get(modifier) != null;
		}

		// Token: 0x06007928 RID: 31016 RVA: 0x002FE9BC File Offset: 0x002FCBBC
		public InstanceType Get(ModifierType modifier)
		{
			foreach (InstanceType instanceType in this.ModifierList)
			{
				if (instanceType.modifier == modifier)
				{
					return instanceType;
				}
			}
			return default(InstanceType);
		}

		// Token: 0x06007929 RID: 31017 RVA: 0x002FEA30 File Offset: 0x002FCC30
		public InstanceType Get(string id)
		{
			foreach (InstanceType instanceType in this.ModifierList)
			{
				if (instanceType.modifier.Id == id)
				{
					return instanceType;
				}
			}
			return default(InstanceType);
		}

		// Token: 0x0600792A RID: 31018 RVA: 0x002FEAA8 File Offset: 0x002FCCA8
		public void Serialize(BinaryWriter writer)
		{
			writer.Write(this.ModifierList.Count);
			foreach (InstanceType instanceType in this.ModifierList)
			{
				writer.WriteKleiString(instanceType.modifier.Id);
				long position = writer.BaseStream.Position;
				writer.Write(0);
				long position2 = writer.BaseStream.Position;
				Serializer.SerializeTypeless(instanceType, writer);
				long position3 = writer.BaseStream.Position;
				long num = position3 - position2;
				writer.BaseStream.Position = position;
				writer.Write((int)num);
				writer.BaseStream.Position = position3;
			}
		}

		// Token: 0x0600792B RID: 31019 RVA: 0x002FEB88 File Offset: 0x002FCD88
		public void Deserialize(IReader reader)
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				string text = reader.ReadKleiString();
				int num2 = reader.ReadInt32();
				int position = reader.Position;
				InstanceType instanceType = this.Get(text);
				if (instanceType == null && this.resources != null)
				{
					ModifierType modifierType = this.resources.TryGet(text);
					if (modifierType != null)
					{
						instanceType = this.CreateInstance(modifierType);
					}
				}
				if (instanceType == null)
				{
					if (text != "Condition")
					{
						DebugUtil.LogWarningArgs(new object[]
						{
							this.gameObject.name,
							"Missing modifier: " + text
						});
					}
					reader.SkipBytes(num2);
				}
				else if (!(instanceType is ISaveLoadable))
				{
					reader.SkipBytes(num2);
				}
				else
				{
					Deserializer.DeserializeTypeless(instanceType, reader);
					if (reader.Position != position + num2)
					{
						DebugUtil.LogWarningArgs(new object[]
						{
							"Expected to be at offset",
							position + num2,
							"but was only at offset",
							reader.Position,
							". Skipping to catch up."
						});
						reader.SkipBytes(position + num2 - reader.Position);
					}
				}
			}
		}

		// Token: 0x04005A8A RID: 23178
		public List<InstanceType> ModifierList = new List<InstanceType>();

		// Token: 0x04005A8C RID: 23180
		private ResourceSet<ModifierType> resources;
	}
}
