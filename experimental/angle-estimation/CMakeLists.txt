cmake_minimum_required(VERSION 3.0.1)

project(angle_estimation)

#########################################################
set(CMAKE_CXX_FLAGS "-w -O3 -march=native")     #  -fsanitize=address
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
# FIND Packages
#########################################################
find_package(OpenCV REQUIRED)
# find_package(gflags REQUIRED)
# find_package(glog REQUIRED)
find_package(Ceres REQUIRED)
#########################################################
# Include Files
#########################################################
include_directories(${OpenCV_INCLUDE_DIRS}
                    ${CERES_INCLUDE_DIRS}
					 include
					 ../undistort/include)

add_library(helpers-cc src/triangulation.cc
                    src/sequential_probability_ratio.cc
                    src/random.cc
                    src/pose_util.cc
                    src/fundamental_matrix_util.cc
                    src/five_point.cpp
                    src/estimate_uncalibrated_relative_pose.cc
                    src/estimate_twoview_info.cc
                    src/estimate_relative_pose.cpp
                    src/essential_matrix_utils.cc
                    src/eight_point_fundamental_matrix.cc
                    src/correspondance.cc
                    src/helpers.cc)

# add_library(correspondance-cc src/correspondance.cc)

add_executable(angle_estimation_5pt src/find_deviation_angle.cpp)
# add_executable(find_trajectory src/find_trajectory.cpp)
# add_executable(trajectory_tracker src/trajectory_tracker.cpp)

target_link_libraries(angle_estimation_5pt ${OpenCV_LIBS}
                                        ${CERES_LIBRARIES}
                                        gflags
                                        helpers-cc)

# target_link_libraries(find_trajectory ${OpenCV_LIBS})
# target_link_libraries(trajectory_tracker ${OpenCV_LIBS} correspondance-cc)
