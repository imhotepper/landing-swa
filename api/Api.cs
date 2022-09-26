using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace daiot
{
    public class Api
    {
        private readonly ILogger<Api> _logger;

        public Api(ILogger<Api> log) => _logger = log;

        [FunctionName("save-email")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        // [OpenApiParameter(name: "email", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Email** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string email = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Console.WriteLine($"Requeste body: {requestBody}");
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            _logger.LogInformation($"Body parts: {JsonConvert.SerializeObject(data)}");
            email = email ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(email)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {email}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}

