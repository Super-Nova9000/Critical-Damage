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


	public int[] hitPos = new int[2];
	private Vector2 velocity; //Create velocity with data type Vector2
	private bool onBot = false; //When lower hitbox
	private bool onLeft = false; //When left hitbox
	private bool onRight = false; //When right hitbox
	private bool onTop = false; //When right hitbox


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var velocity = Vector2.Zero; //Set X and Y velocities to zero
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public async override void _Process(double delta)
	{
		if (onTop)
		{
			velocity.Y = (-1) * velocity.Y; //invert vertical velocity
			onTop = false;
			onRight = false;
			onLeft = false;
		}
		else if (onLeft && !onBot && velocity.X < 0)
		{
			Position = new Godot.Vector2(hitPos[0] + 98, Position.Y);
			velocity.X = velocity.X * (float)-0.75;
			onBot = false;
		}
		else if (onRight && !onBot && velocity.X > 0)
		{
			Position = new Godot.Vector2(hitPos[0] - 98, Position.Y);
			velocity.X = velocity.X * (float)-0.75;
			onBot = false;
		}
		else if (onBot && !(onLeft || onRight))
		{
			await ToSignal(GetTree().CreateTimer(0.01f), SceneTreeTimer.SignalName.Timeout); //Delay 0.01s
			Position = new Godot.Vector2(Position.X, hitPos[1] - 97); //Snap to top of platform
		}

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
			velocity.Y = (float)-1.5; //Set vertical velocity
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

		if (velocity.Y < 0) //If lateral movement
		{
			bucketAnimate.Play();
			bucketAnimate.Animation = "fall";
			bucketAnimate.FlipV = false; //Don't flip vartically

			bucketAnimate.FlipH = velocity.X < 0; //If moving left, flip sprite
		}
		else if (velocity.X != 0) //If lateral movement
		{
			bucketAnimate.Play();
			bucketAnimate.Animation = "walk";
			bucketAnimate.FlipV = false; //Don't flip vartically

			bucketAnimate.FlipH = velocity.X < 0; //If moving left, flip sprite
		}
		else
		{
			bucketAnimate.Animation = "stand"; //Stop animation if not moving laterally
		}
	}

<<<<<<< HEAD
	private async void _on_bot_area_entered(Area2D area)
	{
		GD.Print("BotHit");
		onBot = true;
		await ToSignal(GetTree().CreateTimer(0.01f), SceneTreeTimer.SignalName.Timeout);
		GD.Print("bucket " + yTarget);
		Position = new Godot.Vector2(Position.X, yTarget);
=======
	private void _on_bot_area_entered(Area2D area) //If lower hitbox hits
	{
		onBot = true; //Disable gravity while on ground
>>>>>>> 85c1de641d3604480af0af9a988b6b199f287db2
	}
	private void _on_bot_area_exited(Area2D area) //When lower hitbox leaves
	{
		onBot = false; //Enable gravity while NOT on ground
	}

	private void _on_top_area_entered(Area2D area) //If hit head
	{
		onTop = true;
	} //If hit head, and moving up, invert Y velocity
	private void _on_top_area_exited(Area2D area) //If hit head
	{
		onTop = false;
	} //If hit head, and moving up, invert Y velocity

	private void _on_right_area_entered(Area2D area)
	{
		onRight = true;
	} //If hit right side, invert lateral velocity if in air, and stop right movement
	private void _on_right_area_exited(Area2D area)
	{
		onRight = false;
	}

	private void _on_left_area_entered(Area2D area)
	{
		onLeft = true;
	} //If hit left side, invert lateral velocity if in air, and stop left movement
	private void _on_left_area_exited(Area2D area)
	{
		onLeft = false;
	}
}
