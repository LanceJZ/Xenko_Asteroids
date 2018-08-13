/**************************
***** Compiler Parameters *****
***************************
@P EffectName: XenkoForwardShadingEffect
@P   - Material.PixelStageSurfaceShaders: mixin MaterialSurfaceArray [{layers = [mixin MaterialSurfaceDiffuse [{diffuseMap = ComputeColorConstantColorLink<Material.DiffuseValue>}], mixin MaterialSurfaceLightingAndShading [{surfaces = [MaterialSurfaceShadingDiffuseLambert<false>]}]]}]
@P Material.PixelStageStreamInitializer: mixin MaterialStream, MaterialPixelShadingStream
@P Lighting.DirectLightGroups: mixin LightDirectionalGroup<8>, LightClusteredPointGroup, LightClusteredSpotGroup
@P Lighting.EnvironmentLights: LightSimpleAmbient, EnvironmentLight
@P XenkoEffectBase.RenderTargetExtensions: mixin
***************************
****  ConstantBuffers  ****
***************************
cbuffer PerDraw [Size: 416]
@C    World_id31 => Transformation.World
@C    WorldInverse_id32 => Transformation.WorldInverse
@C    WorldInverseTranspose_id33 => Transformation.WorldInverseTranspose
@C    WorldView_id34 => Transformation.WorldView
@C    WorldViewInverse_id35 => Transformation.WorldViewInverse
@C    WorldViewProjection_id36 => Transformation.WorldViewProjection
@C    WorldScale_id37 => Transformation.WorldScale
@C    EyeMS_id38 => Transformation.EyeMS
cbuffer PerMaterial [Size: 16]
@C    constantColor_id102 => Material.DiffuseValue
cbuffer PerView [Size: 720]
@C    View_id24 => Transformation.View
@C    ViewInverse_id25 => Transformation.ViewInverse
@C    Projection_id26 => Transformation.Projection
@C    ProjectionInverse_id27 => Transformation.ProjectionInverse
@C    ViewProjection_id28 => Transformation.ViewProjection
@C    ProjScreenRay_id29 => Transformation.ProjScreenRay
@C    Eye_id30 => Transformation.Eye
@C    NearClipPlane_id87 => Camera.NearClipPlane
@C    FarClipPlane_id88 => Camera.FarClipPlane
@C    ZProjection_id89 => Camera.ZProjection
@C    ViewSize_id90 => Camera.ViewSize
@C    AspectRatio_id91 => Camera.AspectRatio
@C    _padding_PerView_Default => _padding_PerView_Default
@C    LightCount_id83 => DirectLightGroupPerView.LightCount.directLightGroups[0]
@C    Lights_id85 => LightDirectionalGroup.Lights.directLightGroups[0]
@C    ClusterDepthScale_id96 => LightClustered.ClusterDepthScale
@C    ClusterDepthBias_id97 => LightClustered.ClusterDepthBias
@C    ClusterStride_id98 => LightClustered.ClusterStride
@C    AmbientLight_id101 => LightSimpleAmbient.AmbientLight.environmentLights[0]
@C    _padding_PerView_Lighting => _padding_PerView_Lighting
***************************
******  Resources    ******
***************************
@R    PerMaterial => PerMaterial [Stage: None, Slot: (-1--1)]
@R    LightClusters_id94 => LightClustered.LightClusters [Stage: None, Slot: (-1--1)]
@R    LightClusters_id94 => LightClustered.LightClusters [Stage: None, Slot: (-1--1)]
@R    LightIndices_id95 => LightClustered.LightIndices [Stage: None, Slot: (-1--1)]
@R    LightIndices_id95 => LightClustered.LightIndices [Stage: None, Slot: (-1--1)]
@R    PointLights_id99 => LightClusteredPointGroup.PointLights [Stage: None, Slot: (-1--1)]
@R    PointLights_id99 => LightClusteredPointGroup.PointLights [Stage: None, Slot: (-1--1)]
@R    SpotLights_id100 => LightClusteredSpotGroup.SpotLights [Stage: None, Slot: (-1--1)]
@R    SpotLights_id100 => LightClusteredSpotGroup.SpotLights [Stage: None, Slot: (-1--1)]
@R    PerDraw => PerDraw [Stage: Vertex, Slot: (0-0)]
@R    PerView => PerView [Stage: Vertex, Slot: (1-1)]
@R    LightClusters_id94 => LightClustered.LightClusters [Stage: Pixel, Slot: (0-0)]
@R    LightIndices_id95 => LightClustered.LightIndices [Stage: Pixel, Slot: (1-1)]
@R    PointLights_id99 => LightClusteredPointGroup.PointLights [Stage: Pixel, Slot: (2-2)]
@R    SpotLights_id100 => LightClusteredSpotGroup.SpotLights [Stage: Pixel, Slot: (3-3)]
@R    PerMaterial => PerMaterial [Stage: Pixel, Slot: (0-0)]
@R    PerView => PerView [Stage: Pixel, Slot: (1-1)]
***************************
*****     Sources     *****
***************************
@S    ShaderBase => acbe3d4d44a046eede871176bee9c754
@S    ShaderBaseStream => a3a5bf8185f2a3d89972293f806430d3
@S    ShadingBase => a56c21640e78c756e8d56651480eb9f5
@S    ComputeColor => ded06879812e042b84d284d2272e4b4a
@S    TransformationBase => be8628f6067b518dd5c7b3fe338b9320
@S    NormalStream => b59c6ee174b93be981cc113a7a70d70b
@S    TransformationWAndVP => 37eaa3c16c9e83fd77e04c47c6794803
@S    PositionStream4 => b1f2243b30eb87e6e40bc6af56b4fd18
@S    PositionHStream4 => 0c0bb9059a8e3199d9ec950f3a7d2b66
@S    Transformation => ce8c4a6980d1f949f5f9aed15679c96d
@S    NormalFromMesh => 7e93d74a2c2c59456ebca897ee2c4bdb
@S    NormalBase => 118edc0075e4c3a87c3cb4570a808039
@S    NormalUpdate => 9bf3758fe45b1105750554fed463a15e
@S    MaterialSurfacePixelStageCompositor => cb7b21ca902e1b289c9d765318058f5a
@S    PositionStream => d767885dace5d697c6532bb4f0f5b3f8
@S    MaterialPixelShadingStream => ce423644e4da2371ef9e6b451c39edf7
@S    MaterialPixelStream => 83c4d63f50d89eb133457f26bcc25822
@S    MaterialStream => f4c30b25d4f10a4a3809b97598bfca17
@S    IStreamInitializer => 37d0ab08af9e2896a54aa43c0ea2ad0f
@S    LightStream => 622a7adc4e53980a3d0e0fecffc76661
@S    DirectLightGroupArray => b6017f14fd58343b7a54fefa2ca81610
@S    DirectLightGroup => 08ae1198dfb5788ff8584d747c3b8368
@S    ShadowGroup => 05cfb339de033879838a6606f047a866
@S    ShadowStream => 38650f3182fa7d3cab3bae43f9398f4e
@S    TextureProjectionGroup => 47f8334211d8d5ae6d98f32a923759cc
@S    EnvironmentLightArray => 4129555ea2051a98f5cf5e315d791d3e
@S    EnvironmentLight => 6aceaad54f2057382904d7f6dfa58e4f
@S    IMaterialSurface => d56637e1116951bd72b1817daa1a6158
@S    LightDirectionalGroup => f9970193df80a5a2cd0e2949fd2c68f8
@S    DirectLightGroupPerView => 687613ae4b0bcdb3a67791902ac3ff9d
@S    LightDirectional => d971eabfd75851e9aac331559fd4c302
@S    LightClusteredPointGroup => c5703508c16a19f7acb9ebe6676eeb4a
@S    LightClustered => 82f6bcf6878ecb8f1cf19364969848f1
@S    ScreenPositionBase => b66b925e44d5cef758dd09d560d88680
@S    Camera => 38d8cc7176ac62fc4d9c97d70f9013ab
@S    LightPoint => 5cc6bc739ab0f656d8a48332e8ca569b
@S    LightUtil => 2587d00dc291bbace485f1420ae464ad
@S    LightClusteredSpotGroup => d9e6bd2fe470894c4d8bebba13305bf2
@S    LightSpot => ff13b9737bc99d2458fa5aa679b6b8f0
@S    SpotLightDataInternalShader => 4c9afecddd31668dbea2f90869a2d5f0
@S    LightSpotAttenuationDefault => a424c87e1fb86625b52e4ab0ab01bf46
@S    LightSimpleAmbient => 5216c2bc901f0543bc29393671a09451
@S    MaterialSurfaceArray => ca7a8b492198ae093d4f490c2ba6aaec
@S    MaterialSurfaceDiffuse => c60d7ca8058e062fcba77cc9b4c7d496
@S    IMaterialSurfacePixel => b5f583c7b871b6a4ebe5c7411883503e
@S    ComputeColorConstantColorLink => a30d6cd76f7c0875cdeffdb3ae8bae33
@S    MaterialSurfaceLightingAndShading => 68ac8c9f25624fdc15ea262fbc868030
@S    Math => 49d7f7706890095f248caeaf232f4db4
@S    IMaterialSurfaceShading => 43b5938a14c30cfc19b2ddcb76824cbe
@S    MaterialSurfaceShadingDiffuseLambert => ebe9647a0e11903ef16a793ffdbdd34c
***************************
*****     Stages      *****
***************************
@G    Vertex => 2393b57aa6b88307537fc5f609f5655b
//
// Generated by Microsoft (R) HLSL Shader Compiler 10.1
//
//
// Buffer Definitions: 
//
// cbuffer PerDraw
// {
//
//   float4x4 World_id31;               // Offset:    0 Size:    64
//   float4x4 WorldInverse_id32;        // Offset:   64 Size:    64 [unused]
//   float4x4 WorldInverseTranspose_id33;// Offset:  128 Size:    64
//   float4x4 WorldView_id34;           // Offset:  192 Size:    64 [unused]
//   float4x4 WorldViewInverse_id35;    // Offset:  256 Size:    64 [unused]
//   float4x4 WorldViewProjection_id36; // Offset:  320 Size:    64 [unused]
//   float3 WorldScale_id37;            // Offset:  384 Size:    12 [unused]
//   float4 EyeMS_id38;                 // Offset:  400 Size:    16 [unused]
//
// }
//
// cbuffer PerView
// {
//
//   float4x4 View_id24;                // Offset:    0 Size:    64 [unused]
//   float4x4 ViewInverse_id25;         // Offset:   64 Size:    64 [unused]
//   float4x4 Projection_id26;          // Offset:  128 Size:    64 [unused]
//   float4x4 ProjectionInverse_id27;   // Offset:  192 Size:    64 [unused]
//   float4x4 ViewProjection_id28;      // Offset:  256 Size:    64
//   float2 ProjScreenRay_id29;         // Offset:  320 Size:     8 [unused]
//   float4 Eye_id30;                   // Offset:  336 Size:    16 [unused]
//   float NearClipPlane_id87;          // Offset:  352 Size:     4 [unused]
//      = 0x3f800000 
//   float FarClipPlane_id88;           // Offset:  356 Size:     4 [unused]
//      = 0x42c80000 
//   float2 ZProjection_id89;           // Offset:  360 Size:     8 [unused]
//   float2 ViewSize_id90;              // Offset:  368 Size:     8 [unused]
//   float AspectRatio_id91;            // Offset:  376 Size:     4 [unused]
//   float4 _padding_PerView_Default;   // Offset:  384 Size:    16 [unused]
//   int LightCount_id83;               // Offset:  400 Size:     4 [unused]
//   
//   struct DirectionalLightData
//   {
//       
//       float3 DirectionWS;            // Offset:  416
//       float3 Color;                  // Offset:  432
//
//   } Lights_id85[8];                  // Offset:  416 Size:   252 [unused]
//   float ClusterDepthScale_id96;      // Offset:  668 Size:     4 [unused]
//   float ClusterDepthBias_id97;       // Offset:  672 Size:     4 [unused]
//   float2 ClusterStride_id98;         // Offset:  676 Size:     8 [unused]
//   float3 AmbientLight_id101;         // Offset:  688 Size:    12 [unused]
//   float4 _padding_PerView_Lighting;  // Offset:  704 Size:    16 [unused]
//
// }
//
//
// Resource Bindings:
//
// Name                                 Type  Format         Dim      HLSL Bind  Count
// ------------------------------ ---------- ------- ----------- -------------- ------
// PerDraw                           cbuffer      NA          NA            cb0      1 
// PerView                           cbuffer      NA          NA            cb1      1 
//
//
//
// Input signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// POSITION                 0   xyzw        0     NONE   float   xyzw
// NORMAL                   0   xyz         1     NONE   float   xyz 
//
//
// Output signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// POSITION_WS              0   xyzw        0     NONE   float   xyzw
// SV_Position              0   xyzw        1      POS   float   xyzw
// NORMALWS                 0   xyz         2     NONE   float   xyz 
// SCREENPOSITION_ID86_SEM     0   xyzw        3     NONE   float   xyzw
//
vs_5_0
dcl_globalFlags refactoringAllowed
dcl_constantbuffer CB0[11], immediateIndexed
dcl_constantbuffer CB1[20], immediateIndexed
dcl_input v0.xyzw
dcl_input v1.xyz
dcl_output o0.xyzw
dcl_output_siv o1.xyzw, position
dcl_output o2.xyz
dcl_output o3.xyzw
dcl_temps 2
//
// Initial variable locations:
//   v0.x <- __input__.Position_id20.x; v0.y <- __input__.Position_id20.y; v0.z <- __input__.Position_id20.z; v0.w <- __input__.Position_id20.w; 
//   v1.x <- __input__.meshNormal_id15.x; v1.y <- __input__.meshNormal_id15.y; v1.z <- __input__.meshNormal_id15.z; 
//   o3.x <- <VSMain return value>.ScreenPosition_id86.x; o3.y <- <VSMain return value>.ScreenPosition_id86.y; o3.z <- <VSMain return value>.ScreenPosition_id86.z; o3.w <- <VSMain return value>.ScreenPosition_id86.w; 
//   o2.x <- <VSMain return value>.normalWS_id18.x; o2.y <- <VSMain return value>.normalWS_id18.y; o2.z <- <VSMain return value>.normalWS_id18.z; 
//   o1.x <- <VSMain return value>.ShadingPosition_id0.x; o1.y <- <VSMain return value>.ShadingPosition_id0.y; o1.z <- <VSMain return value>.ShadingPosition_id0.z; o1.w <- <VSMain return value>.ShadingPosition_id0.w; 
//   o0.x <- <VSMain return value>.PositionWS_id21.x; o0.y <- <VSMain return value>.PositionWS_id21.y; o0.z <- <VSMain return value>.PositionWS_id21.z; o0.w <- <VSMain return value>.PositionWS_id21.w
//
#line 622 "E:\Documents\Xenko Projects\Xenko_Asteroids\Asteroids\Bin\Windows\Debug\log\shader_XenkoForwardShadingEffect_7c2d452d7c1618c9916b3699a5c91139.hlsl"
dp4 r0.x, v0.xyzw, cb0[0].xyzw  // r0.x <- streams.PositionWS_id21.x
dp4 r0.y, v0.xyzw, cb0[1].xyzw  // r0.y <- streams.PositionWS_id21.y
dp4 r0.z, v0.xyzw, cb0[2].xyzw  // r0.z <- streams.PositionWS_id21.z
dp4 r0.w, v0.xyzw, cb0[3].xyzw  // r0.w <- streams.PositionWS_id21.w

#line 701
mov o0.xyzw, r0.xyzw

#line 585
dp4 r1.x, r0.xyzw, cb1[16].xyzw  // r1.x <- <ComputeShadingPosition_id11 return value>.x
dp4 r1.y, r0.xyzw, cb1[17].xyzw  // r1.y <- <ComputeShadingPosition_id11 return value>.y
dp4 r1.z, r0.xyzw, cb1[18].xyzw  // r1.z <- <ComputeShadingPosition_id11 return value>.z
dp4 r1.w, r0.xyzw, cb1[19].xyzw  // r1.w <- <ComputeShadingPosition_id11 return value>.w

#line 701
mov o1.xyzw, r1.xyzw
mov o3.xyzw, r1.xyzw

#line 657
dp3 o2.x, v1.xyzx, cb0[8].xyzx
dp3 o2.y, v1.xyzx, cb0[9].xyzx
dp3 o2.z, v1.xyzx, cb0[10].xyzx

#line 701
ret 
// Approximately 15 instruction slots used
@G    Pixel => 357278a8c1a37bd5c9ec6cbed19279c4
//
// Generated by Microsoft (R) HLSL Shader Compiler 10.1
//
//
// Buffer Definitions: 
//
// cbuffer PerMaterial
// {
//
//   float4 constantColor_id102;        // Offset:    0 Size:    16
//
// }
//
// cbuffer PerView
// {
//
//   float4x4 View_id24;                // Offset:    0 Size:    64 [unused]
//   float4x4 ViewInverse_id25;         // Offset:   64 Size:    64 [unused]
//   float4x4 Projection_id26;          // Offset:  128 Size:    64 [unused]
//   float4x4 ProjectionInverse_id27;   // Offset:  192 Size:    64 [unused]
//   float4x4 ViewProjection_id28;      // Offset:  256 Size:    64 [unused]
//   float2 ProjScreenRay_id29;         // Offset:  320 Size:     8 [unused]
//   float4 Eye_id30;                   // Offset:  336 Size:    16 [unused]
//   float NearClipPlane_id87;          // Offset:  352 Size:     4 [unused]
//      = 0x3f800000 
//   float FarClipPlane_id88;           // Offset:  356 Size:     4 [unused]
//      = 0x42c80000 
//   float2 ZProjection_id89;           // Offset:  360 Size:     8
//   float2 ViewSize_id90;              // Offset:  368 Size:     8 [unused]
//   float AspectRatio_id91;            // Offset:  376 Size:     4 [unused]
//   float4 _padding_PerView_Default;   // Offset:  384 Size:    16 [unused]
//   int LightCount_id83;               // Offset:  400 Size:     4
//   
//   struct DirectionalLightData
//   {
//       
//       float3 DirectionWS;            // Offset:  416
//       float3 Color;                  // Offset:  432
//
//   } Lights_id85[8];                  // Offset:  416 Size:   252
//   float ClusterDepthScale_id96;      // Offset:  668 Size:     4
//   float ClusterDepthBias_id97;       // Offset:  672 Size:     4
//   float2 ClusterStride_id98;         // Offset:  676 Size:     8
//   float3 AmbientLight_id101;         // Offset:  688 Size:    12
//   float4 _padding_PerView_Lighting;  // Offset:  704 Size:    16 [unused]
//
// }
//
//
// Resource Bindings:
//
// Name                                 Type  Format         Dim      HLSL Bind  Count
// ------------------------------ ---------- ------- ----------- -------------- ------
// LightClusters_id94                texture   uint2          3d             t0      1 
// LightIndices_id95                 texture    uint         buf             t1      1 
// PointLights_id99                  texture  float4         buf             t2      1 
// SpotLights_id100                  texture  float4         buf             t3      1 
// PerMaterial                       cbuffer      NA          NA            cb0      1 
// PerView                           cbuffer      NA          NA            cb1      1 
//
//
//
// Input signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// POSITION_WS              0   xyzw        0     NONE   float   xyz 
// SV_Position              0   xyzw        1      POS   float     z 
// NORMALWS                 0   xyz         2     NONE   float   xyz 
// SCREENPOSITION_ID86_SEM     0   xyzw        3     NONE   float   xy w
// SV_IsFrontFace           0   x           4    FFACE    uint   x   
//
//
// Output signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// SV_Target                0   xyzw        0   TARGET   float   xyzw
//
ps_5_0
dcl_globalFlags refactoringAllowed
dcl_constantbuffer CB0[1], immediateIndexed
dcl_constantbuffer CB1[44], dynamicIndexed
dcl_resource_texture3d (uint,uint,uint,uint) t0
dcl_resource_buffer (uint,uint,uint,uint) t1
dcl_resource_buffer (float,float,float,float) t2
dcl_resource_buffer (float,float,float,float) t3
dcl_input_ps linear v0.xyz
dcl_input_ps_siv linear noperspective v1.z, position
dcl_input_ps linear v2.xyz
dcl_input_ps linear v3.xyw
dcl_input_ps_sgv constant v4.x, is_front_face
dcl_output o0.xyzw
dcl_temps 10
//
// Initial variable locations:
//   v0.x <- __input__.PositionWS_id21.x; v0.y <- __input__.PositionWS_id21.y; v0.z <- __input__.PositionWS_id21.z; v0.w <- __input__.PositionWS_id21.w; 
//   v1.x <- __input__.ShadingPosition_id0.x; v1.y <- __input__.ShadingPosition_id0.y; v1.z <- __input__.ShadingPosition_id0.z; v1.w <- __input__.ShadingPosition_id0.w; 
//   v2.x <- __input__.normalWS_id18.x; v2.y <- __input__.normalWS_id18.y; v2.z <- __input__.normalWS_id18.z; 
//   v3.x <- __input__.ScreenPosition_id86.x; v3.y <- __input__.ScreenPosition_id86.y; v3.z <- __input__.ScreenPosition_id86.z; v3.w <- __input__.ScreenPosition_id86.w; 
//   v4.x <- __input__.IsFrontFace_id1; 
//   o0.x <- <PSMain return value>.ColorTarget_id2.x; o0.y <- <PSMain return value>.ColorTarget_id2.y; o0.z <- <PSMain return value>.ColorTarget_id2.z; o0.w <- <PSMain return value>.ColorTarget_id2.w
//
#line 683 "E:\Documents\Xenko Projects\Xenko_Asteroids\Asteroids\Bin\Windows\Debug\log\shader_XenkoForwardShadingEffect_7c2d452d7c1618c9916b3699a5c91139.hlsl"
div r0.xy, v3.xyxx, v3.wwww  // r0.x <- streams.ScreenPosition_id86.x; r0.y <- streams.ScreenPosition_id86.y

#line 651
dp3 r0.z, v2.xyzx, v2.xyzx
lt r0.w, l(0.000000), r0.z

#line 652
rsq r0.z, r0.z
mul r1.xyz, r0.zzzz, v2.xyzx  // r1.x <- streams.normalWS_id18.x; r1.y <- streams.normalWS_id18.y; r1.z <- streams.normalWS_id18.z
movc r1.xyz, r0.wwww, r1.xyzx, v2.xyzx

#line 461
movc r1.xyz, v4.xxxx, r1.xyzx, -r1.xyzx  // r1.x <- streams.normalWS_id18.x; r1.y <- streams.normalWS_id18.y; r1.z <- streams.normalWS_id18.z

#line 475
mov r2.xyz, r1.xyzx  // r2.x <- streams.normalWS_id18.x; r2.y <- streams.normalWS_id18.y; r2.z <- streams.normalWS_id18.z
mov r3.xyz, cb0[0].xyzx  // r3.x <- streams.matDiffuseVisible_id67.x; r3.y <- streams.matDiffuseVisible_id67.y; r3.z <- streams.matDiffuseVisible_id67.z
mov r4.xyz, l(0,0,0,0)  // r4.x <- directLightingContribution.x; r4.y <- directLightingContribution.y; r4.z <- directLightingContribution.z
mov r0.z, l(0)  // r0.z <- i
loop 
  ige r0.w, r0.z, l(8)
  breakc_nz r0.w

#line 477
  ige r0.w, r0.z, cb1[25].x
  if_nz r0.w

#line 479
    break 

#line 480
  endif 

#line 316
  ishl r0.w, r0.z, l(1)

#line 402
  dp3 r1.w, r2.xyzx, -cb1[r0.w + 26].xyzx
  max r1.w, r1.w, l(0.000100)  // r1.w <- streams.NdotL_id47

#line 404
  mul r5.xyz, r1.wwww, cb1[r0.w + 27].xyzx  // r5.x <- streams.lightColorNdotL_id43.x; r5.y <- streams.lightColorNdotL_id43.y; r5.z <- streams.lightColorNdotL_id43.z

#line 390
  mul r5.xyz, r5.xyzx, r3.xyzx

#line 485
  mad r4.xyz, r5.xyzx, l(0.318310, 0.318310, 0.318310, 0.000000), r4.xyzx

#line 475
  iadd r0.z, r0.z, l(1)

#line 487
endloop 

#line 295
add r0.z, v1.z, -cb1[22].z
div r0.z, cb1[22].w, r0.z  // r0.z <- depth

#line 296
mad r0.xy, r0.xyxx, l(1.000000, -1.000000, 0.000000, 0.000000), l(1.000000, 1.000000, 0.000000, 0.000000)
mul r0.xy, r0.xyxx, cb1[42].yzyy

#line 297
mad r0.z, r0.z, cb1[41].w, cb1[42].x
log r0.z, r0.z
max r0.z, r0.z, l(0.000000)

#line 298
mul r0.xy, r0.xyxx, l(0.500000, 0.500000, 0.000000, 0.000000)

#line 297
ftoi r2.xyz, r0.xyzx  // r2.z <- slice

#line 298
mov r2.w, l(0)
ld_indexable(texture3d)(uint,uint,uint,uint) r0.xy, r2.xyzw, t0.xyzw  // r0.x <- streams.lightData_id92.x; r0.y <- streams.lightData_id92.y

#line 381
and r0.z, r0.y, l(0x0000ffff)  // r0.z <- <GetMaxLightCount_id113 return value>

#line 495
mov r2.xyz, r1.xyzx  // r2.x <- streams.normalWS_id18.x; r2.y <- streams.normalWS_id18.y; r2.z <- streams.normalWS_id18.z
mov r2.w, v0.x  // r2.w <- streams.PositionWS_id21.x
mov r3.yz, v0.yyzy  // r3.y <- streams.PositionWS_id21.y; r3.z <- streams.PositionWS_id21.z
mov r5.xyz, cb0[0].xyzx  // r5.x <- streams.matDiffuseVisible_id67.x; r5.y <- streams.matDiffuseVisible_id67.y; r5.z <- streams.matDiffuseVisible_id67.z
mov r6.xyz, r4.xyzx  // r6.x <- directLightingContribution.x; r6.y <- directLightingContribution.y; r6.z <- directLightingContribution.z
mov r7.x, r0.x  // r7.x <- streams.lightIndex_id93
mov r0.w, l(0)  // r0.w <- i
loop 
  ige r1.w, r0.w, r0.z
  breakc_nz r1.w

#line 497
  if_nz r1.w

#line 499
    break 

#line 500
  endif 

#line 282
  ld_indexable(buffer)(uint,uint,uint,uint) r1.w, r7.xxxx, t1.yzwx  // r1.w <- realLightIndex

#line 283
  iadd r7.x, r7.x, l(1)  // r7.x <- streams.lightIndex_id93

#line 285
  ishl r3.w, r1.w, l(1)
  ld_indexable(buffer)(float,float,float,float) r8.xyzw, r3.wwww, t2.xyzw  // r8.x <- pointLight1.x; r8.y <- pointLight1.y; r8.z <- pointLight1.z; r8.w <- pointLight1.w

#line 286
  bfi r1.w, l(31), l(1), r1.w, l(1)
  ld_indexable(buffer)(float,float,float,float) r7.yzw, r1.wwww, t2.wxyz  // r7.y <- pointLight2.x; r7.z <- pointLight2.y; r7.w <- pointLight2.z

#line 213
  mov r3.x, r2.w
  add r8.xyz, -r3.xyzx, r8.xyzx  // r8.x <- lightVector.x; r8.y <- lightVector.y; r8.z <- lightVector.z

#line 214
  dp3 r1.w, r8.xyzx, r8.xyzx
  sqrt r3.x, r1.w  // r3.x <- lightVectorLength

#line 215
  div r8.xyz, r8.xyzx, r3.xxxx  // r8.x <- lightVectorNorm.x; r8.y <- lightVectorNorm.y; r8.z <- lightVectorNorm.z

#line 194
  max r3.x, r1.w, l(0.000100)
  div r3.x, l(1.000000, 1.000000, 1.000000, 1.000000), r3.x  // r3.x <- attenuation

#line 173
  mul r1.w, r8.w, r1.w  // r1.w <- factor

#line 174
  mad r1.w, -r1.w, r1.w, l(1.000000)
  max r1.w, r1.w, l(0.000000)  // r1.w <- smoothFactor

#line 175
  mul r1.w, r1.w, r1.w  // r1.w <- <SmoothDistanceAttenuation_id54 return value>

#line 195
  mul r1.w, r1.w, r3.x  // r1.w <- attenuation

#line 231
  mul r7.yzw, r1.wwww, r7.yyzw  // r7.y <- streams.lightColor_id42.x; r7.z <- streams.lightColor_id42.y; r7.w <- streams.lightColor_id42.z

#line 367
  dp3 r1.w, r2.xyzx, r8.xyzx
  max r1.w, r1.w, l(0.000100)  // r1.w <- streams.NdotL_id47

#line 369
  mul r7.yzw, r1.wwww, r7.yyzw  // r7.y <- streams.lightColorNdotL_id43.x; r7.z <- streams.lightColorNdotL_id43.y; r7.w <- streams.lightColorNdotL_id43.z

#line 390
  mul r7.yzw, r7.yyzw, r5.xxyz

#line 505
  mad r6.xyz, r7.yzwy, l(0.318310, 0.318310, 0.318310, 0.000000), r6.xyzx

#line 495
  iadd r0.w, r0.w, l(1)

#line 507
endloop   // r7.x <- streams.lightIndex_id93

#line 359
ushr r0.x, r0.y, l(16)  // r0.x <- <GetMaxLightCount_id125 return value>

#line 515
mov r2.xyz, r1.xyzx
mov r2.w, v0.x
mov r3.yz, v0.yyzy
mov r0.yzw, cb0[0].xxyz  // r0.y <- streams.matDiffuseVisible_id67.x; r0.z <- streams.matDiffuseVisible_id67.y; r0.w <- streams.matDiffuseVisible_id67.z
mov r4.xyz, r6.xyzx  // r4.x <- directLightingContribution.x; r4.y <- directLightingContribution.y; r4.z <- directLightingContribution.z
mov r1.w, r7.x  // r1.w <- streams.lightIndex_id93
mov r3.w, l(0)  // r3.w <- i
loop 
  ige r4.w, r3.w, r0.x
  breakc_nz r4.w

#line 517
  if_nz r4.w

#line 519
    break 

#line 520
  endif 

#line 254
  ld_indexable(buffer)(uint,uint,uint,uint) r4.w, r1.wwww, t1.yzwx  // r4.w <- realLightIndex

#line 255
  iadd r1.w, r1.w, l(1)  // r1.w <- streams.lightIndex_id93

#line 257
  ishl r5.x, r4.w, l(2)
  ld_indexable(buffer)(float,float,float,float) r5.xyz, r5.xxxx, t3.xyzw  // r5.x <- spotLight1.x; r5.y <- spotLight1.y; r5.z <- spotLight1.z

#line 260
  bfi r7.yzw, l(0, 30, 30, 30), l(0, 2, 2, 2), r4.wwww, l(0, 1, 2, 3)

#line 258
  ld_indexable(buffer)(float,float,float,float) r8.xyz, r7.yyyy, t3.xyzw  // r8.x <- spotLight2.x; r8.y <- spotLight2.y; r8.z <- spotLight2.z

#line 259
  ld_indexable(buffer)(float,float,float,float) r9.xyz, r7.zzzz, t3.xyzw  // r9.x <- spotLight3.x; r9.y <- spotLight3.y; r9.z <- spotLight3.z

#line 260
  ld_indexable(buffer)(float,float,float,float) r7.yzw, r7.wwww, t3.wxyz  // r7.y <- spotLight4.x; r7.z <- spotLight4.y; r7.w <- spotLight4.z

#line 200
  mov r3.x, r2.w
  add r5.xyz, -r3.xyzx, r5.xyzx  // r5.x <- lightVector.x; r5.y <- lightVector.y; r5.z <- lightVector.z

#line 201
  dp3 r3.x, r5.xyzx, r5.xyzx
  sqrt r4.w, r3.x  // r4.w <- lightVectorLength

#line 202
  div r5.xyz, r5.xyzx, r4.wwww  // r5.x <- lightVectorNorm.x; r5.y <- lightVectorNorm.y; r5.z <- lightVectorNorm.z

#line 187
  max r4.w, r3.x, l(0.000100)
  div r4.w, l(1.000000, 1.000000, 1.000000, 1.000000), r4.w  // r4.w <- attenuation

#line 167
  mul r3.x, r9.z, r3.x  // r3.x <- factor

#line 168
  mad r3.x, -r3.x, r3.x, l(1.000000)
  max r3.x, r3.x, l(0.000000)  // r3.x <- smoothFactor

#line 169
  mul r3.x, r3.x, r3.x  // r3.x <- <SmoothDistanceAttenuation_id65 return value>

#line 188
  mul r3.x, r3.x, r4.w  // r3.x <- attenuation

#line 179
  dp3 r4.w, -r8.xyzx, r5.xyzx  // r4.w <- cd

#line 180
  mad_sat r4.w, r4.w, r9.x, r9.y  // r4.w <- attenuation

#line 181
  mul r4.w, r4.w, r4.w

#line 208
  mul r3.x, r3.x, r4.w  // r3.x <- attenuation

#line 223
  mul r7.yzw, r3.xxxx, r7.yyzw  // r7.y <- streams.lightColor_id42.x; r7.z <- streams.lightColor_id42.y; r7.w <- streams.lightColor_id42.z

#line 345
  dp3 r3.x, r2.xyzx, r5.xyzx
  max r3.x, r3.x, l(0.000100)  // r3.x <- streams.NdotL_id47

#line 347
  mul r5.xyz, r3.xxxx, r7.yzwy  // r5.x <- streams.lightColorNdotL_id43.x; r5.y <- streams.lightColorNdotL_id43.y; r5.z <- streams.lightColorNdotL_id43.z

#line 390
  mul r5.xyz, r0.yzwy, r5.xyzx

#line 525
  mad r4.xyz, r5.xyzx, l(0.318310, 0.318310, 0.318310, 0.000000), r4.xyzx

#line 515
  iadd r3.w, r3.w, l(1)

#line 527
endloop   // r1.w <- streams.lightIndex_id93

#line 333
mul r0.xyz, cb0[0].xyzx, cb1[43].xyzx  // r0.x <- <ComputeEnvironmentLightContribution_id138 return value>.x; r0.y <- <ComputeEnvironmentLightContribution_id138 return value>.y; r0.z <- <ComputeEnvironmentLightContribution_id138 return value>.z

#line 546
mad o0.xyz, r4.xyzx, l(3.141593, 3.141593, 3.141593, 0.000000), r0.xyzx

#line 687
mov o0.w, cb0[0].w
ret 
// Approximately 127 instruction slots used
***************************
*************************/
const static int TMaxLightCount_id84 = 8;
static const float PI_id105 = 3.14159265358979323846;
const static bool TIsEnergyConservative_id106 = false;
struct PS_STREAMS 
{
    float4 ScreenPosition_id86;
    float3 normalWS_id18;
    float4 PositionWS_id21;
    float4 ShadingPosition_id0;
    bool IsFrontFace_id1;
    float3 meshNormalWS_id16;
    float3 viewWS_id66;
    float3 shadingColor_id71;
    float matBlend_id39;
    float3 matNormal_id49;
    float4 matColorBase_id50;
    float4 matDiffuse_id51;
    float3 matDiffuseVisible_id67;
    float3 matSpecular_id53;
    float3 matSpecularVisible_id69;
    float matSpecularIntensity_id54;
    float matGlossiness_id52;
    float alphaRoughness_id68;
    float matAmbientOcclusion_id55;
    float matAmbientOcclusionDirectLightingFactor_id56;
    float matCavity_id57;
    float matCavityDiffuse_id58;
    float matCavitySpecular_id59;
    float4 matEmissive_id60;
    float matEmissiveIntensity_id61;
    float matScatteringStrength_id62;
    float2 matDiffuseSpecularAlphaBlend_id63;
    float3 matAlphaBlendColor_id64;
    float matAlphaDiscard_id65;
    float shadingColorAlpha_id72;
    float3 lightPositionWS_id40;
    float3 lightDirectionWS_id41;
    float3 lightColor_id42;
    float3 lightColorNdotL_id43;
    float3 lightSpecularColorNdotL_id44;
    float3 envLightDiffuseColor_id45;
    float3 envLightSpecularColor_id46;
    float lightDirectAmbientOcclusion_id48;
    float NdotL_id47;
    float NdotV_id70;
    float thicknessWS_id82;
    float3 shadowColor_id81;
    float3 H_id73;
    float NdotH_id74;
    float LdotH_id75;
    float VdotH_id76;
    uint2 lightData_id92;
    int lightIndex_id93;
    float4 ColorTarget_id2;
};
struct PS_OUTPUT 
{
    float4 ColorTarget_id2 : SV_Target0;
};
struct PS_INPUT 
{
    float4 PositionWS_id21 : POSITION_WS;
    float4 ShadingPosition_id0 : SV_Position;
    float3 normalWS_id18 : NORMALWS;
    float4 ScreenPosition_id86 : SCREENPOSITION_ID86_SEM;
    bool IsFrontFace_id1 : SV_IsFrontFace;
};
struct VS_STREAMS 
{
    float4 Position_id20;
    float3 meshNormal_id15;
    float4 PositionH_id23;
    float DepthVS_id22;
    float3 meshNormalWS_id16;
    float4 PositionWS_id21;
    float4 ShadingPosition_id0;
    float3 normalWS_id18;
    float4 ScreenPosition_id86;
};
struct VS_OUTPUT 
{
    float4 PositionWS_id21 : POSITION_WS;
    float4 ShadingPosition_id0 : SV_Position;
    float3 normalWS_id18 : NORMALWS;
    float4 ScreenPosition_id86 : SCREENPOSITION_ID86_SEM;
};
struct VS_INPUT 
{
    float4 Position_id20 : POSITION;
    float3 meshNormal_id15 : NORMAL;
};
struct DirectionalLightData 
{
    float3 DirectionWS;
    float3 Color;
};
struct PointLightData 
{
    float3 PositionWS;
    float InvSquareRadius;
    float3 Color;
};
struct PointLightDataInternal 
{
    float3 PositionWS;
    float InvSquareRadius;
    float3 Color;
};
struct SpotLightDataInternal 
{
    float3 PositionWS;
    float3 DirectionWS;
    float3 AngleOffsetAndInvSquareRadius;
    float3 Color;
};
struct SpotLightData 
{
    float3 PositionWS;
    float3 DirectionWS;
    float3 AngleOffsetAndInvSquareRadius;
    float3 Color;
};
cbuffer PerDraw 
{
    float4x4 World_id31;
    float4x4 WorldInverse_id32;
    float4x4 WorldInverseTranspose_id33;
    float4x4 WorldView_id34;
    float4x4 WorldViewInverse_id35;
    float4x4 WorldViewProjection_id36;
    float3 WorldScale_id37;
    float4 EyeMS_id38;
};
cbuffer PerMaterial 
{
    float4 constantColor_id102;
};
cbuffer PerView 
{
    float4x4 View_id24;
    float4x4 ViewInverse_id25;
    float4x4 Projection_id26;
    float4x4 ProjectionInverse_id27;
    float4x4 ViewProjection_id28;
    float2 ProjScreenRay_id29;
    float4 Eye_id30;
    float NearClipPlane_id87 = 1.0f;
    float FarClipPlane_id88 = 100.0f;
    float2 ZProjection_id89;
    float2 ViewSize_id90;
    float AspectRatio_id91;
    float4 _padding_PerView_Default;
    int LightCount_id83;
    DirectionalLightData Lights_id85[TMaxLightCount_id84];
    float ClusterDepthScale_id96;
    float ClusterDepthBias_id97;
    float2 ClusterStride_id98;
    float3 AmbientLight_id101;
    float4 _padding_PerView_Lighting;
};
Texture3D<uint2> LightClusters_id94;
Buffer<uint> LightIndices_id95;
Buffer<float4> PointLights_id99;
Buffer<float4> SpotLights_id100;
float SmoothDistanceAttenuation_id65(float squaredDistance, float lightInvSquareRadius)
{
    float factor = squaredDistance * lightInvSquareRadius;
    float smoothFactor = saturate(1.0f - factor * factor);
    return smoothFactor * smoothFactor;
}
float SmoothDistanceAttenuation_id54(float squaredDistance, float lightInvSquareRadius)
{
    float factor = squaredDistance * lightInvSquareRadius;
    float smoothFactor = saturate(1.0f - factor * factor);
    return smoothFactor * smoothFactor;
}
float GetAngularAttenuation_id67(float3 lightVector, float3 lightDirection, float lightAngleScale, float lightAngleOffset)
{
    float cd = dot(lightDirection, lightVector);
    float attenuation = saturate(cd * lightAngleScale + lightAngleOffset);
    attenuation *= attenuation;
    return attenuation;
}
float GetDistanceAttenuation_id66(float lightVectorLength, float lightInvSquareRadius)
{
    float d2 = lightVectorLength * lightVectorLength;
    float attenuation = 1.0 / (max(d2, 0.01 * 0.01));
    attenuation *= SmoothDistanceAttenuation_id65(d2, lightInvSquareRadius);
    return attenuation;
}
float GetDistanceAttenuation_id56(float lightVectorLength, float lightInvSquareRadius)
{
    float d2 = lightVectorLength * lightVectorLength;
    float attenuation = 1.0 / (max(d2, 0.01 * 0.01));
    attenuation *= SmoothDistanceAttenuation_id54(d2, lightInvSquareRadius);
    return attenuation;
}
float ComputeAttenuation_id68(float3 PositionWS, float3 AngleOffsetAndInvSquareRadius, float3 DirectionWS, float3 position, inout float3 lightVectorNorm)
{
    float3 lightVector = PositionWS - position;
    float lightVectorLength = length(lightVector);
    lightVectorNorm = lightVector / lightVectorLength;
    float3 lightAngleOffsetAndInvSquareRadius = AngleOffsetAndInvSquareRadius;
    float2 lightAngleAndOffset = lightAngleOffsetAndInvSquareRadius.xy;
    float lightInvSquareRadius = lightAngleOffsetAndInvSquareRadius.z;
    float3 lightDirection = -DirectionWS;
    float attenuation = GetDistanceAttenuation_id66(lightVectorLength, lightInvSquareRadius);
    attenuation *= GetAngularAttenuation_id67(lightVectorNorm, lightDirection, lightAngleAndOffset.x, lightAngleAndOffset.y);
    return attenuation;
}
float ComputeAttenuation_id55(PointLightDataInternal light, float3 position, inout float3 lightVectorNorm)
{
    float3 lightVector = light.PositionWS - position;
    float lightVectorLength = length(lightVector);
    lightVectorNorm = lightVector / lightVectorLength;
    float lightInvSquareRadius = light.InvSquareRadius;
    return GetDistanceAttenuation_id56(lightVectorLength, lightInvSquareRadius);
}
void ProcessLight_id69(inout PS_STREAMS streams, SpotLightDataInternal light)
{
    float3 lightVectorNorm;
    float attenuation = ComputeAttenuation_id68(light.PositionWS, light.AngleOffsetAndInvSquareRadius, light.DirectionWS, streams.PositionWS_id21.xyz, lightVectorNorm);
    streams.lightColor_id42 = light.Color * attenuation;
    streams.lightDirectionWS_id41 = lightVectorNorm;
}
void ProcessLight_id58(inout PS_STREAMS streams, PointLightDataInternal light)
{
    float3 lightVectorNorm;
    float attenuation = ComputeAttenuation_id55(light, streams.PositionWS_id21.xyz, lightVectorNorm);
    streams.lightPositionWS_id40 = light.PositionWS;
    streams.lightColor_id42 = light.Color * attenuation;
    streams.lightDirectionWS_id41 = lightVectorNorm;
}
void PrepareEnvironmentLight_id73(inout PS_STREAMS streams)
{
    streams.envLightDiffuseColor_id45 = 0;
    streams.envLightSpecularColor_id46 = 0;
}
float3 ComputeSpecularTextureProjection_id64(float3 positionWS, float3 reflectionWS, int lightIndex)
{
    return 1.0f;
}
float3 ComputeTextureProjection_id63(float3 positionWS, int lightIndex)
{
    return 1.0f;
}
float3 ComputeShadow_id62(inout PS_STREAMS streams, float3 position, int lightIndex)
{
    streams.thicknessWS_id82 = 0.0;
    return 1.0f;
}
void PrepareDirectLightCore_id61(inout PS_STREAMS streams, int lightIndexIgnored)
{
    int realLightIndex = LightIndices_id95.Load(streams.lightIndex_id93);
    streams.lightIndex_id93++;
    SpotLightDataInternal spotLight;
    float4 spotLight1 = SpotLights_id100.Load(realLightIndex * 4);
    float4 spotLight2 = SpotLights_id100.Load(realLightIndex * 4 + 1);
    float4 spotLight3 = SpotLights_id100.Load(realLightIndex * 4 + 2);
    float4 spotLight4 = SpotLights_id100.Load(realLightIndex * 4 + 3);
    spotLight.PositionWS = spotLight1.xyz;
    spotLight.DirectionWS = spotLight2.xyz;
    spotLight.AngleOffsetAndInvSquareRadius = spotLight3.xyz;
    spotLight.Color = spotLight4.xyz;
    ProcessLight_id69(streams, spotLight);
}
float3 ComputeSpecularTextureProjection_id53(float3 positionWS, float3 reflectionWS, int lightIndex)
{
    return 1.0f;
}
float3 ComputeTextureProjection_id52(float3 positionWS, int lightIndex)
{
    return 1.0f;
}
float3 ComputeShadow_id51(inout PS_STREAMS streams, float3 position, int lightIndex)
{
    streams.thicknessWS_id82 = 0.0;
    return 1.0f;
}
void PrepareDirectLightCore_id50(inout PS_STREAMS streams, int lightIndexIgnored)
{
    int realLightIndex = LightIndices_id95.Load(streams.lightIndex_id93);
    streams.lightIndex_id93++;
    PointLightDataInternal pointLight;
    float4 pointLight1 = PointLights_id99.Load(realLightIndex * 2);
    float4 pointLight2 = PointLights_id99.Load(realLightIndex * 2 + 1);
    pointLight.PositionWS = pointLight1.xyz;
    pointLight.InvSquareRadius = pointLight1.w;
    pointLight.Color = pointLight2.xyz;
    ProcessLight_id58(streams, pointLight);
}
void PrepareLightData_id57(inout PS_STREAMS streams)
{
    float projectedDepth = streams.ShadingPosition_id0.z;
    float depth = ZProjection_id89.y / (projectedDepth - ZProjection_id89.x);
    float2 texCoord = float2(streams.ScreenPosition_id86.x + 1, 1 - streams.ScreenPosition_id86.y) * 0.5;
    int slice = int(max(log2(depth * ClusterDepthScale_id96 + ClusterDepthBias_id97), 0));
    streams.lightData_id92 = LightClusters_id94.Load(int4(texCoord * ClusterStride_id98, slice, 0));
    streams.lightIndex_id93 = streams.lightData_id92.x;
}
float3 ComputeSpecularTextureProjection_id47(float3 positionWS, float3 reflectionWS, int lightIndex)
{
    return 1.0f;
}
float3 ComputeTextureProjection_id46(float3 positionWS, int lightIndex)
{
    return 1.0f;
}
float3 ComputeShadow_id45(inout PS_STREAMS streams, float3 position, int lightIndex)
{
    streams.thicknessWS_id82 = 0.0;
    return 1.0f;
}
void PrepareDirectLightCore_id44(inout PS_STREAMS streams, int lightIndex)
{
    streams.lightColor_id42 = Lights_id85[lightIndex].Color;
    streams.lightDirectionWS_id41 = -Lights_id85[lightIndex].DirectionWS;
}
void ResetStream_id88()
{
}
void AfterLightingAndShading_id145()
{
}
void PrepareEnvironmentLight_id134(inout PS_STREAMS streams)
{
    streams.envLightDiffuseColor_id45 = 0;
    streams.envLightSpecularColor_id46 = 0;
}
float3 ComputeEnvironmentLightContribution_id138(inout PS_STREAMS streams)
{
    float3 diffuseColor = streams.matDiffuseVisible_id67;
    return diffuseColor * streams.envLightDiffuseColor_id45;
}
void PrepareEnvironmentLight_id131(inout PS_STREAMS streams)
{
    PrepareEnvironmentLight_id73(streams);
    float3 lightColor = AmbientLight_id101 * streams.matAmbientOcclusion_id55;
    streams.envLightDiffuseColor_id45 = lightColor;
    streams.envLightSpecularColor_id46 = lightColor;
}
void PrepareDirectLight_id122(inout PS_STREAMS streams, int lightIndex)
{
    PrepareDirectLightCore_id61(streams, lightIndex);
    streams.NdotL_id47 = max(dot(streams.normalWS_id18, streams.lightDirectionWS_id41), 0.0001f);
    streams.shadowColor_id81 = ComputeShadow_id62(streams, streams.PositionWS_id21.xyz, lightIndex);
    streams.lightColorNdotL_id43 = streams.lightColor_id42 * streams.shadowColor_id81 * streams.NdotL_id47 * streams.lightDirectAmbientOcclusion_id48;
    streams.lightSpecularColorNdotL_id44 = streams.lightColorNdotL_id43;
    streams.lightColorNdotL_id43 *= ComputeTextureProjection_id63(streams.PositionWS_id21.xyz, lightIndex);
    float3 reflectionVectorWS = reflect(-streams.viewWS_id66, streams.normalWS_id18);
    streams.lightSpecularColorNdotL_id44 *= ComputeSpecularTextureProjection_id64(streams.PositionWS_id21.xyz, reflectionVectorWS, lightIndex);
}
int GetLightCount_id126(inout PS_STREAMS streams)
{
    return streams.lightData_id92.y >> 16;
}
int GetMaxLightCount_id125(inout PS_STREAMS streams)
{
    return streams.lightData_id92.y >> 16;
}
void PrepareDirectLights_id120()
{
}
void PrepareDirectLight_id109(inout PS_STREAMS streams, int lightIndex)
{
    PrepareDirectLightCore_id50(streams, lightIndex);
    streams.NdotL_id47 = max(dot(streams.normalWS_id18, streams.lightDirectionWS_id41), 0.0001f);
    streams.shadowColor_id81 = ComputeShadow_id51(streams, streams.PositionWS_id21.xyz, lightIndex);
    streams.lightColorNdotL_id43 = streams.lightColor_id42 * streams.shadowColor_id81 * streams.NdotL_id47 * streams.lightDirectAmbientOcclusion_id48;
    streams.lightSpecularColorNdotL_id44 = streams.lightColorNdotL_id43;
    streams.lightColorNdotL_id43 *= ComputeTextureProjection_id52(streams.PositionWS_id21.xyz, lightIndex);
    float3 reflectionVectorWS = reflect(-streams.viewWS_id66, streams.normalWS_id18);
    streams.lightSpecularColorNdotL_id44 *= ComputeSpecularTextureProjection_id53(streams.PositionWS_id21.xyz, reflectionVectorWS, lightIndex);
}
int GetLightCount_id114(inout PS_STREAMS streams)
{
    return streams.lightData_id92.y & 0xFFFF;
}
int GetMaxLightCount_id113(inout PS_STREAMS streams)
{
    return streams.lightData_id92.y & 0xFFFF;
}
void PrepareDirectLights_id112(inout PS_STREAMS streams)
{
    PrepareLightData_id57(streams);
}
float3 ComputeDirectLightContribution_id137(inout PS_STREAMS streams)
{
    float3 diffuseColor = streams.matDiffuseVisible_id67;
    return diffuseColor / PI_id105 * streams.lightColorNdotL_id43 * streams.matDiffuseSpecularAlphaBlend_id63.x;
}
void PrepareMaterialPerDirectLight_id30(inout PS_STREAMS streams)
{
    streams.H_id73 = normalize(streams.viewWS_id66 + streams.lightDirectionWS_id41);
    streams.NdotH_id74 = saturate(dot(streams.normalWS_id18, streams.H_id73));
    streams.LdotH_id75 = saturate(dot(streams.lightDirectionWS_id41, streams.H_id73));
    streams.VdotH_id76 = streams.LdotH_id75;
}
void PrepareDirectLight_id98(inout PS_STREAMS streams, int lightIndex)
{
    PrepareDirectLightCore_id44(streams, lightIndex);
    streams.NdotL_id47 = max(dot(streams.normalWS_id18, streams.lightDirectionWS_id41), 0.0001f);
    streams.shadowColor_id81 = ComputeShadow_id45(streams, streams.PositionWS_id21.xyz, lightIndex);
    streams.lightColorNdotL_id43 = streams.lightColor_id42 * streams.shadowColor_id81 * streams.NdotL_id47 * streams.lightDirectAmbientOcclusion_id48;
    streams.lightSpecularColorNdotL_id44 = streams.lightColorNdotL_id43;
    streams.lightColorNdotL_id43 *= ComputeTextureProjection_id46(streams.PositionWS_id21.xyz, lightIndex);
    float3 reflectionVectorWS = reflect(-streams.viewWS_id66, streams.normalWS_id18);
    streams.lightSpecularColorNdotL_id44 *= ComputeSpecularTextureProjection_id47(streams.PositionWS_id21.xyz, reflectionVectorWS, lightIndex);
}
int GetLightCount_id100()
{
    return LightCount_id83;
}
int GetMaxLightCount_id101()
{
    return TMaxLightCount_id84;
}
void PrepareDirectLights_id96()
{
}
void PrepareForLightingAndShading_id142()
{
}
void PrepareMaterialForLightingAndShading_id87(inout PS_STREAMS streams)
{
    streams.lightDirectAmbientOcclusion_id48 = lerp(1.0, streams.matAmbientOcclusion_id55, streams.matAmbientOcclusionDirectLightingFactor_id56);
    streams.matDiffuseVisible_id67 = streams.matDiffuse_id51.rgb * lerp(1.0f, streams.matCavity_id57, streams.matCavityDiffuse_id58) * streams.matDiffuseSpecularAlphaBlend_id63.r * streams.matAlphaBlendColor_id64;
    streams.matSpecularVisible_id69 = streams.matSpecular_id53.rgb * streams.matSpecularIntensity_id54 * lerp(1.0f, streams.matCavity_id57, streams.matCavitySpecular_id59) * streams.matDiffuseSpecularAlphaBlend_id63.g * streams.matAlphaBlendColor_id64;
    streams.NdotV_id70 = max(dot(streams.normalWS_id18, streams.viewWS_id66), 0.0001f);
    float roughness = 1.0f - streams.matGlossiness_id52;
    streams.alphaRoughness_id68 = max(roughness * roughness, 0.001);
}
void ResetLightStream_id86(inout PS_STREAMS streams)
{
    streams.lightPositionWS_id40 = 0;
    streams.lightDirectionWS_id41 = 0;
    streams.lightColor_id42 = 0;
    streams.lightColorNdotL_id43 = 0;
    streams.lightSpecularColorNdotL_id44 = 0;
    streams.envLightDiffuseColor_id45 = 0;
    streams.envLightSpecularColor_id46 = 0;
    streams.lightDirectAmbientOcclusion_id48 = 1.0f;
    streams.NdotL_id47 = 0;
}
void UpdateNormalFromTangentSpace_id23(float3 normalInTangentSpace)
{
}
float4 Compute_id135()
{
    return constantColor_id102;
}
void ResetStream_id89(inout PS_STREAMS streams)
{
    ResetStream_id88();
    streams.matBlend_id39 = 0.0f;
}
void Compute_id179(inout PS_STREAMS streams)
{
    UpdateNormalFromTangentSpace_id23(streams.matNormal_id49);
    if (!streams.IsFrontFace_id1)
        streams.normalWS_id18 = -streams.normalWS_id18;
    ResetLightStream_id86(streams);
    PrepareMaterialForLightingAndShading_id87(streams);

    {
        PrepareForLightingAndShading_id142();
    }
    float3 directLightingContribution = 0;

    {
        PrepareDirectLights_id96();
        const int maxLightCount = GetMaxLightCount_id101();
        int count = GetLightCount_id100();

        for (int i = 0; i < maxLightCount; i++)
        {
            if (i >= count)
            {
                break;
            }
            PrepareDirectLight_id98(streams, i);
            PrepareMaterialPerDirectLight_id30(streams);

            {
                directLightingContribution += ComputeDirectLightContribution_id137(streams);
            }
        }
    }

    {
        PrepareDirectLights_id112(streams);
        const int maxLightCount = GetMaxLightCount_id113(streams);
        int count = GetLightCount_id114(streams);

        for (int i = 0; i < maxLightCount; i++)
        {
            if (i >= count)
            {
                break;
            }
            PrepareDirectLight_id109(streams, i);
            PrepareMaterialPerDirectLight_id30(streams);

            {
                directLightingContribution += ComputeDirectLightContribution_id137(streams);
            }
        }
    }

    {
        PrepareDirectLights_id120();
        const int maxLightCount = GetMaxLightCount_id125(streams);
        int count = GetLightCount_id126(streams);

        for (int i = 0; i < maxLightCount; i++)
        {
            if (i >= count)
            {
                break;
            }
            PrepareDirectLight_id122(streams, i);
            PrepareMaterialPerDirectLight_id30(streams);

            {
                directLightingContribution += ComputeDirectLightContribution_id137(streams);
            }
        }
    }
    float3 environmentLightingContribution = 0;

    {
        PrepareEnvironmentLight_id131(streams);

        {
            environmentLightingContribution += ComputeEnvironmentLightContribution_id138(streams);
        }
    }

    {
        PrepareEnvironmentLight_id134(streams);

        {
            environmentLightingContribution += ComputeEnvironmentLightContribution_id138(streams);
        }
    }
    streams.shadingColor_id71 += directLightingContribution * PI_id105 + environmentLightingContribution;
    streams.shadingColorAlpha_id72 = streams.matDiffuse_id51.a;

    {
        AfterLightingAndShading_id145();
    }
}
void Compute_id163(inout PS_STREAMS streams)
{
    float4 colorBase = Compute_id135();
    streams.matDiffuse_id51 = colorBase;
    streams.matColorBase_id50 = colorBase;
}
void ResetStream_id90(inout PS_STREAMS streams)
{
    ResetStream_id89(streams);
    streams.matNormal_id49 = float3(0, 0, 1);
    streams.matColorBase_id50 = 0.0f;
    streams.matDiffuse_id51 = 0.0f;
    streams.matDiffuseVisible_id67 = 0.0f;
    streams.matSpecular_id53 = 0.0f;
    streams.matSpecularVisible_id69 = 0.0f;
    streams.matSpecularIntensity_id54 = 1.0f;
    streams.matGlossiness_id52 = 0.0f;
    streams.alphaRoughness_id68 = 1.0f;
    streams.matAmbientOcclusion_id55 = 1.0f;
    streams.matAmbientOcclusionDirectLightingFactor_id56 = 0.0f;
    streams.matCavity_id57 = 1.0f;
    streams.matCavityDiffuse_id58 = 0.0f;
    streams.matCavitySpecular_id59 = 0.0f;
    streams.matEmissive_id60 = 0.0f;
    streams.matEmissiveIntensity_id61 = 0.0f;
    streams.matScatteringStrength_id62 = 1.0f;
    streams.matDiffuseSpecularAlphaBlend_id63 = 1.0f;
    streams.matAlphaBlendColor_id64 = 1.0f;
    streams.matAlphaDiscard_id65 = 0.1f;
}
float4 ComputeShadingPosition_id11(float4 world)
{
    return mul(world, ViewProjection_id28);
}
void PostTransformPosition_id6()
{
}
void PreTransformPosition_id4()
{
}
void Compute_id41(inout PS_STREAMS streams)
{

    {
        Compute_id163(streams);
    }

    {
        Compute_id179(streams);
    }
}
void ResetStream_id40(inout PS_STREAMS streams)
{
    ResetStream_id90(streams);
    streams.shadingColorAlpha_id72 = 1.0f;
}
void PostTransformPosition_id12(inout VS_STREAMS streams)
{
    PostTransformPosition_id6();
    streams.ShadingPosition_id0 = ComputeShadingPosition_id11(streams.PositionWS_id21);
    streams.PositionH_id23 = streams.ShadingPosition_id0;
    streams.DepthVS_id22 = streams.ShadingPosition_id0.w;
}
void TransformPosition_id5()
{
}
void PreTransformPosition_id10(inout VS_STREAMS streams)
{
    PreTransformPosition_id4();
    streams.PositionWS_id21 = mul(streams.Position_id20, World_id31);
}
float4 Shading_id31(inout PS_STREAMS streams)
{
    streams.viewWS_id66 = normalize(Eye_id30.xyz - streams.PositionWS_id21.xyz);
    streams.shadingColor_id71 = 0;
    ResetStream_id40(streams);
    Compute_id41(streams);
    return float4(streams.shadingColor_id71, streams.shadingColorAlpha_id72);
}
void PSMain_id1()
{
}
void BaseTransformVS_id8(inout VS_STREAMS streams)
{
    PreTransformPosition_id10(streams);
    TransformPosition_id5();
    PostTransformPosition_id12(streams);
}
void VSMain_id0()
{
}
void PSMain_id3(inout PS_STREAMS streams)
{
    PSMain_id1();
    streams.ColorTarget_id2 = Shading_id31(streams);
}
void GenerateNormal_PS_id22(inout PS_STREAMS streams)
{
    if (dot(streams.normalWS_id18, streams.normalWS_id18) > 0)
        streams.normalWS_id18 = normalize(streams.normalWS_id18);
    streams.meshNormalWS_id16 = streams.normalWS_id18;
}
void GenerateNormal_VS_id21(inout VS_STREAMS streams)
{
    streams.meshNormalWS_id16 = mul(streams.meshNormal_id15, (float3x3)WorldInverseTranspose_id33);
    streams.normalWS_id18 = streams.meshNormalWS_id16;
}
void VSMain_id9(inout VS_STREAMS streams)
{
    VSMain_id0();
    BaseTransformVS_id8(streams);
}
void PSMain_id20(inout PS_STREAMS streams)
{
    GenerateNormal_PS_id22(streams);
    PSMain_id3(streams);
}
void VSMain_id19(inout VS_STREAMS streams)
{
    VSMain_id9(streams);
    GenerateNormal_VS_id21(streams);
}
PS_OUTPUT PSMain(PS_INPUT __input__)
{
    PS_STREAMS streams = (PS_STREAMS)0;
    streams.PositionWS_id21 = __input__.PositionWS_id21;
    streams.ShadingPosition_id0 = __input__.ShadingPosition_id0;
    streams.normalWS_id18 = __input__.normalWS_id18;
    streams.ScreenPosition_id86 = __input__.ScreenPosition_id86;
    streams.IsFrontFace_id1 = __input__.IsFrontFace_id1;
    streams.ScreenPosition_id86 /= streams.ScreenPosition_id86.w;
    PSMain_id20(streams);
    PS_OUTPUT __output__ = (PS_OUTPUT)0;
    __output__.ColorTarget_id2 = streams.ColorTarget_id2;
    return __output__;
}
VS_OUTPUT VSMain(VS_INPUT __input__)
{
    VS_STREAMS streams = (VS_STREAMS)0;
    streams.Position_id20 = __input__.Position_id20;
    streams.meshNormal_id15 = __input__.meshNormal_id15;
    VSMain_id19(streams);
    streams.ScreenPosition_id86 = streams.ShadingPosition_id0;
    VS_OUTPUT __output__ = (VS_OUTPUT)0;
    __output__.PositionWS_id21 = streams.PositionWS_id21;
    __output__.ShadingPosition_id0 = streams.ShadingPosition_id0;
    __output__.normalWS_id18 = streams.normalWS_id18;
    __output__.ScreenPosition_id86 = streams.ScreenPosition_id86;
    return __output__;
}
