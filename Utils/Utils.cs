using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Http;

namespace Azure.SQLDB.Samples.DynamicSchema
{
    public enum Style
    {
        Classic,
        Hybrid,
        Document
    }

    public enum Verb
    {
        Get,
        Post,
        Put,
        Delete,
        Patch
    }
    
    public static class Utils
    {
        public static void EnrichJsonResult(HttpRequest request, JsonNode result, string path)
        {
            var baseUrl = request.Scheme + "://" +  request.Host + "/" + path;

            var InjectUrl = new Action<JsonObject>(i =>
            {
                if (i != null)
                {
                    var itemId = i["id"]?.GetValue<int?>();
                    if (itemId != null) i["url"] = baseUrl + $"/{itemId}";
                }
            });

            switch (result)
            {
                case JsonObject jsonObject:
                    InjectUrl(jsonObject);
                    break;

                case JsonArray jsonArray:
                    foreach (var item in jsonArray)
                    {
                        InjectUrl(item as JsonObject);
                    }
                    break;
            }
        }
    }
}