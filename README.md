# GrpcGenerator
Generates gRpc .net code for Sql databases

# Purpose of this project
The intention of this code is to be able to automatically generate a .net core gRPC services that exposes a SQL database.


# Project design
The generator connects to a SQL database and enumerates the tables, views and stored procedures.
Based on the structure of these sql objects, gRPC protofile and gRpc .net service is created

## gRPC proto file structure
### Table structure


# Roadmap
Priority 1
ability to generate proto file
ability to generate C# service
