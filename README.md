.NET to NestJS Microservice Communication Package 
Introduction 
 
This package is designed to simplify communication between .NET Project and NestJS Microservices.  
Many teams work in a microservices architecture and sometimes use different languages to develop different services.  
This package helps you to easily communicate between your .NET projects and  NestJS Microservices . 
 
Problem 
 
Communication between .NET projects and NestJS microservices is not always straightforward.  
You may encounter problems such as: 
 
Data format mismatches 
Lack of agreement on communication protocols 
Lack of suitable libraries to simplify the process 
Solution 
 
This package solves the problem of communication 
 between .NET projects and NestJS microservices 
 by providing a simple and understandable interface. 
 
How to Use 
 
Using this package is very simple.  
Just follow these steps: 
 
Add the package to your C# project. 
Use the Services.addService function to register your nets service. 
Use the Services.Send function to send a request to your NestJS Microservice . 
Your callback function will be executed when the response is received from the NestJS Microservice. 
 
Example 
 
--> C# 
//you can use another overload 
Services.addService({ 
  port: 8580, 
}); 
 
//you can use another overload 
Services.Send(8580, 'pattern', 'dataStringfy', (response) => { 
  console.log(response); 
}); 
  
  
  
 
This package uses TCP by default for communication. 
  
For more information, refer to the full package documentation. 
Benefits of Using this Package 
  
Eliminates the need to write complex code 
Saves time and effort 
Increases reliability and stability 
  
 

