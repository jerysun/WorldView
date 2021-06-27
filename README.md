# WorldView - Viewing world starts from cities!

This project offers the web APIs for companies or individuals to access and manage the global cities information, it's an open source project based on ASP.NET.

## It has the following features:
- REST APIs and a simple Client
- Automapper
- DI (Dependency Injection)
- Entity Framework Core
- Repository-Service Pattern
- HttpClient
- Pagination
- DTO
- JWT
- Complete CRUD with HTTP PATCH method
- Users management based on the roles group

## API Design Specifications

### (1) API design examples

#### Requirements:

##### Add City - adds the city to storage. Fields:
- city name, 
- state 
- country, 
- tourist rating (1-5), 
- date established
- estimated population

##### Get city – get by city name, returns the following: 
- city id
- name
- state 
- country
- tourist rating (1-5)
- date established
- estimated population
- 2 digit country code
- 3 digit country code
- currency code
- weather for the city.

#### The third party library APIs:

The country information (country code / currency code) should be taken from an external API: REST countries API (https://restcountries.eu/#api-endpoints-all - returns detailed country information)

The weather information should be taken from an external API: OpenWeatherMap REST API (https://openweathermap.org - returns detailed weather information for a particular city)

### (2) API Scheme examples

Methods     Endpoints
-------------------------------------------------------------
- GET       /api/v1/city?name={city name}&code={country code}
- POST      /api/v1/city
- GET       /api/v1/city/listall
- PUT       /api/v1/city/{id:int}
- PATCH     /api/v1/city/{id:int}
- DELETE    /api/v1/city/{id:int}

### (3) JSON format

GET https://localhost:5001/api/v1/city?name=Boston&code=US

The response is:

```json
{
    "id": 5,
    "name": "Boston",
    "state": "Massachusettes",
    "country": "United States of America",
    "toruistRating": 3,
    "dateEstablished": "1819-02-21T00:00:00",
    "population": 1000000,
    "alpha2Code": "US",
    "alpha3Code": "USA",
    "currenciesCode": "USD",
    "weather": "snow, -2°C"
}
```

Have fun,

Jerry Sun ( jerysun007@hotmail.com )