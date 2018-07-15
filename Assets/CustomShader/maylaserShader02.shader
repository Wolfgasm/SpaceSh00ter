// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:32990,y:32663,varname:node_4795,prsc:2|emission-2393-OUT;n:type:ShaderForge.SFN_Tex2d,id:6074,x:32235,y:32601,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:0cceeb6d95920fc49a6ad995e9e5315e,ntxv:0,isnm:False|UVIN-2897-OUT;n:type:ShaderForge.SFN_Multiply,id:2393,x:32686,y:32774,varname:node_2393,prsc:2|A-6074-RGB,B-2053-RGB,C-797-RGB,D-9248-OUT,E-297-OUT;n:type:ShaderForge.SFN_VertexColor,id:2053,x:32272,y:32779,varname:node_2053,prsc:2;n:type:ShaderForge.SFN_Color,id:797,x:32235,y:32930,ptovrint:True,ptlb:Color,ptin:_TintColor,varname:_TintColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Vector1,id:9248,x:32235,y:33081,varname:node_9248,prsc:2,v1:2;n:type:ShaderForge.SFN_Multiply,id:297,x:32518,y:32965,varname:node_297,prsc:2|A-6074-A,B-797-A;n:type:ShaderForge.SFN_Time,id:8188,x:31533,y:32401,varname:node_8188,prsc:2;n:type:ShaderForge.SFN_Vector1,id:985,x:31480,y:32653,cmnt:1,varname:node_985,prsc:2,v1:0.2;n:type:ShaderForge.SFN_Vector1,id:1070,x:31457,y:32741,varname:node_1070,prsc:2,v1:0;n:type:ShaderForge.SFN_Append,id:3138,x:31691,y:32653,varname:node_3138,prsc:2|A-985-OUT,B-1070-OUT;n:type:ShaderForge.SFN_Multiply,id:7519,x:31838,y:32396,varname:node_7519,prsc:2|A-8188-T,B-3138-OUT;n:type:ShaderForge.SFN_TexCoord,id:1364,x:31908,y:32210,varname:node_1364,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Add,id:2897,x:32183,y:32216,varname:node_2897,prsc:2|A-1364-UVOUT,B-7519-OUT;proporder:6074-797;pass:END;sub:END;*/

Shader "Shader Forge/maylaserShader02" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _TintColor ("Color", Color) = (0.5,0.5,0.5,1)
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _TintColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 node_8188 = _Time;
                float2 node_2897 = (i.uv0+(node_8188.g*float2(0.2,0.0)));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_2897, _MainTex));
                float3 emissive = (_MainTex_var.rgb*i.vertexColor.rgb*_TintColor.rgb*2.0*(_MainTex_var.a*_TintColor.a));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG_COLOR(i.fogCoord, finalRGBA, fixed4(0,0,0,1));
                return finalRGBA;
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
