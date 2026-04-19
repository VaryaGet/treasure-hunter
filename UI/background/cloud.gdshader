shader_type canvas_item;

uniform float speed = 0.5;          // скорость движения (чем больше, тем быстрее)
uniform float speed2 = 0.5;          // скорость движения (чем больше, тем быстрее)
uniform sampler2D cloud_texture : source_color;

//void fragment() {
    //// Смещаем UV по оси X, используя время TIME (автоматическая переменная Godot)
    //vec2 uv = UV;
    //uv.x += TIME * speed;
    //
    //// Зацикливаем координату, чтобы текстура бесконечно повторялась
    //uv.x = fract(uv.x);
    //
    //// Берём цвет из текстуры с новыми UV
    //COLOR = texture(cloud_texture, uv);
//}

void fragment() {
    vec2 tex_size = vec2(textureSize(cloud_texture, 0)); // размер текстуры в пикселях
    vec2 uv = UV;
    
    // Смещение в пикселях (с учётом времени)
    float offset_pixels = TIME * speed2;
    // Округляем до целого пикселя
    offset_pixels = floor(offset_pixels);
    // Переводим обратно в UV (0..1)
    uv.x += offset_pixels / tex_size.x;
    uv.x = fract(uv.x);
    
    COLOR = texture(cloud_texture, uv);
}
