#version 440 

in vec2 bgl_TexCoord;


layout (std140) uniform Animations {
  float currentTime[128];
};

layout (std140) uniform FPS {
  float fps[128];
};

layout (std140) uniform TexCoords {
  vec2 texCoords[128];
};

uniform sampler2D textureAtlas;







void main() {

    // currentTime[gl_InstanceID];
    // frame, frmaeFPS


    
   // gl_FragColor = vec2();


}