#version 330

varying vec4 vColor;

uniform int uColorAxes;

void main(void)
{
	gl_FragColor = vColor;
}