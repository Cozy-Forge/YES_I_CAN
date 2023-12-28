// Global Inputs
CBUFFER_START(AtgGrass)
    float4 _AtgTerrainShiftSurface;
    float4 _AtgWindDirSize;
    float4 _AtgWindStrengthMultipliers;
    float4 _AtgSinTime;
    float4 _AtgGrassFadeProps;
    float2 _AtgVertexFadeProps;
    float4 _AtgGrassShadowFadeProps;
    float3 _AtgSurfaceCameraPosition;
    float _AtgBaseWind;

    float _AtgBendFrequency;
    float _AtgBendFrequencyLastFrame;
CBUFFER_END
TEXTURE2D(_AtgWindRT); SAMPLER(sampler_AtgWindRT);
TEXTURE2D(_AtgWindRTPrevious); SAMPLER(sampler_AtgWindRTPrevious);


//	Simple random function
inline float nrand(float2 pos) {
	return frac(sin(dot(pos, half2(12.9898f, 78.233f))) * 43758.5453f);
}

float4 SmoothCurve( float4 x ) {   
	return x * x * ( 3.0f - 2.0f * x );   
}
float4 TriangleWave( float4 x ) {   
    return abs( frac( x + 0.5f ) * 2.0f - 1.0f );   
}
float4 SmoothTriangleWave( float4 x ) {   
    return SmoothCurve( TriangleWave( x ) );   
}

    
void FoliageBending_float
(
	float Time,

	float3 PositionOS,
	float3 NormalOS,
	float3 TangentOS,
	float4 VertexColor,
	float2 UV,

	float WindSampleRadius,
	float WindLOD,
	float PerInstanceVariation,
	float PhaseOffset,

	float MainBending,
	float BranchBending,
	float WindDirToBranchBending,
	float EdgeFlutter,
	
	float Turbulence,
	float TurbulenceFrequency,
	float TurbulenceMask,

	float Stretchiness,
	float NormalDisplacement,

	float WindFadeDist,
	float WindFadeRange,

	float2 MinMaxScales,
	float4 HealthyColor,
	float4 DryColor,

	float Clip,

	float UpperBound,

	out real3 outPositionOS,
	out real3 outNormalOS,
	out real3 outTangentOS,
	out real4 outVertexToFragmentInterpolator
)
{
	outPositionOS = PositionOS;
	outNormalOS = NormalOS;
	outTangentOS = TangentOS;
	outVertexToFragmentInterpolator = 0;

	#if !defined(SHADERGRAPH_PREVIEW)

	//  Wind animation - mapping
		#define vMainBending VertexColor.a
		#define vBranchBending VertexColor.b
		#define vPhase VertexColor.r
		#define vEdgeFlutter VertexColor.g

	//	#define vPhase (1-VertexColor.g) //NR, NS

	//	We leave this in the setup func as this will quiet the compiler.
		//float InstanceScale = 1.0f;
		//int TextureLayer = 0;
		// #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
		// 	InstanceScale = frac(unity_ObjectToWorld[3].w);
		// 	TextureLayer = unity_ObjectToWorld[3].w - InstanceScale;
		// 	InstanceScale *= 100.0f;
		// 	//#if defined(_NORMAL)
		// 	//	terrainNormal = unity_ObjectToWorld[3].xyz;
		// 	//#endif
		// 	unity_ObjectToWorld[3] = float4(0, 0, 0, 1.0f);
		// #endif

		#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
			const float scale = InstanceScale;
		#else 
			const float scale = length(float3(UNITY_MATRIX_M[0].x, UNITY_MATRIX_M[1].x, UNITY_MATRIX_M[2].x));
		#endif
		const float3 pivot = UNITY_MATRIX_M._m03_m13_m23;
        const float3 dist = 
    	#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING == 0)
			pivot - _WorldSpaceCameraPos.xyz;
		#else
			pivot;
		#endif
        const float SqrDist = dot(dist, dist);
    
    //  Calculate far fade factor
    	#if (SHADERPASS == SHADERPASS_SHADOWS)
            // TODO: Check why i can't revert this as well? Clip?
            // MIND: Use foliage params here!
            float fade = 1.0f - saturate((SqrDist - _AtgGrassShadowFadeProps.z) * _AtgGrassShadowFadeProps.w);
        #elif (SHADERPASS == SHADERPASS_DEPTH_ONLY)
            float fade = saturate(( _AtgGrassFadeProps.x - SqrDist) * _AtgGrassFadeProps.y);
        #else
            float fade = saturate(( _AtgGrassFadeProps.x - SqrDist) * _AtgGrassFadeProps.y);
        #endif
    //  Cull based on far culling distance
        if (fade == 0.0f) {
            outPositionOS /= fade;
            return;
        }

	//	Get some random value per instance
		float random = nrand(  float2(scale, 1.0 - scale) );

	//  Calculate near fade factor / reversed!
        const float smallScaleClipping = saturate(( SqrDist - _AtgGrassFadeProps.z) * _AtgGrassFadeProps.w);
        float clip = (random < Clip)? 1 : 0;
        clip = 1.0f - smallScaleClipping * clip;
    //  Cull based on near culling distance
        if (clip == 0.0f) {
            outPositionOS /= clip;
            return;
        }

    //	Calculate Vertex Fade
    	const float vertexClipping = saturate(( SqrDist - _AtgVertexFadeProps.x) * _AtgVertexFadeProps.y);
    	float vclip = (UV.x < 0)? 1 : 0;
    	clip = saturate(clip - vertexClipping * vclip);
        if (clip == 0.0f) {
            outPositionOS /= clip;
            return;
        }

        fade *= clip;
    
    //  Apply fading
        outPositionOS.xyz = lerp(outPositionOS.xyz, float3(0, 0, 0), (1.0 - fade).xxx);

    //	Fade Wind
		float windFade = saturate(( WindFadeDist * WindFadeDist - SqrDist) * WindFadeRange);
    	if (windFade > 0)
    	{

    	//	Time
			float localTime = _AtgBendFrequency;
	    
		//	Handle motion vectors
			bool renderPreviousPosition = false;
			#if (SHADERPASS == SHADERPASS_MOTION_VECTORS)
			//	motionvectors rely on _LastTimeParameters.xyz
				if(Time != _Time.y) {
					localTime = _AtgBendFrequencyLastFrame;
					renderPreviousPosition = true;
					if(SqrDist > 100) {
						//return;
					}
				}
			#endif
	
		//  Wind animation
			float originalLength = length(outPositionOS);
			float3 samplePosWS = GetAbsolutePositionWS(TransformObjectToWorld(outPositionOS.xyz * WindSampleRadius));
		//	Handle motion vectors
			float2 windUV = samplePosWS.xz * _AtgWindDirSize.w  - random.xx * PerInstanceVariation; // * WindMultiplier.w; // no iphase!!!
		    #if (SHADERPASS == SHADERPASS_MOTION_VECTORS)
				float4 wind;
				if(renderPreviousPosition) {
					wind = SAMPLE_TEXTURE2D_LOD(_AtgWindRTPrevious, sampler_AtgWindRT, windUV, WindLOD);
				}
				else {
					wind = SAMPLE_TEXTURE2D_LOD(_AtgWindRT, sampler_AtgWindRT, windUV, WindLOD);
				}
			#else
				float4 wind = SAMPLE_TEXTURE2D_LOD(_AtgWindRT, sampler_AtgWindRT, windUV, WindLOD);
			#endif

		    float3 windDir = _AtgWindDirSize.xyz;
		    windDir = TransformWorldToObjectDir(windDir);
		    //float animWind = wind.r * (wind.g * 2.0f - 0.243f);
		    float animWind = wind.r * wind.g;
			animWind *= _AtgWindStrengthMultipliers.y; // Contains mainWind from WindZone * foliage specific multiplier
		//	Fade out wind
			animWind *= windFade;

		    const float fDetailAmp = 0.1;
			const float fBranchAmp = 0.3;
			
		//	MIND: Camera relative rendering!
			float fObjPhase = dot(GetAbsolutePositionWS(UNITY_MATRIX_M._m03_m13_m23), 2); // cry uses 2
			float fBranchPhase = fObjPhase + vPhase * PhaseOffset;
			float fVtxPhase = dot(PositionOS.xyz, vEdgeFlutter + fBranchPhase);

			float2 vWavesIn = localTime.xx + float2(fVtxPhase, fBranchPhase);
			float4 vWaves = frac( vWavesIn.xxyy * float4(1.975, 0.793, 0.375, 0.193) ) * 2.0 - 1.0;
			vWaves = SmoothTriangleWave( vWaves );
			float2 vWavesSum = vWaves.xz + vWaves.yw;

		//	Break up WindDir
			windDir.xz = windDir.xz + (frac(fObjPhase) * 2 - 1).xx * 0.25; 
			windDir = normalize(windDir);

			float3 offset = 0;
		//	Primary bending - which is only animated by the incoming texture wind
			offset = vMainBending * MainBending * windDir * animWind;
		
		//	Edge Flutter
			float3 bend = vEdgeFlutter * EdgeFlutter * fDetailAmp * NormalOS.xyz;
		//	Branch bending - old style
			float branchBending = vBranchBending * BranchBending * fBranchAmp;
			bend.y += branchBending;
			offset += vWavesSum.xyx * bend * animWind; 

		//	Turbulence
			Turbulence = Turbulence * lerp(1, vEdgeFlutter, TurbulenceMask);
			//UNITY_BRANCH // No good!
			if(Turbulence > 0) {
				float tOffset = (vBranchBending + vPhase) * 4; // + vMainBending) * .5; // this is scale
			//	Get a unique frequency per object and phase
				float tFrequency = TurbulenceFrequency * (localTime.x + fObjPhase * 8 + vPhase);
				float4 tWaves = SmoothTriangleWave(float4( tFrequency + tOffset, tFrequency * 0.75 + tOffset, tFrequency * 0.5 + tOffset, tFrequency * .25 + tOffset));
				float noiseSum = tWaves.x + tWaves.y + (tWaves.z * tWaves.w);
				noiseSum = 1 - noiseSum;
				offset += NormalOS.xyz * noiseSum * vBranchBending * Turbulence * fDetailAmp * wind.g * _AtgWindStrengthMultipliers.y * windFade;
			}

		//	Branch bending along windDir
			//UNITY_BRANCH // No good!
			if(WindDirToBranchBending > 0) {
				offset.xz += windDir.xz * animWind * vBranchBending * fBranchAmp * WindDirToBranchBending * vWavesSum.y; // do we want to animate it (vWavesSum.y)? only way to factor in phase...
			}

		//	Apply Wind Animation
			#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
				outPositionOS -= offset;
				outNormalOS.xz -= offset.xz * PI * NormalDisplacement;
			#else 
				outPositionOS += offset;
				outNormalOS.xz += offset.xz * PI * NormalDisplacement;
			#endif
		//	Here we apply normalize - will be stripped if we do not connect outTangentOS.
			outTangentOS = Orthonormalize(outTangentOS, normalize(outNormalOS));

		//	Preserve length
			outPositionOS = lerp(normalize(outPositionOS) * originalLength, outPositionOS, Stretchiness.xxx);
		}

	//  Set color variation
		float normalizedScale = (scale - _MinMaxScales.x) * _MinMaxScales.y;
	    normalizedScale = saturate(normalizedScale);
	    outVertexToFragmentInterpolator.x = normalizedScale;
		outVertexToFragmentInterpolator.y = smoothstep(0, UpperBound, vMainBending); // AO

	#endif
}