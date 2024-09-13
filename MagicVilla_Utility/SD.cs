namespace MagicVilla_Utility
{
	public static class SD
	{
		public enum ApiType{

			GET, POST, PUT, DELETE
		
		}

		public static string SessionToken = "JWTTOKEN";
		public static string ApiVersion = "V2";
		public const string Admin  = "Admin";
        public const string Customer = "Customer";
    }
}