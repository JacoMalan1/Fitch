#version 330

in vec2 texCoord;
out vec4 fragcolor;

uniform sampler2D texSampler;

void main() {

    fragcolor = texture(texSampler, texCoord);

    if (fragcolor.a <= 0.2) {
        discard;
    }

}
