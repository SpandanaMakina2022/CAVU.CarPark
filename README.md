# CAVU Airport Car Park API

* All projects in the solution target .Net 6.0  
* Developed with VS 2022
* The APIs have been tested with Swagger and getting the expected results
* The test project unit tests have been executed and working

## Project details  

* Solution:         CAVU.CarPark
* API project:      CAVU.CarPark.API  
                  	This project presents the API call Action methods on car park booking.
* Data project:     CAVU.CarPark.Data  
                  	This project Bookings data access methods to return to the controller API calls 
* Services project: CAVU.CarPark.Service  
                  	This project holds the business logic to be performed based on the Bookings API request and the data is injected to the booking service
* Abstractions:     CAVU.CarPark.Abstractions  
                  	This project holds the interfaces and booking model to the booking data and the service. 
* Test project:     CAVU.CarPark.Test  
                  	This project holds the nUnit test cases to test the Booking API calls.
