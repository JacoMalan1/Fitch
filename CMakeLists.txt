cmake_minimum_required(VERSION 3.12)
project(Fitch)

set(CMAKE_CXX_STANDARD 17)
set(OpenGL_GL_PREFERENCE "GLVND")

add_executable(

    Fitch game/Block.h game/Block.cpp lodepng.h
    lodepng.cpp main.h game/Player.cpp tools.cpp main.cpp
    graphics/Texture2D.cpp graphics/Shader.cpp graphics/Shader.h
    graphics/VAO.cpp graphics/VAO.h graphics/VBO.cpp graphics/VBO.h
    graphics/Drawable.h graphics/Mesh.cpp graphics/Mesh.h game/PhysicsObject.h
    imgui/imgui.h imgui/imgui.cpp imgui/imgui_draw.cpp imgui/imgui_internal.h
    imgui/imconfig.h imgui/imstb_rectpack.h imgui/imstb_textedit.h imgui/imstb_truetype.h
    imgui/imgui_widgets.cpp imgui/imgui_impl_glfw.h imgui/imgui_impl_glfw.cpp
    imgui/imgui_impl_opengl3.h imgui/imgui_impl_opengl3.cpp

        graphics/Rectangle.cpp graphics/Rectangle.h)

find_package(glfw3 REQUIRED)
find_package(OpenGL REQUIRED)
find_package(Box2D REQUIRED)

target_link_libraries(Fitch GLEW)
target_link_libraries(Fitch glfw)
target_link_libraries(Fitch GL)
target_link_libraries(Fitch Box2D)

configure_file(shaders/fshader.glsl shaders/fshader.glsl COPYONLY)
configure_file(shaders/vshader.glsl shaders/vshader.glsl COPYONLY)
configure_file(shaders/pvshader.glsl shaders/pvshader.glsl COPYONLY)
configure_file(shaders/pfshader.glsl shaders/pfshader.glsl COPYONLY)
configure_file(shaders/tvshader.glsl shaders/tvshader.glsl COPYONLY)
configure_file(shaders/tfshader.glsl shaders/tfshader.glsl COPYONLY)
configure_file(shaders/bvshader.glsl shaders/bvshader.glsl COPYONLY)
configure_file(content/player.png content/player.png COPYONLY)
configure_file(content/solid.png content/solid.png COPYONLY)
configure_file(content/background.png content/background.png COPYONLY)
configure_file(content/level1.fl content/level1.fl COPYONLY)
configure_file(content/level2.fl content/level2.fl COPYONLY)
configure_file(content/level3.fl content/level3.fl COPYONLY)
