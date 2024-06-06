using FluentAssertions;
using PactNet;
using PactNet.Matchers;
using PactNet.Output.Xunit;
using SimCorp.Gain.Messages.Data;
using SimCorp.Gain.Messages.System.Workflow;
using System.Text.Json;
using Xunit.Abstractions;

namespace MessageContract.Tests;

public class GainEventProcessorTests
{
    private readonly IMessagePactBuilderV4 _messagePact;

    public GainEventProcessorTests(ITestOutputHelper output)
    {
        IPactV4 v4 = Pact.V4("OperationTracking", "Gain", new PactConfig
        {
            PactDir = Path.Combine("..", "..", "..", "..", "pacts"),
            DefaultJsonSettings = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            },
            Outputters = [new XunitOutput(output)]
        });
        _messagePact = v4.WithMessageInteractions();
    }

    [Fact]
    public void ReceiveSomeStockEvents()
    {
        Dictionary<string, List<object>> expectedGroupByMessagesWithType = Steps.PrepareData("ExpectedMessages.json");
        Dictionary<string, List<object>> actualGroupByMessagesWithType = Steps.PrepareData("ActualMessagesForTesting.json");

        ICollection<string> messageTypes = expectedGroupByMessagesWithType.Keys;
        foreach (string messageType in messageTypes)
        {
            Type eventType = EventTypeMapper.GetTypeForEventName(messageType);
            int actualMessageCount = actualGroupByMessagesWithType.TryGetValue(messageType, out List<object>? actualGroupedMsgs)
                ? actualGroupedMsgs.Count
                : 1;

            if (!expectedGroupByMessagesWithType.TryGetValue(messageType, out List<object>? expectedGroupedMsgs))
            {
                throw new ArgumentException($"No matching expected message group found");
            }

            switch (eventType.Name)
            {
              case "WorkflowCreated":
                this._messagePact
                  .ExpectsToReceive($"{eventType.Name} Message from Gain for the feed upload request")
                  .Given($"{eventType.Name} events are pushed to the queue")
                  .WithMetadata("key", "valueKey")
                  .WithJsonContent(Match.MinType(MessageExpectationProvider.GetExpectations(eventType), actualMessageCount))
                  .Verify<ICollection<WorkflowCreated>>(events =>
                  {
                    events.Should().BeEquivalentTo([expectedGroupedMsgs[0]]);
                  });
                break;
              case "WorkflowFinished":
                this._messagePact
                  .ExpectsToReceive($"{eventType.Name} Message from Gain for the feed upload request")
                  .Given($"{eventType.Name} events are pushed to the queue")
                  .WithMetadata("key", "valueKey")
                  .WithJsonContent(Match.MinType(MessageExpectationProvider.GetExpectations(eventType), actualMessageCount))
                  .Verify<ICollection<WorkflowFinished>>(events =>
                  {
                    events.Should().BeEquivalentTo([expectedGroupedMsgs[0]]);
                  });
                break;
              case "WorkflowReady":
                this._messagePact
                  .ExpectsToReceive($"{eventType.Name} Message from Gain for the feed upload request")
                  .Given($"{eventType.Name} events are pushed to the queue")
                  .WithMetadata("key", "valueKey")
                  .WithJsonContent(Match.MinType(MessageExpectationProvider.GetExpectations(eventType), actualMessageCount))
                  .Verify<ICollection<WorkflowReady>>(events =>
                  {
                    events.Should().BeEquivalentTo([expectedGroupedMsgs[0]]);
                  });
                break;
              case "WorkflowWaiting":
                this._messagePact
                  .ExpectsToReceive($"{eventType.Name} Message from Gain for the feed upload request")
                  .Given($"{eventType.Name} events are pushed to the queue")
                  .WithMetadata("key", "valueKey")
                  .WithJsonContent(Match.MinType(MessageExpectationProvider.GetExpectations(eventType), actualMessageCount))
                  .Verify<ICollection<WorkflowWaiting>>(events =>
                  {
                    events.Should().BeEquivalentTo([expectedGroupedMsgs[0]]);
                  });
                break;
              case "WorkflowChangedBusinessObject":
                this._messagePact
                  .ExpectsToReceive($"{eventType.Name} Message from Gain for the feed upload request")
                  .Given($"{eventType.Name} events are pushed to the queue")
                  .WithMetadata("key", "valueKey")
                  .WithJsonContent(Match.MinType(MessageExpectationProvider.GetExpectations(eventType), actualMessageCount))
                  .Verify<ICollection<WorkflowChangedBusinessObject>>(events =>
                  {
                    events.Should().BeEquivalentTo([expectedGroupedMsgs[0]]);
                  });
                break;
              case "BusinessObjectChanged":
                this._messagePact
                  .ExpectsToReceive($"{eventType.Name} Message from Gain for the feed upload request")
                  .Given($"{eventType.Name} events are pushed to the queue")
                  .WithMetadata("key", "valueKey")
                  .WithJsonContent(Match.MinType(MessageExpectationProvider.GetExpectations(eventType), actualMessageCount))
                  .Verify<ICollection<BusinessObjectChanged>>(events =>
                  {
                    events.Should().BeEquivalentTo([expectedGroupedMsgs[0]]);
                  });
                break;
            }
        }
    }
}


//Issue/Todo:
//How to use Match to validate that the actual value is one of the given expected values.
//Need to manually create the expected message in the consumer test (only for required fields):
//  Define expected values for some properties.
//  Skip non-required fields.
//Not easy to debug why the provider test fails.