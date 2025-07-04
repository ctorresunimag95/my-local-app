# Cinema Distributed System

This repository contains a sample distributed system for a cinema management and reservation system. The solution demonstrates how to run distributed systems using Docker Compose or Aspire, making it easy for new developers to onboard and set up the system locally.

## Prerequisites

- Docker
- Docker Compose
- Aspire CLI (if running with Aspire)

## Getting Started

### Clone the Repository

```sh
git clone https://github.com/ctorresunimag95/my-local-app.git
cd my-local-app
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
- **gateway**: API Gateway (YARP) — [http://localhost:5265](http://localhost:5265)
- **web**: Angular front-end — [http://localhost:54163](http://localhost:54163)
- **sb-emulator**: Azure Service Bus emulator
- **sqledge**: Azure SQL Edge (used internally by the Service Bus Emulator)
- **aspire-dashboard**: OpenTelemetry dashboard
- **cosmos-db**: Azure Cosmos DB emulator
- **redis**: Redis cache

#### Accessing the Services

- **Angular App**: [http://localhost:54163](http://localhost:54163)
- **Gateway (YARP)**: [http://localhost:5265](http://localhost:5265)
- **Management Service**: http://localhost:43301
- **Reservation Service**: http://localhost:43302
- **Aspire Dashboard**: http://localhost:18888

---

### Option 2: Running the AppHost Project

Aspire simplifies the setup and management of dependencies. To run the full distributed system, including the gateway and Angular app:

1. Open the solution in your IDE (e.g., Visual Studio or Visual Studio Code).
2. Set the **AppHost** project as the startup project.
3. Run the project.

The AppHost project will automatically start all required services and dependencies, including:

- Angular front-end (port 54163)
- API Gateway (YARP, port 5265)
- Management and Reservation services
- Redis
- Azure Service Bus emulator
- Azure Cosmos DB emulator

### OMDb API Key Requirement

The **Management** service uses the [OMDb API](https://www.omdbapi.com/) to fetch movie information.  
To enable the movie publishing feature, you need to obtain a free OMDb API key:

1. Go to [https://www.omdbapi.com/apikey.aspx](https://www.omdbapi.com/apikey.aspx) and request a free API key.
2. Once you receive your key, provide it to the application as follows:

- **Docker Compose:**  
  Add the key to your `.env` file or as an environment variable, for example:  
  `OMDB_API_KEY=your_api_key_here`

- **Aspire (AppHost):**  
  You can securely store the key using [.NET user secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets).  
  Run the following command in the `Cinema.AppHost` project directory:  
  ```sh
  dotnet user-secrets set "omdbApiKey" "your_api_key_here"
  ```

Without this key, the publisher functionality in the Management application will not be able to retrieve real time movie information.

#### Accessing the Services

- **Angular App**: [http://localhost:54163](http://localhost:54163)
- **Gateway (YARP)**: [http://localhost:5265](http://localhost:5265)
- **Management Service**: http://localhost:43301
- **Reservation Service**: http://localhost:43302
- **Aspire Dashboard**: http://localhost:18888

---

## Angular Application

The Angular front-end provides a user interface for both movie reservations and management.

### Screenshots

#### Reservation Screen

<!-- Add a screenshot of the reservation screen here -->
![Reservation Screen](screenshots/reservation.png)

#### Management - Publish Movie Screen

<!-- Add a screenshot of the management publish movie screen here -->
![Publish Movie Screen](screenshots/publish-movie.png)


### Stopping the Services

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
- `Cinema.Gateway/`: API Gateway using YARP.
- `Cinema.Web/`: Angular front-end application.
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