using Godot;
using System;

public partial class Bucket : Area2D
{

	[Signal]
	public delegate void HitEventHandler();

	[Export]
	public int gravity = 1; //Gravity, global value
	[Export]
	public int bSpeed = 400; //Bucket's speed

	private Vector2 velocity; //Create velocity with data type Vector2
	private bool onGround = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var velocity = Vector2.Zero; //Set X and Y velocities to zero
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("move_left") && onGround) //When A key pressed
		{
			velocity.X = -1; //Set X velocity
		}
		else if (Input.IsActionPressed("move_left") && !onGround) //When A key pressed and NOT on ground
		{
			velocity.X += (float)(-0.75 * delta); //Slowly change velocity
			velocity.X = Mathf.Clamp(velocity.X, -1, 1); //Clamp velocity
		}

		if (Input.IsActionPressed("move_right") && onGround) //When D key pressed
		{
			velocity.X = 1; //Set X velocity
		}
		else if (Input.IsActionPressed("move_right") && !onGround) //When D key pressed and NOT on ground
		{
			velocity.X += (float)(0.75 * delta); //Slowly change velocity
			velocity.X = Mathf.Clamp(velocity.X, -1, 1); //Clamp velocity
		}

		if (!(Input.IsActionPressed("move_left")) && !(Input.IsActionPressed("move_right")) && onGround) //If no lateral key pressed
		{
			velocity.X = 0; //No velocity
		}

		if (Input.IsActionPressed("jump") && onGround) //When space pressed
		{
			velocity.Y = -1; //Set vertical velocity
		}
		else if (!onGround) //If not on ground
		{
			velocity.Y += gravity * (float)delta; //Slowly increase vertical velocity via gravity
			velocity.Y = Mathf.Clamp(velocity.Y, -2, 2); //Clamp velocity (twice fall speed)
		}
		else //If on ground AND space NOT pressed
		{
			velocity.Y = 0; //Reset vertical velocity
		}

		Position += (velocity * bSpeed) * (float)delta; //Update position

		var bucketAnimate = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		if (velocity.X != 0)
		{
			bucketAnimate.Play();
			bucketAnimate.Animation = "walk";
			bucketAnimate.FlipV = false;

			bucketAnimate.FlipH = velocity.X < 0;
		}
		else
		{
			bucketAnimate.Stop();
		}
	}

	private void _on_platform_top_hit()
	{
		GD.Print("TopHit");
		onGround = true;
	}
	private void _on_area_exited(Area2D area) //When leaving area
	{
		onGround = false;
	}


	private void _on_platform_bot_hit()
	{
		GD.Print("BotHit");
	}
}
