using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace OpenApiV3Parser.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var doc = OpenApiParser.ParseFromFile("openApiV3Sample.json");

            //Paths
            //foreach (var (path, pathItem) in doc.Paths.Keys)
            //{
            //    foreach (var (method, op) in pathItem.EnumerateOperations())
            //    {
            //        Console.WriteLine($"{method} {path}  tags=[{string.Join(",", op.Tags ?? new())}]");
            //    }
            //}

            //// Types (component schemas)
            //Console.WriteLine($"Schemas: {doc.Components?.Schemas?.Count ?? 0}");
            //if (doc.Components?.Schemas != null && doc.Components.Schemas.TryGetValue("LoginRequest", out var loginReq))
            //{
            //    Console.WriteLine($"LoginRequest type={loginReq.Type}, props={loginReq.Properties?.Count ?? 0}");
            //}

        }

        [TestMethod]
        public void TestMapping()
        {
            var doc = OpenApiParser.ParseFromFile("openApiV3Sample.json");
            var definition = Mappings.OpenApiDefinitionMapper.Map(doc);
            foreach (var endpoint in definition.Endpoints)
            {
                Console.WriteLine($"{endpoint.Method} {endpoint.Path}  group={endpoint.EntityGroup}");
            }
        }
    }
}


