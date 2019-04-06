#include <iostream>
#include <memory>
#include "Player.h"
#include "../tools.h"
#include "../main.h"

Player::Player(const Player& other) : position(other.position), velocity(other.velocity), acceleration(other.acceleration),
            width(other.width), height(other.height), texture(other.texture), shaderProgram(other.shaderProgram), vao(other.vao),
            vbo(other.vbo)
{

    this->collisionList = std::make_unique<std::vector<RigidBody*>>();
    *this->collisionList = *other.collisionList;

}

Player::Player(glm::vec2 position, const char *texture_path) : position(position) {
    this->collisionList = std::make_unique<std::vector<RigidBody*>>();
    this->velocity = glm::vec2(0, 0);
    this->acceleration = glm::vec2(0, 0);
    this->texture = fitchio::loadBMP(texture_path);
    this->width = this->texture.width;
    this->height = this->texture.height;
}

Player::Player(glm::vec2 position, float width, float vertexArrayheight) : position(position), width(width), height(height) {
    this->collisionList = std::make_unique<std::vector<RigidBody*>>();
    this->velocity = glm::vec2(0, 0);
    this->acceleration = glm::vec2(0, 0);
    this->texture = { 0, 0, 0 };
}

void Player::setPos(glm::vec2 pos) {
    this->position = pos;
}

glm::vec2 Player::getPos() {
    return this->position;
}

RigidBody* Player::asPBody() {
    auto rb = new RigidBody(this->position.x, this->position.y, this->width, this->height, 1.0f, All);
    rb->setVelocity(this->velocity);
    return rb;

}

void Player::applyForce(glm::vec2 force) {
    acceleration += force;
}

void Player::collideWith(RigidBody* body) {
    collisionList->emplace_back(body);
}

void Player::handleInput(GLFWwindow* const window) {

    if (glfwGetKey(window, GLFW_KEY_D) == GLFW_PRESS) {
        this->applyForce(glm::vec2((isStanding) ? 1.2f : 0.5f, 0));
        this->direction = Right;
    }
    if (glfwGetKey(window, GLFW_KEY_A) == GLFW_PRESS) {
        this->applyForce(glm::vec2((isStanding) ? -1.2f : -0.5f, 0));
        this->direction = Left;
    }
    if (glfwGetKey(window, GLFW_KEY_SPACE) == GLFW_PRESS && this->isStanding) {
        this->applyForce(glm::vec2(0, -30.0f));
        this->isStanding = false;
    }

}

float Player::getWidth() const { return this->width; }
float Player::getHeight() const { return this->height; }

void Player::setMatrix(glm::mat4 mat) {
    this->drawMat = mat;
}

Player::~Player() = default;

void Player::init() {

    this->vao = VAO::create();
    this->vao.bind();
    this->vbo = VBO::create(GL_ARRAY_BUFFER);
    this->vbo.bind();
    this->shaderProgram = std::make_shared<Shader>("shaders/pvshader.glsl", "shaders/pfshader.glsl");
    this->shaderProgram->compile();

    auto vertices = new float[16] {

            position.x, position.y, 0.0f, 0.0f,
            position.x + width, position.y, 1.0f, 0.0f,
            position.x, position.y + height, 0.0f, 1.0f,
            position.x + width, position.y + height, 1.0f, 1.0f

    };

    this->vbo.sendData(vertices, 16, GL_DYNAMIC_DRAW);
    delete[] vertices;

}

void Player::update() {

    float vertices[] = {

            position.x, position.y, 0.0f, 0.0f,
            position.x + width, position.y, 1.0f, 0.0f,
            position.x, position.y + height, 0.0f, 1.0f,
            position.x + width, position.y + height, 1.0f, 1.0f

    };

    this->vbo.sendData(vertices, 16, GL_DYNAMIC_DRAW);

}

void Player::draw() {

    this->vao.bind();
    this->vbo.bind();
    glUseProgram(this->shaderProgram->getID());
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
