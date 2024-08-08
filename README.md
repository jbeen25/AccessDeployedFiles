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



------------------------------------- Update/Solution ---------------------------------------------

For anyone else coming across this same issue, I have a solution. I am purposefully leaving the error in place so you can see the problem.

As of August 2024, any time a file is dragged & dropped into the Raw folder this triggers an event within Visual Studios to edit the project code to remove those files on deployment. Simply remove the automatically generated code [(here)](https://github.com/jbeen25/AccessDeployedFiles/blob/main/src/AccessDeployedFiles/AccessDeployedFiles/AccessDeployedFiles.csproj#L75-L83) and this project will work properly.

Thanks to those who helped figure out the [issue](https://github.com/dotnet/maui/issues/23833).

------------------------------------- Improved ----------------------------------------------------

Now enables proper streaming of binary files as StreamReader and StreamWriter only work for Text files. This is common knowledge but I was not thinking about this when orginally uploading this project.

```csharp
using (var source = await FileSystem.OpenAppPackageFileAsync(fileName))
{
	using (var destination = file.Create())
	{
		await source.CopyToAsync(destination);
	}
}
```