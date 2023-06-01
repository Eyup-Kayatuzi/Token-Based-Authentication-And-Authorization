namespace WepApiCrudWithJwt.Options
{
    public class JwtOption
    {
        public string Secret { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public double ForTimeAdjust { get; set; }

    }
}
