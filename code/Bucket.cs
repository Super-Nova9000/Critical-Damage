using Godot;
using System;

public partial class Bucket : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	private float flySpeed = 0;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		//Gravity
		if (!IsOnFloor() && !IsOnWall()) //If not on floor AND not on ground
		{
			velocity += GetGravity() * (float)delta; //Accelerate under gravity
		}
		else if (!IsOnFloor() && IsOnWall()) //If not on floor BUT on ground
		{
			if (Velocity.Y < 0) //If moving upwards
			{
				velocity += GetGravity() * (float)delta; //Accelerate under gravity
			}
			else //If moving downwards
			{
				velocity += (GetGravity() / 2) * (float)delta; //Accelerate under half gravity
			}
			velocity.Y = Mathf.Clamp(velocity.Y, 0.5f * JumpVelocity, -JumpVelocity); //Clamp velocity
		}

		//Jumping
		if (Input.IsActionJustPressed("jump") && IsOnFloor()) //If on floor AND jump attempted
		{
			velocity.Y = JumpVelocity; //Jump
		}
		else if (Input.IsActionJustPressed("jump") && IsOnWall() && !IsOnFloor()) //If against wall, not against wall AND jump attempted
																				  //The "if !IsOnFloor" is provided by the initial query in the function (line 34)
		{
			velocity.Y = JumpVelocity; //Jump

			var wallPos = GetLastSlideCollision().GetPosition();
			//Get position of object that was just collided with

			if (wallPos.X < Position.X) //If wall that was just hit is to the left
			{
				velocity.X = JumpVelocity * (float)-0.5; //Move away from wall
			}
			else //If wall that was just hit is to the right
			{
				velocity.X = JumpVelocity * (float)0.5; //Move away from wall
			}
		}

		//Left / Right movement
		Vector2 direction = Input.GetVector("move_left", "move_right", "jump", "ui_down");
		//Resolve vector from pressed movement keys

		if (direction != Vector2.Zero && IsOnFloor()) //If movement keys pressed AND on floor
		{
			velocity.X = direction.X * Speed; //Turn lateral movement keys into velocity
		}
		else if (direction != Vector2.Zero && !IsOnFloor()) //If movement keys pressed AND NOT on floor
		{
			velocity.X += (Speed * (float)0.05) * direction.X; //Lateral acceleration instead of instant velocity
			velocity.X = Mathf.Clamp(velocity.X, -Speed, Speed);
			//Clamp speed so Bucket travels at the same max speed in the air and on the ground
		}
		else if (direction == Vector2.Zero && IsOnFloor()) //If not moving AND on floor
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed); //This was preset, it stops the character
		}

		Velocity = velocity; //Change variable to Godot value
		MoveAndSlide(); //Process movement commands

		//Animations
		var animate = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		if (velocity.Y > 0) //If falling
		{
			animate.Play();
			animate.Animation = "Fall";

			if (velocity.X != 0) //If X velocity is NOT 0
								 //This is here to preserve the direction Bucket is looking in when jumping while otherwise stationary
			{
				animate.FlipH = (velocity.X < 0); //Flip animation image if moving left
			}
		}
		else if (velocity.X != 0) //When moving laterally
		{
			animate.Play();
			animate.Animation = "Walk";

			animate.FlipH = (velocity.X < 0); //Flip animation image if moving left
		}
		else
		{
			animate.Play();
			animate.Animation = "Stand"; //Standing still
		}
	}
}
