
using System.Text.Json.Serialization;

namespace MiXTools.Model
{
    public class Tag : ITag
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        public Tag() : this("") { }
        public Tag(string name) { Name = name; }
        public override string ToString()
        {
            return $"Tag name: {Name}";
        }
    }
}
