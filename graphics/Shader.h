//
// Created by jacom on 2018/09/16.
//

#ifndef FITCH_SHADER_H
#define FITCH_SHADER_H


#include <GL/glew.h>

class Shader {

private:
    GLuint id;

    const char* vertex_location;
    const char* fragment_location;

public:
    Shader(const char* vertex_location, const char* fragment_location);
    Shader();
    void compile();
    GLuint getID();

};


#endif //FITCH_SHADER_H
