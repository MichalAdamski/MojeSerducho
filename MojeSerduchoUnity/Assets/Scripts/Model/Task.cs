using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

public class Task
{
    [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
    public string Name;

    public static Task FromJson(string json)
    {
        return JsonConvert.DeserializeObject<Task>(json, Converter.Settings);
    }

    public static partial class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = { new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal } }
        };
    }
}
