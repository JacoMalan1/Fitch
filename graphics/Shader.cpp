//
// Created by jacom on 2018/09/16.
//

#include <iostream>
#include <cstring>
#include "Shader.h"
#include "../tools.h"

Shader::Shader() = default;

Shader::Shader(const char *vertex_location, const char *fragment_location) : vertex_location(vertex_location), fragment_location(fragment_location) {
    this->id = glCreateProgram();
}

Shader::Shader(unsigned int id) : id(id), vertex_location(""), fragment_location("") {}

GLuint Shader::getID() { return this->id; }

static void checkCompile(GLuint shader, GLenum type) {

    GLint status = 0;
    glGetShaderiv(shader, GL_COMPILE_STATUS, &status);

    if (status == GL_FALSE) {

        GLint logLength = 0;
        glGetShaderiv(shader, GL_INFO_LOG_LENGTH, &logLength);

        char* log = (char*)alloca(logLength * sizeof(char));
        glGetShaderInfoLog(shader, logLength, &logLength, log);

        std::cerr << "Failed to compile " << (type == GL_VERTEX_SHADER ? "vertex" : "fragment") << " shader!" << std::endl;
        std::cerr << log << std::endl;

    }

}

void Shader::compile() {

    // Checks wether source locations were specified.
    if (strcmp(this->vertex_location, "") == 0 || strcmp(this->fragment_location, "") == 0)
        return;

    GLuint vshader = glCreateShader(GL_VERTEX_SHADER);
    GLuint fshader = glCreateShader(GL_FRAGMENT_SHADER);

    const char* vss = fitchio::loadFile(this->vertex_location);
    const char* fss = fitchio::loadFile(this->fragment_location);

    glShaderSource(vshader, 1, &vss, nullptr);
    glShaderSource(fshader, 1, &fss, nullptr);

    glCompileShader(vshader);
    glCompileShader(fshader);

    checkCompile(vshader, GL_VERTEX_SHADER);
    checkCompile(fshader, GL_FRAGMENT_SHADER);

    glAttachShader(this->id, vshader);
    glAttachShader(this->id, fshader);

    glLinkProgram(this->id);

    delete[] vss;
    delete[] fss;

    glDeleteShader(vshader);
    glDeleteShader(fshader);

    compiled = true;

    // std::cout << "Compiled shader: " << this->id << std::endl;

}

Shader::~Shader() {
    glDeleteProgram(this->id);
}

void Shader::bind() const {
    glUseProgram(this->id);
}

bool Shader::isCompiled() const { return compiled; }