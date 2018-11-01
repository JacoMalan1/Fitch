#include <iostream>
#include "Physics.h"

namespace fitch {

    void limitVector(glm::vec2& vector, float mag) {

        float currentMag = 0.0f;

        auto x_s = (float)pow(vector.x, 2);
        auto y_s = (float)pow(vector.y, 2);
        currentMag = (float)sqrt(x_s + y_s);

        if (currentMag > mag) {

            float new_x = x_s * mag / currentMag;
            float new_y = y_s * mag / currentMag;

            vector.x = new_x;
            vector.y = new_y;

        }

    }

    void limitVector_lin(glm::vec2& vector, float l_mag) {

        if (vector.x > l_mag)
            vector.x = l_mag;
        if (vector.x < -l_mag)
            vector.x = -l_mag;

        if (vector.y > l_mag)
            vector.y = l_mag;
        if (vector.y < -l_mag)
            vector.y = -l_mag;

    }

    void makeCollide(RigidBody &dynBody, RigidBody &statBody) {

        RigidBody* statBodyPtr = &statBody;

        bool found = false;
        for (RigidBody b : *dynBody.getCollisionList()) {
            if (&b == statBodyPtr)
                found = true;
        }

        // Check to see if the bodies intersect each other
        if (!dynBody.getCBox().intersects(statBody.getCBox()))
            return;

        // TODO: Retain X-velocity if standing.
        glm::vec2 isect = Rectangle2D::intersection(dynBody.getCBox(), statBody.getCBox());
        if (isect.y <= isect.x) {

            if (dynBody.getVelocity().y > 0) {
                dynBody.setPosition(glm::vec2(dynBody.getPosition().x, dynBody.getPosition().y - isect.y));
            } else {
                dynBody.setPosition(glm::vec2(dynBody.getPosition().x, dynBody.getPosition().y + isect.y));
            }
            dynBody.setVelocity(glm::vec2(dynBody.getVelocity().x, 0));

        } else {

            if (dynBody.getVelocity().x > 0) {
                dynBody.setPosition(glm::vec2(dynBody.getPosition().x - isect.x, dynBody.getPosition().y));

            } else {
                dynBody.setPosition(glm::vec2(dynBody.getPosition().x + isect.x, dynBody.getPosition().y));
            }

            dynBody.setVelocity(glm::vec2(0, dynBody.getVelocity().y));

        }

    }

}