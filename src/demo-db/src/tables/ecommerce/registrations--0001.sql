CREATE TABLE ecommerce.registrations (
    Id SERIAL PRIMARY KEY,
    Email TEXT NOT NULL,
    FirstName TEXT NOT NULL,
    LastName TEXT NOT NULL,
    AddressLine1 TEXT NOT NULL,
    AddressLine2 TEXT NOT NULL,
    City TEXT NOT NULL,
    State TEXT NOT NULL,
    Zip TEXT NOT NULL,
    RegistrationDate timestamp NOT NULL,
    BookTitle TEXT NOT NULL,
    InsertedOn timestamp default now()
);