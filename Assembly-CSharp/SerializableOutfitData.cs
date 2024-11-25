using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

// Token: 0x02000A92 RID: 2706
public static class SerializableOutfitData
{
	// Token: 0x06004F51 RID: 20305 RVA: 0x001C8584 File Offset: 0x001C6784
	public static int GetVersionFrom(JObject jsonData)
	{
		int result;
		if (jsonData["Version"] == null)
		{
			result = 1;
		}
		else
		{
			result = jsonData.Value<int>("Version");
			jsonData.Remove("Version");
		}
		return result;
	}

	// Token: 0x06004F52 RID: 20306 RVA: 0x001C85BC File Offset: 0x001C67BC
	public static SerializableOutfitData.Version2 FromJson(JObject jsonData)
	{
		int versionFrom = SerializableOutfitData.GetVersionFrom(jsonData);
		if (versionFrom == 1)
		{
			return SerializableOutfitData.Version2.FromVersion1(SerializableOutfitData.Version1.FromJson(jsonData));
		}
		if (versionFrom != 2)
		{
			DebugUtil.DevAssert(false, string.Format("Version {0} of OutfitData is not supported", versionFrom), null);
			return new SerializableOutfitData.Version2();
		}
		return SerializableOutfitData.Version2.FromJson(jsonData);
	}

	// Token: 0x06004F53 RID: 20307 RVA: 0x001C8609 File Offset: 0x001C6809
	public static JObject ToJson(SerializableOutfitData.Version2 data)
	{
		return SerializableOutfitData.Version2.ToJson(data);
	}

