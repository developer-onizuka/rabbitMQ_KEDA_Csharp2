# rabbitMQ_KEDA_Csharp2

# 1. Send Data to Temporary Queue by dotnet app
```
# export RABBITMQ_IPADDR="192.168.33.220"
# export RABBITMQ_DLX="dlx.employee-queue-tmp"
# export RABBITMQ_MESSAGECOUNT="500"
# cd sendImage-to-rabbitmqDLX2/
# dotnet run
 [x] Sent 1
 [x] Sent 2
 [x] Sent 3
 ...
 [x] Sent 498
 [x] Sent 499
 [x] Sent 500
 Press [enter] to exit.
```

# 2. Face Detection by the function of python Library
This function is
- to dequeue the EmployeeEntity record including image from the "employee-queue-temp".
- to process its image and make a new face's image by python AI Library.
- to enqueue the EmployeeEntity record including image and face's image back to the employee-queue.
```
# cd rabbitmq-to-faceDetectedMQ
# cat <<EOF > local.settings.json 
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet",
        "RabbitMQConnection": "amqp://user:PASSWORD@rabbitmq.default.svc.cluster.local:5672",
        "RabbitMQ_IPaddress": "rabbitmq",
        "RabbitMQ_DLX": "dlx.employee-queue",
        "PrimaryConnection": "mongodb://xxxxxxxxxx",
        "SecondaryConnection": "mongodb://xxxxxxxxxx"
    }
}
EOF

# func kubernetes deploy --name rabbitmq-to-facedetectedmq --registry 192.168.1.5:5000 --max-replicas 16 --polling-interval 5 --cooldown-period 60
```

# 3. Insert the record to MongoDB
This function is
- to dequeue the EmployeeEntity record including image and face's image from the "employee-queue".
- to insert it to MongoDB.

```
# cd rabbitmq-to-hybridCloud2
# cat <<EOF > local.settings.json 
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet",
        "RabbitMQConnection": "amqp://user:PASSWORD@rabbitmq.default.svc.cluster.local:5672",
        "RabbitMQ_IPaddress": "rabbitmq",
        "RabbitMQ_DLX": "dlx.employee-queue-failure",
        "PrimaryConnection": "mongodb://mongo-0:27017,mongo-1:27017,mongo-2:27017/?replicaSet=myReplicaSet",
        "SecondaryConnection": "mongodb://mongo-0:27017,mongo-1:27017,mongo-2:27017/?replicaSet=myReplicaSet"
    }
}
EOF

# func kubernetes deploy --name rabbitmq-to-hybridcloud2 --registry 192.168.1.5:5000 --max-replicas 16 --polling-interval 5 --cooldown-period 60
```
