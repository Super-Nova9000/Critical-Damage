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
		int[] imgDims = { lvlImage.GetWidth(), lvlImage.GetHeight() };

		for (int i = 0; i <= imgDims[0]; i++) //Pointer X axis
		{
			for (int j = 0; j <= imgDims[1]; j++) //Pointer Y axis
			{
				var coords = new Vector2I(i, j);
				Color colour = lvlImage.GetPixelv(coords);
				processColour(colour, i, j);
			}
		}
	}

	public void processColour(Color colour, int x, int y)
	{
		float[] rgb = { (colour[0] * 255), (colour[1] * 255), (colour[2] * 255), (colour[3] * 255) };
	}
}
