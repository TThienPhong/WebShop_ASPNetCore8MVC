﻿using System.Text;

namespace WebShop_ASPNetCore8MVC_v1.Helpers
{
	public class MyUtil
	{
		public static string UploadHinh(IFormFile Hinh, string folder)
		{
			try
			{

				var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Hinh", folder, Hinh.FileName);
				using (var myfile = new FileStream(fullPath, FileMode.CreateNew))
				{
					Hinh.CopyTo(myfile);
				}
				return Hinh.FileName;
			}
			catch (Exception ex)
			{
				return string.Empty;
			}
		}
        public static void DeleteHinh(string tenFile, string folder)
        {
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Hinh", folder, tenFile);
            try
            {               
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Không xoá được Hình {fullPath}. " + ex.Message, ex);
            }
        }

        public static string GenerateRamdomKey(int length = 5)
		{
			var pattern = @"qazwsxedcrfvtgbyhnujmiklopQAZWSXEDCRFVTGBYHNUJMIKLOP!";
			var sb = new StringBuilder();
			var rd = new Random();
			for (int i = 0; i < length; i++)
			{
				sb.Append(pattern[rd.Next(0, pattern.Length)]);
			}

			return sb.ToString();
		}
	}
}
