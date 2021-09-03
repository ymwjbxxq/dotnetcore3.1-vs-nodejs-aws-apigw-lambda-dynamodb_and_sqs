using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.SQS;
using Amazon.SQS.Model;
using ApigwLambdaDynamoDbSqs.Dtos;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace ApigwLambdaDynamoDbSqs
{
    public class Function
    {
        private static readonly AmazonDynamoDBClient DynamoDbClient = new AmazonDynamoDBClient();
        private static readonly AmazonSQSClient AmazonSqsClient = new AmazonSQSClient(); 
        
        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest apigProxyEvent, ILambdaContext context)
        {
            //Console.WriteLine($"Processing request data for request {JsonSerializer.Serialize(apigProxyEvent)}.");
            var myDto = JsonSerializer.Deserialize<MyDto>(apigProxyEvent.Body);
            await SaveToDynamodb(myDto);
            await SendToSqs(myDto);

            return new APIGatewayProxyResponse
            {
                Body = apigProxyEvent.Body,
                StatusCode = 200,
            };
        }

        private static async Task SendToSqs(MyDto myDto)
        {
            var sendMessageRequest = new SendMessageRequest
            {
                QueueUrl = "https://sqs.eu-central-1.amazonaws.com/xxxxx/your-sqs",
                MessageBody = JsonSerializer.Serialize(myDto)
            };
            await AmazonSqsClient.SendMessageAsync(sendMessageRequest);
        }

        private static async Task SaveToDynamodb(MyDto myDto)
        {
            var request = new UpdateItemRequest
            {
                TableName = "your-table",
                Key = new Dictionary<string,AttributeValue>
                {
                    {
                        "Id", new AttributeValue
                        {
                            S = myDto.Id
                        }
                    }
                },
                UpdateExpression = "SET Fullname = :fullname",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {
                        ":fullname", new AttributeValue {
                            S =  myDto.Fullname
                        }
                    }
                },
                ReturnValues = "ALL_NEW"
            };
            await DynamoDbClient.UpdateItemAsync(request);
        }
    }
}