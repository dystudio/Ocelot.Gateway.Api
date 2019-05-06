# Ocelot.Gateway.Api
An Ocelot api gateway for aggregating microservices responses and an Angular 7.0 UI.


![GitHub release](https://img.shields.io/github/release/singhrahulnet/Ocelot.Gateway.Api.svg?style=for-the-badge) ![Maintenance](https://img.shields.io/maintenance/yes/2019.svg?style=for-the-badge)

![GitHub Release Date](https://img.shields.io/github/release-date/singhrahulnet/Ocelot.Gateway.Api.svg?style=plastic) [![.Net Framework](https://img.shields.io/badge/DotNet-Framework_2.1-blue.svg?style=plastic)](https://www.microsoft.com/net/download/dotnet-core/2.1)  ![GitHub language count](https://img.shields.io/github/languages/count/singhrahulnet/Ocelot.Gateway.Api.svg?style=plastic) ![GitHub top language](https://img.shields.io/github/languages/top/singhrahulnet/Ocelot.Gateway.Api.svg) 

### Setup Details

#### Environment Setup

> Download/install [![.Net Framework](https://img.shields.io/badge/DotNet-Framework_2.1-blue.svg?style=plastic)](https://www.microsoft.com/net/download/dotnet-core/2.1) to run the project   


> Download and install [![Node.js](https://img.shields.io/badge/Node.js-Latest-blue.svg?style=plastic)](https://nodejs.org/en/) and [![VSCode](https://img.shields.io/badge/VSCode-Latest-blue.svg?style=plastic)](https://code.visualstudio.com/)

> Download/clone the repository.


#### Running the Application
> Start the Gateway first. Open *Movies.Gateway.Api* folder in VS Code terminal and run -

    dotnet run

> Start the *Backend-For-Frontend* api. Open *CheapestMovies.Api* folder in VS Code terminal and run -

    dotnet run

> Open http://localhost:3000 in browser and you should see swagger site with following end points -

<img src="https://github.com/singhrahulnet/Ocelot.Gateway.Api/blob/master/refImg/swagger.PNG">

> Start the Angular UI. Open *ngClientApp* folder in VS Code terminal and run following command. The UI should be available at http://localhost:4200.
    
    ng serve -o

> To run the unit tests, open *CheapestMovies.Test* folder in VS Code terminal and run following command.
    
    dotnet test


### Problem Statement

Build a web app to allow customers to get the cheapest price for movies from following providers in a timely manner. There are 2 API operations available for 2 popular movie databases, cinemaworld and filmworld - 

> /api/{cinemaworld or filmworld}/movies : This returns the movies that are available

> /api/{cinemaworld or filmworld}/movie/{ID}: This returns the details of a single movie


### Solution
> **Api Gateway Pattern:** is implemented using Ocelot. The gateway provides single entry-point for group of microservices i.e. different *movie worlds* in our case. Ideally this api should be in private network.

> **.NET Core Api:** a public REST api on top of the gateway

> **Client App:** Angular UI to show all the available movies and fetch cheapest price of a movie

<img width="50%" height="50%" src="https://github.com/singhrahulnet/Ocelot.Gateway.Api/blob/master/refImg/ocelot.PNG">


#### Design Highlights

##### ReRoutes
> The Api Gateway handles upstream and downstream ReRoutes based on configuration. In order to add a new *movie world* simply add details to ocelot.config

<img width="50%" height="50%" src="https://github.com/singhrahulnet/Ocelot.Gateway.Api/blob/master/refImg/addmovieworld.png">

##### Request Aggregation
> The Api Gateway allows to specify Aggregate ReRoutes that compose multiple normal ReRoutes and map their responses into one object. For example, in our case a single call to /api/movies/{Id} would split the calls to multiple *movie worlds* and aggregate the responses before sending it to the client.

<img width="50%" height="50%" src="https://github.com/singhrahulnet/Ocelot.Gateway.Api/blob/master/refImg/aggregates.PNG">

##### Caching and QoS
> Add caching configuration easily to each of the microservice or at *Global* level. Quality of Service (QoS) options are available too.    

<img width="50%" height="50%" src="https://github.com/singhrahulnet/Ocelot.Gateway.Api/blob/master/refImg/caching.PNG">

<img width="50%" height="50%" src="https://github.com/singhrahulnet/Ocelot.Gateway.Api/blob/master/refImg/qos.PNG">


##### S.O.L.I.D Principles
> The software design is lucid, extensible and maintainable by adhering to S.O.L.I.D principles.

##### Inversion of Control
> The dependencies are inverted using IoC container.

##### Unit Tests
> The xUnit (with Moq) tests are written to make the software robust. Include the test cases in CI/CD pipeline.


### Support or Contact
Having any trouble? Please read out this [documentation](https://github.com/singhrahulnet/Ocelot.Gateway.Api/blob/master/README.md) or [contact](mailto:singh.rahul.net@gmail.com) to sort it out.

[![HitCount](http://hits.dwyl.io/singhrahulnet/Ocelot.Gateway.Api/projects/1.svg)](http://hits.dwyl.io/singhrahulnet/Ocelot.Gateway.Api/projects/1)  ![GitHub contributors](https://img.shields.io/github/contributors/singhrahulnet/Ocelot.Gateway.Api.svg?style=plastic)
 
 
 
Keep Coding :-) 

