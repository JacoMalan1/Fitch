#include <sstream>
#include <string.h>
#include <fstream>
#include "tools.h"
#include "lodepng.h"
#include <vector>
#include <iostream>

using namespace std;

namespace fitchio {

    const char * loadFile(const char *file_path) {

        std::ifstream fs(file_path);

        if (!fs.is_open()) {
            fprintf(stderr, "Error!");
            return "";
        }

        stringstream sstr;
        sstr << fs.rdbuf();
        string contents = sstr.str();

        char* retVal = new char[contents.length() + 1];
        strcpy(retVal, contents.c_str());

        return retVal;

    }

    Texture2D loadBMP(const char* image_path) {

        std::vector<unsigned char> png;
        std::vector<unsigned char> image;
        unsigned width, height;

        unsigned error = lodepng::load_file(png, image_path);
        if (error) {
            std::fprintf(stderr, "Error [%d]: %s\n", error, lodepng_error_text(error));
            return { 0, 0, 0 };
        }

        error = lodepng::decode(image, width, height, png);
        if (error) {
            std::fprintf(stderr, "Error [%d]: %s\n", error, lodepng_error_text(error));
            return { 0, 0, 0 };
        }

        GLuint textureID;
        glGenTextures(1, &textureID);
        glBindTexture(GL_TEXTURE_2D, textureID);

        glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, width, height, 0, GL_BGR, GL_UNSIGNED_BYTE, image.data());
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);

        return { textureID, (int)width, (int)height };

    }

}