using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

public partial class Enemy : CharacterBody2D
{
	private Bucket bucket;
	private Godot.Timer attackTimer;

	public override void _Ready()
	{
		var root = GetParent();
		bucket = (Bucket)root.GetNode("Bucket"); //Retrieve Bucket

		attackTimer = GetNode<Godot.Timer>("AttackTimer");
	}

	public override void _PhysicsProcess(double delta)
	{
		var collisionData = GetLastSlideCollision(); //Get collision data

		if (collisionData != null && collisionData.GetCollider() == bucket && attackTimer.TimeLeft == 0) //If collider is Bucket
		{
			bucket.Damage(5); //Hurt Bucket
			attackTimer.Start();
		}

		MoveAndSlide();
	}
}
