#include "StaticBody.h"

StaticBody::StaticBody(glm::vec2 position, float width, float height) : position(position), width(width), height(height) {}

void StaticBody::update() {

    return;

}

Rectangle2D StaticBody::getCBox() {

    return Rectangle2D(position.x, position.y, width, height);

}

glm::vec2 StaticBody::getPosition() { return position; }
void StaticBody::setPosition(glm::vec2 position) { this->position = position; }
