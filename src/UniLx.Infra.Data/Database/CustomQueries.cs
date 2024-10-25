namespace UniLx.Infra.Data.Database
{
    public static class CustomQueries
    {
        public static string AlterAdvertisementTableAddGeoPoint = "ALTER TABLE unilxdb.mt_doc_advertisement ADD COLUMN geopoint geometry(Point, 4326);";
        public static string InsertGeopointOnAdvertisementTable = "UPDATE unilxdb.mt_doc_advertisement SET geopoint = ST_SetSRID(ST_MakePoint(?, ?), 4326) WHERE id = ?";
    }
}
