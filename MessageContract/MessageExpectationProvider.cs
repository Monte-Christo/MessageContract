using PactNet.Matchers;
using SimCorp.Gain.Messages.Data;
using SimCorp.Gain.Messages.Shared.BusinessObject;
using SimCorp.Gain.Messages.Shared.Workflow;
using SimCorp.Gain.Messages.System.Operations;
using SimCorp.Gain.Messages.System.Workflow;

namespace MessageContract.Tests
{
    internal static class MessageExpectationProvider
    {
        //Expectations on each property of each message type
        private static readonly object WorkflowCreatedContent = new
        {
            StartArg = Match.Type("Aim.Gain.StaticData.DataManagement.BusinessProcess.FreshAggregateRequestBusinessProcessStartArgument"),
            Description = Match.Type("Party from feed Excel-Party [IdBBCompany:abc]"),
            Priority = Match.Type(ProcessPriority.Normal),
            Ancestry = Match.Type(Array.Empty<Ancestor>()),
            StartsBulkOperation = Match.Type(false),
            ContextData = Match.Type(new Dictionary<string, object>()),
            WorkflowType = Match.Type(WorkflowType.Business),
            WorkflowId = Match.Number(11),
            CorrelationId = Match.Type("sba"),
        };

        private static readonly object WorkflowFinishedContent = new
        {
            State = Match.Type(WorkflowFinishedState.Finished),
            WorkflowId = Match.Number(123),
            WorkflowType = Match.Type(WorkflowType.Business),
            CorrelationId = Match.Type("abc")
        };

        private static readonly object WorkflowChangedBusinessObjectContent = new
        {
            BusinessObjectReference = Match.Type(
                new BusinessObjectReference()
                {
                    Model = "SimCorpDimension",
                    DataType = "Party",
                    Domain = "Golden",
                    Id = 123,
                    Version = 1,
                }),
            Title = Match.Include("GP_5232"),
            Discriminator = "Master",
            WorkflowId = Match.Number(123),
            WorkflowType = Match.Type(WorkflowType.Business),
            CorrelationId = Match.Type("aa")
        };

        private static readonly object StartOperationFailedContent = new
        {
        };

        private static readonly object StartOperationSucceededContent = new
        {
        };

        private static readonly object WorkflowReadyContent = new
        {
            WorkflowId = Match.Number(123),
            WorkflowType = Match.Type(WorkflowType.Business),
            CorrelationId = Match.Type("aa")
        };

        private static readonly object WorkflowWaitingContent = new
        {
            WaitingFor = Match.Type(WorkflowWaitingFor.Workflow),
            WorkflowId = Match.Number(123),
            CorrelationId = Match.Type("aa")
        };

        private static readonly object BusinessObjectChangedContent = new
        {
            WorkflowReference = Match.Type(
                new
                {
                    CorrelationId = Match.Type("ab"),
                    Id = Match.Number(12),
                    Type = Match.Type(WorkflowType.Scrubbing),
                }),
            Discriminator = Match.Type("Master"),
            References = Match.Type(new[]
            {
                new
                {
                    Ref = Match.Type(new
                    {
                        Model = "SimCorpDimension",
                        DataType = "Party",
                        Domain = "Silver",
                        Id = 12,
                        Version = 1
                    }),
                    Type = Match.Type(DataReferenceType.Link)
                }
            }),
            Fingerprint = Match.Type("12"),
            Model = Match.Type("SimCorpDimension"),
            DataType = Match.Type("Party"),
            Domain = Match.Type("Golden"),
            Id = Match.Number(12)
        };

        private static readonly Dictionary<Type, object> ExpectedEventDict = new Dictionary<Type, object>
        {
            { typeof(StartOperationFailed) , StartOperationFailedContent },
            { typeof(StartOperationSucceeded) , StartOperationSucceededContent },

            { typeof(WorkflowCreated) , WorkflowCreatedContent  },
            { typeof(WorkflowFinished), WorkflowFinishedContent },
            { typeof(WorkflowReady), WorkflowReadyContent },
            { typeof(WorkflowWaiting), WorkflowWaitingContent },
            { typeof(WorkflowChangedBusinessObject), WorkflowChangedBusinessObjectContent },

            { typeof(BusinessObjectChanged), BusinessObjectChangedContent }

        };

        public static object GetExpectations(Type eventType)
        {
            if (ExpectedEventDict.TryGetValue(eventType, out object content))
            {
                return content;
            }
            else
            {
                throw new ArgumentException($"{eventType} is not a recognised type name. Please consider adding this to {nameof(EventTypeMapper)}.");
            }
        }
    }
}