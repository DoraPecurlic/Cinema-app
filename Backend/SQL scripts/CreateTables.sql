CREATE TABLE "Role" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "RoleName" VARCHAR(255) NOT NULL
);

CREATE TABLE "User" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "Email" VARCHAR(255) NOT NULL,
    "Password" VARCHAR(255) NOT NULL,
    "FirstName" VARCHAR(255) NOT NULL,
    "LastName" VARCHAR(255) NOT NULL,
    "RoleId" UUID,
    "IsActive" BOOLEAN NOT NULL,
    "DateCreated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "DateUpdated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT "FK_User_Role_RoleId" FOREIGN KEY ("RoleId") REFERENCES "Role" ("Id")
);

CREATE TABLE "Genre" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "Name" VARCHAR(255) NOT NULL
);

CREATE TABLE "Language" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "Name" VARCHAR(255) NOT NULL
);

CREATE TABLE "Movie" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "Title" VARCHAR(255) NOT NULL,
    "Description" VARCHAR(255) NOT NULL,
    "Duration" INT NOT NULL,
    "LanguageId" UUID,
    "CoverUrl" VARCHAR(255),
    "TrailerUrl" VARCHAR(255),
    "AdminId" UUID,
    "GenreId" UUID,
    "IsActive" BOOLEAN NOT NULL,
    "DateCreated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "DateUpdated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "CreatedByUserId" UUID,
    "UpdatedByUserId" UUID,
    CONSTRAINT "FK_Movie_User_AdminId" FOREIGN KEY ("AdminId") REFERENCES "User" ("Id"),
    CONSTRAINT "FK_Movie_Language_LanguageId" FOREIGN KEY ("LanguageId") REFERENCES "Language" ("Id"),
    CONSTRAINT "FK_Movie_Genre_GenreId" FOREIGN KEY ("GenreId") REFERENCES "Genre" ("Id")
);

CREATE TABLE "Review" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "Description" VARCHAR(255) NOT NULL,
    "Rating" INT NOT NULL,
    "UserId" UUID,
    "MovieId" UUID,
    "IsActive" BOOLEAN NOT NULL,
    "DateCreated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "DateUpdated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "CreatedByUserId" UUID,
    "UpdatedByUserId" UUID,
    CONSTRAINT "FK_Review_User_UserId" FOREIGN KEY ("UserId") REFERENCES "User" ("Id"),
    CONSTRAINT "FK_Review_Movie_MovieId" FOREIGN KEY ("MovieId") REFERENCES "Movie" ("Id")
);

CREATE TABLE "Actor" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "Name" VARCHAR(255) NOT NULL,
    "IsActive" BOOLEAN NOT NULL,
    "DateCreated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "DateUpdated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "CreatedByUserId" UUID,
    "UpdatedByUserId" UUID
);

CREATE TABLE "MovieActor" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "MovieId" UUID,
    "ActorId" UUID,
    CONSTRAINT "FK_MovieActor_Movie_MovieId" FOREIGN KEY ("MovieId") REFERENCES "Movie" ("Id"),
    CONSTRAINT "FK_MovieActor_Actor_ActorId" FOREIGN KEY ("ActorId") REFERENCES "Actor" ("Id")
);

CREATE TABLE "Projection" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "Date" DATE NOT NULL,
    "Time" TIME NOT NULL,
    "MovieId" UUID,
    "UserId" UUID,
    "IsActive" BOOLEAN NOT NULL,
    "DateCreated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "DateUpdated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "CreatedByUserId" UUID,
    "UpdatedByUserId" UUID,
    CONSTRAINT "FK_Projection_Movie_MovieId" FOREIGN KEY ("MovieId") REFERENCES "Movie" ("Id"),
    CONSTRAINT "FK_Projection_User_UserId" FOREIGN KEY ("UserId") REFERENCES "User" ("Id")
);

CREATE TABLE "Payment" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "TotalPrice" DECIMAL(10, 2) NOT NULL,
    "IsActive" BOOLEAN NOT NULL,
    "DateCreated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "DateUpdated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "CreatedByUserId" UUID,
    "UpdatedByUserId" UUID
);

CREATE TABLE "Ticket" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "Price" DECIMAL(10, 2) NOT NULL,
    "PaymentId" UUID,
    "UserId" UUID,
    "ProjectionId" UUID,
    "IsActive" BOOLEAN NOT NULL,
    "DateCreated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "DateUpdated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "CreatedByUserId" UUID,
    "UpdatedByUserId" UUID,
    CONSTRAINT "FK_Ticket_Payment_PaymentId" FOREIGN KEY ("PaymentId") REFERENCES "Payment" ("Id"),
    CONSTRAINT "FK_Ticket_User_UserId" FOREIGN KEY ("UserId") REFERENCES "User" ("Id"),
    CONSTRAINT "FK_Ticket_Projection_ProjectionId" FOREIGN KEY ("ProjectionId") REFERENCES "Projection" ("Id")
);

CREATE TABLE "Hall" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "HallNumber" INT NOT NULL
);

CREATE TABLE "Seat" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "SeatNumber" INT NOT NULL,
    "RowNumber" INT NOT NULL,
    "HallId" UUID,
    CONSTRAINT "FK_Seat_Hall_HallId" FOREIGN KEY ("HallId") REFERENCES "Hall" ("Id")
);

CREATE TABLE "ProjectionHall" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "ProjectionId" UUID,
    "HallId" UUID,
    CONSTRAINT "FK_ProjectionHall_Projection_ProjectionId" FOREIGN KEY ("ProjectionId") REFERENCES "Projection" ("Id"),
    CONSTRAINT "FK_ProjectionHall_Hall_HallId" FOREIGN KEY ("HallId") REFERENCES "Hall" ("Id")
);

CREATE TABLE "SeatReserved" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "TicketId" UUID,
    "ProjectionId" UUID,
    "SeatId" UUID,
    "IsActive" BOOLEAN NOT NULL,
    "DateCreated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "DateUpdated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "CreatedByUserId" UUID NOT NULL,
    "UpdatedByUserId" UUID NOT NULL,
    CONSTRAINT "FK_SeatReserved_Ticket_TicketId" FOREIGN KEY ("TicketId") REFERENCES "Ticket" ("Id"),
    CONSTRAINT "FK_SeatReserved_Projection_ProjectionId" FOREIGN KEY ("ProjectionId") REFERENCES "Projection" ("Id"),
    CONSTRAINT "FK_SeatReserved_Seat_SeatId" FOREIGN KEY ("SeatId") REFERENCES "Seat" ("Id")
);