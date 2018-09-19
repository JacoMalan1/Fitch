//
// Created by jacom on 2018/09/18.
//

#include <GL/glew.h>
#include "VAO.h"

void VAO::bind() {
    glBindVertexArray(this->id);
}
VAO::VAO() = default;

VAO::VAO(GLuint id) : id(id) {}


VAO VAO::create() {

    GLuint id;
    glCreateVertexArrays(1, &id);

    return VAO(id);

}