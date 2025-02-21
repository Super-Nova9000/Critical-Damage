using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

public partial class Enemy : CharacterBody2D
{
	private Bucket bucket;
	private Godot.Timer attackTimer;
	private Godot.Timer patrolTimer;

	public const float Speed = 200.0f;
	public const float JumpVelocity = -575.0f;

	public override void _Ready()
	{
		var root = GetParent();
		bucket = (Bucket)root.GetNode("Bucket"); //Retrieve Bucket

		attackTimer = GetNode<Godot.Timer>("AttackTimer");
		patrolTimer = GetNode<Godot.Timer>("PatrolTimer");
	}

	public override void _PhysicsProcess(double delta)
	{
		var collisionData = GetLastSlideCollision(); //Get collision data
		Vector2 bucketPos = bucket.GetPos();

		if (collisionData != null && collisionData.GetCollider() == bucket && attackTimer.TimeLeft == 0) //If collider is Bucket
		{
			bucket.Damage(5); //Hurt Bucket
			attackTimer.Start();
		}

		bool agro = isInRange(bucketPos);
		if (agro)
		{
			Velocity = playerAgro(bucketPos, (float)delta);
		}
		else
		{
			Velocity = Vector2.Zero;
		}

		MoveAndSlide();
	}

	private bool isInRange(Vector2 tarPos)
	{
		bool inRangeX = (tarPos[0] > Position[0] - (10 * 96)) && (tarPos[0] < Position[0] + (10 * 96));
		bool inRangeY = (tarPos[1] > Position[1] - (3 * 96)) && (tarPos[1] < Position[1] + (3 * 96));
		return inRangeX && inRangeY;
	}
	private Vector2 playerAgro(Vector2 tarPos, float delta)
	{
		Vector2 velocity;
		bool invVel = tarPos[0] < Position[0];
		if (invVel)
		{
			velocity = new Vector2(-Speed, 0);
		}
		else
		{
			velocity = new Vector2(Speed, 0);
		}

		if (!IsOnFloor())
		{
			velocity += GetGravity();
		}

		float xDiff = Math.Abs(Position[0] - tarPos[0]);
		if (xDiff <= 10 && tarPos[1] > Position[1])
		{
			velocity = new Vector2(velocity[0], JumpVelocity * (float)delta);
		}
		return velocity;
	}
}
