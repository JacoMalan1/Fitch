#version 330

layout(location = 0) in vec2 vertexPosition;
layout(location = 1) in vec2 texCoordPosition;

uniform mat4 projMat;

out vec2 texCoord;

void main() {

    texCoord = texCoordPosition;
    gl_Position = projMat * vec4(vertexPosition, 1, 1);

}
