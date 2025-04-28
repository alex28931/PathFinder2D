using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;

namespace TopDownGame
{
    class NegativeFX : PostProcessingEffect
    {
        static string fragmentShader = @"
#version 330 core

in vec2 uv;
uniform sampler2D tex;
out vec4 out_color;

void main()
{
    vec4 tex_col=texture(tex, uv);

    out_color = 1.f-tex_col;
}

";
        public NegativeFX() : base(fragmentShader)
        {

        }
    }
}
