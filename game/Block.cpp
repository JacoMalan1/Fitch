#include "Block.h"

Block::Block(glm::vec2 position, BlockType type) : position(position), type(type) {}

RigidBody* Block::asRBody() {

    auto rb = new RigidBody(position.x, position.y, BLOCK_SIZE, BLOCK_SIZE, 1.0f, All);
    return rb;

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
            this->position.x, this->position.y, 0.0f, 1.0f, 1.0f, 1.0f,
            this->position.x + BLOCK_SIZE, this->position.y, 0.0f, 1.0f, 1.0f, 1.0f,
            this->position.x, this->position.y + BLOCK_SIZE, 0.0f, 1.0f, 1.0f, 1.0f,
            this->position.x + BLOCK_SIZE, this->position.y + BLOCK_SIZE, 0.0f, 1.0f, 1.0f, 1.0f
    };

    glBufferData(this->buffer.type, sizeof(vertices), &vertices, GL_DYNAMIC_DRAW);

}

void Block::initShaders() {

    this->shader = Shader("shaders/vshader.glsl", "shaders/fshader.glsl");
    this->shader.compile();

}

void Block::render() {

    this->vertexArray.bind();
    this->buffer.bind();
    glUseProgram(this->shader.getID());

    glEnableVertexAttribArray(0);
    glEnableVertexAttribArray(1);

    glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(float), nullptr);
    glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(float), (void*)(3 * sizeof(float)));

    glDrawArrays(GL_QUADS, 0, 4);

    glDisableVertexAttribArray(0);
    glDisableVertexAttribArray(1);

}

void Block::render(const glm::mat4& projMat) {

    this->vertexArray.bind();
    this->buffer.bind();
    glUseProgram(this->shader.getID());

    GLint MatrixID = glGetUniformLocation(this->shader.getID(), "projMat");
    glUniformMatrix4fv(MatrixID, 1, GL_FALSE, &projMat[0][0]);

    glEnableVertexAttribArray(0);
    glEnableVertexAttribArray(1);

    glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(float), nullptr);
    glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(float), (void*)(3 * sizeof(float)));

    glDrawArrays(GL_TRIANGLE_STRIP, 0, 4);

    glDisableVertexAttribArray(0);
    glDisableVertexAttribArray(1);

}

void Block::resendBuffer() {

    this->vertexArray.bind();
    this->buffer.bind();

    float vertices[] = {
            this->position.x, this->position.y, 0.0f, 1.0f, 1.0f, 1.0f,
            this->position.x + BLOCK_SIZE, this->position.y, 0.0f, 1.0f, 1.0f, 1.0f,
            this->position.x, this->position.y + BLOCK_SIZE, 0.0f, 1.0f, 1.0f, 1.0f,
            this->position.x + BLOCK_SIZE, this->position.y + BLOCK_SIZE, 0.0f, 1.0f, 1.0f, 1.0f
    };

    glBufferSubData(this->buffer.type, 0, sizeof(vertices), &vertices);

}
