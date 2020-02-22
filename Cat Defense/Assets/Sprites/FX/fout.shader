// Varyings
static float4 _v_vColour = {0, 0, 0, 0};
static float2 _v_vTexcoord = {0, 0};

static float4 gl_Color[1] =
{
    float4(0, 0, 0, 0)
};


uniform float3 _f_Colour1 : register(c3);
uniform float3 _f_Colour10 : register(c4);
uniform float3 _f_Colour11 : register(c5);
uniform float3 _f_Colour12 : register(c6);
uniform float3 _f_Colour2 : register(c7);
uniform float3 _f_Colour3 : register(c8);
uniform float3 _f_Colour4 : register(c9);
uniform float3 _f_Colour5 : register(c10);
uniform float3 _f_Colour6 : register(c11);
uniform float3 _f_Colour7 : register(c12);
uniform float3 _f_Colour8 : register(c13);
uniform float3 _f_Colour9 : register(c14);
uniform float _gm_AlphaRefValue : register(c15);
uniform bool _gm_AlphaTestEnabled : register(c16);
uniform sampler2D _gm_BaseTexture : register(s0);
uniform float4 _gm_FogColour : register(c17);
uniform bool _gm_PS_FogEnabled : register(c18);

float4 gl_texture2D(sampler2D s, float2 t)
{
    return tex2D(s, t);
}

#define GL_USES_FRAG_COLOR
;
;
;
;
;
void _DoAlphaTest(in float4 _SrcColour)
{
{
if(_gm_AlphaTestEnabled)
{
{
if((_SrcColour.w <= _gm_AlphaRefValue))
{
{
discard;
;
}
;
}
;
}
;
}
;
}
}
;
void _DoFog(inout float4 _SrcColour, in float _fogval)
{
{
if(_gm_PS_FogEnabled)
{
{
(_SrcColour = lerp(_SrcColour, _gm_FogColour, clamp(_fogval, 0.0, 1.0)));
}
;
}
;
}
}
;
;
;
;
;
;
;
;
;
;
;
;
;
;
;
void gl_main()
{
{
float4 _col = gl_texture2D(_gm_BaseTexture, _v_vTexcoord);
if(all(_col.xyz == _f_Colour1.xyz))
{
{
(_col.xyz = _f_Colour2.xyz);
}
;
}
;
if(all(_col.xyz == _f_Colour3.xyz))
{
{
(_col.xyz = _f_Colour4.xyz);
}
;
}
;
if(all(_col.xyz == _f_Colour5.xyz))
{
{
(_col.xyz = _f_Colour6.xyz);
}
;
}
;
if(all(_col.xyz == _f_Colour7.xyz))
{
{
(_col.xyz = _f_Colour8.xyz);
}
;
}
;
if(all(_col.xyz == _f_Colour9.xyz))
{
{
(_col.xyz = _f_Colour10.xyz);
}
;
}
;
if(all(_col.xyz == _f_Colour11.xyz))
{
{
(_col.xyz = _f_Colour12.xyz);
}
;
}
;
(gl_Color[0] = (_v_vColour * _col));
}
}
;
struct PS_INPUT
{
    float4 v0 : TEXCOORD0;
    float2 v1 : TEXCOORD1;
};

struct PS_OUTPUT
{
    float4 gl_Color0 : COLOR0;
};

PS_OUTPUT main(PS_INPUT input)
{
    _v_vColour = input.v0;
    _v_vTexcoord = input.v1.xy;

    gl_main();

    PS_OUTPUT output;
    output.gl_Color0 = gl_Color[0];

    return output;
}
