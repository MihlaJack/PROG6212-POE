-- RaceDay Database Seed Data
-- This script adds sample data for testing purposes
-- Run this AFTER the initial database creation

-- Add sample Organisers
INSERT INTO [Users] (FirstName, LastName, Email, PasswordHash, Role, CreatedAt)
VALUES 
('Thabo', 'Mokoena', 'thabo@mokoena.com', '$2a$11$examplehash123456789012345678901234567890123456789012345678', 'Organiser', GETDATE()),
('Sarah', 'van der Merwe', 'sarah@vandermerwe.com', '$2a$11$examplehash123456789012345678901234567890123456789012345678', 'Organiser', GETDATE());

-- Add sample Participants
INSERT INTO [Users] (FirstName, LastName, Email, PasswordHash, Role, CreatedAt)
VALUES 
('John', 'Smith', 'john@smith.com', '$2a$11$examplehash123456789012345678901234567890123456789012345678', 'Participant', GETDATE()),
('Emily', 'Johnson', 'emily@johnson.com', '$2a$11$examplehash123456789012345678901234567890123456789012345678', 'Participant', GETDATE());

-- Add sample Events
INSERT INTO [Events] (Name, Description, EventDate, Location, Distance, EventType, OrganiserId, CreatedAt)
VALUES 
('Cape Town Marathon', 'The premier marathon event in South Africa', '2026-12-01T08:00:00', 'Cape Town', 42.195, 'Run', 1, GETDATE()),
('Two Oceans Ultra Marathon', 'Challenging ultra marathon across two oceans', '2026-12-15T06:00:00', 'Cape Town', 56.2, 'Run', 1, GETDATE()),
('Pretoria Cycle Tour', 'Annual cycling event through Pretoria', '2027-01-10T07:00:00', 'Pretoria', 100, 'Cycle', 2, GETDATE());

-- Add sample Categories for first event
INSERT INTO [Categories] (Name, Description, EventId)
VALUES 
('Under 20', 'Runners aged under 20', 1),
('20-39', 'Runners aged 20-39', 1),
('40-49', 'Runners aged 40-49', 1),
('50+', 'Runners aged 50 and above', 1);

-- Add sample Categories for second event
INSERT INTO [Categories] (Name, Description, EventId)
VALUES 
('Standard', 'Standard 56km distance', 2),
('Advanced', 'Advanced runners only', 2),
('Team', 'Team participation', 2);

-- Add sample Categories for third event
INSERT INTO [Categories] (Name, Description, EventId)
VALUES 
('100km', 'Full 100km route', 3),
('50km', 'Half distance 50km', 3),
('30km', 'Short distance 30km', 3);

-- Add sample Enrolments
INSERT INTO [Enrolments] (UserId, EventId, CategoryId, EnrolledAt)
VALUES 
(3, 1, 1, GETDATE()),
(4, 1, 2, GETDATE()),
(3, 2, 1, GETDATE()),
(4, 3, 1, GETDATE());
