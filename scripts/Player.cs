using Godot;

public partial class Player : RigidBody3D
{
	[Export]
	public float Speed { get; set; } = 5.0f;

	[Export]
	public NodePath CameraPath { get; set; } = new("../Camera3D");

	private Camera3D _camera;

	public override void _Ready()
	{
		_camera = GetNode<Camera3D>(CameraPath);
	}

	public override void _PhysicsProcess(double delta)
	{
		var input = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		if (Input.IsPhysicalKeyPressed(Key.A))
			input.X -= 1.0f;
		if (Input.IsPhysicalKeyPressed(Key.D))
			input.X += 1.0f;
		if (Input.IsPhysicalKeyPressed(Key.W))
			input.Y -= 1.0f;
		if (Input.IsPhysicalKeyPressed(Key.S))
			input.Y += 1.0f;

		if (input.LengthSquared() > 1.0f)
			input = input.Normalized();

		var forward = -_camera.GlobalBasis.Z;
		forward.Y = 0.0f;
		forward = forward.Normalized();

		var right = _camera.GlobalBasis.X;
		right.Y = 0.0f;
		right = right.Normalized();

		var movement = right * input.X + forward * -input.Y;
		LinearVelocity = new Vector3(movement.X * Speed, LinearVelocity.Y, movement.Z * Speed);
	}
}
