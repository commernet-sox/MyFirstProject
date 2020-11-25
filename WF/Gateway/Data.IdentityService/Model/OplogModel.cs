namespace Data.IdentityService.Model
{
    public class OplogModel : BaseModel
    {
        public long LogId { get; set; }
        public int? LogType { get; set; }

        public string Mid { get; set; }
        public string Mname { get; set; }

    }
}
