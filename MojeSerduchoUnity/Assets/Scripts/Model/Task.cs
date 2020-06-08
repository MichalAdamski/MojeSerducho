using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MyHeart
{
    public class Task
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name;
        [JsonProperty("isdone", NullValueHandling = NullValueHandling.Ignore)]
        public int IsDone;
        [JsonProperty("taskID", NullValueHandling = NullValueHandling.Ignore)]
        public string TaskId;
        [JsonProperty("starttime", NullValueHandling = NullValueHandling.Ignore)]
        public TimeSpan? StartTime;
        [JsonProperty("endtime", NullValueHandling = NullValueHandling.Ignore)]
        public TimeSpan? EndTime;
        [JsonProperty("duration", NullValueHandling = NullValueHandling.Ignore)]
        public float Duration;
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description;

        public bool DoNotifyBeforeEnd { get; set; }
        public bool DoNotifyBeforeStart { get; set; }
        public bool ToDo { get; set; }

        public static Task FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Task>(json, Converter.Settings);
        }

        public class TaskList
        {
            [JsonProperty("tasks", NullValueHandling = NullValueHandling.Ignore)]
            public List<Task> Tasks;

            public static TaskList FromJson(string json)
            {
                return JsonConvert.DeserializeObject<TaskList>(json, Converter.Settings);
            }
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
}

