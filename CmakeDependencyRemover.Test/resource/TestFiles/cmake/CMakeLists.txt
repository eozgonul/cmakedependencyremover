cmake_minimum_required (VERSION 3.0)
set(CMAKE_CONFIGURATION_TYPES Debug Release)
project (Boggle)

include("AddPrecompiledHeader.cmake")

get_filename_component(ProjectDirectory ${CMAKE_CURRENT_SOURCE_DIR} DIRECTORY)

set(THIRDPARTY_PATH "${ProjectDirectory}/thirdparty")

add_subdirectory("${CMAKE_SOURCE_DIR}/../boggle_library/cmake" "${CMAKE_SOURCE_DIR}/../boggle_library")
add_subdirectory("${CMAKE_SOURCE_DIR}/../boggle_test/cmake" "${CMAKE_SOURCE_DIR}/../boggle_test")