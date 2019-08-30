#include <iostream>
#include <cstring>
#include <boost/format.hpp>
#include <sstream>
#include "Shader.h"
#include "../main.h"
#include "../tools.h"
#include "../logging/Logger.h"

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

        Logger& logger = fitch::getLogger();

        std::stringstream msg;
        msg << boost::format("Failed to compile %s shader (ID = %d)") % ((type == GL_VERTEX_SHADER) ? "vertex" : "fragment") % shader;
        std::string sLog = log;

        logger.addString("Shader", msg.str(), ERR);
        logger.addString("Shader", sLog, ERR);

    }

}

void Shader::compile() {

    fitch::getLogger().addString("Shader", (boost::format("Compiling shader %d") % id).str());

    // Checks wether source locations were specified.
    if (strcmp(this->vertex_location, "") == 0 || strcmp(this->fragment_location, "") == 0)
        return;

    GLuint vshader = glCreateShader(GL_VERTEX_SHADER);
    GLuint fshader = glCreateShader(GL_FRAGMENT_SHADER);

    auto vss_opt = fitchio::loadFile(this->vertex_location);
    auto fss_opt = fitchio::loadFile(this->fragment_location);

    if (!vss_opt || !fss_opt)
        fitch::getLogger().addString("Shader", "Error loading shader files!", ERR);

    const char* vss = vss_opt.value()->c_str();
    const char* fss = fss_opt.value()->c_str();

    glShaderSource(vshader, 1, &vss, nullptr);
    glShaderSource(fshader, 1, &fss, nullptr);

    glCompileShader(vshader);
    glCompileShader(fshader);

    checkCompile(vshader, GL_VERTEX_SHADER);
    checkCompile(fshader, GL_FRAGMENT_SHADER);

    glAttachShader(this->id, vshader);
    glAttachShader(this->id, fshader);

    glLinkProgram(this->id);

    delete vss_opt.value();
    delete fss_opt.value();

    glDeleteShader(vshader);
    glDeleteShader(fshader);

    compiled = true;

    fitch::getLogger().addString("Shader", (boost::format("Shader %d compiled successfully!") % this->id).str());

}

Shader::~Shader() {
    glDeleteProgram(this->id);
}

void Shader::bind() const {
    glUseProgram(this->id);
}

bool Shader::isCompiled() const { return compiled; }