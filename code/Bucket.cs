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
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}
		GD.Print(velocity.Y);

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}
		else if (Input.IsActionJustPressed("jump") && IsOnWall())
		{
			velocity.Y = JumpVelocity * (float)1;
			velocity.X = JumpVelocity * (float)0.5;
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
