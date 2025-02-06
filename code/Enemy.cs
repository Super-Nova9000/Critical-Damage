using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Enemy : CharacterBody2D
{
	public Bucket Bucket;
	public override void _PhysicsProcess(double delta)
	{

		var collisionData = GetLastSlideCollision();

		var root = GetParent();
		var bucket = root.GetNode<Bucket>("Bucket"); //Interact with Bucket

		if (collisionData.GetCollider() == bucket)
		{
			bucket.Damage(5);
		}
	}
}
