Guest API:

This ASP.NET Core Web API project provides endpoints for managing guest entities, including adding guests, adding phone numbers for existing guests, and retrieving guest information.


Setup:

Clone the repository to your local machine.

Open the solution in Visual Studio or your preferred IDE.

Build the solution to restore packages and build the project.

Run the project using IIS Express or your preferred hosting environment.



Usage:

AddGuest:

Endpoint: /api/guests

Method: POST

Body:

{

  "title": 0,
  
  "firstName": "John",
  
  "lastName": "Doe",
  
  "birthDate": "1980-01-01",
  
  "email": "abhi.upa@example.com",
  
  "phoneNumbers": ["1234567890", "9876543210"]
  
}

Adds a new guest to the database.



AddPhone:

Endpoint: /api/guests/{guestId}/phones

Method: POST

Body:
{
  "guestId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "phoneNumber": "5555555555"
}
  
Adds a new phone number for the guest with the specified guestId.

GetGuestById

Endpoint: /api/guests/{guestId}

Method: GET

Retrieves guest information by guestId.


GetAllGuests

Endpoint: /api/guests

Method: GET

Retrieves information for all guests.



Validation

The AddGuest endpoint requires name and at least one phone number.

The AddPhone endpoint ensures that phone numbers are not duplicated.



Logging

Basic logging is implemented using NLog for each endpoint.


Assumptions

The project uses an in-memory database for simplicity. Production applications should use a persistent database.


Challenges Faced

Challenge 1: Implementing validation for the AddGuest and AddPhone endpoints required careful consideration of data requirements and error handling.

Challenge 2: Integrating logging required configuring the logger and ensuring it captured relevant information for each endpoint.



Future Improvements

Token Authentication: Implement token authentication to secure the API endpoints and restrict access to authorized users only.

Additional Test Cases: Expand the test suite to cover more scenarios and ensure robust testing of the API functionality.

