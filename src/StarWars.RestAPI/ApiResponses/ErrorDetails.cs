using System.Text.Json;
using System.Text.Json.Serialization;

namespace StarWars.RestAPI.ApiResponses
{
    public class ErrorDetails
    {        
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
