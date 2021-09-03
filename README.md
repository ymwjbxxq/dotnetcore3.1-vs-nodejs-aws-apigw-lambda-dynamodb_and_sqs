# dotnetcore3.1 vs node.js14.x

This is my third series of comparisons. 

* The [first](https://github.com/ymwjbxxq/dotnetcore3.1-vs-nodejs-aws-stepfunction-test) was a more cold start test.
* The [second](https://github.com/ymwjbxxq/dotnetcore3.1-vs-nodejs-aws-sqs-lambda-dynamodb) was a comparison in a warm status

This time is the overall performance in a different microservice scenario:
APIGW -> Lambda -> DynamoDB and SQS

### What I did ###

Two Lambda functions:

* [Dotnetcore3.1](https://github.com/ymwjbxxq/dotnetcore3.1-vs-nodejs-aws-apigw-lambda-dynamodb_and_sqs/tree/main/dotnetcore3.1/ApigwLambdaDynamoDbSqs)
* [Nodejs14.x](https://github.com/ymwjbxxq/dotnetcore3.1-vs-nodejs-aws-apigw-lambda-dynamodb_and_sqs/tree/main/node14.x)

### TESTS ###

The first noticeable thing is the package size:

* Manual post with [Postman](https://www.postman.com/).
* Process 1000 requests using dotnet HTTPClient.
* Run a more extended load with [Artillery](https://artillery.io/docs/guides/getting-started/installing-artillery.html).

### Manual post with Postman ###

![picture](https://github.com/ymwjbxxq/dotnetcore3.1-vs-nodejs-aws-apigw-lambda-dynamodb_and_sqs/blob/main/comparison1.png)

### METRICS 1000 requests ###

**Nodejs14.x:**

![picture](https://github.com/ymwjbxxq/dotnetcore3.1-vs-nodejs-aws-apigw-lambda-dynamodb_and_sqs/blob/main/node.test1.1000.png)

![picture](https://github.com/ymwjbxxq/dotnetcore3.1-vs-nodejs-aws-apigw-lambda-dynamodb_and_sqs/blob/main/node.test2.1000.png)

**Dotnetcore3.1:**

![picture](https://github.com/ymwjbxxq/dotnetcore3.1-vs-nodejs-aws-apigw-lambda-dynamodb_and_sqs/blob/main/dotnet.test1.1000.png)

![picture](https://github.com/ymwjbxxq/dotnetcore3.1-vs-nodejs-aws-apigw-lambda-dynamodb_and_sqs/blob/main/dotnet.test2.1000.png)

### Run a more extended load with Artillery ###

There is Artillery [file](https://github.com/ymwjbxxq/dotnetcore3.1-vs-nodejs-aws-apigw-lambda-dynamodb_and_sqs/blob/main/dotnetcore3.1/ApigwLambdaDynamoDbSqs/test.yml) where I have set up a few scenarios. Of course, it depends on the local machine where you are running the test, so it is possible that the expectations are not matching, but it serves to run a constant load to the two endpoints.

Once you installed just run
``` > artillery test.yml ```

You can even generate a report
``` >  artillery run --output report.json test.yml ```

and export it to HTML
``` > artillery report --output report.html report.json ```

The results of these tests are:

![picture](https://github.com/ymwjbxxq/dotnetcore3.1-vs-nodejs-aws-apigw-lambda-dynamodb_and_sqs/blob/main/node.artillery.png)

![picture](https://github.com/ymwjbxxq/dotnetcore3.1-vs-nodejs-aws-apigw-lambda-dynamodb_and_sqs/blob/main/dotnet.artillery.png)

**Lambda Insight:**

![picture](https://github.com/ymwjbxxq/dotnetcore3.1-vs-nodejs-aws-apigw-lambda-dynamodb_and_sqs/blob/main/lambda.insights.png)

### Conclusion ###

The rumours will say that the language in a serverless scenario is essential and to prefer interpreted languages over dotnetcore3.1 and if the cold start time is so important to you.
However, after seeing the results, I would say that is not important anymore because if latency is crucial, there is the option of [Provisioned concurrency](https://docs.aws.amazon.com/lambda/latest/dg/configuration-concurrency.html), but once you use it better to stay with a compiled language like dotnetcore3.1.
Given a choice, dotnetcore3.1 is faster and cheaper to run in a serverless scenario in the long term.

Maybe in a small website with sporadic requests, interpreted languages are better, so it usually depends on the requirement.

Perhaps [Go](https://golang.org/) could be even a better option.
