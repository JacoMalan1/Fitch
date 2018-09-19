//
// Created by jacom on 2018/09/18.
//

#ifndef FITCH_VAO_H
#define FITCH_VAO_H

#include <GL/glew.h>

struct VAO {

    GLuint id;
    void bind();
    explicit VAO(GLuint id);
    VAO();

    static VAO create();

};


#endif //FITCH_VAO_H
