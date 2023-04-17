using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TextEditor.Models;

namespace TextEditor.Controllers
{

	public class ContentController : Controller
    {
		public string errorMessage = "";
		public string SuccessMessage;

		private IConfiguration configuration;
		public ContentController(IConfiguration configuration)
		{
			this.configuration = configuration;
		}

		public IActionResult Dashboard()
        {
			List<ContentModel> FileList = new List<ContentModel>();
			try
			{
				String conn = configuration.GetConnectionString("texteditor");
				SqlConnection connection = new SqlConnection(conn);
				connection.Open();

				string query = "SELECT * FROM content";
				SqlCommand sqlCommand = new SqlCommand(query, connection);

				SqlDataReader reader = sqlCommand.ExecuteReader();
				while (reader.Read())
				{
					ContentModel obj = new ContentModel();
					obj.FileId = (int)reader[0];
					obj.FileName = "" + reader[1];
					obj.Content = "" + reader[2];
					obj.FileOwner = "" + reader[3];

					FileList.Add(obj);
				}
				reader.Close();
				connection.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return View(FileList);
		}


		public IActionResult NewFile()
		{
			return View();
		}

		[HttpPost]
		public IActionResult AddContenttoDB()
		{
			Console.WriteLine("In post");
			try
			{
				String conn = configuration.GetConnectionString("texteditor");
				SqlConnection connection = new SqlConnection(conn);
				connection.Open();




				string filename = Request.Form["filename"];
				string ownername = Request.Form["ownername"];
				string content = Request.Form["content"];


				string query1 = $"SELECT * FROM content where FileName = '{filename}'";
				SqlCommand sqlCommands = new SqlCommand(query1, connection);

				SqlDataReader reader = sqlCommands.ExecuteReader();
				while (reader.Read())
				{
					errorMessage = "File Name already Exsist";
					return RedirectToAction("NewFile");

				}
				reader.Close();


				string query = $"INSERT INTO Content VALUES('{filename}','{content}','{ownername}')";
				SqlCommand sqlCommand = new SqlCommand(query, connection);

				sqlCommand.ExecuteNonQuery();

				connection.Close();

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return RedirectToAction("Dashboard");
		}

		public IActionResult EditFile()
		{
			int Id = Convert.ToInt32(Request.Query["id"]);
			ContentModel obj = new ContentModel();
			try
			{
				String conn = configuration.GetConnectionString("texteditor");
				SqlConnection connection = new SqlConnection(conn);
				connection.Open();

				string query = $"SELECT * FROM content where FileId = {Id}";
				SqlCommand sqlCommand = new SqlCommand(query, connection);

				SqlDataReader reader = sqlCommand.ExecuteReader();
				while (reader.Read())
				{
					
					obj.FileId = (int)reader[0];
					obj.FileName = "" + reader[1];
					obj.Content = "" + reader[2];
					obj.FileOwner = "" + reader[3];

				
				}
				reader.Close();
				connection.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return View(obj);
		}


		[HttpPost]
		public IActionResult UpdateContenttoDb()
		{
			Console.WriteLine("In post");
			try
			{
				String conn = configuration.GetConnectionString("texteditor");
				SqlConnection connection = new SqlConnection(conn);
				connection.Open();

				int fileId = Convert.ToInt32(Request.Form["fileid"]);
				string filename = Request.Form["filename"];
				string ownername = Request.Form["ownername"];
				string content = Request.Form["content"];

				string query = $"Update Content set FileName = '{filename}' ,FileContent = '{content}',FileOwner = '{ownername}' where FileId = {fileId}";
				SqlCommand sqlCommand = new SqlCommand(query, connection);

				sqlCommand.ExecuteNonQuery();

				connection.Close();

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return RedirectToAction("Dashboard");
		}

		public IActionResult DeleteFile(int id)
		{
			try
			{
				int Id = Convert.ToInt32(Request.Query["id"]);

				String conn = configuration.GetConnectionString("texteditor");
				SqlConnection connection = new SqlConnection(conn);
				connection.Open();

				string query = $"DELETE FROM content WHERE FileId= {id} ";
				SqlCommand sqlCommand = new SqlCommand(query, connection);
				sqlCommand.ExecuteNonQuery();
				connection.Close();

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return RedirectToAction("Dashboard");
		}


	}
}
