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

		// Add the gravity.
		if (!IsOnFloor() && !IsOnWall())
		{
			velocity += GetGravity() * (float)delta;
		}
		else if (!IsOnFloor() && IsOnWall())
		{
			if (Velocity.Y < 0)
			{
				velocity += GetGravity() * (float)delta;
			}
			else
			{
				velocity += (GetGravity() / 2) * (float)delta;
			}
			velocity.Y = Mathf.Clamp(velocity.Y, 0.5f * JumpVelocity, -JumpVelocity);
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}
		else if (Input.IsActionJustPressed("jump") && IsOnWall())
		{
			var wallPos = GetLastSlideCollision().GetPosition();

			velocity.Y = JumpVelocity;
			if (wallPos.X < Position.X)
			{
				velocity.X = JumpVelocity * (float)-0.5;
				GD.Print("Left");
			}
			else
			{
				velocity.X = JumpVelocity * (float)0.5;
				GD.Print("Right");
			}
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("move_left", "move_right", "jump", "ui_down");
		if (direction != Vector2.Zero && IsOnFloor())
		{
			velocity.X = direction.X * Speed;
		}
		else if (direction != Vector2.Zero && !IsOnFloor())
		{
			velocity.X += (Speed * (float)0.05) * direction.X;
			velocity.X = Mathf.Clamp(velocity.X, -Speed, Speed);
		}
		else if (direction == Vector2.Zero && IsOnFloor())
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();


		var animate = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		if (velocity.Y > 0)
		{
			animate.Play();
			animate.Animation = "Fall";

			animate.FlipH = (velocity.X < 0);
		}
		else if (velocity.X != 0)
		{
			animate.Play();
			animate.Animation = "Walk";

			animate.FlipH = (velocity.X < 0);
		}
		else
		{
			animate.Play();
			animate.Animation = "Stand";
		}
	}
}
