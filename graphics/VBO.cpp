//
// Created by jacom on 2018/09/18.
//

#include "VBO.h"

VBO::VBO(GLuint id, GLenum type) : id(id), type(type) {}
VBO::VBO() = default;

VBO VBO::create(GLenum type) {

    GLuint id;
    glCreateBuffers(1, &id);

    return VBO(id, type);

}

void VBO::bind() {
    glBindBuffer(this->type, this->id);
}
