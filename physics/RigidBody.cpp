//
// Created by jacom on 2018/08/20.
//

#include <memory>
#include <iostream>
#include "RigidBody.h"
#include "../main.h"

RigidBody::RigidBody(float x, float y, float width, float height, float mass, CollisionType collisionType)
    : position(glm::vec2(x, y)), width(width), height(height), mass(mass)
{

    this->collisionType = collisionType;
    this->collisionList = std::make_shared<std::vector<RigidBody>>();

}

RigidBody::RigidBody(const RigidBody &other)
    : velocity(other.velocity), acceleration(other.acceleration), position(other.position), width(other.width), height(other.height),
      mass(other.mass), collisionBox(other.collisionBox), collisionType(other.collisionType), shader(other.shader), buffer(other.buffer),vertexArray(other.vertexArray)
{

    this->collisionList = std::make_shared<std::vector<RigidBody>>();
    *this->collisionList = *other.collisionList;

}

glm::vec2 RigidBody::getPosition() { return position; }
glm::vec2 RigidBody::getVelocity() { return velocity; }

void RigidBody::setVelocity(glm::vec2 velocity) { this->velocity = velocity; }

void RigidBody::setPosition(glm::vec2 position) {
    this->position = position;
}

void RigidBody::addCollider(RigidBody& collider) {
    this->collisionList->emplace_back(collider);
}

std::shared_ptr<std::vector<RigidBody>> RigidBody::getCollisionList() {
    return this->collisionList;
}

float RigidBody::getMass() {
    return mass;
}

Rectangle2D RigidBody::getCBox() {
    return { position.x, position.y, width, height };
}

void RigidBody::update() {

    velocity += acceleration;
    glm::vec2(collisionBox.x, collisionBox.y) += velocity;

    acceleration = glm::vec2(0, 0);

}

void RigidBody::registerRenderer() {

    this->vertexArray = VAO::create();
    this->vertexArray.bind();
    this->initBuffer();

}

void RigidBody::initShaders() {

    this->shader = Shader("shaders/vshader.glsl", "shaders/fshader.glsl");
    this->shader.compile();

}

void RigidBody::initBuffer() {

    this->buffer = VBO::create(GL_ARRAY_BUFFER);
    this->buffer.bind();

    float vertices[] = {
            this->position.x, this->position.y, 0.0f, 1.0f, 1.0f, 1.0f,
            this->position.x + width, this->position.y, 0.0f, 1.0f, 1.0f, 1.0f,
            this->position.x, this->position.y + height, 0.0f, 1.0f, 1.0f, 1.0f,
            this->position.x + width, this->position.y + height, 0.0f, 1.0f, 1.0f, 1.0f
    };

    glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), &vertices, GL_DYNAMIC_DRAW);

}

void RigidBody::render() {

    this->vertexArray.bind();
    glUseProgram(this->shader.getID());

    glEnableVertexArrayAttrib(this->vertexArray.id, 0);
    glEnableVertexArrayAttrib(this->vertexArray.id, 1);

    glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(float), nullptr);
    glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(float), (void*)(3 * sizeof(float)));

    glDrawArrays(GL_TRIANGLE_STRIP, 0, 4);

    glDisableVertexArrayAttrib(this->vertexArray.id, 0);
    glDisableVertexArrayAttrib(this->vertexArray.id, 1);

}

void RigidBody::render(const glm::mat4& projMat) {

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

void RigidBody::resendBuffer() {

    this->buffer.bind();

    float vertices[] = {
            position.x, position.y, 0.0f, 1.0f, 1.0f, 1.0f,
            position.x + width, position.y, 0.0f, 1.0f, 1.0f, 1.0f,
            position.x, position.y + height, 0.0f, 1.0f, 1.0f, 1.0f,
            position.x + width, position.y + height, 0.0f, 1.0f, 1.0f, 1.0f
    };

    glBufferSubData(GL_ARRAY_BUFFER, 0, sizeof(vertices), &vertices);

}

RigidBody::~RigidBody() {
}