	// Token: 0x06004F54 RID: 20308 RVA: 0x001C8614 File Offset: 0x001C6814
	public static string ToJsonString(JObject data)
	{
		string result;
		using (StringWriter stringWriter = new StringWriter())
		{
			using (JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter))
			{
				data.WriteTo(jsonTextWriter, Array.Empty<JsonConverter>());
				result = stringWriter.ToString();
			}
		}
		return result;
	}

	// Token: 0x06004F55 RID: 20309 RVA: 0x001C8674 File Offset: 0x001C6874
	public static void ToJsonString(JObject data, TextWriter textWriter)
	{
		using (JsonTextWriter jsonTextWriter = new JsonTextWriter(textWriter))
		{
			data.WriteTo(jsonTextWriter, Array.Empty<JsonConverter>());
		}
	}

	// Token: 0x040034BE RID: 13502
	public const string VERSION_KEY = "Version";

	// Token: 0x02001ABE RID: 6846
	public class Version2
	{
		// Token: 0x0600A106 RID: 41222 RVA: 0x003818F4 File Offset: 0x0037FAF4
		public static SerializableOutfitData.Version2 FromVersion1(SerializableOutfitData.Version1 data)
		{
			Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry> dictionary = new Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry>();
			foreach (KeyValuePair<string, string[]> keyValuePair in data.CustomOutfits)
			{
				string text;
				string[] array;
				keyValuePair.Deconstruct(out text, out array);
				string key = text;
				string[] itemIds = array;
				dictionary.Add(key, new SerializableOutfitData.Version2.CustomTemplateOutfitEntry
				{
					outfitType = "Clothing",
					itemIds = itemIds
				});
			}
			Dictionary<string, Dictionary<string, string>> dictionary2 = new Dictionary<string, Dictionary<string, string>>();
			foreach (KeyValuePair<string, Dictionary<ClothingOutfitUtility.OutfitType, string>> keyValuePair2 in data.DuplicantOutfits)
			{
				string text;
				Dictionary<ClothingOutfitUtility.OutfitType, string> dictionary3;
				keyValuePair2.Deconstruct(out text, out dictionary3);
				string key2 = text;
				Dictionary<ClothingOutfitUtility.OutfitType, string> dictionary4 = dictionary3;
				Dictionary<string, string> dictionary5 = new Dictionary<string, string>();
				dictionary2[key2] = dictionary5;
				foreach (KeyValuePair<ClothingOutfitUtility.OutfitType, string> keyValuePair3 in dictionary4)
				{
					ClothingOutfitUtility.OutfitType outfitType;
					keyValuePair3.Deconstruct(out outfitType, out text);
					ClothingOutfitUtility.OutfitType outfitType2 = outfitType;
					string value = text;
					dictionary5.Add(Enum.GetName(typeof(ClothingOutfitUtility.OutfitType), outfitType2), value);
				}
			}
			return new SerializableOutfitData.Version2
			{
				PersonalityIdToAssignedOutfits = dictionary2,
				OutfitIdToUserAuthoredTemplateOutfit = dictionary
			};
		}

		// Token: 0x0600A107 RID: 41223 RVA: 0x00381A60 File Offset: 0x0037FC60
		public static SerializableOutfitData.Version2 FromJson(JObject jsonData)
		{
			return jsonData.ToObject<SerializableOutfitData.Version2>(SerializableOutfitData.Version2.GetSerializer());
		}

		// Token: 0x0600A108 RID: 41224 RVA: 0x00381A6D File Offset: 0x0037FC6D
		public static JObject ToJson(SerializableOutfitData.Version2 data)
		{
			JObject jobject = JObject.FromObject(data, SerializableOutfitData.Version2.GetSerializer());
			jobject.AddFirst(new JProperty("Version", 2));
			return jobject;
		}

		// Token: 0x0600A109 RID: 41225 RVA: 0x00381A90 File Offset: 0x0037FC90
		public static JsonSerializer GetSerializer()
		{
			if (SerializableOutfitData.Version2.s_serializer != null)
			{
				return SerializableOutfitData.Version2.s_serializer;
			}
			SerializableOutfitData.Version2.s_serializer = JsonSerializer.CreateDefault();
			SerializableOutfitData.Version2.s_serializer.Converters.Add(new StringEnumConverter());
			return SerializableOutfitData.Version2.s_serializer;
		}

		// Token: 0x04007D89 RID: 32137
		public Dictionary<string, Dictionary<string, string>> PersonalityIdToAssignedOutfits = new Dictionary<string, Dictionary<string, string>>();

		// Token: 0x04007D8A RID: 32138
		public Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry> OutfitIdToUserAuthoredTemplateOutfit = new Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry>();

		// Token: 0x04007D8B RID: 32139
		private static JsonSerializer s_serializer;

		// Token: 0x02002603 RID: 9731
		public class CustomTemplateOutfitEntry
		{
			// Token: 0x0400A930 RID: 43312
			public string outfitType;

			// Token: 0x0400A931 RID: 43313
			public string[] itemIds;
		}
	}

	// Token: 0x02001ABF RID: 6847
	public class Version1
	{
		// Token: 0x0600A10B RID: 41227 RVA: 0x00381AE0 File Offset: 0x0037FCE0
		public static JObject ToJson(SerializableOutfitData.Version1 data)
		{
			return JObject.FromObject(data);
		}

		// Token: 0x0600A10C RID: 41228 RVA: 0x00381AE8 File Offset: 0x0037FCE8
		public static SerializableOutfitData.Version1 FromJson(JObject jsonData)
		{
			SerializableOutfitData.Version1 version = new SerializableOutfitData.Version1();
			SerializableOutfitData.Version1 result;
			using (JsonReader jsonReader = jsonData.CreateReader())
			{
				string a = null;
				string b = "DuplicantOutfits";
				string b2 = "CustomOutfits";
				while (jsonReader.Read())
				{
					JsonToken tokenType = jsonReader.TokenType;
					if (tokenType == JsonToken.PropertyName)
					{
						a = jsonReader.Value.ToString();
					}
					if (tokenType == JsonToken.StartObject && a == b)
					{
						ClothingOutfitUtility.OutfitType outfitType = ClothingOutfitUtility.OutfitType.LENGTH;
						while (jsonReader.Read())
						{
							tokenType = jsonReader.TokenType;
							if (tokenType == JsonToken.EndObject)
							{
								break;
							}
							if (tokenType == JsonToken.PropertyName)
							{
								string key = jsonReader.Value.ToString();
								while (jsonReader.Read())
								{
									tokenType = jsonReader.TokenType;
									if (tokenType == JsonToken.EndObject)
									{
										break;
									}
									if (tokenType == JsonToken.PropertyName)
									{
										Enum.TryParse<ClothingOutfitUtility.OutfitType>(jsonReader.Value.ToString(), out outfitType);
										while (jsonReader.Read())
										{
											tokenType = jsonReader.TokenType;
											if (tokenType == JsonToken.String)
											{
												string value = jsonReader.Value.ToString();
												if (outfitType != ClothingOutfitUtility.OutfitType.LENGTH)
												{
													if (!version.DuplicantOutfits.ContainsKey(key))
													{
														version.DuplicantOutfits.Add(key, new Dictionary<ClothingOutfitUtility.OutfitType, string>());
													}
													version.DuplicantOutfits[key][outfitType] = value;
													break;
												}
												break;
											}
										}
									}
								}
							}
						}
					}
					else if (a == b2)
					{
						string text = null;
						while (jsonReader.Read())
						{
							tokenType = jsonReader.TokenType;
							if (tokenType == JsonToken.EndObject)
							{
								break;
							}
							if (tokenType == JsonToken.PropertyName)
							{
								text = jsonReader.Value.ToString();
							}
							if (tokenType == JsonToken.StartArray)
							{
								JArray jarray = JArray.Load(jsonReader);
								if (jarray != null)
								{
									string[] array = new string[jarray.Count];
									for (int i = 0; i < jarray.Count; i++)
									{
										array[i] = jarray[i].ToString();
									}
									if (text != null)
									{
										version.CustomOutfits[text] = array;
									}
								}
							}
						}
					}
				}
				result = version;
			}
			return result;
		}

		// Token: 0x04007D8C RID: 32140
		public Dictionary<string, Dictionary<ClothingOutfitUtility.OutfitType, string>> DuplicantOutfits = new Dictionary<string, Dictionary<ClothingOutfitUtility.OutfitType, string>>();

		// Token: 0x04007D8D RID: 32141
		public Dictionary<string, string[]> CustomOutfits = new Dictionary<string, string[]>();
	}
}
