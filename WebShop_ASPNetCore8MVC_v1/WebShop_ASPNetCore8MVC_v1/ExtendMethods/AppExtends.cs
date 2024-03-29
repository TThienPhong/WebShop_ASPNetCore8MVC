using Azure;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net;

namespace WebShop_ASPNetCore8MVC_v1.ExtendMethods
{
	public static class  AppExtends
	{
		public static void AddStatusCodePage(this IApplicationBuilder app){ 
			app.UseStatusCodePages(appError => {
				appError.Run(async context => {
					var respone = context.Response; 
					var code = respone.StatusCode; 
					var content = @$"<html> 
						<head> 
							<meta charset='UTF-8' /> 
							<title>Lỗi {code}</title> 
						</head> 
						<body> 
							<p style='color: red; font-size: 30px'> Có lỗi xảy ra: {code} {(HttpStatusCode)code} </p> 
							<a href='/'>Về trang chủ</a>
						</body> 
					</html>";
					await respone.WriteAsync(content);
				}); 				
			}); //Code 400 599
		}
	}
}
