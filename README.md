# ingame-arquetipo
Este repositorio es para indicar el arquetipo de proyecto a usarse para la aplicación de InGame.

## Consideraciones generales:

* Este repositorio puede evolucionar con el tiempo.
* Se usa la guia de microservicios en .NET Core, enfocado en el uso de DDD y CQRS.
* Las capas de cada microservicio no estaran separadas por proyecto, sino por carpetas. Se pueden presentar excepciones.
* Cada microservicio tendrá un proyeto de UnitTest asociado.

## Estructura de la solución

_Reemplazar la palabra Servicio por el nombre del microservicio_
 
```
├── src
│   ├── Services
|   |   |──Servicio
|   |   |   |──tests
|   |   |   |   |──Servicio.UnitTests
|   |   |   |──Servicio.API
├── tests
├── docker-compose.yml
└── .gitignore
```

## Estructura de un microservicio basico tipo CRUD

```
├── Servicio.API
│   ├── Controllers
|   |── Domain
|   |   |── Models
|   |── HealthChecks
|   |── Infrastructure
|   |── Middleware
|   |   |──ExceptionHadling
|   |   |──Logging
|   |── Dockerfile
|   |── Program.cs
```

## Referencias:

* https://github.com/thirschel/dotnet-cqrs-microservice-template
* https://learn.microsoft.com/es-es/dotnet/architecture/microservices/architect-microservice-container-applications/microservices-architecture
* https://www.milanjovanovic.tech/blog/clean-architecture-folder-structure
* https://learn.microsoft.com/en-us/dotnet/architecture/microservices/multi-container-microservice-net-applications/data-driven-crud-microservice
* https://github.com/dotnet-architecture/eShopOnContainers/wiki/Simplified-CQRS-and-DDD
