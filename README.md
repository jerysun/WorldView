# WorldView - Viewing world starts from cities!

This project offers the web APIs for companies or individuals to access and manage the global cities information, it's an open source project based on ASP.NET.

## It has the following features:
- REST APIs and a simple Client
- Automapper
- DI (Dependency Injection)
- Entity Framework
- Repository-Service Pattern
- HttpClient
- DTO

## API Design Specifications

### (1) API design examples

#### Requirements:

•	Add City - adds the city to storage. Fields:
o	city name, 
o	state 
o	country, 
o	tourist rating (1-5), 
o	date established
o	estimated population

•	Get city – get by city name, returns the following: 
o	city id
o	name
o	state 
o	country
o	tourist rating (1-5)
o	date established
o	estimated population
o	2 digit country code
o	3 digit country code
o	currency code
o	weather for the city.

#### The third party library APIs:

The country information (country code / currency code) should be taken from an external API: REST countries API (https://restcountries.eu/#api-endpoints-all - returns detailed country information)

The weather information should be taken from an external API: OpenWeatherMap REST API (https://openweathermap.org - returns detailed weather information for a particular city)

### (2) API Scheme examples

Methods		Endpoints
-------------------------------------------------------------
GET			/api/v1/city?name={city name}&code={country code}
POST		/api/v1/city

### (3) JSON format

GET https://localhost:5001/api/v1/city?name=Boston&code=US

The response is:

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
    "weather": "snow"
}


Have fun,

Jerry Sun ( jerysun007@hotmail.com )