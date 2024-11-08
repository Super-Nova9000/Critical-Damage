using Godot;
using System;

public partial class BucketCollide : Area2D
{
	[Signal]
	public delegate void HitEventHandler();
	[Signal]
	public delegate void NotHitEventHandler();

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
		EmitSignal(SignalName.Hit);
	}

	private void _on_area_exited(Area2D area)
	{
		EmitSignal(SignalName.NotHit);
	}
}
