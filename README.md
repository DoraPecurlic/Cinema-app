# Cinema Booking Application

![React](https://img.shields.io/badge/React-18-61DAFB?logo=react&logoColor=black)
![C%23](https://img.shields.io/badge/C%23-512BD4?logo=csharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-8-512BD4?logo=dotnet&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?logo=postgresql&logoColor=white)

A full-stack cinema booking application for browsing movies, selecting projections and seats, reserving tickets and managing cinema content.

CineMoon provides separate workflows for customers and administrators. 
Customers can create an account, explore available movies, reserve seats and review their payment history. 
Administrators can manage movies, actors and projections through protected application routes.

## Features

### Customer Features

- User registration and JWT-based authentication
- Movie browsing with filtering, sorting and pagination
- Movie details, actors and user reviews
- Projection and seat selection
- Seat reservation and ticket creation
- Simulated payment workflow
- Email confirmation with reservation details
- Personal payment history

### Administrator Features

- Role-protected administration routes
- Add and update movies and actors
- Create and manage cinema projections
- Review recorded payments

## Technology Stack

| Area | Technology |
| --- | --- |
| Frontend | React 18, React Router, Bootstrap, Axios |
| Backend | C#, ASP.NET Core Web API, .NET 8 |
| Database | PostgreSQL, Npgsql |
| Authentication | JWT, BCrypt |
| Architecture | Controller, service and repository layers |
| Supporting libraries | Autofac, AutoMapper, MailKit |

## Architecture

The React client communicates with the ASP.NET Core REST API. Backend controllers delegate application logic to service classes, while repositories execute asynchronous PostgreSQL operations.

## Project Structure

```text
Cinema-app/
├── Backend/
│   ├── Cinema/
│   │   ├── Cinema.WebApi/          # API controllers and configuration
│   │   ├── Cinema.Service/         # Application logic
│   │   ├── Cinema.Repository/      # PostgreSQL data access
│   │   ├── Cinema.Model/           # Domain models
│   │   ├── Cinema.Mapper/          # AutoMapper profiles
│   │   └── DTO/                    # API request and response models
│   └── SQL scripts/                # Database schema and sample data
└── Frontend/
    └── cinema/                     # React application
```




