namespace UniLx.Infra.Data.Database
{
    public static class CustomQueries
    {
        public const string AlterAdvertisementTableAddGeoPoint = "ALTER TABLE unilxdb.mt_doc_advertisement ADD COLUMN IF NOT EXISTS geopoint geometry(Point, 4326) GENERATED ALWAYS AS (ST_SetSRID(ST_MakePoint((data -> 'address' ->> 'longitude')::double precision, (data -> 'address' ->> 'latitude')::double precision), 4326)) STORED;";
        public const string FindNearestAdvertisements = "ST_DWithin(geopoint, ST_SetSRID(ST_MakePoint(?, ?), 4326)::geography, ?)";
        public const string InsertGeopointOnAdvertisementTable = "UPDATE unilxdb.mt_doc_advertisement SET geopoint = ST_SetSRID(ST_MakePoint(?, ?), 4326) WHERE id = ?";
    }
}
