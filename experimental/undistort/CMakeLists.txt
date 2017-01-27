cmake_minimum_required(VERSION 3.0.1)

project(undistort)

#########################################################
set(CMAKE_CXX_FLAGS "-Wall -O3 -march=native")
#########################################################
# FIND C++11 Support
#########################################################
include(CheckCXXCompilerFlag)
CHECK_CXX_COMPILER_FLAG("-std=c++11" COMPILER_SUPPORTS_CXX11)
CHECK_CXX_COMPILER_FLAG("-std=c++0x" COMPILER_SUPPORTS_CXX0X)
if(COMPILER_SUPPORTS_CXX11)
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -std=c++11")
elseif(COMPILER_SUPPORTS_CXX0X)
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -std=c++0x")
else()
        message(STATUS "The compiler ${CMAKE_CXX_COMPILER} has no C++11 support. Please use a different C++ compiler.")
endif()

#########################################################
# FIND OPENCV
#########################################################
find_package(OpenCV REQUIRED)
#########################################################
# Include Files
#########################################################
include_directories(${OpenCV_INCLUDE_DIRS} include)

add_executable(undistort src/undistort_test.cpp)

target_link_libraries(undistort ${OpenCV_LIBS})
