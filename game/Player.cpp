#include <iostream>
#include <memory>
#include <Box2D/Collision/Shapes/b2PolygonShape.h>
#include "Player.h"
#include "../tools.h"
#include "../main.h"

Player::Player(const Player& other) : position(other.position), width(other.width), height(other.height),
    texture(other.texture), shaderProgram(other.shaderProgram), vao(other.vao),
            vbo(other.vbo)
{

}

Player::Player(glm::vec2 position, const char *texture_path) : position(position) {
    this->texture = fitchio::loadBMP(texture_path);
    this->width = this->texture.width;
    this->height = this->texture.height;
}

void Player::setPos(glm::vec2 pos) {
    this->position = pos;
    this->physicsBody->SetTransform(fitchtools::pixToWorld(this->position), 0);
}

glm::vec2 Player::getPos() const {
    return this->position;
}

void Player::handleInput(GLFWwindow* const window) {



}

float Player::getWidth() const { return this->width; }
float Player::getHeight() const { return this->height; }

void Player::setMatrix(glm::mat4 mat) {
    this->drawMat = mat;
}

b2Body* Player::getBody() const { return physicsBody; }
void Player::setBody(b2Body *body) { physicsBody = body; }

void Player::initPhysics(b2World* world) {

    using namespace fitchtools;

    b2BodyDef bodyDef;
    bodyDef.type = b2_dynamicBody;
    bodyDef.position = pixToWorld(this->position);
    physicsBody = world->CreateBody(&bodyDef);
    physicsBody->SetFixedRotation(true);
    physicsBody->SetSleepingAllowed(false);

    b2PolygonShape box;
    box.SetAsBox(pixToWorld(width / 2), pixToWorld(height / 2));

    float bR = pixToWorld(4);
    b2CircleShape c0;
    c0.m_p.Set(pixToWorld(-width / 2), pixToWorld(-height / 2));
    c0.m_radius = bR;

    b2CircleShape c1;
    c1.m_p.Set(pixToWorld(-width / 2), pixToWorld(height / 2));
    c1.m_radius = bR;

    b2CircleShape c2;
    c2.m_p.Set(pixToWorld(width / 2), pixToWorld(-height / 2));
    c2.m_radius = bR;

    b2CircleShape c3;
    c3.m_p.Set(pixToWorld(width / 2), pixToWorld(height / 2));
    c3.m_radius = bR;

    b2FixtureDef fd;
    fd.shape = &box;
    fd.density = 1.0f;
    fd.friction = 0.1f;
    fd.restitution = 0.0f;
    physicsBody->CreateFixture(&fd);

    b2FixtureDef fd0;
    fd0.shape = &c0;
    fd0.density = 1.0f;
    fd0.friction = 0.1f;
    fd0.restitution = 0.0f;
    physicsBody->CreateFixture(&fd0);

    b2FixtureDef fd1;
    fd1.shape = &c1;
    fd1.density = 1.0f;
    fd1.friction = 0.1f;
    fd1.restitution = 0.0f;
    physicsBody->CreateFixture(&fd1);

    b2FixtureDef fd2;
    fd2.shape = &c2;
    fd2.density = 1.0f;
    fd2.friction = 0.1f;
    fd2.restitution = 0.0f;
    physicsBody->CreateFixture(&fd2);

    b2FixtureDef fd3;
    fd3.shape = &c3;
    fd3.density = 1.0f;
    fd3.friction = 0.1f;
    fd3.restitution = 0.0f;
    physicsBody->CreateFixture(&fd3);

    physicsWorld = world;

}

void Player::init() {

    this->vao = VAO::create();
    this->vao.bind();
    this->vbo = VBO::create(GL_ARRAY_BUFFER);
    this->vbo.bind();
    this->shaderProgram = std::make_shared<Shader>("shaders/pvshader.glsl", "shaders/pfshader.glsl");
    this->shaderProgram->compile();

    std::shared_ptr<float[]> vertices(new float[16] {

            position.x, position.y, 0.0f, 0.0f,
            position.x + width, position.y, 1.0f, 0.0f,
            position.x, position.y + height, 0.0f, 1.0f,
            position.x + width, position.y + height, 1.0f, 1.0f

    });

    this->vbo.sendData(vertices, 16, GL_DYNAMIC_DRAW);

}

void Player::update() {

    using namespace fitchtools;

    b2Vec2 bPos = physicsBody->GetPosition();
    position = worldToPix(bPos);
    float maxVel = 1.0f;
    b2Vec2 vel = physicsBody->GetLinearVelocity();
    if (vel.Length() > maxVel)
        physicsBody->SetLinearVelocity((maxVel / vel.Length()) * vel);

    std::shared_ptr<float[]> vertices(new float[16] {

            position.x, position.y, 0.0f, 0.0f,
            position.x + width, position.y, 1.0f, 0.0f,
            position.x, position.y + height, 0.0f, 1.0f,
            position.x + width, position.y + height, 1.0f, 1.0f

    });

    this->vbo.sendData(vertices, 16, GL_DYNAMIC_DRAW);

}

void Player::draw() {

    this->vao.bind();
    this->vbo.bind();
    this->shaderProgram->bind();
    this->texture.bind();

    GLint MatrixID = glGetUniformLocation(this->shaderProgram->getID(), "projMat");
    glUniformMatrix4fv(MatrixID, 1, GL_FALSE, &drawMat[0][0]);

    glEnableVertexAttribArray(0);
    glEnableVertexAttribArray(1);

    glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), nullptr);
    glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), (void*)(2 * sizeof(float)));

    glDrawArrays(GL_TRIANGLE_STRIP, 0, 4);

    glDisableVertexAttribArray(0);
    glDisableVertexAttribArray(1);

}
