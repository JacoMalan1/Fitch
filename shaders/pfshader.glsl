#version 330

in vec2 texCoord;
out vec3 fragcolor;

uniform sampler2D texSampler;

void main() {

    fragcolor = texture(texSampler, texCoord).rgb;

}
