using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class Radar : Area2D
{
    [Export(PropertyHint.Range, "0.0,1.0,0.01")]
    public float FindPercentage = 0.85f;

    public float Radius { get; private set; }

    private HashSet<TreasureBody> _treasuresInZone = new();
    private GradientTexture1D _texture;
    private Node2D _locator;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
        Radius = ((CircleShape2D)GetNode<CollisionShape2D>("CollisionShape2D").Shape).Radius;
        _texture = GetNode<Sprite2D>("Level").Texture as GradientTexture1D;
        _locator = GetNode<Node2D>("Locator");
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body.IsInGroup(Groups.TreasureCollision))
        {
            if (!((TreasureBody)body).IsFound)
            {
                _treasuresInZone.Add((TreasureBody)body);
            }
        }
    }

    private void OnBodyExited(Node2D body)
    {
        if (body.IsInGroup(Groups.TreasureCollision))
        {
            _treasuresInZone.Remove((TreasureBody)body);
        }
    }

    public override void _Process(double delta)
    {
        _treasuresInZone.RemoveWhere(t => !IsInstanceValid(t) || t.IsFound);

        if (_treasuresInZone.Count != 0)
        {
            var maxDist = 0f;
            TreasureBody nearestTreasure = null;
            foreach (var treasureBody in _treasuresInZone)
            {
                var dist = GlobalPosition.DistanceTo(treasureBody.GlobalPosition);
                var clamp = Mathf.Clamp(dist, 0, Radius);
                var nDist = 1 - Mathf.InverseLerp(0, Radius, clamp); // 0 - far 1 - close

                treasureBody.CheckTimer(nDist >= FindPercentage);

                if (maxDist < nDist)
                {
                    maxDist = nDist;
                    nearestTreasure = treasureBody;
                }
            }

            if (nearestTreasure != null)
            {
                UpdateLocator(nearestTreasure.GlobalPosition);
            }

            UpdateTexture(maxDist);
        }
        else
        {
            UpdateTexture(0);
            _locator.Hide();
        }
    }

    private void UpdateTexture(float value)
    {
        _texture.Gradient.SetOffset(1, value);
    }

    private void UpdateLocator(Vector2 target)
    {
        var direction = (target - GlobalPosition).Normalized();
        var angle = direction.Angle() - Mathf.Pi / 2;
        _locator.Rotation = angle;
        _locator.Show();
    }
}
