# RapidPay

## Overview
RapidPay is a payment processing API developed as part of a distillery code challenge. This project is built using .NET 9 and follows best practices for API development, including authentication, authorization, and HTTPS redirection.

## Features
- **Card creation**: Allows users to create a new card.
- **Card balance**: Allows users to check the balance of a card.
- **Payments**: Allows users to make payments using a card.
- **Universal Payment Fee**: Applies a universal fee to all payments.
- **Swagger Integration**: Provides API documentation and testing interface.
- **Authentication and Authorization**: Secures the API endpoints.

## Prerequisites
- .NET 9 SDK
- SQL Server Express
- Visual Studio 2022

## Getting Started

### Build and Run the Project
1. Open the solution in Visual Studio 2022.
2. Restore the NuGet packages.
3. Build the solution.
4. Run the project.

### Configuration
Ensure that the `appsettings.json` file is properly configured with your settings, including any necessary path base configuration.

### Database Setup
1. Configure the database settings in the `appsettings.json` file.
2. Update the database by running the following command `Update-database` in the Package Manager Console
3. Use the SQL script `CreateCardNumbers.sql` included in the solution to create the necessary database tables for storing card information.

### Usage
Once the project is running, you can access the Swagger UI at `https://localhost:5001/swagger` to explore and test the API endpoints.
To get a authorization token, use admin/admin as user/password. Given this is a prototype, user and password are hardcoded just to 
show JWT implementation.

## Troubleshooting
If you encounter the `System.InvalidOperationException` related to path base configuration, ensure that the path base is set correctly using `IApplicationBuilder.UsePathBase()`
before calling `app.Run()`.

## Next Improvements
Add Unit test module to ensure services and repository layers are working as expected.

## Contributing
Feel free to fork the repository and submit pull requests. For major changes, please open an issue first to discuss what you would like to change.

## License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
