-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2025-06-21 13:16:47.285

-- tables
-- Table: Event
CREATE TABLE Event (
    ID int  NOT NULL IDENTITY,
    Title varchar(100)  NOT NULL,
    Description varchar(300)  NOT NULL,
    Date datetime  NOT NULL,
    MaxParticipants int  NOT NULL,
    CONSTRAINT Event_pk PRIMARY KEY  (ID)
);

-- Table: Participant
CREATE TABLE Participant (
    ID int  NOT NULL IDENTITY,
    FirstName varchar(100)  NOT NULL,
    LastName varchar(100)  NOT NULL,
    Email varchar(100)  NOT NULL,
    CONSTRAINT Participant_pk PRIMARY KEY  (ID)
);

-- Table: Registration
CREATE TABLE Registration (
    ID int  NOT NULL IDENTITY,
    RegisteredAt datetime  NOT NULL,
    Participant_ID int  NOT NULL,
    Event_ID int  NOT NULL,
    CONSTRAINT Registration_pk PRIMARY KEY  (ID)
);

-- Table: Speaker
CREATE TABLE Speaker (
    ID int  NOT NULL IDENTITY,
    FirstName varchar(100)  NOT NULL,
    LastName varchar(100)  NOT NULL,
    CONSTRAINT Speaker_pk PRIMARY KEY  (ID)
);

-- Table: SpeakerEvent
CREATE TABLE SpeakerEvent (
    Event_ID int  NOT NULL,
    Speaker_ID int  NOT NULL,
    CONSTRAINT SpeakerEvent_pk PRIMARY KEY  (Event_ID,Speaker_ID)
);

-- foreign keys
-- Reference: Registration_Event (table: Registration)
ALTER TABLE Registration ADD CONSTRAINT Registration_Event
    FOREIGN KEY (Event_ID)
    REFERENCES Event (ID);

-- Reference: Registration_Participant (table: Registration)
ALTER TABLE Registration ADD CONSTRAINT Registration_Participant
    FOREIGN KEY (Participant_ID)
    REFERENCES Participant (ID);

-- Reference: SpeakerEvent_Event (table: SpeakerEvent)
ALTER TABLE SpeakerEvent ADD CONSTRAINT SpeakerEvent_Event
    FOREIGN KEY (Event_ID)
    REFERENCES Event (ID);

-- Reference: SpeakerEvent_Speaker (table: SpeakerEvent)
ALTER TABLE SpeakerEvent ADD CONSTRAINT SpeakerEvent_Speaker
    FOREIGN KEY (Speaker_ID)
    REFERENCES Speaker (ID);

-- End of file.

