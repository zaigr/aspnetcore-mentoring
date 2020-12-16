using System.Text.Json.Serialization;

namespace Northwind.Api.Models.Response
{
    public class ObjectIdResponse
    {
        public ObjectIdResponse(int id)
        {
            Id = id;
        }

        [JsonPropertyName("id")]
        public int Id { get; }
    }
}
