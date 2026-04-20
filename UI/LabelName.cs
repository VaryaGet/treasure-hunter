using Godot;
using System;

public partial class LabelName : Label
{
	public override void _Ready()
	{
		Text = GetParent<Btn>().Named();
		AddThemeColorOverride("font_color", new Color(0, 0, 0));
		AddThemeFontSizeOverride("font_size", 9);
	}
}
