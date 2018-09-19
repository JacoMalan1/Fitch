//
// Created by jacom on 2018/08/21.
//

#ifndef FITCH_RECTANGLE2D_H
#define FITCH_RECTANGLE2D_H

#include <glm/glm.hpp>

struct Rectangle2D {

    float x, y, width, height;

    Rectangle2D();
    Rectangle2D(float& x, float& y, float& width, float& height);

    bool intersects(const Rectangle2D& otherRect);
    static glm::vec2 intersection(const Rectangle2D& rect1, const Rectangle2D& rect2);

};

#endif //FITCH_RECTANGLE2D_H
