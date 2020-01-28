BEGIN TRANSACTION;

PRAGMA foreign_keys = ON;

CREATE TABLE IF NOT EXISTS "TaxiZone" (
	"Id"		INTEGER,
	"Borough"	TEXT,
	"Zone"		TEXT,
	PRIMARY KEY("Id")
) WITHOUT ROWID;

CREATE TABLE IF NOT EXISTS "TaxiTrip" (
	"Id"				INTEGER,
	"PickUpTimestamp"	INTEGER NOT NULL,
	"DropOffTimestamp"	INTEGER NOT NULL,
	"PickUpLocationId"	INTEGER NOT NULL,
	"DropOffLocationId"	INTEGER NOT NULL,
	"VehicleType"		INTEGER NOT NULL,
	"Cost"				FLOAT,
	PRIMARY KEY("Id"),
	FOREIGN KEY("PickUpLocationId") REFERENCES TaxiZone(Id),
	FOREIGN KEY("DropOffLocationId") REFERENCES TaxiZone(Id)
) WITHOUT ROWID;

CREATE INDEX IF NOT EXISTS "idx_TaxiTrip_Locations" ON "TaxiTrip" (
	"PickUpLocationId"	ASC,
	"DropOffLocationId"	ASC,
	"PickUpTimestamp" ASC,
	"DropOffTimestamp" ASC
);

COMMIT;
