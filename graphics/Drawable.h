//
// Created by jacom on 2019/03/31.
//

#ifndef FITCH_DRAWABLE_H
#define FITCH_DRAWABLE_H


class Drawable {

public:
    Drawable() = default;
    virtual void init() = 0;
    virtual void update() = 0;
    virtual void draw() = 0;

};


#endif //FITCH_DRAWABLE_H
