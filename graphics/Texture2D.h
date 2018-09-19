//
// Created by jacom on 2018/08/22.
//

#ifndef FITCH_TEXTURE2D_H
#define FITCH_TEXTURE2D_H


#include <GL/glew.h>

struct Texture2D {

    GLuint ID;
    int width, height;

    Texture2D();
    Texture2D(GLuint ID, int width, int height);

};


#endif //FITCH_TEXTURE2D_H
