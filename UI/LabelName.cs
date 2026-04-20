using Godot;
using System;

public partial class LabelName : Label
{
	public override void _Ready()
	{
		Text = GetParent<Btn>().Named();
	}
}
