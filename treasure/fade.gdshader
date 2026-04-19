// label_fade.shader
shader_type canvas_item;

uniform float alpha : hint_range(0, 1) = 1.0;

void fragment() {
    COLOR.a *= alpha;
}
