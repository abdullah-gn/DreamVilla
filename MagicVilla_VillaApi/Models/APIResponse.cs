using System.Net;

namespace DreamVilla_VillaApi.Models
{
	public class APIResponse
	{
        public HttpStatusCode StatusCode { get; set; }
		public bool IsSucceed { get; set; } = true;

		public List<string> ErrorMessage { get; set; }

		public object Result {  get; set; } 
	}
}
