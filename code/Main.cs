using Godot;
using System;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Numerics;
using System.Runtime.CompilerServices;

public partial class Main : Node
{
	[Export]
	public PackedScene PlatformScene { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		makePlatform(20, 0, 1026);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void makePlatform(int length, int X, int Y) //Length of platform (1 length = 54 pixels), platform starting X positon and Y position
	{
		X = X + 48; //Nudge first platform by 48 pixels, so the edge of the platform is at the start position
		for (int i = 0; i < length; i++) //Loop for length
		{
			Platform platform = PlatformScene.Instantiate<Platform>(); //Create new instance of a platform piece

			int placeX = X + (i * 96); //Make sure next piece is placed next to exisiting piece
			int placeY = Y;

			platform.Position = new Godot.Vector2(placeX, placeY); //Set piece position

			AddChild(platform); //Spawn the platform piece
		}
	}
}
