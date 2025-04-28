using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace TopDownGame
{
    class GrayScaleFX : PostProcessingEffect
    {
        static string fragmentShader = @"
#version 330 core

in vec2 uv;
uniform sampler2D tex;
out vec4 out_color;

void main()
{
    vec4 tex_col=texture(tex, uv);
    
    float gray=(tex_col.x+tex_col.y+tex_col.z)/3.f;

    out_color = vec4(gray, gray, gray, 1.f);
}

";
        public GrayScaleFX() : base(fragmentShader)
        {

        }
    }
}
