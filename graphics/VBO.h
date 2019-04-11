//
// Created by jacom on 2018/09/18.
//

#ifndef FITCH_VBO_H
#define FITCH_VBO_H


#include <GL/glew.h>
#include <memory>

struct VBO {

private:
    bool sent = false;

public:
    GLuint id;
    GLenum type;

    explicit VBO(GLuint id, GLenum type);
    VBO();
    static VBO create(GLenum type);
    void bind();
    void sendData(std::shared_ptr<float[]> data, int elems, GLenum drawType);
    void sendData(float* data, int elems, GLenum drawType);

};


#endif //FITCH_VBO_H
