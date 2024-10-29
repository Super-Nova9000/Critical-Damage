using Godot;
using System;

public partial class PlatHalves : Area2D
{
	[Signal]
	public delegate void HitEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_area_entered(Area2D area)
	{
		EmitSignal(SignalName.Hit);
	}
}
