-- Migration to add image URL columns
-- Run this after the initial database creation

ALTER TABLE [Users]
ADD [ProfilePictureUrl] NVARCHAR(MAX) NULL;

ALTER TABLE [Events]
ADD [BannerImageUrl] NVARCHAR(MAX) NULL;
