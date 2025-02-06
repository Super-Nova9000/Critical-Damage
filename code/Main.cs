using Godot;
using System;
using System.Diagnostics.Tracing;
using System.IO;

public partial class Main : Node2D
{
	private string lvlNum = "1";
	private Image lvlImage;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		LevelPlace(lvlNum);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void LevelPlace(string lvlNum)
	{
		GetImage(lvlNum);
		int[] imgDims = { lvlImage.GetWidth() - 1, lvlImage.GetHeight() - 1 };

		for (int i = 0; i <= imgDims[0]; i++) //Pointer X axis
		{
			for (int j = 0; j <= imgDims[1]; j++) //Pointer Y axis
			{
				var colour = GetColour(i, j); //Get the colour of the pixel at stated coordinates
				ProcessColour(colour, i, j);
			}
		}
	}

	private void ProcessColour(string colour, int x, int y) //Figure out what the colour at coordinate refers to, and place the corresponding object there
	{
		x = (x * 96) + 48;
		y = (y * 96) + 48; //Change coordinates from pixels to tiles

		string tileType = "NotFound"; //The type of tile to be placed, by default a placeholder
		bool flipMe = false;

		if (colour == "FFFFFF") //Create platform on white pixel
		{
			tileType = "Platform";
		}
		else if (colour == "00FF" || colour == "06464") //Create slope on blue pixel
		{
			tileType = "SlopePlatform";
			flipMe = (colour == "06464"); //Flip slope on turquoise pixel
		}
		else if (colour == "EA6B70")
		{
			tileType = "Enemy";
		}

		if (colour != "000" && colour != "FFFFFF") { GD.Print(colour); }

		var scene = GD.Load<PackedScene>("res://" + tileType + ".tscn"); //Load instance of tileType
		var placeMe = (Node2D)scene.Instantiate(); //Instantiate the loaded scene
		AddChild(placeMe); //Spawn scene
		placeMe.Position = new Godot.Vector2(x, y); //Set position of scene

		if (flipMe)
		{
			var scale = new Vector2(-1, 1); //Invert X scaling
			placeMe.Scale = scale; //Set new scale
		}

	}

	private string GetColour(int x, int y) //Returns the Hex colour at a given pixel
	{
		var coords = new Vector2I(x, y); //Change provided coordinates into type Vector2I
		var colour = lvlImage.GetPixelv(coords); //Get the colour as type Color (RGB)
		return ((colour.R8).ToString("X") + (colour.G8).ToString("X") + (colour.B8).ToString("X")); //Change type Color into String and from RGB to Hex
	}
	private void GetImage(string lvlNum) //Sets the global variable lvlImage that stores the level data
	{
		string imgName = "lvl" + lvlNum + ".png"; //Format number into file name
		string filePath = Directory.GetCurrentDirectory(); //Find path of root folder
		filePath = Path.Combine(filePath, "levels", imgName); //Set target to file in lvlNum

		lvlImage = Image.LoadFromFile(filePath); //Load and set image
	}
}
