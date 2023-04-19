using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using SQSDemo.Model;
using System.Text.Json;
using static Amazon.Lambda.SQSEvents.SQSEvent;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SQSDemo;

public class FunctionDemo
{
    private readonly JsonSerializerOptions options;
    /// <summary>
    /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
    /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
    /// region the Lambda function is executed in.
    /// </summary>
    public FunctionDemo()
    {
        options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
    }

    /// <summary>
    /// This method is called for every Lambda invocation. This method takes in an SQS event object and can be used 
    /// to respond to SQS messages.
    /// </summary>
    /// <param name="evnt"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task FunctionDemoHandler(SQSEvent evnt, ILambdaContext context)
    {
        foreach (var message in evnt.Records)
        {
            await ProcessMessageAsync(message, context);
        }
    }

    private async Task ProcessMessageAsync(SQSMessage message, ILambdaContext context)
    {
        var contact = JsonSerializer.Deserialize<Contact>(message.Body, options);

        if (contact == null)
            return;

        context.Logger.Log($"MessageId: {message.MessageId} | Contact: {contact.Name} | Number: {contact.Number}");

        // TODO: Do interesting work based on the new message
        await Task.CompletedTask;
    }
}