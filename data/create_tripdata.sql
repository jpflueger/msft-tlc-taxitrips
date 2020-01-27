BEGIN TRANSACTION;
DROP TABLE IF EXISTS "TaxiTrip";
CREATE TABLE IF NOT EXISTS "TaxiTrip" (
	"PickUpDateTime"	DATETIME,
	"DropOffDateTime"	DATETIME,
	"PickUpLocationId"	BIGINT,
	"DropOffLocationId"	BIGINT,
	"Cost"	FLOAT,
	"VehicleType"	BIGINT,
	"Duration"	BIGINT
);
DROP TABLE IF EXISTS "TaxiZone";
CREATE TABLE IF NOT EXISTS "TaxiZone" (
	"LocationId"	INTEGER,
	"Borough"	TEXT,
	"Zone"	TEXT
);
DROP INDEX IF EXISTS "idx_TaxiZone_PK";
CREATE UNIQUE INDEX IF NOT EXISTS "idx_TaxiZone_PK" ON "TaxiZone" (
	"LocationId"	ASC
);
DROP INDEX IF EXISTS "idx_TaxiTrip_Locations";
CREATE INDEX IF NOT EXISTS "idx_TaxiTrip_Locations" ON "TaxiTrip" (
	"PickUpLocationId"	ASC,
	"DropOffLocationId"	ASC
);
COMMIT;
