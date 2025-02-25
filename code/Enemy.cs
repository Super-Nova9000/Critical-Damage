using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

public partial class Enemy : CharacterBody2D
{
	private Bucket bucket;
	private Godot.Timer attackTimer;
	private Godot.Timer patrolTimer;
	private Godot.RayCast2D rayCast;

	public const float Speed = 100.0f;
	public const float JumpVelocity = -575.0f;

	public override void _Ready()
	{
		var root = GetParent();
		bucket = (Bucket)root.GetNode("Bucket"); //Retrieve Bucket

		attackTimer = GetNode<Godot.Timer>("AttackTimer");
		patrolTimer = GetNode<Godot.Timer>("PatrolTimer");
		//Get timers
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity;

		var collisionData = GetLastSlideCollision(); //Get collision data
		Vector2 bucketPos = bucket.GetPos();

		if (collisionData != null && collisionData.GetCollider() == bucket && attackTimer.TimeLeft == 0) //If collider is Bucket
		{
			bucket.Damage(5); //Hurt Bucket
			attackTimer.Start();
		}

		//Behaviour queueing
		bool agro = isInRange(bucketPos);
		if (agro)
		{
			velocity = Agro(bucketPos, (float)delta);
		}
		else
		{
			velocity = Vector2.Zero; //Make player idle
		}

		string castSide = null;
		if (velocity[0] < 0) //Change position of rayCast based on what direction enemy is moving
		{
			castSide = "RayCastL";
		}
		else if (velocity[0] > 0)
		{
			castSide = "RayCastR";
		}

		GD.Print(castSide);

		if (castSide != null)
		{
			rayCast = GetNode<Godot.RayCast2D>(castSide);

			if (!rayCast.IsColliding()) { velocity = new Vector2(0, velocity[1]); }
		}
		else
		{
			velocity = new Vector2(0, velocity[1]);
		}

		Velocity = velocity;

		MoveAndSlide();
	}

	private bool isInRange(Vector2 tarPos)//Check if player within agro range of enemy
	{
		int xRange = 10;
		int yRange = 3;
		//Range where enemy can "see" in tiles

		bool inRangeX = (tarPos[0] > Position[0] - (xRange * 96)) && (tarPos[0] < Position[0] + (xRange * 96));
		bool inRangeY = (tarPos[1] > Position[1] - (yRange * 96)) && (tarPos[1] < Position[1] + (yRange * 96));
		//Check if player within range

		return (inRangeX && inRangeY);
		//return result
	}
	private Vector2 Agro(Vector2 tarPos, float delta) //Controls enemy when agro'ed onto bucket
	{
		Vector2 velocity; //Holds behaviour
		bool invVel = tarPos[0] < Position[0]; //If bucket on L/R side
		if (invVel)
		{
			velocity = new Vector2(-Speed, 0); //Move L
		}
		else
		{
			velocity = new Vector2(Speed, 0); //Move R
		}

		if (!IsOnFloor())
		{
			velocity += GetGravity(); //Apply gravity if not on floor
		}

		float xDiff = Math.Abs(Position[0] - tarPos[0]); //Absolute position difference between bucket and enemy
		if (xDiff <= 10 && tarPos[1] > Position[1]) //if within distance threshold and is not below enemy
		{
			velocity = new Vector2(velocity[0], JumpVelocity * (float)delta); //Jump
		}

		return velocity;
	}
}
