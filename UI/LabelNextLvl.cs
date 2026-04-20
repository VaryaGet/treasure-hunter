using Godot;
using System;
using System.Globalization;
using TreasureHunter.balance;

public partial class LabelNextLvl : Label
{
	private Btn parent;

	public void init()
	{
		parent = GetParent<Btn>();
		parent.Upgraded += UpdateText;
		GD.Print(parent.balance.Balanced(parent.type, 1).Cost);
		BValue value = parent.balance.Balanced(parent.type, parent.state.currentLevel(parent.type));
		UpdateText(parent.type, parent.state.currentLevel(parent.type), (float) value.Value, (float) value.Cost);
		AddThemeColorOverride("font_color", new Color(0, 0, 0));
		AddThemeFontSizeOverride("font_size", 10);
	}

	private void UpdateText(UpgradeType type, int level, float value, float cost)
	{
		int levelNext = level + 1;
		if (!parent.balance.Checked(type, levelNext))
		{
			Text = "lvl" + level;
		}
		else
		{
			Text = "lvl" + level + "->" + levelNext;
		}
	}
}
