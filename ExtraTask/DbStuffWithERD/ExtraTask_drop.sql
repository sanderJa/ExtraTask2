-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2025-06-21 13:16:47.285

-- foreign keys
ALTER TABLE Registration DROP CONSTRAINT Registration_Event;

ALTER TABLE Registration DROP CONSTRAINT Registration_Participant;

ALTER TABLE SpeakerEvent DROP CONSTRAINT SpeakerEvent_Event;

ALTER TABLE SpeakerEvent DROP CONSTRAINT SpeakerEvent_Speaker;

-- tables
DROP TABLE Event;

DROP TABLE Participant;

DROP TABLE Registration;

DROP TABLE Speaker;

DROP TABLE SpeakerEvent;

-- End of file.

