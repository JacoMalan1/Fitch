#include <iostream>
#include <GL/glew.h>
#include <GLFW/glfw3.h>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <vector>
#include "tools.h"
#include "physics/Physics.h"
#include "physics/RigidBody.h"
#include "game/Player.h"
#include "main.h"

using namespace glm;

namespace fitch {

    GLFWwindow* window;
    Player* player;
    RigidBody* floorBody = new RigidBody(0, 500, 500, 50, 1.0f, All);

    int width = 800;
    int height = 600;

    int n = 0;
    int framecounter = 0;

    // GL callback for debug messages
    void GLAPIENTRY glCallback( GLenum source, GLenum type, GLuint id, GLenum severity, GLsizei length, const GLchar* message, const void* userParam ) {

        std::fprintf(type == GL_DEBUG_TYPE_ERROR ? stderr : stdout, "%s: type = 0x%x, severity = 0x%x, message = %s\n",
                     (type == GL_DEBUG_TYPE_ERROR ? "ERROR" : "INFO"),
                 type, severity, message );

    }

    void initBuffers() {

        player->initBuffer();
        player->collideWith(floorBody);

    }

    void updateFrame() {

        glfwPollEvents();
        player->update();
        player->resendBuffer();
        player->handleInput(window);
        floorBody->update();

    }

    void renderFrame() {

        glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
        glClearDepth(1.0f);
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

        mat4 projMat = ortho(0.0f, 800.0f, 600.0f, 0.0f);

        player->render(projMat);
        floorBody->render(projMat);

        glfwSwapBuffers(window);

    }

    void windowClosing() {

        delete player;
        delete floorBody;

    }

    void initShaders() {

        player->initShaders();
        floorBody->initShaders();
        floorBody->registerRenderer();

    }

    int initGL() {

        glewExperimental = GL_TRUE;

        glfwInit();

        glfwWindowHint(GLFW_SAMPLES, 4);
        glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
        glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 2);
        glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

        window = glfwCreateWindow(width, height, "Fitch", nullptr, nullptr);

        if (window == nullptr) {
            fprintf(stderr, "Failed to open GL window");
            glfwTerminate();
            return -1;
        }

        glfwMakeContextCurrent(window);
        glewInit();
        glfwSetInputMode(window, GLFW_STICKY_KEYS, GL_TRUE);

        return 0;

    }

    int init() {

        int exitCode;

        // Initialize everything
        if ((exitCode = initGL()) != 0) {
            return exitCode;
        }

        glEnable(GL_DEPTH_TEST);
        glEnable(GL_DEBUG_OUTPUT);
        if (DEBUG_MODE)
            glDebugMessageCallback(glCallback, 0);

        glDepthFunc(GL_LEQUAL);

        // Initialize global variables
        player = new Player(glm::vec2(0, 0), 30, 70);

        // Initialize all data structures here.
        initShaders();
        initBuffers();

        std::printf("OpenGL Context Version: %s\n", glfwGetVersionString());

        auto lastTime = glfwGetTime();
        double maxPeriod = (double)1 / (double)60;
        // Start the main draw loop.
        while (glfwGetKey(window, GLFW_KEY_ESCAPE) != GLFW_PRESS && glfwWindowShouldClose(window) == 0) {

            double nowTime = glfwGetTime();
            double deltaTime = nowTime - lastTime;

            if (deltaTime >= maxPeriod) {

                lastTime = nowTime;
                renderFrame(); // Render everything as updated last frame.
                updateFrame(); // Update everything for rendering next frame.

            }

        }

        windowClosing(); // To be run right before return.

        return 0;

    }

}

int main() {

    return fitch::init();

}
