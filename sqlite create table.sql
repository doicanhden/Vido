CREATE TABLE CardType
(
	CardTypeId INTEGER PRIMARY KEY,
	Name TEXT,
	Description TEXT
);

CREATE TABLE Card
(
	CardId TEXT PRIMARY KEY,
	CardType TEXT,
	IsUse INTEGER,
	IsError INTEGER
);

CREATE TABLE InOutRecord
(
	RecordId INTEGER PRIMARY KEY AUTOINCREMENT,
	InUserId TEXT,
	InLaneCode TEXT,
	InTime TEXT,
	InBackImg TEXT,
	InFrontImg TEXT,

	OutUserId TEXT,
	OutLaneCode TEXT,
	OutTime TEXT,
	OutBackImg TEXT,
	OutFrontImg TEXT,

	CardId TEXT,
	UserData TEXT,
	Comment TEXT,
	FeeValue NUMERIC
);

