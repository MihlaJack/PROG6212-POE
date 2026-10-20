# Data Models

This folder contains all Entity Framework Core entity models.

## Models

- **User.cs**: Represents a user in the system (Organiser or Participant)
- **Event.cs**: Represents a running/walking/cycling event
- **Category.cs**: Represents event categories (age groups, distances)
- **Enrolment.cs**: Links participants to events with a selected category
- **Result.cs**: Captures finish times and positions for participants

## Relationships

- User has many Events (as Organiser)
- User has many Enrolments
- User has many Results
- Event has many Categories
- Event has many Enrolments
- Event has many Results
- Category has many Enrolments
