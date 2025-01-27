using Godot;
using System;
using System.Collections;

public partial class Exception : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		QueueFree();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
