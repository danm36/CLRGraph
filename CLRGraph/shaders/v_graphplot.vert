#version 330

attribute vec3 aVert;

varying vec4 vPosition;

uniform mat4 uPVMMatrix;

void main(void)
{
	gl_Position = uPVMMatrix * vec4(aVert.x, aVert.y, -aVert.z, 1);
	vPosition = vec4(aVert, 1);
}