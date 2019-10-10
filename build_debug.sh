#!/bin/bash

if [ -d build-debug ]; then
    rm -r build-debug
fi

mkdir build-debug
cd build-debug
cmake .. -DCMAKE_BUILD_TYPE=Debug -DCMAKE_C_COMPILER=$(which clang) -DCMAKE_CXX_COMPILER=$(which clang++)
