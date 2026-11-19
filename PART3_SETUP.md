# Part 3 - MVC Application Setup Instructions

## Running the Complete Application

### Prerequisites
- .NET 8.0 SDK
- SQL Server LocalDB or SQL Server
- Docker Desktop (optional, for containerization)
- Azure Storage Account (optional, for file uploads)

### 1. Clone the Repository
```bash
git clone https://github.com/MihlaJack/PROG6212-POE.git
cd PROG6212-POE
```

### 2. Configure Database
Edit `RaceDayAPI/appsettings.json` to set your SQL Server connection string.

### 3. Configure Azure Storage (Optional)
Edit `RaceDayAPI/appsettings.json` to add your Azure Storage connection string.
Create containers "event-banners" and "profile-pictures" in your storage account.

### 4. Run Database Migrations
```bash
cd RaceDayAPI/RaceDayAPI
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 5. Run the API
```bash
cd RaceDayAPI/RaceDayAPI
dotnet run
```
API will run on https://localhost:5201

### 6. Run the MVC Application
```bash
cd RaceDayWeb
dotnet run
```
MVC will run on https://localhost:5202

## Testing the Application

### API Testing
1. Open Swagger UI: https://localhost:5201/swagger
2. Register a user (Organiser or Participant)
3. Login to get a session
4. Test API endpoints

### MVC Testing
1. Open the web app: https://localhost:5202
2. Register a new account
3. Login and test role-specific features
4. Test event creation, enrolment, results

## Docker Deployment

### Build and Run API
```bash
docker build -f RaceDayAPI/Dockerfile -t raceday-api .
docker run -p 5201:80 -e "ConnectionStrings__DefaultConnection=Server=host.docker.internal;Database=RaceDayDB;Trusted_Connection=True;MultipleActiveResultSets=true" raceday-api
```

### Build and Run MVC
```bash
docker build -f RaceDayWeb/Dockerfile -t raceday-web .
docker run -p 5202:80 -e "ApiBaseUrl=http://host.docker.internal:5201" raceday-web
```

## Azure Blob Storage Setup

1. Create an Azure Storage Account
2. Get the connection string
3. Create containers: "event-banners" and "profile-pictures"
4. Set public access to Blob level
5. Update appsettings.json with connection string

## Application Features

### Organiser Features
- Dashboard with event statistics
- Create, edit, delete events
- Upload event banner images
- Manage event categories
- View participant enrolments
- Capture race results

### Participant Features
- Browse upcoming events
- View event details
- Enrol in events with category selection
- Track personal enrolments
- View race results
- Manage profile with picture upload

## Troubleshooting

### API Connection Issues
- Ensure the API is running on the correct port
- Check ApiBaseUrl in RaceDayWeb/appsettings.json
- Verify CORS is enabled in API

### Database Issues
- Run migrations if database doesn't exist
- Check connection string in appsettings.json
- Ensure SQL Server is running

### File Upload Issues
- Verify Azure Storage connection string
- Check container names match
- Ensure public access is enabled

## CI/CD Pipeline

The project includes GitHub Actions CI/CD that:
- Builds both API and MVC projects
- Runs unit tests
- Publishes build artifacts

## Video Presentation

A video demonstration will be provided showing:
- Complete application setup
- API functionality
- MVC interface
- Role-based access
- File uploads
- Docker deployment
