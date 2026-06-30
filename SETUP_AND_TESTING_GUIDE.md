# YogaSakhi AI - Setup & Testing Guide

## Prerequisites

- .NET 6.0 SDK or later
- SQL Server 2019+ (LocalDB or Full Version)
- Visual Studio 2022 or VS Code
- Node.js (optional, for frontend tools)

## Installation & Setup

### Step 1: Clone the Repository

```bash
git clone https://github.com/Harshitha1221/YogaSakhi.git
cd YogaSakhi
```

### Step 2: Create appsettings.json

Update `src/YogaSakhi.API/appsettings.json` with your database connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=YogaSakhiDb;Trusted_Connection=true;"
  },
  "AuthSettings": {
    "JwtSecret": "YourSuperSecretKeyThatIsAtLeast32CharactersLongForJWTTokenGeneration123",
    "JwtIssuer": "YogaSakhi",
    "JwtAudience": "YogaSakhiUsers",
    "JwtExpirationMinutes": 1440
  }
}
```

### Step 3: Install NuGet Packages

```bash
cd src/YogaSakhi.API
dotnet restore
```

### Step 4: Create Database & Run Migrations

```bash
# Install Entity Framework CLI
dotnet tool install --global dotnet-ef

# Create database
dotnet ef database update
```

### Step 5: Run the Application

```bash
dotnet run
```

The application will start at:
- **Web**: https://localhost:5001
- **API Docs (Swagger)**: https://localhost:5001/swagger

## Testing the Application

### Option 1: Using Swagger UI (Recommended)

1. Navigate to: `https://localhost:5001/swagger`
2. Expand the API endpoints
3. Click "Try it out" to test endpoints

### Option 2: Using Postman

#### 1. Register a New User

**POST** `/api/auth/register`

```json
{
  "email": "harshitha@example.com",
  "password": "Password123!",
  "fullName": "Harshitha Vishwanath",
  "age": 28,
  "phoneNumber": "9876543210"
}
```

**Response:**
```json
{
  "success": true,
  "message": "User registered successfully",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "userId": "550e8400-e29b-41d4-a716-446655440000",
    "fullName": "Harshitha Vishwanath",
    "email": "harshitha@example.com",
    "role": "User"
  }
}
```

#### 2. Login

**POST** `/api/auth/login`

```json
{
  "email": "harshitha@example.com",
  "password": "Password123!"
}
```

#### 3. Get All Health Conditions

**GET** `/api/health/conditions`

**Response:**
```json
{
  "healthConditions": [
    {
      "id": 1,
      "name": "PCOS Management",
      "description": "Polycystic Ovary Syndrome - Hormonal & Metabolic Health",
      "iconUrl": "/images/pcos.png",
      "exerciseCount": 3,
      "symptomCount": 5
    }
  ]
}
```

#### 4. Get Health Condition Details

**GET** `/api/health/conditions/1`

**Response:**
```json
{
  "id": 1,
  "name": "PCOS Management",
  "description": "...",
  "overview": "...",
  "symptoms": [...],
  "exercises": [...],
  "dietPlans": [...],
  "treatments": [...]
}
```

#### 5. Create Assessment

**POST** `/api/assessment/create`

```json
{
  "userId": 1,
  "healthConditionId": 1,
  "assessmentScore": 7,
  "assessmentDetails": "Irregular periods, weight gain, acne",
  "symptoms": "irregular periods, weight gain"
}
```

#### 6. Get User Assessments

**GET** `/api/assessment/user/1`

#### 7. Record Progress

**POST** `/api/assessment/record-progress`

```json
{
  "userId": 1,
  "healthConditionId": 1,
  "progressScore": 8,
  "notes": "Feeling better after following exercises",
  "symptomImprovement": "Periods becoming regular",
  "exercisesCompleted": 5,
  "dietComplianceScore": 85
}
```

#### 8. Analyze Symptoms

**POST** `/api/ai/analyze-symptoms`

```json
{
  "userId": 1,
  "healthConditionId": 1,
  "symptoms": "irregular periods, weight gain, fatigue"
}
```

#### 9. Get AI Recommendation

**GET** `/api/ai/recommendation/1/1`

## Testing via Web UI

### 1. Homepage
Visit: `https://localhost:5001`
- See all health programs
- Register or Login buttons

### 2. Health Programs
Visit: `https://localhost:5001/Health`
- Browse all conditions
- Click to view details

### 3. Register
Visit: `https://localhost:5001/Auth/Register`
- Fill registration form
- Submit
- Get redirected to health programs page

### 4. Login
Visit: `https://localhost:5001/Auth/Login`
- Enter credentials
- Get JWT token stored in localStorage

## Architecture Testing

### CQRS Pattern Testing

1. **Commands** (Write operations):
   - `RegisterUserCommand` → API registers users
   - `LoginUserCommand` → API authenticates users
   - `CreateAssessmentCommand` → API creates assessments
   - `RecordProgressCommand` → API records progress

2. **Queries** (Read operations):
   - `GetHealthConditionQuery` → Fetches single condition
   - `GetAllHealthConditionsQuery` → Fetches all conditions
   - `GetUserAssessmentQuery` → Fetches user assessments
   - `GetAIRecommendationQuery` → Gets AI recommendations

### JWT Authentication Testing

1. Register user → Get JWT token
2. Copy token from response
3. In Postman, set Authorization header:
   ```
   Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
   ```
4. Make authenticated API calls

### AI Service Testing

1. Create assessment with symptoms
2. System generates AI recommendation
3. Verify recommendation contains:
   - Exercise plan
   - Diet recommendations
   - Lifestyle advice
   - Pregnancy considerations (if applicable)

## Database Verification

```sql
-- Check if tables were created
SELECT * FROM [YogaSakhiDb].[dbo].[Users];
SELECT * FROM [YogaSakhiDb].[dbo].[HealthConditions];
SELECT * FROM [YogaSakhiDb].[dbo].[Symptoms];
SELECT * FROM [YogaSakhiDb].[dbo].[Exercises];
SELECT * FROM [YogaSakhiDb].[dbo].[DietPlans];
```

## Troubleshooting

### Issue: Database Connection Error

**Solution:**
```bash
# Verify SQL Server is running
# Update connection string in appsettings.json
# Run migrations again
dotnet ef database drop
dotnet ef database update
```

### Issue: JWT Token Expired

**Solution:**
- Register/Login again to get new token
- Token expiration is set to 1440 minutes (24 hours)

### Issue: Port Already in Use

**Solution:**
```bash
# Use different port
dotnet run --urls=https://localhost:5002
```

## Performance Notes

- Database queries are optimized with indexes on Email field
- JWT tokens reduce database queries for authentication
- MediatR pattern allows easy addition of logging/caching
- Repository pattern allows easy unit testing

## Next Steps

1. Deploy to Azure App Service
2. Set up Azure SQL Database
3. Configure CI/CD pipeline with Azure DevOps
4. Add unit tests
5. Implement caching layer

---

**Built with ❤️ for Women's Wellness**
