using Godot;
using System;
using System.Data;

public partial class Platform : Area2D
{
	private Bucket Player;

	private int contactPos;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Player = GetParent().GetNode<Node2D>("Bucket") as Bucket;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_area_entered(Area2D area)
	{
		contactPos = ((int)Position.Y - 97);
		Player.yTarget = contactPos;
	}
}
