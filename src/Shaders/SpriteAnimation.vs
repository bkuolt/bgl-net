#version 440 

layout (std140) uniform Positions {
  vec2 positions[128];
};

layout (std140) uniform Animations {
  float currentTime[128];
};

layout (std140) uniform FPS {
  float fps[128];
};

layout (std140) uniform Quads {
  uint indices[128];
};

uniform mat4 ProjectionMatrix;
uniform mat4 ViewMatrix;

out vec2 bgl_TexCoord;

const vec2 Vertices[4] = {
    vec2(-1, -1),
    vec2(-1,  1),
    vec2( 1,  1),
    vec2( 1, -1)
};


void main() {
    vec2 position = positions[indices[gl_InstanceID]];
    gl_Position = Vertices[gl_VertexID] + position;
    bgl_TexCoord =  Vertices[gl_VertexID];
}