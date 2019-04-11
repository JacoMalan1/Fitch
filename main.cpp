#include <iostream>
#include <GL/glew.h>
#include <GLFW/glfw3.h>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <cstring>
#include <Box2D/Box2D.h>
#include "game/Block.h"
#include "game/Player.h"
#include "tools.h"
#include "graphics/Mesh.h"

using namespace glm;
DebugMode debugMode = Off;

namespace fitch {

    GLFWwindow* window;
    Player* player;
    Texture2D TEXTURE_SOLID;
    Shader* blockShader;
    int levelCount = 1;
    std::vector<Drawable*> drawList;
    std::vector<Block>* blockList;
    Mesh* blockMesh;
    b2Vec2 gravity;
    b2World* world;

    int width, height;

    void GLAPIENTRY glCallback( GLenum source, GLenum type, GLuint id, GLenum severity, GLsizei length, const GLchar* message, const void* userParam ) {

        if (type == GL_DEBUG_TYPE_OTHER)
            return;

        std::fprintf(type == GL_DEBUG_TYPE_ERROR ? stderr : stdout, "%s: type = 0x%x, severity = 0x%x, message = %s\n",
        (type == GL_DEBUG_TYPE_ERROR ? "ERROR" : type == GL_DEBUG_TYPE_PERFORMANCE ? "PERFORMANCE" : "INFO"),
        type, severity, message );

    }

    // Gets run before the main loop starts
    void loadWindow() {

        glEnable(GL_BLEND);
        glEnable(GL_DEPTH_TEST);

        glDepthFunc(GL_LEQUAL);
        glBlendFuncSeparate(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA, GL_ONE, GL_ONE_MINUS_SRC_ALPHA);

        if (getDebug() == Full) {
            glEnable(GL_DEBUG_OUTPUT);
            glDebugMessageCallback(glCallback, nullptr);
        }

        blockMesh = new Mesh();
        gravity = b2Vec2(0.0f, -2.0f);
        world = new b2World(gravity);

        // Load and compile the shader for textured rectangles.
        blockShader = new Shader("shaders/tvshader.glsl", "shaders/tfshader.glsl");
        blockShader->compile();

        // Initialize player object.
        player = new Player(vec2(0, 0), "content/player.png");
        std::string sLevel = (std::string)"content/level" + std::to_string(levelCount) + (std::string)".fl";
        blockList = fitchio::loadLevel(sLevel.c_str());

        // Pre-load solid.png
        TEXTURE_SOLID = fitchio::loadBMP("content/solid.png");

        drawList.reserve(1 + blockList->size());

        drawList.emplace_back((Drawable*)player);
        for (Block& b : *blockList) {

            b.setShader(blockShader);

            if (b.getType() == Solid) {
                b.setTexture(TEXTURE_SOLID);
            } else if (b.getType() == Start) {
                player->setPos(b.screenPos() - vec2(0, player->getHeight()));
                continue;
            }

            blockMesh->addMeshElement(b.getVertices(), 4, 4);

        }

        blockMesh->setShader(blockShader);
        blockMesh->setTexture(TEXTURE_SOLID);
        drawList.emplace_back((Drawable*)blockMesh);

        for (Drawable* drawable : drawList)
            drawable->init();

    }

    // Gets run after the main loop stops
    void windowClosing() {

        delete blockShader;
        delete blockList;
        delete player;

    }

    // Gets run before rendering each frame
    void updateFrame() {

        // player->handleInput(window);
        //
        // for (Drawable* drawable : drawList)
        //     drawable->update();

    }

    // Renders the current frame
    void renderFrame() {

        glClearColor(100.0f / 255, 149.0f / 255, 237.0f / 255, 1.0f);
        glClearDepth(1.0f);
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
        mat4 projMat = ortho(0.0f, (float)width, (float)height, 0.0f);
        mat4 modelMat = mat4(1);
        projMat = translate(projMat, vec3(-player->getPos() + vec2(width / 2, height / 2) - vec2(player->getWidth() / 2, player->getHeight() / 2), 0.0f));
        mat4 viewMat(1);
        mat4 mvp(1);

        mvp *= projMat;
        mvp *= viewMat;
        mvp *= modelMat;

        for (Drawable* drawable : drawList) {
            drawable->setMatrix(mvp);
            drawable->draw();
        }

        glfwSwapBuffers(window);

    }

    int init() {

        glewExperimental = GL_TRUE;

        if (!glfwInit()) {
            std::fprintf(stderr, "%s\n", "GLFW initialization failed!");
            return -1;
        }

        glfwWindowHint(GLFW_SAMPLES, 4);
        glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
        glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 2);
        glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

        width = 800;
        height = 600;
        window = glfwCreateWindow(width, height, "Fitch", nullptr, nullptr);

        glfwMakeContextCurrent(window);

        if (glewInit() != 0) {
            std::fprintf(stderr, "%s\n", "GLEW initialization failed!");
            return -1;
        }

        loadWindow();

        double lastTime = glfwGetTime();
        double fps = 120;
        double maxPeriod = 1 / fps;
        glfwSetInputMode(window, GLFW_STICKY_KEYS, GL_TRUE);

        // Start main game loop
        while (!glfwWindowShouldClose(window) && glfwGetKey(window, GLFW_KEY_ESCAPE) != GLFW_PRESS) {

            double time = glfwGetTime();
            double deltaTime = time - lastTime;

            // Insure that the update function only gets called 60 times per second.
            if (deltaTime >= maxPeriod) {

                updateFrame();
                lastTime = time;

            }

            renderFrame();
            glfwPollEvents();

        }

        windowClosing();

        return 0;
    }

}

DebugMode fitch::getDebug() { return debugMode; }

int main(int argc, const char** argv) {

    if (argc > 0 && fitch::getDebug() == Off) {
        if (strncmp(argv[0], "--debug", strlen(argv[0])) != 0 ||
            strncmp(argv[0], "-d", strlen(argv[0])) != 0) {
            debugMode = Full;
        }
    }

    return fitch::init();
}
