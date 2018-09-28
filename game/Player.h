#ifndef FITCH_PLAYER_H
#define FITCH_PLAYER_H
#include <glm/glm.hpp>
#include <GL/glew.h>
#include <GLFW/glfw3.h>
#include "../graphics/Renderable.h"
#include "../physics/Physics.h"
#include "../graphics/Texture2D.h"
#include "../graphics/VAO.h"
#include "../graphics/VBO.h"
#include "../main.h"

enum Direction {
    Left,
    Right
};

class Player : Renderable {

    // TODO: Add a texture system.

private:

    const float T_VELOCITY = 50.0f;

    glm::vec2 position;
    glm::vec2 velocity;
    glm::vec2 acceleration;
    const glm::vec2 gravity = glm::vec2(0, 0.9f);
    bool isRunning = false;
    bool isStanding = false;
    float width;
    float height;

    Texture2D texture;
    Direction direction = Right;

    Shader shader;
    VAO vertexArray;
    VBO buffer;

    std::vector<RigidBody*>* collisionList;

public:

    Player(glm::vec2 position, float width, float height);
    Player(glm::vec2 position, const char* texture_path);
    Player() = default;
    Player(const Player& other);
    ~Player();

    void render() override;
    void render(const glm::mat4& projMat);
    void update();
    void update(bool sendBufferData);
    void initAll();
    void initBuffer() override;
    void resendBuffer() override;
    void initShaders() override;
    void applyForce(glm::vec2 force);
    void handleInput(GLFWwindow* window);
    void collideWith(RigidBody* body);
    RigidBody* asPBody();

    friend void fitch::renderFrame();

    GLuint getVertexArrayName();

};


#endif //FITCH_PLAYER_H
