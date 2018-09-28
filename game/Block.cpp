#include "Block.h"
#include "../tools.h"

Block::Block(glm::vec2 position, BlockType type) : position(position), type(type) {}

RigidBody* Block::asRBody() {

    auto rb = new RigidBody(position.x, position.y, BLOCK_SIZE, BLOCK_SIZE, 1.0f, All);
    return rb;

}

Block::Block(glm::vec2 position, BlockType type, const char* texture_path) : position(position), type(type) {

    texture = fitchio::loadBMP(texture_path);

}

int Block::getSize() {

    return BLOCK_SIZE;

}

void Block::initBuffer() {

    this->vertexArray = VAO::create();
    this->vertexArray.bind();
    this->buffer = VBO::create(GL_ARRAY_BUFFER);
    this->buffer.bind();

    float vertices[] = {
            this->position.x, this->position.y, 0.0f, 0.0f,
            this->position.x + BLOCK_SIZE, this->position.y, 1.0f, 0.0f,
            this->position.x, this->position.y + BLOCK_SIZE, 0.0f, 1.0f,
            this->position.x + BLOCK_SIZE, this->position.y + BLOCK_SIZE, 1.0f, 1.0f
    };

    glBufferData(this->buffer.type, sizeof(vertices), &vertices, GL_DYNAMIC_DRAW);

}

void Block::initShaders() {

    this->shader = Shader("shaders/tvshader.glsl", "shaders/tfshader.glsl");
    this->shader.compile();

}

void Block::render() {

    this->vertexArray.bind();
    this->buffer.bind();
    this->texture.bind();
    glUseProgram(this->shader.getID());

    glEnableVertexAttribArray(0);
    glEnableVertexAttribArray(1);

    glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), nullptr);
    glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), (void*)(2 * sizeof(float)));

    glDrawArrays(GL_QUADS, 0, 4);

    glDisableVertexAttribArray(0);
    glDisableVertexAttribArray(1);

}

void Block::render(const glm::mat4& projMat) {

    this->vertexArray.bind();
    this->buffer.bind();
    this->texture.bind();
    glUseProgram(this->shader.getID());

    GLint MatrixID = glGetUniformLocation(this->shader.getID(), "projMat");
    glUniformMatrix4fv(MatrixID, 1, GL_FALSE, &projMat[0][0]);

    glEnableVertexAttribArray(0);
    glEnableVertexAttribArray(1);

    glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), nullptr);
    glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), (void*)(2 * sizeof(float)));

    glDrawArrays(GL_TRIANGLE_STRIP, 0, 4);

    glDisableVertexAttribArray(0);
    glDisableVertexAttribArray(1);

}

void Block::resendBuffer() {

    this->vertexArray.bind();
    this->buffer.bind();

    float vertices[] = {
            this->position.x, this->position.y, 0.0f, 0.0f,
            this->position.x + BLOCK_SIZE, this->position.y, 1.0f, 0.0f,
            this->position.x, this->position.y + BLOCK_SIZE, 0.0f, 1.0f,
            this->position.x + BLOCK_SIZE, this->position.y + BLOCK_SIZE, 1.0f, 1.0f
    };

    glBufferSubData(this->buffer.type, 0, sizeof(vertices), &vertices);

}
