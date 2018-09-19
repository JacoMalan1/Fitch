//
// Created by jacom on 2018/09/16.
//

#include "Shader.h"
#include "../tools.h"

Shader::Shader() = default;

Shader::Shader(const char *vertex_location, const char *fragment_location) : vertex_location(vertex_location), fragment_location(fragment_location) {
    this->id = glCreateProgram();
}

GLuint Shader::getID() { return this->id; }

void Shader::compile() {

    GLuint vshader = glCreateShader(GL_VERTEX_SHADER);
    GLuint fshader = glCreateShader(GL_FRAGMENT_SHADER);

    const char* vss = fitchio::loadFile(this->vertex_location);
    const char* fss = fitchio::loadFile(this->fragment_location);

    glShaderSource(vshader, 1, &vss, nullptr);
    glShaderSource(fshader, 1, &fss, nullptr);

    glCompileShader(vshader);
    glCompileShader(fshader);

    glAttachShader(this->id, vshader);
    glAttachShader(this->id, fshader);

    glLinkProgram(this->id);

    delete[] vss;
    delete[] fss;

    glDeleteShader(vshader);
    glDeleteShader(fshader);

}
