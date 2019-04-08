#ifndef FITCH_PLAYER_H
#define FITCH_PLAYER_H
#include <glm/glm.hpp>
#include <GL/glew.h>
#include <GLFW/glfw3.h>
#include <memory>
#include "../graphics/Drawable.h"
#include "../physics/Physics.h"
#include "../graphics/Texture2D.h"
#include "../graphics/VAO.h"
#include "../graphics/VBO.h"
#include "../main.h"

enum Direction {
    Left,
    Right
};

class Player : Drawable {

private:
    const float T_VELOCITY = 70.0f;
    glm::vec2 position;
    glm::vec2 velocity;
    glm::vec2 acceleration;
    const glm::vec2 gravity = glm::vec2(0, 0.9f);
    bool isStanding = false;
    float width;
    float height;

    Texture2D texture;
    Direction direction = Right;

    std::shared_ptr<Shader> shaderProgram;
    VAO vao;
    VBO vbo;

    glm::mat4 drawMat;

    std::unique_ptr<std::vector<RigidBody*>> collisionList;

public:

    Player(glm::vec2 position, float width, float height);
    Player(glm::vec2 position, const char* texture_path);
    Player() = default;
    Player(const Player& other);
    ~Player();

    void applyForce(glm::vec2 force);
    void handleInput(GLFWwindow* window);
    void collideWith(RigidBody* body);
    RigidBody* asPBody();
    void setPos(glm::vec2 pos);
    glm::vec2 getPos();

    friend void fitch::renderFrame();

    float getWidth() const;
    float getHeight() const;

    void setMatrix(glm::mat4 mat) override;

    void init() override;
    void update() override;
    void draw() override;

};


#endif //FITCH_PLAYER_H
