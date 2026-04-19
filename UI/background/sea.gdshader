shader_type canvas_item;

uniform float wave_strength = 0.010;
uniform float wave_speed = 1.0;
uniform float effect_height = 0.2; // Высота эффекта (0-1, где 0.5 = верхняя половина)

void fragment() {
    vec2 uv = UV;
    
    // Создаем маску: 1 вверху, 0 внизу
    float mask = 1.0 - smoothstep(0.0, effect_height, uv.y);
    
    // Эффект применяется только где mask > 0
    vec2 distorted_uv = uv;
    distorted_uv.x += sin(uv.y * 50.0 + TIME * wave_speed) * wave_strength * mask;
    distorted_uv.x += sin(uv.y * 100.0 + TIME * 5.0) * wave_strength * 0.5 * mask;
    
    COLOR = texture(TEXTURE, distorted_uv);
}