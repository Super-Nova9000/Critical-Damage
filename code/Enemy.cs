using Godot;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

public partial class Enemy : CharacterBody2D
{
	Random rnd = new Random(); //Random numbers class
	private Bucket bucket; //Bucket
	private Godot.Timer attackTimer; //Timer to space out attacks
	private Godot.Timer patrolTimer; //Timer to take a break while idling
	private Godot.RayCast2D rayCast; //Raycast to stop from falling off things
	private int target; //Target position in tiles
	public Godot.Vector2 startPos; //Starting position as a vector2

	public const float Speed = 200.0f; //Max speed of enemy

	public override void _Ready()
	{
		var root = GetParent();
		bucket = (Bucket)root.GetNode("Bucket"); //Retrieve Bucket
		startPos = Position;

		attackTimer = GetNode<Godot.Timer>("AttackTimer");
		patrolTimer = GetNode<Godot.Timer>("PatrolTimer");
		//Get timers
	}

	public override void _PhysicsProcess(double delta)
	{
		Godot.Vector2 velocity;

		var collisionData = GetLastSlideCollision(); //Get collision data
		Godot.Vector2 bucketPos = bucket.GetPos();

		if (collisionData != null && collisionData.GetCollider() == bucket && attackTimer.TimeLeft == 0) //If collider is Bucket
		{
			bucket.Damage(5); //Hurt Bucket
			attackTimer.Start();
		}

		//Behaviour queueing
		bool agro = isInRange(bucketPos); //If player is in range of enemy
		if (agro)
		{
			velocity = Agro(bucketPos, (float)delta); //Agro behaviour
		}
		else
		{
			velocity = Idle(); //Make player idle
		}

		if (!checkCast(velocity)) { velocity = new Godot.Vector2(0, velocity[1]); } //If raycast sees no floor, stop enemy from moving

		Velocity = velocity;

		MoveAndSlide();
	}

	private bool isInRange(Godot.Vector2 tarPos)//Check if player within agro range of enemy
	{
		int xRange = 7; //Enemy horizontal vision (tiles)
		int yRange = 4; //Enemy vertical vision (tiles)
		int rangeBoundary = 90; //If enemy within +/- of value (pixels), stop moving
								//This means the enemy will wait underneather something if it sees Bucket

		bool inRangeX = (tarPos[0] > Position[0] - (xRange * 96)) && (tarPos[0] < Position[0] + (xRange * 96));
		bool inRangeY = (tarPos[1] > Position[1] - (yRange * 96)) && (tarPos[1] < Position[1] + (yRange * 96));
		//Check if player within range

		bool withinRange = (Position[0] + rangeBoundary) < tarPos[0] || (Position[0] - rangeBoundary) > tarPos[0]; //Check if bucket in same tile

		return (inRangeX && inRangeY) && withinRange;
		//return result
	}
	private Godot.Vector2 Agro(Godot.Vector2 tarPos, float delta) //Controls enemy when agro'ed onto bucket
	{
		Godot.Vector2 velocity; //Holds behaviour
		bool invVel = tarPos[0] < Position[0]; //If bucket on L/R side
		if (invVel)
		{
			velocity = new Godot.Vector2(-Speed, 0); //Move L
		}
		else
		{
			velocity = new Godot.Vector2(Speed, 0); //Move R
		}

		if (!IsOnFloor())
		{
			velocity += GetGravity(); //Apply gravity if not on floor
		}

		return velocity;
	}

	Godot.Vector2 idleVelocity; //Velocity variable for Idle(), since it was bugging
	private Godot.Vector2 Idle() //Idle behaviour
	{
		int tilePos = (int)(Position[0] / 96); //Convert current position into tiles

		if (tilePos == target || patrolTimer.TimeLeft != 0) //Check if enemy is at target, or that the enemy is still taking a break
		{
			if (patrolTimer.TimeLeft != 0) //If enemy is still taking a break
			{
				idleVelocity[0] = 0;
			}
			else
			{
				patrolTimer.Start(); //Take a break
				NewTarget(); //Get a new target
			}
		}
		else //If not at target, or taking a break
		{
			if (tilePos > target) //If target on left
			{
				idleVelocity[0] = -Speed / 2; //Move left at half speed
			}
			else //If target on right
			{
				idleVelocity[0] = Speed / 2; //Move right at half speed
			}

			if (!checkCast(idleVelocity) || IsOnWall()) //Check if movement in desired direction is possible
			{ //If not possible
				idleVelocity[0] = 0; //Stop
				NewTarget(); //Get a new target
			}
		}

		if (!IsOnFloor()) { idleVelocity += GetGravity(); } //If not on floor, apply gravity

		return idleVelocity;
	}
	private void NewTarget()
	{
		int idleRange = 10; //Range from startpos that new targets can be generated in
		target = (int)(rnd.Next(-idleRange, idleRange) + (startPos[0] / 96)); //Generate new target (in tiles)
	}
	private bool checkCast(Godot.Vector2 velocity) //Returns true if raycast sees a floor, else returns false
	{
		string castSide = null;
		if (velocity[0] < 0) //Change position of rayCast based on what direction enemy is moving
		{
			castSide = "RayCastL"; //Left side
		}
		else if (velocity[0] > 0)
		{
			castSide = "RayCastR"; //Right side
		}

		if (castSide != null)
		{
			rayCast = GetNode<Godot.RayCast2D>(castSide); //Get raycast, dependent on direction in castSide

			return rayCast.IsColliding();
		}
		else
		{
			return true;
		}
	}
}
