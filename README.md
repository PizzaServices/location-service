# location-service

This repository will serve as a basis to implement a location microservice. Within the service different locations like hotels, restaurants or similar can be stored and based on the user location within a certain radius the closest locations can be found.

 <p align="center">
  <img src="https://miro.medium.com/max/700/1*jWI3ORSCy9erDhDDRTJW3w.png" alt="location search image"/>
</p>

## Motivation

I was always impressed by how Google, for example, could so quickly find places nearby. Or even e.g. food delivery services check if you are still in the delivery area. So I read up a bit on the matter and now try to implement what I learned here.

## Architecture

Basically it should be possible to use different databases to execute the queries. For this there is the interface <mark>IDataAccess</mark> , which defines the methods that must then be implemented for each individual database. When starting the Docker container, it should then be possible to specify which database you want to use and that should be it. 

So far, however, only the in memory variant has been implemented. Further connections will be added in the future.

## Alogrithms 

For the in memory implementation I chose an approach via GeoHash. With the help of a proximity hash I can find places that are within a certain radius relatively fast. Furthermore, I would like to take a look at the implementation of GeoRaptor.

[GeoHash](https://github.com/dice89/proximityhash)

[GeoRaptor](https://github.com/ashwin711/georaptor)

## Todos

- [x] Create Solutions
- [x] Implement InMemory Data Structure
- [ ] Implement MongoDb support
- [ ] Implement PostgreSQL support
- [ ] Implement authentication support
- [x] Implement rate limiting
- [ ] Implement caching
- [ ] Implement sharding for InMemory
- [ ] Implement own persistant storage
- [ ] Improve Performance


License:
--------

The code is orginally from Ashwin Nair, and Alexander Mueller 

Licensed under the Apache License, Version 2.0. 

```
Copyright 2017 Ashwin Nair <https://www.linkedin.com/in/nairashwin7>
Copyright 2017 Alexander Mueller <https://www.linkedin.com/in/alexander-m%C3%BCller-727315a7/>

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
```
