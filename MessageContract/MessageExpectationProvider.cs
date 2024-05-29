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
        private readonly static object workflowCreatedContent = new
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

        private readonly static object workflowFinishedContent = new
        {
            State = Match.Type(WorkflowFinishedState.Finished),
            WorkflowId = Match.Number(123),
            WorkflowType = Match.Type(WorkflowType.Business),
            CorrelationId = Match.Type("abc")
        };

        private readonly static object workflowChangedBusinessObjectContent = new
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

        private readonly static object startOperationFailedContent = new
        {
        };

        private readonly static object startOperationSucceededContent = new
        {
        };

        private readonly static object workflowReadyContent = new
        {
            WorkflowId = Match.Number(123),
            WorkflowType = Match.Type(WorkflowType.Business),
            CorrelationId = Match.Type("aa")
        };

        private readonly static object workflowWaitingContent = new
        {
            WaitingFor = Match.Type(WorkflowWaitingFor.Workflow),
            WorkflowId = Match.Number(123),
            CorrelationId = Match.Type("aa")
        };

        private readonly static object businessObjectChangedContent = new
        {
            WorkflowReference = Match.Type(
                new
                {
                    CorrelationId = Match.Type("ab"),
                    Id = Match.Number(12),
                    Type = Match.Type(WorkflowType.Scrubbing),
                }),
            Discriminator = Match.Type("Master"),
            // To Do: Check how to set up Match rule for object type.
            // The error: Expected property events[0].Data to be a dictionary or collection of key-value pairs that is keyed to type System.String
            //Data = Match.Type(
            //    new
            //    {
            //        GainPartyId = Match.Type("123"),
            //        Issuer = Match.Type(true),
            //        PartyFreeCode20 = Match.Type("abc"),
            //        Party = Match.Type("GP_5"),
            //        PartyName = Match.Type("GP_5"),
            //        ReviewDate = Match.Type("2024-01-01T00:00:00Z"),
            //        BBGCompany = Match.Type("123"),
            //        GainID = Match.Type("GP_5")
            //    }),
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

        private readonly static Dictionary<Type, object> expectedEventDict = new Dictionary<Type, object>
        {
            { typeof(StartOperationFailed) , startOperationFailedContent },
            { typeof(StartOperationSucceeded) , startOperationSucceededContent },

            { typeof(WorkflowCreated) , workflowCreatedContent  },
            { typeof(WorkflowFinished), workflowFinishedContent },
            { typeof(WorkflowReady), workflowReadyContent },
            { typeof(WorkflowWaiting), workflowWaitingContent },
            { typeof(WorkflowChangedBusinessObject), workflowChangedBusinessObjectContent },

            { typeof(BusinessObjectChanged), businessObjectChangedContent }

        };

        public static object GetExpectations(Type eventType)
        {
            if (expectedEventDict.TryGetValue(eventType, out object content))
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