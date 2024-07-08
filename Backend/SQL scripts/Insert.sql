INSERT INTO "Role" ("RoleName") VALUES ('Admin'), ('User');

INSERT INTO "User" ("Email", "Password", "FirstName", "LastName", "RoleId", "IsActive") 
VALUES 
('admin@example.com', 'password123', 'Admin', 'User', (SELECT "Id" FROM "Role" WHERE "RoleName" = 'Admin'), TRUE),
('user@example.com', 'password123', 'Regular', 'User', (SELECT "Id" FROM "Role" WHERE "RoleName" = 'User'), TRUE);

INSERT INTO "Genre" ("Name")
VALUES 
('Action'),
('Drama');

INSERT INTO "Language" ("Name")
VALUES 
('English'),
('French');

INSERT INTO "Movie" ("Title", "Description", "Duration", "LanguageId", "CoverUrl", "TrailerUrl", "AdminId", "GenreId", "IsActive")
VALUES 
('Example Movie', 'This is an example description.', 120, 
(SELECT "Id" FROM "Language" WHERE "Name" = 'English'), 
'http://example.com/cover.jpg', 'http://example.com/trailer.mp4', 
(SELECT "Id" FROM "User" WHERE "Email" = 'admin@example.com'), 
(SELECT "Id" FROM "Genre" WHERE "Name" = 'Action'), TRUE);

INSERT INTO "Review" ("Description", "Rating", "UserId", "MovieId", "IsActive")
VALUES 
('Great movie!', 5, (SELECT "Id" FROM "User" WHERE "Email" = 'user@example.com'), (SELECT "Id" FROM "Movie" WHERE "Title" = 'Example Movie'), TRUE);

INSERT INTO "Actor" ("Name", "IsActive")
VALUES 
('Actor One', TRUE),
('Actor Two', TRUE);

INSERT INTO "MovieActor" ("MovieId", "ActorId")
VALUES 
((SELECT "Id" FROM "Movie" WHERE "Title" = 'Example Movie'), (SELECT "Id" FROM "Actor" WHERE "Name" = 'Actor One')),
((SELECT "Id" FROM "Movie" WHERE "Title" = 'Example Movie'), (SELECT "Id" FROM "Actor" WHERE "Name" = 'Actor Two'));

-- INSERT za tablicu Projection
INSERT INTO "Projection" ("Date", "Time", "MovieId", "UserId", "IsActive")
VALUES 
('2024-06-20', '19:00:00', (SELECT "Id" FROM "Movie" WHERE "Title" = 'Example Movie'), (SELECT "Id" FROM "User" WHERE "Email" = 'admin@example.com'), TRUE);

INSERT INTO "Payment" ("TotalPrice", "IsActive")
VALUES 
(10.00, TRUE);

INSERT INTO "Ticket" ("Price", "PaymentId", "UserId", "ProjectionId", "IsActive")
VALUES 
(10.00, (SELECT "Id" FROM "Payment"), (SELECT "Id" FROM "User" WHERE "Email" = 'user@example.com'), (SELECT "Id" FROM "Projection"), TRUE);

INSERT INTO "Hall" ("HallNumber")
VALUES 
(1),
(2);

INSERT INTO "Seat" ("SeatNumber", "RowNumber", "HallId")
VALUES 
(1, 1, (SELECT "Id" FROM "Hall" WHERE "HallNumber" = 1)),
(2, 1, (SELECT "Id" FROM "Hall" WHERE "HallNumber" = 1)),
(1, 2, (SELECT "Id" FROM "Hall" WHERE "HallNumber" = 2)),
(2, 2, (SELECT "Id" FROM "Hall" WHERE "HallNumber" = 2));

INSERT INTO "ProjectionHall" ("ProjectionId", "HallId")
VALUES 
((SELECT "Id" FROM "Projection"), (SELECT "Id" FROM "Hall" WHERE "HallNumber" = 1));

INSERT INTO "SeatReserved" ("TicketId", "ProjectionId", "SeatId", "IsActive", "CreatedByUserId", "UpdatedByUserId")
VALUES 
((SELECT "Id" FROM "Ticket"), (SELECT "Id" FROM "Projection"), (SELECT "Id" FROM "Seat" WHERE "SeatNumber" = 1 AND "RowNumber" = 1), TRUE, (SELECT "Id" FROM "User" WHERE "Email" = 'admin@example.com'), (SELECT "Id" FROM "User" WHERE "Email" = 'admin@example.com'));