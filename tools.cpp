
#include <sstream>
#include <string.h>
#include <fstream>
#include "tools.h"

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

        unsigned char header[54];
        int dataPos;
        int width, height;
        int imageSize;

        unsigned char *data;

        FILE* file = fopen(image_path, "rb");
        if (!file) {
            fprintf(stderr, "Image could not be opened.\n");
            return { 0, 0, 0 };
        }

        if (fread(header, 1, 54, file) != 54) {
            fprintf(stderr, "Not a valid BMP image!\n");
            return { 0, 0, 0 };
        }

        if (header[0] != 'B' || header[1] != 'M') {
            fprintf(stderr, "Not a valid BMP image!\n");
            return { 0, 0, 0 };
        }

        dataPos = *(int*)&(header[0x0A]);
        imageSize = *(int*)&(header[0x22]);
        width = *(int*)&(header[0x12]);
        height = *(int*)&(header[0x16]);

        imageSize = imageSize == 0 ? width * height * 3 : imageSize;
        dataPos = dataPos == 0 ? dataPos = 54 : dataPos;

        data = new unsigned char[imageSize];
        fread(data, 1, imageSize, file);
        fclose(file);

        GLuint textureID;
        glGenTextures(1, &textureID);
        glBindTexture(GL_TEXTURE_2D, textureID);

        glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, width, height, 0, GL_BGR, GL_UNSIGNED_BYTE, data);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);

        delete[] data; // Clean up data

        return { textureID, width, height };

    }

}