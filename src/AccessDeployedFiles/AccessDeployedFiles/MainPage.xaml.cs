namespace AccessDeployedFiles
{
	public partial class MainPage : ContentPage
	{
		int count = 0;

		public MainPage()
		{
			InitializeComponent();

			// Simple goes to system root
			var f = new DirectoryInfo(Directory.GetCurrentDirectory()).GetFiles();

			// Doesn't provide a means to copy the file so I can get a definite path.
			var g = ImageSource.FromResource("dotnet_bot.png");
		}

		private async void MoveAssetsClicked(object sender, EventArgs e)
		{
			await MoveFile("AboutAssets.txt");
		}
		private void DeleteAssetsClicked(object sender, EventArgs e)
		{
			DeleteFile("AboutAssets.txt");
		}

		private async void MoveDotTxtClicked(object sender, EventArgs e)
		{
			await MoveFile("dotnetbot.txt");
		}
		private void DeleteDotTxtClicked(object sender, EventArgs e)
		{
			DeleteFile("dotnetbot.txt");
		}

		private async void MoveDotPngClicked(object sender, EventArgs e)
		{
			await MoveFile("dotnet.png");
		}
		private void DeleteDotPngClicked(object sender, EventArgs e)
		{
			DeleteFile("dotnet.png");
		}

		private async Task MoveFile(string fileName)
		{
			FileInfo file = new FileInfo(Path.Combine(FileSystem.AppDataDirectory, fileName));

			if (!file.Exists)
			{
				try
				{
					var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
					var reader = new StreamReader(stream);

					var contents = reader.ReadToEnd();

					StreamWriter writer = new(file.FullName);

					await writer.WriteAsync(contents);

					MessageLabel.Text = "File Copied from Raw";
				}
				catch (Exception ex)
				{
					MessageLabel.Text = ex.Message;
				}
			}
			else MessageLabel.Text = "File Exist in AppData";
		}
		private void DeleteFile(string fileName)
		{
			var file = new FileInfo(Path.Combine(FileSystem.AppDataDirectory, fileName));
			if (file.Exists)
			{
				try
				{
					file.Delete();
					MessageLabel.Text = "File Deleted from AppData";
				}
				catch (Exception ex)
				{
					MessageLabel.Text = ex.Message;
				}
			}
			else MessageLabel.Text = "File Missing in AppData";
		}
	}

}
