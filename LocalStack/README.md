1. Prerequesites:
- Docker
- AWS CLI + aws configure (Temp Key, Id, Region == eu-west-3, Output == json)

2. Commands:
 - Create docker-compose.yml file
 ``` YML

version: '3.0'

services:
  localstack:
    image: localstack/localstack:latest
    environment:
      - AWS_DEFAULT_REGION=eu-west-3
      - EDGE_PORT=4566
    ports:
      - '4566-4597:4566-4597'
    volumes:
      - "${TMPDIR:-/tmp/localstack}:/var/lib/localstack"
      - "/var/run/docker.sock:/var/run/docker.sock"

 ```
 - at the same location where **docker-compose.yml** file is open console and type **docker-compose up** *(data will be deleted when you stop the container with docker-compose down)*. It will start **LocalStack** in docker container.

 - when it's ready you should see something like

 ``` TXT
localstack-localstack-1  | Waiting for all LocalStack services to be ready
localstack-localstack-1  | 2022-10-19 10:52:47,726 CRIT Supervisor is running as root.  Privileges were not dropped because no user is specified in the config file.  If you intend to run as root, you can set user=root in the config file to avoid this message.
localstack-localstack-1  | 2022-10-19 10:52:47,730 INFO supervisord started with pid 15
localstack-localstack-1  | 2022-10-19 10:52:48,733 INFO spawned: 'infra' with pid 20
localstack-localstack-1  | 2022-10-19 10:52:49,735 INFO success: infra entered RUNNING state, process has stayed up for > than 1 seconds (startsecs)
localstack-localstack-1  | 
localstack-localstack-1  | LocalStack version: 1.2.1.dev
localstack-localstack-1  | LocalStack Docker container id: 970b122abc76
localstack-localstack-1  | LocalStack build date: 2022-10-18
localstack-localstack-1  | LocalStack build git hash: f54aa8bf
localstack-localstack-1  | 
localstack-localstack-1  | Ready.
 ```

- type **http://localhost:4566/health** you should see response like this:

``` JSON
{
	"features": {
		"initScripts": "initialized"
	},
	"services": {
		"acm": "available",
		"apigateway": "available",
		"cloudformation": "available",
		"cloudwatch": "available",
		"config": "available",
		"dynamodb": "available",
		"dynamodbstreams": "available",
		"ec2": "available",
		"es": "available",
		"events": "available",
		"firehose": "available",
		"iam": "available",
		"kinesis": "available",
		"kms": "available",
		"lambda": "available",
		"logs": "available",
		"opensearch": "available",
		"redshift": "available",
		"resource-groups": "available",
		"resourcegroupstaggingapi": "available",
		"route53": "available",
		"route53resolver": "available",
		"s3": "available",
		"s3control": "available",
		"secretsmanager": "available",
		"ses": "available",
		"sns": "available",
		"sqs": "available",
		"ssm": "available",
		"stepfunctions": "available",
		"sts": "available",
		"support": "available",
		"swf": "available",
		"transcribe": "available"
	},
	"version": "1.2.1.dev"
}
```

- you could start playing with your local AWS instance ;) 

3. Play with SQS 

- **create** SQS:

Type in PowerShell console:
``` PowerShell
 aws --endpoint-url=http://localhost:4566 sqs create-queue --queue-name mysqs
```

Response:
```JSON
{
    "QueueUrl": "http://localhost:4566/000000000000/mysqs"
}
```

- **list** SQS:

```PowerShell
aws --endpoint-url=http://localhost:4566 sqs list-queues
```

Response:

```JSON
{
    "QueueUrls": [
        "http://localhost:4566/000000000000/mysqs"
    ]
}
```

- **read** SQS messages:

```PowerShell
aws --endpoint-url=http://localhost:4566 sqs receive-message --queue-url http://localhost:4566/000000000000/mysqs
```

- **create** SNS topic:

```PowerShell
aws --endpoint-url=http://localhost:4566 sns create-topic --name mysns-topic
```

Response:
```JSON
{
    "TopicArn": "arn:aws:sns:eu-west-2:000000000000:mysns-topic"
}
```

- **list** SNS subscriptions

```PowerShell
aws --endpoint-url=http://localhost:4566 sns list-subscriptions
```

Response:
```JSON
{
    "Subscriptions": []
}
```

- subscribe to SNS topic using SQS - need SNS topic ARN and SQS Queue Name

```PowerShell
aws --endpoint-url=http://localhost:4566 sns subscribe --topic-arn arn:aws:sns:eu-west-2:000000000000:mysns-topic --protocol sqs --notification-endpoint http://localhost:4566/000000000000/mysqs

```

Response:
```JSON
{
    "SubscriptionArn": "arn:aws:sns:eu-west-2:000000000000:mysns-topic:65dfd7db-724f-44db-8c30-fbb251e99696"
}
```

- SNS publish message to topic
```PowerShell
aws --endpoint-url=http://localhost:4566 sns publish  --topic-arn arn:aws:sns:eu-west-2:000000000000:mysns-topic --message 'Welcome to LocalStack!'
```

Response
```JSON
{
    "MessageId": "0cbfde5c-bb89-46ea-a182-17d4a8bf0896"
}
```

- Read SQS messages from SNS topic

```PowerShell
aws --endpoint-url=http://localhost:4566 sqs receive-message --queue-url http://localhost:4566/000000000000/mysqs
```

Response:
```JSON
{
    "Messages": [
        {
            "MessageId": "00c3def7-d11a-4e28-a915-c32da8558cd9",
            "ReceiptHandle": "OTY1N2FkZGUtNGVhNy00Mjg0LTgwNzQtNDBjMDk3NmU5MjBmIGFybjphd3M6c3FzOmV1LXdlc3QtMjowMDAwMDAwMDAwMDA6bXlzcXMgMDBjM2RlZjctZDExYS00ZTI4LWE5MTUtYzMyZGE4NTU4Y2Q5IDE2NjYxNzgxMTAuOTc2NjY4NA==",
            "MD5OfBody": "2e908a7c5db0234b285379393a30aa86",
            "Body": "{\"Type\": \"Notification\", \"MessageId\": \"0cbfde5c-bb89-46ea-a182-17d4a8bf0896\", \"TopicArn\": \"arn:aws:sns:eu-west-2:000000000000:mysns-topic\", \"Message\": \"Welcome to LocalStack!\", \"Timestamp\": \"2022-10-19T11:13:21.453Z\", \"SignatureVersion\": \"1\", \"Signature\": \"EXAMPLEpH+..\", \"SigningCertURL\": \"https://sns.us-east-1.amazonaws.com/SimpleNotificationService-0000000000000000000000.pem\", \"UnsubscribeURL\": \"http://localhost:4566/?Action=Unsubscribe&SubscriptionArn=arn:aws:sns:eu-west-2:000000000000:mysns-topic:65dfd7db-724f-44db-8c30-fbb251e99696\"}"
        }
    ]
}
```