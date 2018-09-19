//
// Created by jacom on 2018/08/15.
//

#include <glm/glm.hpp>

using namespace glm;

struct Vertex {

    vec3 position;
    vec3 color;

    Vertex(vec3 position, vec3 color) {

        this->position = position;
        this->color = color;

    }

};