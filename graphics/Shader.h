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
    Shader(unsigned int id);
    Shader(const char* vertex_location, const char* fragment_location);
    Shader();
    Shader(const Shader& other) = default;
    ~Shader();
    void compile();
    void bind() const;
    GLuint getID();

};


#endif //FITCH_SHADER_H
