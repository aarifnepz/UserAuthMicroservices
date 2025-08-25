# User and Authorization Microservices

This repository contains a microservices-based user and authorization system built with **.NET 8**. The project demonstrates inter-service communication using `HttpClient` and follows key principles of a microservices architecture.

## Architecture

The system is composed of two independent services:

### 1. UserService

Manages all user-related operations.

- **Endpoints**:
  - `POST /api/users`: Creates a new user.
  - `GET /api/users/{id}`: Retrieves user details, with results cached for 1 minute.
  - `GET /api/users`: Lists all users.
- **Technologies**: .NET 8 Web API, Entity Framework Core (In-Memory), and `IMemoryCache`.

### 2. RoleService

Manages user roles and authorization.

- **Endpoints**:
  - `POST /api/roles/assign`: Assigns a role to a user.
  - `GET /api/roles/check`: Checks if a user has a specific role.
- **Technologies**: .NET 8 Web API, Entity Framework Core (In-Memory), and `HttpClient` for service-to-service communication.

## Getting Started

Follow these steps to set up and run the project locally.

### Prerequisites

- .NET 8 SDK

### Running the Services

1. Clone this repository: `git clone https://github.com/aarifnepz/UserAuthMicroservices.git`

   `cd UserAuthMicroservices`

2. Navigate to the `UserService` directory in one terminal and run the application. It is configured to run on port 5000.
   ```bash
   cd UserService
   dotnet run
   ```
3. In a separate terminal, navigate to the `RoleService` directory and run the application.

   ```bash
   cd ../RoleService
   dotnet run
   ```

4. The services will start on different ports. The URLs for testing can be found in the terminal output.

## Key Concepts Demonstrated

- **Microservices Architecture**: The project is split into two independent services with distinct responsibilities.
- **Service-to-Service Communication**: The `RoleService` uses `HttpClient` to call the `UserService` to validate user information.
- **In-Memory Caching**: The `UserService` uses `IMemoryCache` to improve the performance of repeated requests for the same user.
- **Data Isolation**: Each service manages its own data store (in-memory databases).
