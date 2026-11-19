# Part 3 Status Report

## Completed Work ✅

### 1. Azure Blob Storage Integration
- ✅ Added `Azure.Storage.Blobs` NuGet package to API
- ✅ Created `IBlobStorageService` interface
- ✅ Implemented `BlobStorageService` for file operations
- ✅ Created `FileUploadController` with endpoints for:
  - Event banner uploads (`/api/files/upload/event-banner`)
  - Profile picture uploads (`/api/files/upload/profile-picture`)
- ✅ Updated `Event` model with `BannerImageUrl` property
- ✅ Updated `User` model with `ProfilePictureUrl` property
- ✅ Updated all DTOs to include image URL fields
- ✅ Updated `EventsController` to handle banner images
- ✅ Updated `UsersController` to handle profile pictures
- ✅ Registered blob storage service in `Program.cs`
- ✅ Added Azure Storage connection string to `appsettings.json`

### 2. MVC Project Setup
- ✅ Created `RaceDayWeb` ASP.NET Core MVC project
- ✅ Added project to solution (`RaceDayAPI.sln`)
- ✅ Created `Program.cs` with:
  - Session management
  - API service registration
  - CORS configuration
  - Routing setup
- ✅ Created `ApiService` for API communication with methods for:
  - Authentication (login, register)
  - Event management (CRUD)
  - Category management
  - Enrolments
  - Results
  - User profile
  - File uploads
- ✅ Created API models (DTOs) in `Models/ApiModels.cs`
- ✅ Configured API base URL in `appsettings.json`

### 3. MVC Controllers
- ✅ `HomeController` - Home and About pages
- ✅ `AuthController` - Login, Register, Logout with session management
- ✅ `OrganiserController` with actions:
  - Dashboard (stats and summary)
  - Events (list, create, edit, delete)
  - Categories (list, create)
  - Enrolments (view participants)
  - Results (capture finish times)
- ✅ `ParticipantController` with actions:
  - Index (browse events)
  - Details (event info with enrolment)
  - Enrol (enrol in event)
  - MyEnrolments (view enrolments)
  - MyResults (view race history)
  - Profile (manage profile with picture upload)

### 4. UI/UX
- ✅ Created `_Layout.cshtml` with:
  - Bootstrap 5 integration
  - Sporting theme with custom colors (orange, blue, dark)
  - Responsive navigation bar
  - Role-based menu items
  - User authentication status display
  - Footer with links
- ✅ Created `_ViewImports.cshtml`
- ✅ Created `_ViewStart.cshtml`
- ✅ Custom CSS for:
  - Hero sections
  - Status badges (confirmed/pending)
  - Event banners
  - Profile pictures
  - Card styling

### 5. Docker Configuration
- ✅ Created `Dockerfile` for MVC application
- ✅ Multi-stage build process
- ✅ Configured exposed ports
- ✅ Root `Dockerfile` for both API and MVC

### 6. CI/CD
- ✅ Updated `.github/workflows/ci-cd.yml` to:
  - Build API project
  - Build MVC project
  - Publish both artifacts

### 7. Documentation
- ✅ Created `PART3_COMPLETION_GUIDE.md` with detailed instructions
- ✅ Created `PART3_STATUS.md` (this file)

## Remaining Work 📋

### 1. MVC Views (CRITICAL - NOT STARTED)
The following Razor view files need to be created:

**Home Views (2 files):**
- `Views/Home/Index.cshtml` - Landing page with hero section
- `Views/Home/About.cshtml` - About page

**Auth Views (2 files):**
- `Views/Auth/Login.cshtml` - Login form
- `Views/Auth/Register.cshtml` - Registration form with role selection

**Organiser Views (8 files):**
- `Views/Organiser/Dashboard.cshtml` - Dashboard with event stats, enrolment counts, upcoming events
- `Views/Organiser/Events.cshtml` - List of events with edit/delete actions
- `Views/Organiser/CreateEvent.cshtml` - Form with all fields and image upload
- `Views/Organiser/EditEvent.cshtml` - Form pre-filled with event data
- `Views/Organiser/Categories.cshtml` - List of categories for an event
- `Views/Organiser/CreateCategory.cshtml` - Category creation form
- `Views/Organiser/Enrolments.cshtml` - Table of enrolled participants
- `Views/Organiser/Results.cshtml` - Form to capture finish times

