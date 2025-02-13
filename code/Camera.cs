using Godot;
using System;

public partial class Camera : Camera2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var root = GetParent();
		var bucket = (Node2D)root.GetNode("Bucket"); //Interact with Bucket
		float x = bucket.Position[0]; //Set to Bucket's position
		if (x < 960) //If Bucket is too far left
		{
			x = 960; //Set position to start point
		}
		Position = new Vector2(x, 650); //Set camera position
	}
}
