namespace AccessDeployedFiles
{
	public partial class MainPage : ContentPage
	{
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
			await MoveFile("aboutassets.txt", false);
		}
		private void DeleteAssetsClicked(object sender, EventArgs e)
		{
			DeleteFile("AboutAssets.txt");
		}

		private async void MoveDotTxtClicked(object sender, EventArgs e)
		{
			await MoveFile("dotnetbot.txt", false);
		}
		private void DeleteDotTxtClicked(object sender, EventArgs e)
		{
			DeleteFile("dotnetbot.txt");
		}

		private async void MoveDotPngClicked(object sender, EventArgs e)
		{
			await MoveFile("dotnet.png", false);
		}
		private void DeleteDotPngClicked(object sender, EventArgs e)
		{
			DeleteFile("dotnet.png");
		}

		private async Task MoveFile(string fileName, bool textFile)
		{
			FileInfo file = new FileInfo(Path.Combine(FileSystem.AppDataDirectory, fileName));

			if (!file.Exists)
			{
				try
				{
					// Checks to make sure files exists at deployment.
					if (!await FileSystem.AppPackageFileExistsAsync(fileName))
						throw new Exception("File " + fileName + " does not exist. Edit project code and delete <ItemGroup> that removed files.");

					if (textFile)
					{ // Recommended Method when dealing with text files but not necessary.
						using (var stream = await FileSystem.OpenAppPackageFileAsync(fileName))
						{
							using (var reader = new StreamReader(stream))
							{
								var contents = reader.ReadToEnd();

								using (StreamWriter writer = file.CreateText())
									await writer.WriteAsync(contents);
							}
						}
					}
					else
					{ // The only method that can be used for binary files. (Can be used for text files too.)
						using (var source = await FileSystem.OpenAppPackageFileAsync(fileName))
						{
							using (var destination = file.Create())
							{
								await source.CopyToAsync(destination);
							}
						}
					}

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