**Participant Views (5 files):**
- `Views/Participant/Index.cshtml` - Grid of events with filters
- `Views/Participant/Details.cshtml` - Event details, categories, enrolment button
- `Views/Participant/MyEnrolments.cshtml` - List of enrolled events with status
- `Views/Participant/MyResults.cshtml` - Table of race results with positions
- `Views/Participant/Profile.cshtml` - Profile form with picture upload

**Total: 17 view files needed**

### 2. API Service Authentication
The `ApiService` needs improvement:
- Better session cookie handling
- Proper error handling with user-friendly messages
- Retry logic for failed requests
- File upload form data formatting

### 3. Database Migration
A migration is needed to add new columns:
```bash
dotnet ef migrations add AddImageUrls --project RaceDayAPI/RaceDayAPI
dotnet ef database update --project RaceDayAPI/RaceDayAPI
```

### 4. Azure Storage Setup
If using Azure:
1. Create Azure Storage Account
2. Get connection string
3. Update `appsettings.json`
4. Create containers: "event-banners", "profile-pictures"

Alternative: Use local file storage for development

### 5. Testing
- Test all MVC views render correctly
- Test file upload functionality
- Test Docker containerization
- Verify end-to-end workflows
- Test role-based access

### 6. Git Commits
Need 20+ meaningful commits for Part 3 (currently at 1 commit for Part 3 work)

Suggested commits:
1. Add Azure Blob Storage package
2. Create blob storage service
3. Create file upload controller
4. Update Event model with banner URL
5. Update User model with profile picture URL
6. Update DTOs with image fields
7. Register blob storage service
8. Create MVC project structure
9. Create API service
10. Create API models
11. Create Home controller
12. Create Auth controller
13. Create Organiser controller
14. Create Participant controller
15. Create shared layout
16. Create Home views
17. Create Auth views
18. Create Organiser views
19. Create Participant views
20. Add Dockerfile
21. Update CI/CD workflow
22. Add Part 3 documentation

### 7. Documentation Updates
Update `README.md`:
- Add MVC application section
- Add Docker instructions
- Add Azure Storage setup
- Update video link
- Update CI/CD screenshot
- Add Part 3 features

## Estimated Completion Time

**MVC Views:** 6-8 hours (17 views)
**Testing:** 2-3 hours
**Azure Setup:** 1-2 hours (or 30 min for local storage)
**Documentation:** 1 hour
**Git Commits:** 30 minutes

**Total: 10-14 hours**

## Priority Order

1. **HIGH PRIORITY:** Create MVC views (without this, the app won't work)
2. **HIGH PRIORITY:** Create database migration
3. **MEDIUM PRIORITY:** Test the application
4. **MEDIUM PRIORITY:** Azure Storage setup (can use placeholders initially)
5. **LOW PRIORITY:** Docker testing
6. **LOW PRIORITY:** Documentation updates

## Next Steps

1. Create the 17 MVC view files (refer to `PART3_COMPLETION_GUIDE.md` for details)
2. Run database migration to add image URL columns
3. Test the application end-to-end
4. Make frequent commits as you complete each view
5. Update documentation
6. Create Part 3 video
7. Submit

## Repository Status

- **Branch:** main
- **Commits:** 21 total (20 from Part 2, 1 from Part 3)
- **Current Commit:** "Add Azure Blob Storage integration to API"
- **Repository:** https://github.com/MihlaJack/PROG6212-PART-2

## Notes

- The API and MVC projects are now in the same solution
- The MVC app communicates with the API via HTTP
- Session management is implemented separately in both apps
- CORS is configured to allow cross-origin requests
- The sporting theme is applied via custom CSS in the layout
- Bootstrap 5 provides responsive design
- Image uploads go through the API to Azure Blob Storage
