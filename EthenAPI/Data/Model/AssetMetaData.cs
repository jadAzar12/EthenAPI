namespace EthenAPI.Data.Model
{
    public class AssetMetaData
    {
        public int Id { get; set; }
        public string File_Name { get; set; }
        public string Original_File_Name { get; set; }
        public string File_Extension { get; set; }
        public string File_Size { get; set; }
        public DateTime Upload_date { get; set; }
    }
}
