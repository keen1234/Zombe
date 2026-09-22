using Godot;

public partial class Player : RigidBody3D
{
	[Export]
	public float Speed { get; set; } = 5.0f;

	public override void _PhysicsProcess(double delta)
	{
		var direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		if (Input.IsPhysicalKeyPressed(Key.A))
			direction.X -= 1.0f;
		if (Input.IsPhysicalKeyPressed(Key.D))
			direction.X += 1.0f;
		if (Input.IsPhysicalKeyPressed(Key.W))
			direction.Y -= 1.0f;
		if (Input.IsPhysicalKeyPressed(Key.S))
			direction.Y += 1.0f;

		if (direction.LengthSquared() > 1.0f)
			direction = direction.Normalized();

		LinearVelocity = new Vector3(direction.X * Speed, LinearVelocity.Y, direction.Y * Speed);
	}
}
