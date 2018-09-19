#include "Rectangle2D.h"

Rectangle2D::Rectangle2D()
    : x(0), y(0), width(0), height(0) {}


Rectangle2D::Rectangle2D(float &x, float &y, float &width, float &height)
    : x(x), y(y), width(width), height(height) {}

bool Rectangle2D::intersects(const Rectangle2D &otherRect) {

    return !(this->x > otherRect.x + otherRect.width || this->y > otherRect.y + otherRect.height || this->x + this->width < otherRect.x || this->y + this->height < otherRect.y);

}

glm::vec2 Rectangle2D::intersection(const Rectangle2D& rect1, const Rectangle2D& rect2) {

    float x_overlap = glm::max(0.0f, glm::min(rect1.x + rect1.width, rect2.x + rect2.width) - glm::max(rect1.x, rect2.x));
    float y_overlap = glm::max(0.0f, glm::min(rect1.y + rect1.height, rect2.y + rect2.height) - glm::max(rect1.y, rect2.y));

    return glm::vec2(x_overlap, y_overlap);

}