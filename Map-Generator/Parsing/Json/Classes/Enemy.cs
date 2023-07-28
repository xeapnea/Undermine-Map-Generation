﻿using Newtonsoft.Json;

namespace Map_Generator.Parsing.Json.Classes
{
    public class Enemy
    {
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("difficulty")] public int Difficulty { get; set; }
        [JsonProperty("rougedifficulty")] public int RougeDifficulty { get; set; }
        [JsonProperty("canbesolo")] public bool CanBeSolo { get; set; }
        [JsonProperty("max")] public int Max { get; set; }
        [JsonProperty("type")] public int Type { get; set; }

        public int GetDifficulty() => Save.storymode ? Difficulty : RougeDifficulty;
    }
}