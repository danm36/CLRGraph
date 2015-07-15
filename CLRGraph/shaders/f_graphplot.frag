﻿#version 330

#define LOG2 1.442695

#define CM_SOLID 0
#define CM_DISTANCEFOG 1
#define CM_WORLDCOORDS 2
#define CM_BOUNDSCOORDS 3

#define PERSP_FAR 1500.0
#define FOG_DENSITY 6.0


varying vec4 vPosition;

uniform vec4 uSeriesColor;
uniform float uSeriesColorScale;
uniform int uColorMode;

void main(void)
{
	vec4 finalColor = uSeriesColor;

	switch(uColorMode)
	{
		default: case CM_SOLID: break;
		case CM_DISTANCEFOG:
			float fog = (gl_FragCoord.z / gl_FragCoord.w) / PERSP_FAR * FOG_DENSITY;

			finalColor = mix(finalColor * 0.3, finalColor, clamp(1.0 - fog, 0.0, 1.0));
			break;
		case CM_WORLDCOORDS:
			finalColor = vPosition / 512;
			finalColor += 0.5;
			break;
		case CM_BOUNDSCOORDS:
			finalColor = vPosition;
			break;
	}

	finalColor.a = 0.5;
	gl_FragColor = finalColor * uSeriesColorScale;
}