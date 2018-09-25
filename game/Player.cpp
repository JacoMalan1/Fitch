//
// Created by jacom on 2018/08/22.
//

#include<iostream>
#include "Player.h"
#include "../tools.h"
#include "../main.h"

Player::Player(const Player& other) : position(other.position), velocity(other.velocity), acceleration(other.acceleration),
            width(other.width), height(other.height), texture(other.texture), shader(other.shader), vertexArray(other.vertexArray),
            buffer(other.buffer)
{

    this->collisionList = new std::vector<RigidBody*>();
    *this->collisionList = *other.collisionList;

}

void Player::initAll() {
    this->initShaders();
    this->initBuffer();
}

Player::Player(glm::vec2 position, const char *texture_path) : position(position) {
    this->collisionList = new std::vector<RigidBody*>();
    this->velocity = glm::vec2(0, 0);
    this->acceleration = glm::vec2(0, 0);
    this->texture = fitchio::loadBMP(texture_path);
    this->width = this->texture.width;
    this->height = this->texture.height;
}

Player::Player(glm::vec2 position, float width, float height) : position(position), width(width), height(height) {
    this->collisionList = new std::vector<RigidBody*>();
    this->velocity = glm::vec2(0, 0);
    this->acceleration = glm::vec2(0, 0);
    this->texture = { 0, 0, 0 };
}

void Player::initBuffer() {

    this->vertexArray = VAO::create();
    this->vertexArray.bind();
    this->buffer = VBO::create(GL_ARRAY_BUFFER);
    this->buffer.bind();

    float vertices[] = {

            position.x, position.y, 0, 0,
            position.x + width, position.y, 1, 0,
            position.x, position.y + height, 0, 1,
            position.x + width, position.y + height, 1, 1

    };

    glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), &vertices, GL_DYNAMIC_DRAW);

    if (this->texture.ID == 0) {
        std::cout << "No texture" << std::endl;
        this->texture = fitchio::loadBMP("content/player.png");
    }

}

void Player::update(bool sendBufferData) {
    this->update();
    this->resendBuffer();
}

void Player::resendBuffer() {

    this->buffer.bind();

    float vertices[] = {

            position.x, position.y, 0.0f, 0.0f,
            position.x + width, position.y, 1.0f, 0.0f,
            position.x, position.y + height, 0.0f, 1.0f,
            position.x + width, position.y + height, 1.0f, 1.0f

    };

    glBufferSubData(GL_ARRAY_BUFFER, 0, sizeof(vertices), &vertices);

}

void Player::render() {

    glBindVertexArray(this->vertexArray.id);
    glUseProgram(this->shader.getID());

    glEnableVertexAttribArray(0);
    glEnableVertexAttribArray(1);

    glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), nullptr);
    glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), (void*)(2 * sizeof(float)));

    glDrawArrays(GL_TRIANGLE_STRIP, 0, 4);

    glDisableVertexAttribArray(0);
    glDisableVertexAttribArray(1);

}

void Player::initShaders() {

    shader = Shader("shaders/pvshader.glsl", "shaders/pfshader.glsl");
    shader.compile();

}

RigidBody* Player::asPBody() {
    auto rb = new RigidBody(this->position.x, this->position.y, this->width, this->height, 1.0f, All);
    rb->setVelocity(this->velocity);
    return rb;

}

static std::ostream& operator<<(std::ostream& stream, glm::vec2 vector2) {
    stream << "X: " << vector2.x << " Y: " << vector2.y;
    return stream;
}

void Player::update() {

    velocity += acceleration;
    fitch::limitVector_lin(velocity, T_VELOCITY);

    position += velocity;
    velocity *= 0.9f;
    acceleration = glm::vec2(0, 0);
    applyForce(gravity);

    std::vector<RigidBody*>* checkList;
    checkList = collisionList;

    for (RigidBody* body : *checkList) {

        RigidBody* oldBody = this->asPBody();
        fitch::makeCollide(*oldBody, *body);
        this->position = oldBody->getPosition();
        this->velocity = oldBody->getVelocity();

        delete oldBody;

    }

}

void Player::applyForce(glm::vec2 force) {
    acceleration += force;
}

void Player::collideWith(RigidBody* body) {
    collisionList->emplace_back(body);
}

GLuint Player::getVertexArrayName() { return this->vertexArray.id; }

void Player::render(const glm::mat4& projMat) {

    this->vertexArray.bind();
    this->buffer.bind();
    glUseProgram(this->shader.getID());

    GLint MatrixID = glGetUniformLocation(this->shader.getID(), "projMat");
    glUniformMatrix4fv(MatrixID, 1, GL_FALSE, &projMat[0][0]);

    glEnableVertexArrayAttrib(this->vertexArray.id, 0);
    glEnableVertexArrayAttrib(this->vertexArray.id, 1);

    glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), nullptr);
    glVertexAttribPointer(1, 2, GL_FLOAT, GL_TRUE, 4 * sizeof(float), (void*)(2 * sizeof(float)));

    glDrawArrays(GL_TRIANGLE_STRIP, 0, 4);

    glDisableVertexArrayAttrib(this->vertexArray.id, 0);
    glDisableVertexArrayAttrib(this->vertexArray.id, 1);

    glBindVertexArray(0);

}

void Player::handleInput(GLFWwindow* const window) {

    if (glfwGetKey(window, GLFW_KEY_D) == GLFW_PRESS)
        this->applyForce(glm::vec2(1.0f, 0));
    if (glfwGetKey(window, GLFW_KEY_A) == GLFW_PRESS)
        this->applyForce(glm::vec2(-1.0f, 0));
    if (glfwGetKey(window, GLFW_KEY_SPACE) == GLFW_PRESS)
        this->applyForce(glm::vec2(0, -10.0f));

}

Player::~Player() {
    delete collisionList;
}
