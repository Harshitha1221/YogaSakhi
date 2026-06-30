# YogaSakhiAI - Women's Health & Wellness Platform

## Overview
YogaSakhiAI is an intelligent AI-powered health assistant designed specifically for women. It provides personalized guidance for PCOS, Thyroid disorders, Core Strengthening, Pregnancy Yoga, and Diastasis Recti recovery.

## Technology Stack
- **Framework**: ASP.NET Core 6.0
- **Architecture**: CQRS with MediatR
- **Authentication**: JWT (JSON Web Tokens)
- **ORM**: Entity Framework Core
- **Database**: SQL Server
- **Design Patterns**: Repository, Dependency Injection, Mediator
- **CI/CD**: Azure DevOps
- **Cloud**: Azure App Service

## Project Structure
```
YogaSakhi/
├── src/
│   ├── YogaSakhi.Domain/              # Domain models
│   ├── YogaSakhi.Application/         # CQRS Commands & Queries
│   ├── YogaSakhi.Infrastructure/      # Data access, Auth, Services
│   ├── YogaSakhi.API/                 # Web API
│   └── YogaSakhi.Web/                 # MVC UI
├── tests/
│   ├── YogaSakhi.Tests.Unit/
│   └── YogaSakhi.Tests.Integration/
├── docs/
│   ├── Architecture.md
│   ├── API-Documentation.md
│   └── Database-Schema.md
├── azure-pipelines.yml
└── README.md
```

## Key Features
✅ CQRS Pattern with MediatR  
✅ JWT Authentication & Authorization  
✅ AI-Powered Health Recommendations  
✅ Responsive Web UI  
✅ RESTful API  
✅ Unit & Integration Tests  
✅ Azure DevOps CI/CD Pipeline  
✅ Database Migrations  

## Getting Started

### Prerequisites
- .NET 6.0 SDK
- SQL Server 2019+
- Visual Studio 2022

### Setup
1. Clone the repository
2. Update connection string in `appsettings.json`
3. Run migrations: `dotnet ef database update`
4. Run the application

## Features by Condition

### PCOS (Polycystic Ovary Syndrome)
- Personalized diet plans focused on insulin resistance
- Specific exercises for hormonal balance
- Symptom tracking
- Dietary recommendations (low GI foods)

### Thyroid Management
- Exercise routines for thyroid health
- Iodine-rich food recommendations
- Energy level monitoring
- Metabolism tracking

### Core Strengthening
- Progressive core exercises
- Posture correction guides
- Stability training
- Video demonstrations

### Pregnancy Yoga
- Trimester-specific poses
- Safe exercise modifications
- Prenatal breathing techniques
- Labor preparation exercises

### Diastasis Recti Recovery
- Targeted abdominal exercises
- Safe movement patterns
- Core restoration program
- Post-natal recovery tracking

## API Documentation
See `docs/API-Documentation.md` for detailed API endpoints.

## Authentication
The application uses JWT (JSON Web Tokens) for secure authentication. Users can:
- Register with email and password
- Login to receive JWT token
- Use token for API requests
- Refresh token when expired

## CQRS Architecture

### Commands (Write Operations)
- `RegisterUserCommand`
- `LoginUserCommand`
- `CreateAssessmentCommand`
- `RecordProgressCommand`

### Queries (Read Operations)
- `GetHealthConditionQuery`
- `GetAllHealthConditionsQuery`
- `GetUserAssessmentQuery`
- `GetAIRecommendationQuery`
- `AnalyzeSymptomsQuery`

## Deployment

### Azure Deployment Steps
1. Create Azure SQL Database
2. Create Azure App Service
3. Configure connection strings
4. Set up Azure DevOps pipeline
5. Deploy using CI/CD

See `docs/Deployment.md` for detailed instructions.

## Testing

```bash
# Run unit tests
dotnet test tests/YogaSakhi.Tests.Unit/

# Run integration tests
dotnet test tests/YogaSakhi.Tests.Integration/
```

## Contributing
Please follow the CQRS pattern when adding new features:
1. Create Command/Query in Application layer
2. Create Handler in Application layer
3. Implement in Infrastructure layer
4. Add API endpoint in API layer

## License
MIT License

## Support
For issues and feature requests, please create a GitHub issue.

---

**Built with ❤️ for Women's Wellness**
