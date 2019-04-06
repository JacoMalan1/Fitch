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

RigidBody::~RigidBody() = default;
