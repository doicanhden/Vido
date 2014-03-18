CREATE TABLE CardType
(
	CardTypeId INTEGER PRIMARY KEY,
	Name TEXT,
	Description TEXT
);

CREATE TABLE Card
(
	CardId TEXT PRIMARY KEY,
	CardTypeId INTEGER,
	IsUse NUMERIC,

	FOREIGN KEY(CardTypeId) REFERENCES CardType(CardTypeId)
);

CREATE TABLE EntryExit
(
	CardId TEXT PRIMARY KEY,

	EntryPlateNumber TEXT,
	EntryTime TEXT,
	EntryPlateImage TEXT,
	EntryFaceImage TEXT,

	ExitPlateNumber TEXT,
	ExitTime TEXT,
	ExitPlateImage TEXT,
	ExitFaceImage TEXT
);

INSERT INTO CardType(CardTypeId, Name, Description)
VALUES(0, 'THEXE', 'The xe');

INSERT INTO Card(CardId, CardTypeId, IsUse) VALUES('0228282404', 0, 1);
INSERT INTO Card(CardId, CardTypeId, IsUse) VALUES('0082408356', 0, 1);
INSERT INTO Card(CardId, CardTypeId, IsUse) VALUES('0082408340', 0, 1);
INSERT INTO Card(CardId, CardTypeId, IsUse) VALUES('0228282932', 0, 1);
INSERT INTO Card(CardId, CardTypeId, IsUse) VALUES('0228373332', 0, 1);
INSERT INTO Card(CardId, CardTypeId, IsUse) VALUES('0086260500', 0, 1);
INSERT INTO Card(CardId, CardTypeId, IsUse) VALUES('0228282868', 0, 1);
INSERT INTO Card(CardId, CardTypeId, IsUse) VALUES('0228282916', 0, 1);
INSERT INTO Card(CardId, CardTypeId, IsUse) VALUES('0086260804', 0, 1);
INSERT INTO Card(CardId, CardTypeId, IsUse) VALUES('0082408404', 0, 1);




















