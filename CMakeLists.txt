cmake_minimum_required(VERSION 3.10)
project(Fitch)

set(CMAKE_CXX_STANDARD 17)
set(OpenGL_GL_PREFERENCE "GLVND")

add_executable(Fitch physics/StaticBody.h physics/StaticBody.cpp game/Block.h game/Block.cpp lodepng.h lodepng.cpp main.h graphics/Renderable.cpp game/Player.cpp physics/PhysicsBody.cpp physics/Rectangle2D.cpp physics/Physics.h physics/Physics.cpp physics/RigidBody.cpp tools.cpp main.cpp graphics/Texture2D.cpp graphics/Shader.cpp graphics/Shader.h graphics/VAO.cpp graphics/VAO.h graphics/VBO.cpp graphics/VBO.h)

find_package(glfw3 REQUIRED)
find_package(OpenGL REQUIRED)

target_link_libraries(Fitch GLEW)
target_link_libraries(Fitch glfw)
target_link_libraries(Fitch GL)

configure_file(shaders/fshader.glsl shaders/fshader.glsl COPYONLY)
configure_file(shaders/vshader.glsl shaders/vshader.glsl COPYONLY)
configure_file(shaders/pvshader.glsl shaders/pvshader.glsl COPYONLY)
configure_file(shaders/pfshader.glsl shaders/pfshader.glsl COPYONLY)
configure_file(shaders/tvshader.glsl shaders/tvshader.glsl COPYONLY)
configure_file(shaders/tfshader.glsl shaders/tfshader.glsl COPYONLY)
configure_file(content/player.png content/player.png COPYONLY)
configure_file(content/solid.png content/solid.png COPYONLY)
configure_file(content/level1.fl content/level1.fl COPYONLY)
