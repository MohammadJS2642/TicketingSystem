# Ticketing System

To use this project, just download and run the api layer 

## Request

In the root directory of the project, the collection.json file for use in Postman was added. By adding that to the Postman, you can see the request.

## Authenticate
### user: admin
For login as admin, use auth/login, and in the body set 
```
{
  "email": "admin@gmail.com",
  "password": "Admin@123"
}
```
By that, you can use:   
PUT (/tickets/:id) \
GET (/gettickets) \
GET (/tickets/stats) \
GET (/tickets/:id) \

### user: Employee
For login as an employee, use auth/login and in the body set 
```
{
  "email": "employee@gmail.com",
  "password": "Employee@123"
}
```
By that, you can use:   
POST (/tickets) \
GET (/tickets/my) \

## Contributing

Pull requests are welcome. For major changes, please open an issue first
to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
