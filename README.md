# Introduction
This application is used to retrieve basic user information of GitHub user accounts for a given list of usernames.

# Usage
Checkout and run the solution in Visual Studio 2022. The application will run on the following url `https://localhost:44338`

# Testing
Test the `retrieveUsers` endpoint from postman by providing the following information:

**Endpoint**

`POST https://localhost:44338/Users/retrieveUsers`

**Request Body**

```
[
    "sharmilabogireddy",
    "SharmilaBogireddy",
    "azure",
    "ddddddddffffffffffffffff333"
]
```
