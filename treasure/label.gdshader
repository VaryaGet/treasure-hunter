// label_fade.shader
shader_type canvas_item;

uniform float alpha : hint_range(0, 1) = 1.0;
uniform float speed = 10.0;

void vertex(){
	VERTEX += vec2(0, speed * (-1.0 + alpha));
}

void fragment() {
    COLOR.a *= alpha;
}
