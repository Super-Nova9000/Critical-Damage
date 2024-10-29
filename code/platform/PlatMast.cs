using Godot;
using System;

public partial class PlatMast : Node2D
{
	[Signal]
	public delegate void TopHitEventHandler();

	[Signal]
	public delegate void BotHitEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Bucket bucket = GetNode<Bucket>("Bucket");
		bucket.Connect(PlatMast.SignalName.BotHit, Callable.From(_on_platform_bot_hit));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_top_half_hit()
	{
		EmitSignal(SignalName.TopHit);
		//GD.Print("Top Hit");
	}

	public void _on_bot_half_hit()
	{
		EmitSignal(SignalName.BotHit);
		//GD.Print("Bot Hit");

	}
}
