namespace MagicVilla_Web.Models;
using static MagicVilla_Utility.SD;

	public class ApiRequest
	{
	public ApiType ApiType { get; set; } = ApiType.GET;

	public string? ApiUrl { get; set; }

	public object? Data { get; set; }

    public string token { get; set; }

}

