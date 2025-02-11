using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

public partial class Enemy : CharacterBody2D
{
	[Export]
	public Bucket bucket;
	public int yomama = 0;
	public override void _Ready()
	{
		var root = GetParent();
		bucket = (Bucket)root.GetNode("Bucket"); //Retrieve Bucket
		GD.Print(bucket);
	}

	public override void _PhysicsProcess(double delta)
	{
		var collisionData = GetLastSlideCollision(); //Get collision data
		GD.Print("AAAAAAAAAAA0");

		if ((collisionData.GetCollider()).Equals(bucket)) //If collider is Bucket
		{
			bucket.Damage(5); //Hurt Bucket
		}
	}
}
