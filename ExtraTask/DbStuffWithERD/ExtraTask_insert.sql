INSERT INTO Speaker (FirstName, LastName) VALUES
('Anna', 'Kowalska'),
('Jan', 'Nowak'),
('Karol', 'Wiśniewski');

INSERT INTO Participant (FirstName, LastName, Email) VALUES
('Marta', 'Zielińska', 'marta@example.com'),
('Piotr', 'Grabowski', 'piotr@example.com'),
('Ewa', 'Maj', 'ewa@example.com'),
('Tomasz', 'Kurek', 'tomasz@example.com');

INSERT INTO Event (Title, Description, Date, MaxParticipants) VALUES
('ASP.NET Core Workshop', 'Warsztaty z ASP.NET Core i EF Core', '2025-07-10', 2),
('Tech Conference 2025', 'Konferencja technologiczna z wieloma prelekcjami', '2025-08-01', 3);

INSERT INTO Registration (RegisteredAt, Participant_ID, Event_ID) VALUES
('2025-06-20 10:00:00', 1, 1),
('2025-06-21 15:00:00', 2, 1),
('2025-06-22 12:00:00', 3, 2),
('2025-06-22 13:00:00', 4, 2);

INSERT INTO SpeakerEvent (Event_ID, Speaker_ID) VALUES
(1, 1),
(1, 2),
(2, 3);