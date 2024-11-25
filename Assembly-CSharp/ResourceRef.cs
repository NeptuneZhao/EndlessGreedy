using System;
using System.Runtime.Serialization;
using KSerialization;

// Token: 0x02000A5A RID: 2650
[SerializationConfig(MemberSerialization.OptIn)]
public class ResourceRef<ResourceType> : ISaveLoadable where ResourceType : Resource
{
	// Token: 0x06004CE4 RID: 19684 RVA: 0x001B7588 File Offset: 0x001B5788
	public ResourceRef(ResourceType resource)
	{
		this.Set(resource);
	}

	// Token: 0x06004CE5 RID: 19685 RVA: 0x001B7597 File Offset: 0x001B5797
	public ResourceRef()
	{
	}

	// Token: 0x17000583 RID: 1411
	// (get) Token: 0x06004CE6 RID: 19686 RVA: 0x001B759F File Offset: 0x001B579F
	public ResourceGuid Guid
	{
		get
		{
			return this.guid;
		}
	}

	// Token: 0x06004CE7 RID: 19687 RVA: 0x001B75A7 File Offset: 0x001B57A7
	public ResourceType Get()
	{
		return this.resource;
	}

	// Token: 0x06004CE8 RID: 19688 RVA: 0x001B75AF File Offset: 0x001B57AF
	public void Set(ResourceType resource)
	{
		this.guid = null;
		this.resource = resource;
	}

	// Token: 0x06004CE9 RID: 19689 RVA: 0x001B75BF File Offset: 0x001B57BF
	[OnSerializing]
	private void OnSerializing()
	{
		if (this.resource == null)
		{
			this.guid = null;
			return;
		}
		this.guid = this.resource.Guid;
	}

	// Token: 0x06004CEA RID: 19690 RVA: 0x001B75EC File Offset: 0x001B57EC
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.guid != null)
		{
			this.resource = Db.Get().GetResource<ResourceType>(this.guid);
			if (this.resource != null)
			{
				this.guid = null;
			}
		}
	}

	// Token: 0x0400331B RID: 13083
	[Serialize]
	private ResourceGuid guid;

	// Token: 0x0400331C RID: 13084
	private ResourceType resource;
}
