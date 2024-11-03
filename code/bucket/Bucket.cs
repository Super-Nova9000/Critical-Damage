using Godot;
using System;

public partial class Bucket : Node2D
{

	[Signal]
	public delegate void HitEventHandler();

	[Export]
	public int gravity = 1; //Gravity, global value
	[Export]
	public int bSpeed = 400; //Bucket's speed



	private Vector2 velocity; //Create velocity with data type Vector2
	private bool onBot = false; //When lower hitbox
	private bool onTop = false; //When top hitbox
	private bool onLeft = false; //When left hitbox
	private bool onRight = false; //When right hitbox

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var velocity = Vector2.Zero; //Set X and Y velocities to zero
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("move_left") && onBot && !onLeft) //When A key pressed
		{
			velocity.X = -1; //Set X velocity
		}
		else if (Input.IsActionPressed("move_left") && !onBot && !onLeft) //When A key pressed and NOT on ground
		{
			velocity.X += (float)(-0.75 * delta); //Slowly change velocity
			velocity.X = Mathf.Clamp(velocity.X, -1, 1); //Clamp velocity
		}

		if (Input.IsActionPressed("move_right") && onBot && !onRight) //When D key pressed
		{
			velocity.X = 1; //Set X velocity
		}
		else if (Input.IsActionPressed("move_right") && !onBot && !onRight) //When D key pressed and NOT on ground
		{
			velocity.X += (float)(0.75 * delta); //Slowly change velocity
			velocity.X = Mathf.Clamp(velocity.X, -1, 1); //Clamp velocity
		}

		if (!(Input.IsActionPressed("move_left")) && !(Input.IsActionPressed("move_right")) && onBot) //If no lateral key pressed
		{
			velocity.X = 0; //No velocity
		}

		if (Input.IsActionPressed("jump") && onBot) //When space pressed
		{
			velocity.Y = -1; //Set vertical velocity
		}
		else if (!onBot) //If not on ground
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

	private void _on_bot_area_entered(Area2D area)
	{
		GD.Print("BotHit");
		onBot = true;
		GD.Print(contactPos);
		Position = new Godot.Vector2(Position.X, contactPos);
	}
	private void _on_bot_area_exited(Area2D area) //When leaving area
	{
		onBot = false;
	}

	private void _on_top_area_entered(Area2D area)
	{
		onTop = true;
		GD.Print("TopHit");
		GD.Print(velocity.Y);
		if (velocity.Y < 0)
		{
			velocity.Y = (-1) * velocity.Y;
			GD.Print(velocity.Y);
		}
	}
	private void _on_top_area_exited(Area2D area) //When leaving area
	{
		onTop = false;
	}

	private void _on_right_area_entered(Area2D area)
	{
		onRight = true;
		GD.Print("RightHit");
		GD.Print(velocity.X);
		velocity.X = (-1) * velocity.X;
		GD.Print(velocity.X);

	}
	private void _on_right_area_exited(Area2D area) //When leaving area
	{
		onRight = false;
	}

	private void _on_left_area_entered(Area2D area)
	{
		onLeft = true;
		GD.Print("LeftHit");
		GD.Print(velocity.X);
		velocity.X = (-1) * velocity.X;
		GD.Print(velocity.X);
	}
	private void _on_left_area_exited(Area2D area) //When leaving area
	{
		onLeft = false;
	}


	private void _on_platform_bot_hit(Area2D area)
	{
		GD.Print("BotHit");
	}
}
