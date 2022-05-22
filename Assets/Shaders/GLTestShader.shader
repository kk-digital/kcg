Shader "GLTestShader" { // defines the name of the shader 
    SubShader{ // Unity chooses the subshader that fits the GPU best
       Pass {
         GLSLPROGRAM

            #ifdef VERTEX // here begins the vertex shader

            varying vec4 position;
            // this is a varying variable in the vertex shader

             void main()
             {
                position = gl_Vertex + vec4(0.5, 0.5, 0.5, 0.0);
                // Here the vertex shader writes output data
                // to the varying variable. We add 0.5 to the 
                // x, y, and z coordinates, because the 
                // coordinates of the cube are between -0.5 and
                // 0.5 but we need them between 0.0 and 1.0. 
                gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
            }

            #endif // here ends the vertex shader

            #ifdef FRAGMENT // here begins the fragment shader

            varying vec4 position;
            // this is a varying variable in the fragment shader

            void main()
            {
               gl_FragColor = position;
               // Here the fragment shader reads intput data 
               // from the varying variable. The red, gree, blue, 
               // and alpha component of the fragment color are 
               // set to the values in the varying variable. 
            }

            #endif // here ends the fragment shader

            ENDGLSL
       }

    }
}