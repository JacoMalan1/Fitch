//
// Created by jacom on 2018/08/22.
//

#ifndef FITCH_RENDERABLE_H
#define FITCH_RENDERABLE_H
#include <GL/glew.h>

class Renderable {

private:
    GLuint vertexArrayName;
    GLuint bufferName;
    GLuint shaderProgramName;

public:
    Renderable();
    virtual void initBuffer() = 0;
    virtual void resendBuffer() = 0;
    virtual void render() = 0;
    virtual void initShaders() = 0;

};


#endif //FITCH_RENDERABLE_H
