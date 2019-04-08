#ifndef FITCH_DRAWABLE_H
#define FITCH_DRAWABLE_H

#include <glm/glm.hpp>

class Drawable {

public:
    Drawable() = default;
    virtual void setMatrix(glm::mat4 mat) = 0;
    virtual void init() = 0;
    virtual void update() = 0;
    virtual void draw() = 0;

};


#endif
