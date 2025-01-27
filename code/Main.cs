using Godot;
using System;
using System.Diagnostics.Tracing;
using System.IO;

public partial class Main : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		levelPlace("1");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void levelPlace(string lvlNum)
	{
		lvlNum = "lvl" + lvlNum + ".png"; //Format into file name
		string filePath = Directory.GetCurrentDirectory(); //Find path of root folder
		filePath = Path.Combine(filePath, "levels", lvlNum); //Set target to file from lvlNum

		var lvlImage = Image.LoadFromFile(filePath); //Load image
		int[] imgDims = { lvlImage.GetWidth() - 1, lvlImage.GetHeight() - 1 };

		for (int i = 0; i <= imgDims[0]; i++) //Pointer X axis
		{
			for (int j = 0; j <= imgDims[1]; j++) //Pointer Y axis
			{
				var coords = new Vector2I(i, j); //Generate coordinates as type Vector2I for following line
				Color colour = lvlImage.GetPixelv(coords); //Get the colour of the pixel at stated coordinates
				processColour(colour, i, j);
			}
		}
	}

	public void processColour(Color colour, int x, int y) //Figure out what the colour at coordinate refers to, and place the corresponding object there
	{
		string hexColour = (colour.R8).ToString("X") + (colour.G8).ToString("X") + (colour.B8).ToString("X"); //Convert stated colour from RGB to Hex

		x = (x * 96) + 48;
		y = (y * 96) + 48; //Change coordinates from pixels to tiles

		PackedScene scene;

		if (hexColour == "FFFFFF") //If pixel white
		{
			scene = GD.Load<PackedScene>("res://Platform.tscn"); //Create instance of platform
		}
		else
		{
			scene = GD.Load<PackedScene>("res://Exception.tscn"); //Temporary scene if no others valid
		}

		var placeMe = (Node2D)scene.Instantiate(); //Instantiate the stated scene
		AddChild(placeMe); //Spawn scene
		placeMe.Position = new Godot.Vector2(x, y); //Set position of scene

	}
}
