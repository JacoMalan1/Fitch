//
// Created by jacom on 2019/04/08.
//

#ifndef FITCH_MESH_H
#define FITCH_MESH_H

#include <vector>
#include <glm/glm.hpp>
#include "Drawable.h"
#include "Shader.h"
#include "VBO.h"
#include "VAO.h"
#include "Texture2D.h"

class Mesh : Drawable {

private:
    std::vector<float> vertexList;
    std::vector<unsigned int> indexList;
    unsigned int curIndex = 0;

    Shader* shaderProgram;

    VBO vbo; // Vertex buffer Object
    VBO ibo; // Index buffer Object
    VAO vao; // Vertex Attribute Object

    glm::mat4 drawMat;

    Texture2D texture;

public:
    Mesh();

    void setTexture(Texture2D tex);
    void setMatrix(glm::mat4 mat) override;
    void addMeshElement(std::shared_ptr<float[]> data, int count, int size);
    void addMeshElement(float* data, int count, int size);
    void setShader(Shader* shader);

    void init() override;
    void update() override;
    void draw() override;

};

#endif //FITCH_MESH_H
