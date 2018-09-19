//
// Created by jacom on 2018/09/18.
//

#ifndef FITCH_VBO_H
#define FITCH_VBO_H


#include <GL/glew.h>

struct VBO {

    GLuint id;
    GLenum type;

    explicit VBO(GLuint id, GLenum type);
    VBO();
    static VBO create(GLenum type);
    void bind();

};


#endif //FITCH_VBO_H
