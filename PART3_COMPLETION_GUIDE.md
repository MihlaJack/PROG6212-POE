# Part 3 Completion Guide

## What Has Been Completed

### 1. Azure Blob Storage Integration ✅
- Added `Azure.Storage.Blobs` package to API
- Created `IBlobStorageService` and `BlobStorageService`
- Created `FileUploadController` for image uploads
- Updated Event model to include `BannerImageUrl`
- Updated User model to include `ProfilePictureUrl`
- Updated DTOs to include image URL fields
- Updated controllers to handle image URLs
- Configured Azure Storage connection string in appsettings.json

### 2. MVC Project Structure ✅
- Created RaceDayWeb MVC project
- Added project to solution
- Created Program.cs with session management
- Created ApiService for API communication
- Created API models (DTOs)
- Created HomeController, AuthController, OrganiserController, ParticipantController
- Created shared layout with Bootstrap 5 and sporting theme
- Implemented session-based authentication
- Role-based navigation in layout

### 3. Docker Configuration ✅
- Created Dockerfile for MVC application
- Configured multi-stage build process
- Set up proper exposed ports
- Updated CI/CD workflow to build both API and MVC projects

## What Still Needs to Be Completed

### 1. MVC Views (REQUIRED)
The following view files need to be created:

**Home Views:**
- `Views/Home/Index.cshtml` - Landing page
- `Views/Home/About.cshtml` - About page

**Auth Views:**
- `Views/Auth/Login.cshtml` - Login form
- `Views/Auth/Register.cshtml` - Registration form

**Organiser Views:**
- `Views/Organiser/Dashboard.cshtml` - Organiser dashboard with stats
- `Views/Organiser/Events.cshtml` - List of events
- `Views/Organiser/CreateEvent.cshtml` - Create event form with image upload
- `Views/Organiser/EditEvent.cshtml` - Edit event form with image upload
- `Views/Organiser/Categories.cshtml` - Category management
- `Views/Organiser/CreateCategory.cshtml` - Create category form
- `Views/Organiser/Enrolments.cshtml` - View event enrolments
- `Views/Organiser/Results.cshtml` - Results entry page

**Participant Views:**
- `Views/Participant/Index.cshtml` - Browse events with filtering
- `Views/Participant/Details.cshtml` - Event details with enrolment
- `Views/Participant/MyEnrolments.cshtml` - My enrolments
- `Views/Participant/MyResults.cshtml` - My race results
- `Views/Participant/Profile.cshtml` - Profile management with picture upload

### 2. API Client Improvements
The ApiService needs to properly handle:
- Session cookie management for authentication
- Error handling and retry logic
- File upload with proper form data

### 3. Database Migration
A new migration is needed to add the BannerImageUrl and ProfilePictureUrl columns:
```bash
dotnet ef migrations add AddImageUrls --project RaceDayAPI/RaceDayAPI
dotnet ef database update --project RaceDayAPI/RaceDayAPI
```

### 4. Azure Storage Setup
You need to:
1. Create an Azure Storage Account
2. Get the connection string
3. Update the connection string in appsettings.json
4. Create containers: "event-banners" and "profile-pictures"

### 5. Testing
- Test all MVC views
- Test image upload functionality
- Test Docker containerization
- Verify CI/CD pipeline builds both projects

### 6. Git Commits
Make at least 20 meaningful commits for Part 3:
1. Add MVC project to solution
2. Create MVC controllers
3. Implement API service
4. Add Azure Blob Storage to API
5. Create file upload controller
6. Update models with image URLs
7. Create shared layout with Bootstrap
8. Create Home views
9. Create Auth views
10. Create Organiser dashboard
11. Create Organiser event management views
12. Create Organiser category management
13. Create Organiser enrolments view
14. Create Organiser results view
15. Create Participant event views
16. Create Participant enrolment views
17. Create Participant results view
18. Create Participant profile view
19. Add Dockerfile
20. Update CI/CD workflow
21. Add database migration for image URLs
22. Update documentation

### 7. Documentation Updates
Update README.md to include:
- MVC application setup instructions
- Docker run command
- Azure Storage setup instructions
- Part 3 features description
- Updated video link
- Updated CI/CD screenshot

## Docker Run Command

```bash
docker build -t raceday-web ./RaceDayWeb
docker run -p 8080:80 -e ApiBaseUrl=https://your-api-url raceday-web
```

## Azure Storage Connection String Format

```
DefaultEndpointsProtocol=https;AccountName=<your-account-name>;AccountKey=<your-account-key>;EndpointSuffix=core.windows.net
```

## Important Notes

1. **Azure Storage is optional for development** - You can use local file storage initially and switch to Azure later
2. **The API must be running** for the MVC app to work
3. **CORS must be configured** to allow the MVC app to communicate with the API
4. **Session management** must work across both applications (consider using a shared session store or JWT tokens)

## Estimated Time to Complete

- MVC Views: 4-6 hours
- Testing: 2-3 hours
- Azure Setup: 1-2 hours
- Documentation: 1 hour
- Git commits: 30 minutes

Total: ~8-12 hours

## Suggested Approach

1. Start with creating the basic views (Home, Auth)
2. Implement Organiser views first (more complex)
3. Then implement Participant views
4. Test each view as you create it
5. Add Azure Storage last (can use placeholder images initially)
6. Update documentation throughout
7. Make frequent commits as you complete each view
