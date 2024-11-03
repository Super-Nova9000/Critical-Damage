using Godot;
using System;

public partial class Platform : Area2D
{
	[Export]
	public int contactPos { get; set; } = 929;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_area_entered(Area2D area)
	{
		contactPos = ((int)Position.Y - 97);
		GD.Print(contactPos);
	}
}
