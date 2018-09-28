#include <iostream>
#include <GL/glew.h>
#include <GLFW/glfw3.h>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include "game/Block.h"
#include "game/Player.h"

using namespace glm;

namespace fitch {

    GLFWwindow* window;
    Player* player;
    Block* block;

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
        glDepthFunc(GL_LEQUAL);

        if (DEBUG_MODE) {
            glEnable(GL_DEBUG_OUTPUT);
            glDebugMessageCallback(glCallback, nullptr);
        }

        player = new Player(vec2(0, 0), "content/player.png");
        block = new Block(vec2(BLOCK_SIZE, height - BLOCK_SIZE * 2), Solid, "content/solid.png");

        player->initAll();
        player->collideWith(block->asRBody());
        block->initBuffer();
        block->initShaders();

    }

    // Gets run after the main loop stops
    void windowClosing() {

    }

    // Gets run before rendering each frame
    void updateFrame() {

        player->handleInput(window);
        player->update(true);
    }

    // Renders the current frame
    void renderFrame() {

        glClearColor(100.0f / 255, 149.0f / 255, 237.0f / 255, 1.0f);
        glClearDepth(1.0f);
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
        mat4 projMat = ortho(0.0f, 800.0f, 600.0f, 0.0f);

        glBindTexture(GL_TEXTURE_2D, player->texture.ID);
        player->render(projMat);
        block->render(projMat);

        glfwSwapBuffers(window);

    }

    int init() {

        glewExperimental = GL_TRUE;

        if (!glfwInit()) {
            std::fprintf(stderr, "GLFW initialization failed!");
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
            std::fprintf(stderr, "GLEW initialization failed!");
            return -1;
        }

        loadWindow();

        double lastTime = glfwGetTime();
        double fps = 60;
        double maxPeriod = 1 / fps;
        glfwSetInputMode(window, GLFW_STICKY_KEYS, GL_TRUE);
        while (!glfwWindowShouldClose(window) && glfwGetKey(window, GLFW_KEY_ESCAPE) != GLFW_PRESS) {

            double time = glfwGetTime();
            double deltaTime = time - lastTime;

            if (deltaTime >= maxPeriod) {

                updateFrame();
                renderFrame();

                lastTime = time;

            }

            glfwPollEvents();

        }

        windowClosing();

        return 0;
    }

}

int main() {
    return fitch::init();
}
