# Cinema Distributed System

This repository contains a sample distributed system for a cinema management and reservation system. The solution demonstrates how to run distributed systems using Docker Compose or Aspire, making it easy for new developers to onboard and set up the system locally.

## Prerequisites

- Docker
- Docker Compose
- Aspire CLI (if running with Aspire)

## Getting Started

### Clone the Repository

```sh
git clone https://github.com/your-username/cinema-distributed-system.git
cd cinema-distributed-system
```

### Set Up Environment Variables

Copy the `.env.template` file to `.env` and fill in the required values:

```sh
cp .env.template .env
```

---

## Running the Project

### Option 1: Using Docker Compose

Use Docker Compose to build and run the services:

```sh
docker compose up --build
```

This command will start the following services:

- **management**: Cinema management service
- **reservation**: Cinema reservation service
- **sb-emulator**: Azure Service Bus emulator
- **sqledge**: Azure SQL Edge (used internally by the Service Bus Emulator)
- **aspire-dashboard**: OpenTelemetry dashboard
- **cosmos-db**: Azure Cosmos DB emulator
- **redis**: Redis cache

#### Accessing the Services

- **Management Service**: http://localhost:43301
- **Reservation Service**: http://localhost:43302
- **Aspire Dashboard**: http://localhost:18888

#### Stopping the Services

To stop the services, run:

```sh
docker compose down
```

---

### Option 2: Running the AppHost Project

Aspire simplifies the setup and management of dependencies, but instead of using the `aspire run` command, you need to run the **AppHost** project directly. This project orchestrates the distributed system and starts all required services.

#### Run the AppHost Project

1. Open the solution in your IDE (e.g., Visual Studio or Visual Studio Code).
2. Set the **AppHost** project as the startup project.
3. Run the project.

The AppHost project will automatically start all required services and dependencies, including:

- Redis
- Azure Service Bus emulator
- Azure Cosmos DB emulator
- Management and Reservation services

#### Accessing the Services

- **Management Service**: http://localhost:5153
- **Reservation Service**: http://localhost:5020
- **Aspire Dashboard**: http://localhost:15105

#### Stopping the Services

To stop the services, stop the AppHost project in your IDE.

---

## Calling the Management Service

To call the Management service and publish a new movie, you can use the following HTTP request:

```http
POST http://localhost:43301/movies/publish
Accept: application/json
Content-Type: application/json

{
  "name": "Lion King 2",
  "description": "The lion king movie",
  "genre": "Adventure",
  "producer": "Dreams",
  "releaseDate": "2024-03-15"
}
```

You can use tools like `curl`, Postman, or any HTTP client to send this request.

---

## Project Structure

- `Cinema.Management/`: Management service for handling cinema operations.
- `Cinema.Reservation/`: Reservation service for handling movie reservations.
- `containers/service-bus/`: Configuration for the Azure Service Bus emulator.
- `compose.yaml`: Docker Compose file to define and run multi-container Docker applications.

---

## Onboarding New Developers

1. Clone the repository.
2. Set up environment variables by copying `.env.template` to `.env`.
3. Choose one of the following options to run the project:
   - Use `docker compose up --build` to start all services.
   - Run the **AppHost** project to start all services with Aspire.
4. Access the services via the provided URLs.

This setup ensures that new developers can quickly get the distributed system up and running locally with minimal effort.

---

## License

This project is licensed under the MIT License.