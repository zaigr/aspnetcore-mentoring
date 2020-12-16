﻿using System.Text.Json.Serialization;

namespace Northwind.Api.Models.Categories
{
    public class CategoryReadModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
