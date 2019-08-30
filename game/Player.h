#ifndef FITCH_PLAYER_H
#define FITCH_PLAYER_H

#include <glm/glm.hpp>
#include <GL/glew.h>
#include <GLFW/glfw3.h>
#include <memory>
#include <Box2D/Dynamics/b2Body.h>
#include "../graphics/Drawable.h"
#include "../graphics/Texture2D.h"
#include "../graphics/VAO.h"
#include "../graphics/VBO.h"
#include "PhysicsObject.h"
#include "../main.h"

enum Direction {
    Left = -1,
    Right = 1
};

class Player : public Drawable, public PhysicsObject {

private:
    glm::vec2 position;
    float width;
    float height;

    Texture2D* texture;
    Direction direction = Right;

    std::shared_ptr<Shader> shaderProgram;
    VAO vao;
    VBO vbo;

    glm::mat4 drawMat{};

    b2Body* physicsBody;
    b2World* physicsWorld;

public:

    Player(glm::vec2 position, const char* texture_path);
    Player(const Player& other);
    ~Player();

    void handleInput(GLFWwindow* window);
    void setPos(glm::vec2 pos);
    glm::vec2 getPos() const;

    friend void fitch::renderFrame();

    float getWidth() const;
    float getHeight() const;

    void setMatrix(glm::mat4 mat) override;

    b2Body* getBody() const;
    void setBody(b2Body* body);
    void initPhysics(b2World* world) override;

    void init() override;
    void update() override;
    void draw() override;

};


#endif //FITCH_PLAYER_H
