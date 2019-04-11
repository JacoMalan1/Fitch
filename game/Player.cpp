#include <iostream>
#include <memory>
#include <Box2D/Collision/Shapes/b2PolygonShape.h>
#include "Player.h"
#include "../tools.h"
#include "../main.h"

Player::Player(const Player& other) : position(other.position), width(other.width), height(other.height),
    texture(other.texture), shaderProgram(other.shaderProgram), vao(other.vao),
            vbo(other.vbo)
{

}

Player::Player(glm::vec2 position, const char *texture_path) : position(position) {
    this->texture = fitchio::loadBMP(texture_path);
    this->width = this->texture.width;
    this->height = this->texture.height;
}

void Player::setPos(glm::vec2 pos) {
    this->position = pos;
}

glm::vec2 Player::getPos() const {
    return this->position;
}

void Player::handleInput(GLFWwindow* const window) {}

float Player::getWidth() const { return this->width; }
float Player::getHeight() const { return this->height; }

void Player::setMatrix(glm::mat4 mat) {
    this->drawMat = mat;
}

b2Body* Player::getBody() const { return physicsBody; }
void Player::setBody(b2Body *body) { physicsBody = body; }

void Player::initPhysics(b2World* world) {

    b2BodyDef bodyDef;
    bodyDef.position.Set(position.x, position.y);
    physicsBody = world->CreateBody(&bodyDef);

    b2PolygonShape box;
    box.SetAsBox(this->width, this->height);
    physicsBody->CreateFixture(&box, 0.0f);

}

void Player::init() {

    this->vao = VAO::create();
    this->vao.bind();
    this->vbo = VBO::create(GL_ARRAY_BUFFER);
    this->vbo.bind();
    this->shaderProgram = std::make_shared<Shader>("shaders/pvshader.glsl", "shaders/pfshader.glsl");
    this->shaderProgram->compile();

    std::shared_ptr<float[]> vertices(new float[16] {

            position.x, position.y, 0.0f, 0.0f,
            position.x + width, position.y, 1.0f, 0.0f,
            position.x, position.y + height, 0.0f, 1.0f,
            position.x + width, position.y + height, 1.0f, 1.0f

    });

    this->vbo.sendData(vertices, 16, GL_DYNAMIC_DRAW);

}

void Player::update() {

    b2Vec2 bPos = physicsBody->GetPosition();
    position = glm::vec2(bPos.x, bPos.y);

    std::shared_ptr<float[]> vertices(new float[16] {

            position.x, position.y, 0.0f, 0.0f,
            position.x + width, position.y, 1.0f, 0.0f,
            position.x, position.y + height, 0.0f, 1.0f,
            position.x + width, position.y + height, 1.0f, 1.0f

    });

    this->vbo.sendData(vertices, 16, GL_DYNAMIC_DRAW);

}

void Player::draw() {

    this->vao.bind();
    this->vbo.bind();
    this->shaderProgram->bind();
    this->texture.bind();

    GLint MatrixID = glGetUniformLocation(this->shaderProgram->getID(), "projMat");
    glUniformMatrix4fv(MatrixID, 1, GL_FALSE, &drawMat[0][0]);

    glEnableVertexAttribArray(0);
    glEnableVertexAttribArray(1);

    glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), nullptr);
    glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), (void*)(2 * sizeof(float)));

    glDrawArrays(GL_TRIANGLE_STRIP, 0, 4);

    glDisableVertexAttribArray(0);
    glDisableVertexAttribArray(1);

}
