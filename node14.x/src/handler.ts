/// <reference types="node" />
import DynamoDB from "aws-sdk/clients/dynamodb";
import SQS from "aws-sdk/clients/sqs";

const dynamoDbClient = new DynamoDB.DocumentClient();
const sqsClient = new SQS();

export const handler = async (event: any, context: any): Promise<any> => {
  console.log(JSON.stringify(event));
  
  const myDto: MyDto = JSON.parse(event.body);
  await saveToDynamodb(myDto);
  await SendToSqs(myDto);

  return {
    Body: event.body,
    StatusCode: 200,
  };
};

async function saveToDynamodb(myDto: MyDto): Promise<void> {
  const params = {
    TableName: "your-table",
    Key: {
      "Id": myDto.Id
    },
    UpdateExpression: "SET Fullname = :fullname",
    ExpressionAttributeValues: {
      ":fullname": myDto.Fullname
    },
    ReturnValues: "UPDATED_NEW"
  };

  await dynamoDbClient.update(params).promise();
}

async function SendToSqs(myDto: MyDto): Promise<void> {
  const params: SQS.Types.SendMessageRequest = {
    MessageBody: JSON.stringify(myDto),
    QueueUrl: "https://sqs.eu-central-1.amazonaws.com/xxxxx/your-sqs"
  };
  await sqsClient.sendMessage(params).promise();
}

export interface MyDto {
  Id: string;
  Fullname: string;
}
