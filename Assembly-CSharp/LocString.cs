using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Token: 0x02000942 RID: 2370
[Serializable]
public class LocString
{
	// Token: 0x170004EF RID: 1263
	// (get) Token: 0x060044F2 RID: 17650 RVA: 0x00189134 File Offset: 0x00187334
	public string text
	{
		get
		{
			return this._text;
		}
	}

	// Token: 0x170004F0 RID: 1264
	// (get) Token: 0x060044F3 RID: 17651 RVA: 0x0018913C File Offset: 0x0018733C
	public StringKey key
	{
		get
		{
			return this._key;
		}
	}

	// Token: 0x060044F4 RID: 17652 RVA: 0x00189144 File Offset: 0x00187344
	public LocString(string text)
	{
		this._text = text;
		this._key = default(StringKey);
	}

	// Token: 0x060044F5 RID: 17653 RVA: 0x0018915F File Offset: 0x0018735F
	public LocString(string text, string keystring)
	{
		this._text = text;
		this._key = new StringKey(keystring);
	}

	// Token: 0x060044F6 RID: 17654 RVA: 0x0018917A File Offset: 0x0018737A
	public LocString(string text, bool isLocalized)
	{
		this._text = text;
		this._key = default(StringKey);
	}

	// Token: 0x060044F7 RID: 17655 RVA: 0x00189195 File Offset: 0x00187395
	public static implicit operator LocString(string text)
	{
		return new LocString(text);
	}

	// Token: 0x060044F8 RID: 17656 RVA: 0x0018919D File Offset: 0x0018739D
	public static implicit operator string(LocString loc_string)
	{
		return loc_string.text;
	}

	// Token: 0x060044F9 RID: 17657 RVA: 0x001891A5 File Offset: 0x001873A5
	public override string ToString()
	{
		return Strings.Get(this.key).String;
	}

	// Token: 0x060044FA RID: 17658 RVA: 0x001891B7 File Offset: 0x001873B7
	public void SetKey(string key_name)
	{
		this._key = new StringKey(key_name);
	}

	// Token: 0x060044FB RID: 17659 RVA: 0x001891C5 File Offset: 0x001873C5
	public void SetKey(StringKey key)
	{
		this._key = key;
	}

	// Token: 0x060044FC RID: 17660 RVA: 0x001891CE File Offset: 0x001873CE
	public string Replace(string search, string replacement)
	{
		return this.ToString().Replace(search, replacement);
	}

	// Token: 0x060044FD RID: 17661 RVA: 0x001891E0 File Offset: 0x001873E0
	public static void CreateLocStringKeys(Type type, string parent_path = "STRINGS.")
	{
		FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
		string text = parent_path;
		if (text == null)
		{
			text = "";
		}
		text = text + type.Name + ".";
		foreach (FieldInfo fieldInfo in fields)
		{
			if (!(fieldInfo.FieldType != typeof(LocString)))
			{
				if (!fieldInfo.IsStatic)
				{
					DebugUtil.DevLogError("LocString fields must be static, skipping. " + parent_path);
				}
				else
				{
					string text2 = text + fieldInfo.Name;
					LocString locString = (LocString)fieldInfo.GetValue(null);
					locString.SetKey(text2);
					string text3 = locString.text;
					Strings.Add(new string[]
					{
						text2,
						text3
					});
					fieldInfo.SetValue(null, locString);
				}
			}
		}
		Type[] nestedTypes = type.GetNestedTypes(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
		for (int i = 0; i < nestedTypes.Length; i++)
		{
			LocString.CreateLocStringKeys(nestedTypes[i], text);
		}
	}

	// Token: 0x060044FE RID: 17662 RVA: 0x001892CC File Offset: 0x001874CC
	public static string[] GetStrings(Type type)
	{
		List<string> list = new List<string>();
		FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
		for (int i = 0; i < fields.Length; i++)
		{
			LocString locString = (LocString)fields[i].GetValue(null);
			list.Add(locString.text);
		}
		return list.ToArray();
	}

	// Token: 0x04002D0C RID: 11532
	[SerializeField]
	private string _text;

	// Token: 0x04002D0D RID: 11533
	[SerializeField]
	private StringKey _key;

	// Token: 0x04002D0E RID: 11534
	public const BindingFlags data_member_fields = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
}
