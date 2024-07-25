# AccessDeployedFiles
This project aims to access deployed files in Maui with the ultimate goal of getting a definitive path to an image as a default.

We are given a "Raw" folder dedicated to accessing deployed files; however, it appears only for text-based only.
The below code breaks at "OpenAppPackageFileAsync()" when it hits anything that is not text-based. I tried setting "dotnet.png" to "dotnetbot.txt" but it does not matter. I believe what happens is "Raw" can not compile anything that is not a text file so it ignores it.
 ```csharp
	async Task LoadMauiAsset()
	{
		using var stream = await FileSystem.OpenAppPackageFileAsync("AboutAssets.txt");
		using var reader = new StreamReader(stream);

		var contents = reader.ReadToEnd();
	}
```
We are also given an "Images" folder that can be accessed; however, it does not provide a means to get a FileInfo.FullName nor a FileStream so I can copy an image.
 ```csharp
var image = ImageSource.FromResource("dotnet_bot.png");
```
