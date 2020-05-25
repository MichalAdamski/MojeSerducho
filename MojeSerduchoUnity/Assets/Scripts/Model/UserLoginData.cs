using System.Globalization;
using MyHeart;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class UserLoginData
{
    [JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)]
    public string Username;
    [JsonProperty("password", NullValueHandling = NullValueHandling.Ignore)]
    public string Password;


    public static UserLoginData FromJson(string json)
    {
        return JsonConvert.DeserializeObject<UserLoginData>(json, UserLoginData.Converter.Settings);
    }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this, UserLoginData.Converter.Settings);
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
