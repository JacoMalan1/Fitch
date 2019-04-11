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

void VBO::sendData(std::shared_ptr<float[]> data, int elems, GLenum drawType) {

    sendData(&data[0], elems, drawType);

}

void VBO::sendData(float* data, int elems, GLenum drawType) {

    this->bind();

    if (!sent) {

        glBufferData(this->type, sizeof(float) * elems, data, drawType);

    } else {

        glBufferSubData(this->type, 0, sizeof(float) * elems, data);

    }


}