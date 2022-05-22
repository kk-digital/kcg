/*
        GLSL SKYBOX SHADER
*/

Shader "GLSkyboxTestShader" {
    Properties{
    }
    SubShader{
       Pass {
            // Start GLSL macro.
          GLSLPROGRAM

            #ifdef VERTEX
            
            //Vertex Shader main function
            void main()
            {
                // World Position Set for Vertex Position
                // (World*View*Projection) * gl_Vertex Position
                gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
            }
            
            #endif
            
            #ifdef FRAGMENT
            
            float remap(float minval, float maxval, float curval)
            {
                // Remapping Input Value
                return (curval - minval) / (maxval - minval);
            }

            // In HLSL(DirectX):Pixel / In GLSL(OpenGL):Fragment, Shader main function
            void main()
            {
                // Assign GREEN color value
                const vec4 GREEN = vec4(0.0, 1.0, 0.0, 1.0);

                // Assign WHITE color value
                const vec4 WHITE = vec4(1.0, 1.0, 1.0, 1.0);

                // Mixing Color using remaping
                gl_FragColor = mix(GREEN, WHITE, remap(0.0, 0.5, 0.3));
            }
            #endif
            
            // Stop GLSL macro.
            ENDGLSL
       }
    }
}
