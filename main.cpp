#include <iostream>
#include <GL/glew.h>
#include <GLFW/glfw3.h>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <cstring>
#include "game/Block.h"
#include "game/Player.h"
#include "tools.h"

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

    int width, height;

    void GLAPIENTRY glCallback( GLenum source, GLenum type, GLuint id, GLenum severity, GLsizei length, const GLchar* message, const void* userParam ) {

        std::fprintf(type == GL_DEBUG_TYPE_ERROR ? stderr : stdout, "%s: type = 0x%x, severity = 0x%x, message = %s\n",
        (type == GL_DEBUG_TYPE_ERROR ? "ERROR" : type == GL_DEBUG_TYPE_PERFORMANCE ? "PERFORMANCE" : "INFO"),
        type, severity, message );

    }

    // Gets run before the main loop starts
    void loadWindow() {

        glEnable(GL_BLEND);
        glBlendFunc(GL_BLEND_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
        glEnable(GL_DEPTH_TEST);
        glEnable(GL_MULTISAMPLE);
        glDepthFunc(GL_LEQUAL);

        if (getDebug() == Full) {
            glEnable(GL_DEBUG_OUTPUT);
            glDebugMessageCallback(glCallback, nullptr);
        }

        blockShader = new Shader("shaders/tvshader.glsl", "shaders/tfshader.glsl");
        blockShader->compile();

        player = new Player(vec2(0, 0), "content/player.png");
        std::string sLevel = (std::string)"content/level" + std::to_string(levelCount) + (std::string)".fl";
        blockList = fitchio::loadLevel(sLevel.c_str());

        TEXTURE_SOLID = fitchio::loadBMP("content/solid.png");

        drawList.push_back((Drawable*)player);
        for (Block& b : *blockList) {
            if (b.getType() == Solid) {
                b.setTexture(TEXTURE_SOLID);
            } else if (b.getType() == Start) {
                player->setPos(glm::vec2(b.getPos()));             
            }

            drawList.emplace_back((Drawable*)&b);
        }

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

        glBindTexture(GL_TEXTURE_2D, player->texture.ID);

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
