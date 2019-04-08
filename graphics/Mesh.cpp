#include "Mesh.h"

Mesh::Mesh() : drawMat(glm::mat4(1)), shaderProgram(new Shader(0)), texture(Texture2D(0, 0, 0)) {

}

void Mesh::setTexture(Texture2D tex) {
    this->texture = tex;
}

void Mesh::addMeshElement(float* data, int count, int size) {

    for (int i = 0; i < count * size; i++) {
        vertexList.emplace_back(data[i]);
    }

    int index = curIndex;
    int indexData[] = {

            index, index + 1, index + 2,
            index, index + 2, index + 3

    };

    for (int i : indexData)
        indexList.emplace_back(i);

    curIndex += 4;

}

void Mesh::setShader(Shader *shader) { this->shaderProgram = shader; }

void Mesh::setMatrix(glm::mat4 mat) { this->drawMat = mat; }

void Mesh::init() {

    if (!shaderProgram->isCompiled())
        shaderProgram->compile();

    vao = VAO::create();
    vbo = VBO::create(GL_ARRAY_BUFFER);
    ibo = VBO::create(GL_ELEMENT_ARRAY_BUFFER);

    vao.bind();
    vbo.bind();
    ibo.bind();

    float* vertexData = vertexList.data();
    auto* indexData = indexList.data();

    glBufferData(GL_ARRAY_BUFFER, vertexList.size() * sizeof(float), vertexData, GL_DYNAMIC_DRAW);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, indexList.size() * sizeof(int), indexData, GL_DYNAMIC_DRAW);

}

void Mesh::update() {}

void Mesh::draw() {

    vao.bind();
    shaderProgram->bind();
    vbo.bind();
    ibo.bind();
    texture.bind();

    GLint MatrixID = glGetUniformLocation(this->shaderProgram->getID(), "projMat");
    glUniformMatrix4fv(MatrixID, 1, GL_FALSE, &drawMat[0][0]);

    glEnableVertexAttribArray(0);
    glEnableVertexAttribArray(1);

    glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), nullptr);
    glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), (void*)(2 * sizeof(float)));

    for (int i = 0; i < indexList.size() / 6; i++) {

        glDrawElements(GL_TRIANGLES, i + 6, GL_UNSIGNED_INT, nullptr);

    }

    glDisableVertexAttribArray(0);
    glDisableVertexAttribArray(1);

}