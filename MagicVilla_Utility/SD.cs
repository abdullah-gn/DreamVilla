﻿namespace MagicVilla_Utility
{
	public static class SD
	{
		public enum ApiType{

			GET, POST, PUT, DELETE
		
		}

		public static string AccessToken = "JWTToken";
		public static string RefreshToken = "RefreshToken";
		public static string ApiVersion = "V2";
		public const string Admin  = "Admin";
        public const string Customer = "Customer";
		public enum ContentType
		{
			Json,
			MultipartFormData
		}
    }
}