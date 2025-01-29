using Godot;
using System;
using System.Collections;

public partial class NotFound : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		QueueFree(); //Kill this scene as soon as it's created
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
